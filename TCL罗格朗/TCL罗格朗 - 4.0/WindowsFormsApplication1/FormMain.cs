#region 命名空间引用
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HG.Configure;
using HG.MyJCZ;
using HG_Socket;
using HG_EVENT;
using HG.LogRecord;
using System.Management;
using HG.Communication;
using System.IO.Ports;
using System.IO;
using System.Threading;
using WindowsFormsApplication1.Register;
using System.Timers;
using HGMark.OPCUAClassLibrary;
using ModbusClassLibrary;
#endregion
namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {
        private MyClient myclience = null;
        private Log log = new Log();
        private SerialPortInfo COMPROIO = new SerialPortInfo();
        private CommInterface ComworkIO = new CommSerial();
        private SerialPortInfo COMPROSAOMA = new SerialPortInfo();
        private CommInterface ComworkSAOMA = new CommSerial();
        private List<string> markedtxtlist = new List<string>();
        private List<string> unmarkedtxtlist = new List<string>();
        public AutoResetEvent _semaphore = new AutoResetEvent(false);
        private ManualResetEvent _manualevent = new ManualResetEvent(false);
        private object _markedlistlock = new object();
        private object _iooutlock = new object();
        string nonestring;

        public FormMain()
        {
            InitializeComponent();
        }
 
        /// <summary>
        /// 主要加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
           
            byte[] nonestringarry = new byte[39];
            for (int a = 0; a < nonestringarry.Length; a++)
            {
                nonestringarry[a] = 0x20;
            }
            nonestring = System.Text.Encoding.ASCII.GetString(nonestringarry);

            byte getcpuidresult = GetCPUIDAuthority();
            if (getcpuidresult == 1)
            {
                CreatProcessThread();
                SetProcessbarvalue(0);
                INITWINDOWSFORM();
                SetProcessbarvalue(20);
                HGLASERINIT();
                SetProcessbarvalue(70);
                for (byte a = 0; a <= 100; a++)
                {
                   Thread.Sleep(20);
                    int addp = (int)(0.29 * a);
                    SetProcessbarvalue(70 +addp );
                }                  
               CommunicationIint();   //通信
               CreatWorkThread();   
               Thread.Sleep(50);
               SetProcessbarvalue(100);
               Thread.Sleep(1000);
             //  timer.Elapsed += new System.Timers.ElapsedEventHandler(Timerevent);
             //  timer.AutoReset = true;
            //  timer.Enabled = true;
            }
            else
            {
                this.Close(); 
            }        
        }

        #region 使用权限
        private System.Timers.Timer timer = new System.Timers.Timer(1000);
        /// <summary>
        /// 用户使用时间限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timerevent(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            ClassRegisterUserAuthority.GetUsertimesWithAuthorityResult re = ClassRegisterUserAuthority.GetUsertimesWithAuthority(StopWork);
            if (re == ClassRegisterUserAuthority.GetUsertimesWithAuthorityResult.CheckusertimesOK)
            {

            }
            else if (re == ClassRegisterUserAuthority.GetUsertimesWithAuthorityResult.CheckusertimesFail)
            {
                MessageBox.Show("err times over", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (re == ClassRegisterUserAuthority.GetUsertimesWithAuthorityResult.CheckusertimesCancel)
            {
                        this.BeginInvoke((EventHandler)(delegate
                        {
                            this.Close();
                        }));
            }
            timer.Enabled = true;    
        }
        /// <summary>
        /// 获取用户使用CPU限制
        /// </summary>
        /// <returns></returns>
        private byte GetCPUIDAuthority()
        {
            byte a = 5;
            while (true)
            {
                ClassRegisterUserAuthority.GetUserCPUIDWithAuthorityResult re = ClassRegisterUserAuthority.GetUserCPUIDWithAuthority();
                if (re == ClassRegisterUserAuthority.GetUserCPUIDWithAuthorityResult.CheckCPUIDOK)
                {
                    a = 1;
                    break;
                }
                else if (re == ClassRegisterUserAuthority.GetUserCPUIDWithAuthorityResult.CheckCPUIdCancel)
                {
                    a = 0;
                    break;
                }
                else if (re == ClassRegisterUserAuthority.GetUserCPUIDWithAuthorityResult.CheckCPUIdFail)
                {
                    continue;
                }

            }
            return a;
        }

        #endregion
        #region 进度条
        private ProcessbarStruct processbarsourcesadd = new ProcessbarStruct();
        /// <summary>
        /// 设置进度条
        /// </summary>
        /// <param name="a"></param>
        private void SetProcessbarvalue(int a)
        {
            lock (processbarsourcesadd._lockprrocessobj)
            {
                processbarsourcesadd.processbarvalue = a;
            }
        }
        private void CreatProcessThread()
        {
            Thread myThread = new Thread(new ParameterizedThreadStart(OnProcessTreadproc));
            myThread.IsBackground = true;
            myThread.Start(this);
        } 
        private void OnProcessTreadproc(object p)
        {
            FormProcess f = new FormProcess(ref processbarsourcesadd);     
            f.ShowDialog();
        }
    #endregion


        private   void StopWork()
        {
            MarkJcz.StopMark();
            MessageBox.Show("Stop Working","",MessageBoxButtons.OK,MessageBoxIcon.Error);

        }
        Thread myThread;
        private void CreatWorkThread()
        {
            if (MarkJcz.ReadPort(8))
            {
                senorstauts = sensorupORdown.down;
            }
            else
            {
                senorstauts = sensorupORdown.up;
            }
             myThread = new Thread(new ParameterizedThreadStart(OnworkFrameTreadproc));
             myThread.IsBackground = true;
             myThread.Start(this);
        }
        private  enum sensorupORdown
        {
            up,
            down
        }
        private sensorupORdown  senorstauts  = sensorupORdown.down;
        private string objname = "name";
        private int adduptrrigersignalstomarknumber = 0;
        private bool adduptrrigersignalstomarknumberstaus = false;
        private void OnworkFrameTreadproc(object p)
        {
            while (true)
            {
                _manualevent.WaitOne();

                try
                {
                  
                  if (!adduptrrigersignalstomarknumberstaus)         
                  {

                    if (!MarkJcz.ReadPort(8))//up
                    {
                        senorstauts = sensorupORdown.up;                                
                    }
                    else //down
                    {
                        if (senorstauts == sensorupORdown.up)
                        {
                            adduptrrigersignalstomarknumber = adduptrrigersignalstomarknumber +1;
                            log.WriteLog("add a trigger signal  :" + adduptrrigersignalstomarknumber.ToString(), textBoxlog);
                            if (adduptrrigersignalstomarknumber == 1)
                            {
                                adduptrrigersignalstomarknumberstaus = true;
                                log.WriteLog("trigger signal is :" + adduptrrigersignalstomarknumber.ToString() + " and adduptrrigersignalstomarknumberstaus is true", textBoxlog);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                        }
                        senorstauts = sensorupORdown.down;

                    }

                }
                else
                  {

                      if (!MarkJcz.ReadPort(8))//up
                      {

                          #region main

                          int listcount = 0;
                          lock (lockprintstring)
                          {
                              listcount = printstring.Count;
                          }
                          if (listcount > 0)
                          {
                              string ptinttxt;
                              lock (lockprintstring)
                              {
                                  ptinttxt = printstring[0];
                              }

                              MarkJcz.ChangeTextByName(ref objname, ref ptinttxt);
                              if (MarkJcz.MarkFlyByInIOSignal())
                              {
                                  if (ptinttxt == nonestring)
                                  {
                                      //MarkJcz.WritePort(5, true);
                                      //Thread.Sleep(10);
                                      //MarkJcz.WritePort(5, false);
                                      //  log.WriteLog("one none text :" + ptinttxt, textBoxlog);
                                  }
                                  log.WriteLog("printed text :" + ptinttxt, textBoxlog);
                                  lock (lockprintstring)
                                  {
                                      if (printstring.Count > 0)
                                      {
                                          printstring.RemoveAt(0);
                                      }
                                  }
                                  while (true)
                                  {
                                      if (MarkJcz.ReadPort(8))
                                      {
                                          log.WriteLog("break a finished card go away", textBoxlog);
                                          break;
                                      }
                                  }
                              }
                              else
                              {
                                  log.WriteLog("MarkFlyByInIoSignal is Stopped :" + ptinttxt, textBoxlog);
                              }

                          }
                          else
                          {
                              log.WriteLog("no data in the buffer,  waitting for data", textBoxlog);
                              MarkJcz.WritePort(5, true);
                              while (true)
                              {
                                  Thread.Sleep(10);
                                  lock (lockprintstring)
                                  {
                                      if (printstring.Count > Convert.ToInt32(textBoxnumberofwipeouterr.Text))
                                      {
                                          break;
                                      }
                                  }
                              }
                              log.WriteLog("get " + printstring.Count.ToString() + "   data in the buffer ,break waitting for data ", textBoxlog);
                              MarkJcz.WritePort(5, false);
                          }
                          #endregion

                      }
                      else
                      {

                      }



                  }
                                              
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
                finally
                {

                }
            }
        }
        /// <summary>
        /// 初始化系统设置界面
        /// </summary>
        private void INITWINDOWSFORM()
        {
            tabControl1.Location = new System.Drawing.Point(4, 4);
            tabControl1.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height);
            string[] comarrys = new string[66];
            for (int i = 1; i <= 66; i++)
            {
                comarrys[i - 1] = "COM" + i.ToString();
            }
            comboBox1.Items.AddRange(comarrys);
            string[] combat = new string[] { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "43000", "56000", "57600", "115200" };
            comboBox2.Items.AddRange(combat);
            string[] comarrys2 = new string[66];
            for (int i = 1; i <= 66; i++)
            {
                comarrys2[i - 1] = "COM" + i.ToString();
            }

            Configure cf = new Configure();
            comboBox1.Text = cf.ReadConfig("COMSET", "COMPORT1", comboBox1.Text);
            comboBox2.Text = cf.ReadConfig("COMSET", "COMBATE1", comboBox2.Text);          
            textBoxIP1.Text = cf.ReadConfig("IPSET", "IPADDER1", textBoxIP1.Text);
            textBoxIPCOM1.Text = cf.ReadConfig("IPSET", "IPCOM1", textBoxIPCOM1.Text);    
            textBoxdatanumberallow.Text =  cf.ReadConfig("DATAALLOWSET", "NUMBER", textBoxdatanumberallow.Text);
            textBoxnumberofwipeouterr.Text= cf.ReadConfig("WIPEOUTERRSET", "WIPEOUTERRNUMBER", textBoxnumberofwipeouterr.Text);
          
        }


        /// <summary>
        /// 初始化激光器
        /// </summary>
        private void HGLASERINIT()
        {
            Configure Configuretem = new Configure();
            textBoxmodelpath.Text = Configuretem.ReadConfig("SystemParam", "EzdModelFilepath", "");
            if (!MarkJcz.InitLaser(this.Handle))//初始化激光器
            {
                MessageBox.Show("初始化激光器失败");
                return;
            }
            string pathstr = textBoxmodelpath.Text;
            if (MarkJcz.LoadEzdFile(ref pathstr, false) != "")//加载 模板
            {
                Configure Configuretems = new Configure();
                Configuretems.WriteConfig("SystemParam", "EzdModelFilepath", textBoxmodelpath.Text);
                MarkJcz.ShowPreviewBmp(pictureBox1);
            }
            else
            {
                MessageBox.Show("加载模板失败");
                return;
            }

        }
        /// <summary>
        /// 通信初始化
        /// </summary>
        private void CommunicationIint()
        {
            //myclience = new MyClient(GetDataFromSocketServer, textBoxIP1.Text, Convert.ToInt32(textBoxIPCOM1.Text), 1000);
            //try
            //{
            //    if (myclience.ConnectToServer())
            //    {
            //        log.WriteLog("连接视觉服务器成功", textBoxlog);
            //    }
            //    else
            //    {
            //        log.WriteLog("连接视觉服务器失败", textBoxlog);
            //        MessageBox.Show("连接视觉服务器失败");
            //      //this.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("connect to vision server fail"+ex.ToString());
            //   // this.Close();
            //}

          
            COMPROIO.PortName =  comboBox1.Text;
            COMPROIO.BaudRate = Convert.ToInt32(comboBox2.Text);
            (ComworkIO as CommSerial).PortInfo = COMPROIO;
            (ComworkIO as CommSerial).SetDataReceived(SerialDataReceivedfromiobord);
            if(ComworkIO.Open())
            {
                log.WriteLog("Com  successful",textBoxlog);
                byte[] wrok1_off = System.Text.Encoding.ASCII.GetBytes("i");
                ComworkIO.Write(wrok1_off);
            }
            else
            {
               MessageBox.Show("Com  fail");
               this.Close();
            }
             
            //COMPROSAOMA.Parity = Parity.Even;
            //(ComworkSAOMA as CommSerial).PortInfo = COMPROSAOMA;
            //(ComworkSAOMA as CommSerial).SetDataReceived(SerialDataReceivedfromsaoma);

            //if (ComworkSAOMA.Open())
            //{
            //    log.WriteLog("com scan code successful", textBoxlog);
            //}
            //else
            //{
            //    MessageBox.Show("com scan code fail");
            //  //this.Close();              
            //}

        }
        /// <summary>
        /// 网络回馈函数
        /// </summary>
        /// <param name="strdata"></param>
        private void GetDataFromSocketServer(string strdata)
        {
   
        }
        /// <summary>
        /// 扫码回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialDataReceivedfromsaoma(object sender, SerialDataReceivedEventArgs e)
        {        

        }

        private List<byte> ListgetbytefromtheTCL = new List<byte>();
        private object lockprintstring = new object();
        private List<string> printstring = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mess"></param>
        private void PrintFreturnGetByteArray(ref byte[] mess)
        {
            string p = null;
            for (int a = 0; a < mess.Length; a++)
            {
                p = p + string.Format("{0:X2} ", mess[a]) + " ";
            }
            log.WriteLog("get byte array :" + p, textBoxlog);
        }
        /// <summary>
        /// IO卡回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialDataReceivedfromiobord(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] getbyte;
                ComworkIO.Read(out getbyte);
              //  PrintFreturnGetByteArray(ref getbyte);
                ListgetbytefromtheTCL.AddRange(getbyte);
                if (ListgetbytefromtheTCL.Count > 0)
                {
                    byte[] getbytefromlist = ListgetbytefromtheTCL.ToArray();
                    ListgetbytefromtheTCL.Clear();
                    for (int a = 0; a < getbytefromlist.Length - 2; a++)
                    {
                        if (getbytefromlist[a] == 0x1b && getbytefromlist[a + 1] == 0x02 && getbytefromlist[a + 2] == 0x1d)//find the start code 
                        {
                            for (int q = a + 3; q < getbytefromlist.Length - 2; q++)
                            {
                                if (getbytefromlist[q] == 0x1b && getbytefromlist[q + 1] == 0x03 && getbytefromlist[q + 2] == 0x0a)//find end  code 
                                {
                                    byte bytenumber = getbytefromlist[a + 3];
                                    int end_codeposition = a + 3 + 2 + bytenumber;
                                    byte[] stringp = new byte[bytenumber];
                                    for (int b = 0; b < stringp.Length; b++)
                                    {
                                        stringp[b] = getbytefromlist[end_codeposition - bytenumber + b];
                                    }

                                    string gettxt = System.Text.Encoding.ASCII.GetString(stringp);
                                    lock (lockprintstring)
                                    {
                                        if (printstring.Count >= Convert.ToInt32(textBoxdatanumberallow.Text))
                                        {
                                            log.WriteLog(gettxt+"  can not added to the list,as the list count is  :" + textBoxdatanumberallow.Text, textBoxlog);
                                        }
                                        else
                                        {
                                            printstring.Add(gettxt);
                                            log.WriteLog("add print text to list:" + gettxt, textBoxlog);
                                        }
                                    }
                                    int morebyteposition = a + bytenumber + 6 + 2;
                                    byte[] remainbyte = new byte[getbytefromlist.Length - gettxt.Length - 8 - a];
                                    for (int c = 0; c < remainbyte.Length; c++)
                                    {
                                        remainbyte[c] = getbytefromlist[morebyteposition + c];
                                    }
                                    getbytefromlist = remainbyte;
                                }
                                else
                                {

                                }
                            }

                        }
                        else
                        {
                        }
                    }
                    ListgetbytefromtheTCL.AddRange(getbytefromlist);
                }


            }
             catch (Exception ex)
            {
           
                MessageBox.Show(ex.ToString());

                return;
            }
            finally
            {
             
            }
        }


        /// <summary>
        /// mark
        /// </summary>
        /// <param name="markstr"></param>
        private bool MarkWork(string markstr)
        {              
           return true;          
        }

        private object _jczlock = new object();

        private void FinishedMarkioout()
        {
            lock (_iooutlock)
            {
                          
            }

        }

 

        private object _markedtextlock = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fstr"></param>
        private void CreatFinishedMarkTxtFile(string path,string fstr)
        {
            lock (_markedtextlock)
            {
                StringBuilder strFile = new StringBuilder();
                strFile.AppendFormat("{0}\\{1}\\{2}\\", path, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                if (!Directory.Exists(strFile.ToString()))
                {
                    Directory.CreateDirectory(strFile.ToString());
                }
                //将数据写入文件中
                strFile.Append(DateTime.Now.ToString("yyyy-MM-dd") + "FinishedMarkedTxt" + ".txt");
                using (StreamWriter swAppend = File.AppendText(strFile.ToString()))
                {
                    swAppend.WriteLine(Convert.ToString(fstr));
                }
            }
        }
        /// <summary>
        /// 手动加载模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {          
            OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "模板文件(*.ezd)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strEzdpathA = openFileDialog1.FileName;
                textBoxmodelpath.Text = strEzdpathA;
                if (MarkJcz.LoadEzdFile(ref strEzdpathA, false) != "")
                {
                    Configure Configuretem = new Configure();
                    Configuretem.WriteConfig("SystemParam", "EzdModelFilepath", strEzdpathA);
                    MarkJcz.ShowPreviewBmp(pictureBox1);
                }
                else
                {
                    MessageBox.Show("加载模板失败");
                    return;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Configure cf = new Configure();
            cf.WriteConfig("COMSET", "COMPORT1", comboBox1.Text);
            cf.WriteConfig("COMSET", "COMBATE1", comboBox2.Text);
            MessageBox.Show("OK");
        }

  

        private void button6_Click(object sender, EventArgs e)
        {    
            Configure cf = new Configure();
            cf.WriteConfig("IPSET", "IPADDER1",textBoxIP1.Text);
            cf.WriteConfig("IPSET", "IPCOM1", textBoxIPCOM1.Text);
            MessageBox.Show("OK");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MarkJcz.Mark(false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MarkJcz.StartRed(20))
            {
                MessageBox.Show("红光预览中。。。");
                MarkJcz.StopRed();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
          

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 1)
            {
                Form f = new FormProtectHGLaserSoft();
                DialogResult dre = f.ShowDialog();

                if (dre == DialogResult.OK)
                {

                }
                else
                {
                    tabControl1.SelectedIndex = 0;
                }
            } 

        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
       

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            FormSetScreat f = new FormSetScreat();
            f.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {      
            //  OPCUAClassLibraryHelper.ConnectServer("http://117.48.203.204:62547/DataAccessServer");
            //  DateTime op = OPCUAClassLibraryHelper.ReadNodeInfo<DateTime>("ns=2;s=Machines/Machine C/AlarmTime");

            for (int a = 0; a<printstring.Count;a++)
            {
                log.WriteLog("clear   " + printstring[a], textBoxlog);
            }
            printstring.Clear();

            MarkJcz.StopMark();
            string objname = "name";
            string ptinttxt = "";
            MarkJcz.ChangeTextByName(ref objname, ref ptinttxt);
         
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Text == "开始")
            {
                _manualevent.Set();
                button8.Text = "停止";
                button7.Enabled = false;
            }
            else if(button8.Text == "停止")
            {
                _manualevent.Reset(); 
                button8.Text = "开始";
                button7.Enabled = true;
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            Configure cf = new Configure();
            cf.WriteConfig("DATAALLOWSET", "NUMBER", textBoxdatanumberallow.Text);
            cf.WriteConfig("WIPEOUTERRSET", "WIPEOUTERRNUMBER", textBoxnumberofwipeouterr.Text);
            MessageBox.Show("OK");
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            MarkJcz.StopMark();
        }

        private void button11_Click_2(object sender, EventArgs e)
        {

          //  1B  02  1D  27  00  20  20  20  20  4B  38  2F  34  32  36  2F  31  30  55  53  4C  47  2D  43  32  20  31  38  30  37  31  39  43  52  31  38  30  32  35  38  20  20  20  20  1B  03  0A

        }
    }
}
