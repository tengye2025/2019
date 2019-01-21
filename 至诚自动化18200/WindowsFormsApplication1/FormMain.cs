#region 命名空间引用
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using HG.Configure;
using HG.MyJCZ;
using HG.LogRecord;
using System.Management;
using HG.Communication;
using System.IO.Ports;
using System.IO;
using System.Threading;
using WindowsFormsApplication1.Register;
using System.Timers;
//using HGMark.OPCUAClassLibrary;
//using ModbusClassLibrary;
using WindowsFormsApplication1.MarkInterface;
using CommNetSocketClass;
using FtpClass;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Odbc;
#endregion
namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {
      
        private Log log = new Log();
        //private SerialPortInfo COMPROIO = new SerialPortInfo();
        //private CommInterface ComworkIO = new CommSerial();
        //private SerialPortInfo COMPROSAOMA = new SerialPortInfo();
        //private CommInterface ComworkSAOMA = new CommSerial();
   

        private object _markedlistlock = new object();
        private object _iooutlock = new object();

        private ManualResetEvent  _workenvent =  new ManualResetEvent(false);
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
            byte getcpuidresult =  GetCPUIDAuthority();
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
                    Thread.Sleep(2);
                    int addp = (int)(0.29 * a);
                    SetProcessbarvalue(70 +addp );
                } 
                                                 
              // CommunicationIint();   //通信
                // DBInit();//初始化数据库连接
                 CreatWorkThread();                   
               Thread.Sleep(50);
               SetProcessbarvalue(100);
               // Thread.Sleep(1000);
              //  timer.Elapsed += new System.Timers.ElapsedEventHandler(Timerevent);
             //   timer.AutoReset = true;
             //   timer.Enabled = true;

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
            HgMarkInterface.CloseLaser(1);
            MessageBox.Show("Stop Working","",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void CreatWorkThread()
        {
            Thread myThread = new Thread(new ParameterizedThreadStart(OnworkFrameTreadproc));
            myThread.IsBackground = true;
            myThread.Start(this);
        }
        string printqrcode = "QRCode";
        string s1 = "1";
        string s2 = "2";
        Configure cypoints = new Configure();
         
   
       private void OnworkFrameTreadproc(object p)
       {
          
            while (true)
            {               
                _workenvent.WaitOne(); 
                if(HgMarkInterface.ReadIO(4))
                {
                    string[] stxt = File.ReadAllLines(textBox1.Text);
                    if (stxt.Length > 0)
                    {
                        string s1s = stxt[0].Substring(0, 10);
                        string s2s = stxt[0].Substring(16, 4);

                        HgMarkInterface.ChangeNameByTxtstring(ref s1, ref s1s, 0);
                        HgMarkInterface.ChangeNameByTxtstring(ref s2, ref s2s, 0);
                        HgMarkInterface.Mark(false, 0);
                        log.WriteLog("print " + s1s + "      " + s2s, textBoxlog);

                        string[] sttxt2 = new string[stxt.Length - 1];
                        for (int a = 0; a < sttxt2.Length; a++)
                        {
                            sttxt2[a] = stxt[a + 1];
                        }
                        File.WriteAllLines(textBox1.Text, sttxt2);
                    }
                    else
                    {
                        MessageBox.Show("没有码");
                    }
                }
                Thread.Sleep(10);     
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
          
            string sorc =   cf.ReadConfig("SORCSET", "SCCHOSE", "");
            switch(sorc)
            {
                case "S": radioButtonasserver.Checked = true;radioButtonasclient.Checked = false; break;
                case "C": radioButtonasserver.Checked = false; radioButtonasclient.Checked = true; break;
                default:break;
            }

        }
        private void DBInit()
        {
            //string csql = "data source=10.60.4.79;initial catalog=bodoe_TRX;user id=wcs;password=123abc;" + "Connect Timeout = 4;";
            //sqlseveroperater.sqlseverClass.SQLSEVERINITSet(ref csql);
             Configure cf = new Configure();
            textBoxdatasourcename.Text = cf.ReadConfig("ODBC", "sourcename", textBoxdatasourcename.Text);
            textBoxloginid.Text= cf.ReadConfig("ODBC", "loginid", textBoxloginid.Text);
            textBoxpassword.Text =cf.ReadConfig("ODBC", "password", textBoxpassword.Text);

            if (textBoxdatasourcename.Text !="" && textBoxloginid.Text!="" && textBoxpassword.Text!="")
            {
                try
                {
                 
                    string ConStr = "DSN="+ textBoxdatasourcename.Text + ";UID="  +  textBoxloginid.Text +";PWD=" + textBoxpassword.Text;
                
                    OdbcConnection odbcCon = new OdbcConnection(ConStr);
                  
                    string SqlStr = "SELECT SPOOL_ID,MATERIAL_COLOR, QR_CODE,HE_TEMP,HB_TEMP,SPOOL_DATE  FROM  f_FIL_LABEL_RETURN_SPOOL()";


                    OdbcDataAdapter odbcAdapter = new OdbcDataAdapter(SqlStr, odbcCon);
        
                    DataSet ds = new DataSet();
                    odbcAdapter.Fill(ds);
                  //  this.dataGridView1.DataSource = ds.Tables[0].DefaultView;

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        /// <summary>
        /// 初始化激光器
        /// </summary>
        private void HGLASERINIT()
        {
            HgMarkInterface.MarkCardinfoST mst = new HgMarkInterface.MarkCardinfoST();
            mst.ph = this.Handle;
            mst.markcard = HgMarkInterface.MarkCardType.markjcz;
            string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            str += "\\Settings.xml";
            mst.systemxmlfilepath = str;
            HgMarkInterface.MarkCardInformationSt = mst;
            Configure Configuretem = new Configure();
            textBoxmodelpath.Text = Configuretem.ReadConfig("SystemParam", "EzdModelFilepath", "");
            if(HgMarkInterface.InitLaser() == HgMarkInterface.Lasererr.initfail)//初始化激光器
            {
                MessageBox.Show("初始化激光器失败");
                return;
            }else
            {
                log.WriteLog("初始化激光器成功",textBoxlog);
            }

                string pathstr = textBoxmodelpath.Text;
                int mmksfileindex = -1;
                mmksfileindex = HgMarkInterface.LoadtheModelFile(ref pathstr);          
                Configure Configuretems = new Configure();
                Configuretems.WriteConfig("SystemParam", "EzdModelFilepath", textBoxmodelpath.Text);
                HgMarkInterface.DrawModelFileInPicture(ref pictureBoxmodelfileshow, mmksfileindex);
                log.WriteLog("加载模板成功", textBoxlog);
                HgMarkInterface.Resetaxis(true, true);
                HgMarkInterface.AxisCorrectOrigin(0);


            if (mst.markcard == HgMarkInterface.MarkCardType.markmsi)
            {
                if (HgMarkInterface.MsiSetPreseveMode(1) == HgMarkInterface.Lasererr.setpresevemodefail)
                {
                    MessageBox.Show("MSI设置预存模式失败");
                    return;
                }else
                {
                    log.WriteLog("MSI预存模式成功", textBoxlog);
                }
            }
            else
            {

            }

        }
        private MyClient myclience = null;
        private MyServer myserver = null;
        NetInterfaceinfo info = new NetInterfaceinfo();
        /// <summary>
        /// 通信初始化
        /// </summary>
        private void CommunicationIint()
        {
            if (radioButtonasclient.Checked == true)
            {
                info.CorS = netApplyInterfaceChose.Clience;
                info.Ip = textBoxIP1.Text;
                info.Port = Convert.ToInt16(textBoxIPCOM1.Text);
                info.OnClienceConnectServerEvent = new ClienceConnectServerComneteventdelegate(this.ClienceConnectServerComnetevent);
                info.OnGetBufBackFunction = new ComnetDataReceiveddelegate(this.GetDataFromSocketServer);
                myclience = new MyClient(ref info);
                try
                {
                    if (myclience.Open())
                    {
                        log.WriteLog("连接服务器成功", textBoxlog);
                 
                    }
                    else
                    {
                        log.WriteLog("连接服务器失败", textBoxlog);
                  
                        MessageBox.Show("连接服务器失败");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("connect to  server fail" + ex.ToString());
                    this.Close();
                }
            }else if(radioButtonasserver.Checked == true)
            {
                info.CorS = netApplyInterfaceChose.Server;
                info.Ip = textBoxIP1.Text;
                info.Port = Convert.ToInt16(textBoxIPCOM1.Text);
                info.OnClienceConnectServerEvent = new ClienceConnectServerComneteventdelegate(this.ClienceConnectServerComnetevent);
                info.OnGetBufBackFunction = new ComnetDataReceiveddelegate(this.GetDataFromSocketServer);
                myserver = new MyServer(ref info);
                try
                {
                   if(myserver.Open())
                    {
                        log.WriteLog("开始监听", textBoxlog);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("server listen fail" + ex.ToString());
                    this.Close();
                }

            }

            //COMPROIO.PortName =  comboBox1.Text;
            //COMPROIO.BaudRate = Convert.ToInt32(comboBox2.Text);
            //(ComworkIO as CommSerial).PortInfo = COMPROIO;
            //(ComworkIO as CommSerial).SetDataReceived(SerialDataReceivedfromiobord);
            //if(ComworkIO.Open())
            //{
            //    log.WriteLog("com IO successful",textBoxlog);
            //    byte[] wrok1_off = System.Text.Encoding.ASCII.GetBytes("i");
            //    ComworkIO.Write(wrok1_off);
            //}
            //else
            //{
            //   MessageBox.Show("com IO fail");
            //  //this.Close();
            //}

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
        private void GetDataFromSocketServer(ref byte[] buf, uint length)
        {
            byte[] pbuf = new byte[length];
            for (int a = 0; a < length; a++)
            {
                pbuf[a] = buf[a];
            }
            string getstring = System.Text.Encoding.ASCII.GetString(pbuf);
            string[] firstanalayarry = getstring.Split(',');
            log.WriteLog("收到数据" + getstring, textBoxlog);      
        }
        /// <summary>
        /// 网络连接动作相应
        /// </summary>
        /// <param name="strdata"></param>
        private void ClienceConnectServerComnetevent(ref string cip, ref string str)
        {

            string ip = cip;
            switch (str)
            {
                case "ClientConnected":
             
                    break;
                case "ClientDisConnected":
              
                    break;
                case "ClientCannotConnecttoserver":

            
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 扫码回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialDataReceivedfromsaoma(object sender, SerialDataReceivedEventArgs e)
        {
         


        }
    
        /// <summary>
        /// IO卡回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialDataReceivedfromiobord(object sender, SerialDataReceivedEventArgs e)
        {
          


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
            if (HgMarkInterface.MarkCardInformationSt.markcard == HgMarkInterface.MarkCardType.markjcz)
            {
                openFileDialog1.Filter = "模板文件(*.ezd)|*.*";
            }
            else if (HgMarkInterface.MarkCardInformationSt.markcard == HgMarkInterface.MarkCardType.markmsi)
            {
                openFileDialog1.Filter = "模板文件(*.mks)|*.*";
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strEzdpathA = openFileDialog1.FileName;
                textBoxmodelpath.Text = strEzdpathA;
                int mksindex = HgMarkInterface.LoadtheModelFile(ref strEzdpathA);
                if (mksindex < 0)
                {
                    MessageBox.Show("加载模板失败");
                    return;                 
                }
                else
                {
                    Configure Configuretem = new Configure();
                    Configuretem.WriteConfig("SystemParam", "EzdModelFilepath", strEzdpathA);
                    HgMarkInterface.DrawModelFileInPicture(ref pictureBoxmodelfileshow, mksindex);
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
            string sorc = string.Empty;
            if(radioButtonasserver.Checked == true)
            {
                sorc = "S";
            }
            else if (radioButtonasclient.Checked == true)
            {
                sorc = "C";
            }
            cf.WriteConfig("SORCSET", "SCCHOSE", sorc);
            cf.WriteConfig("IPSET", "IPADDER1",textBoxIP1.Text);
            cf.WriteConfig("IPSET", "IPCOM1", textBoxIPCOM1.Text);
            MessageBox.Show("OK");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HgMarkInterface.Mark(false,1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
       

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
            if (info.CorS == netApplyInterfaceChose.Server)
            {        
                myserver.Close();
            }
            else if (info.CorS == netApplyInterfaceChose.Clience)
            {        
                myclience.Close();
            }

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            FormSetScreat f = new FormSetScreat();
            f.ShowDialog();
        }
   
        private void button7_Click(object sender, EventArgs e)
        {

       
       
             _workenvent.Set();
            buttonstart.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            buttonstart.Enabled = true;
            HgMarkInterface.StopjczMark();
            _workenvent.Reset();
           
        }
    
       private void buttonmodeldatapath_Click(object sender, EventArgs e)
       {
        

           //OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
           //if (HgMarkInterface.MarkCardInformationSt.markcard == HgMarkInterface.MarkCardType.markjcz)
           //{
           //    openFileDialog1.Filter = "模板文件(*.ezd)|*.*";
           //}
           //else if (HgMarkInterface.MarkCardInformationSt.markcard == HgMarkInterface.MarkCardType.markmsi)
           //{
           //    openFileDialog1.Filter = "模板文件(*.mks)|*.*";
           //}
           //if (openFileDialog1.ShowDialog() == DialogResult.OK)
           //{
           //    string strEzdpathA = openFileDialog1.FileName;
           //    textBoxmodeldatapath.Text = strEzdpathA;
           //    int mksindex = HgMarkInterface.LoadtheModelFile(ref strEzdpathA);
           //    if (mksindex < 0)
           //    {
           //        MessageBox.Show("加载模板失败");
           //        return;
           //    }
           //    else
           //    {
           //        //Configure Configuretem = new Configure();
           //        //Configuretem.WriteConfig("SystemParam", "EzdModelFilepath", strEzdpathA);

           //        HgMarkInterface.DrawModelFileInPicture(ref pictureBoxworkshow, mksindex);
           //    }
           //}
       
        }

   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string[] GetMarkText(string path)
        {
            string[] redline = File.ReadAllLines(path, System.Text.Encoding.Default);//获取文本中每一行数据存在数组中   
            return redline;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
          
        }
        private   void ClienceConnectServerComneteventdelegates(ref string cip, ref string str)
        {
            // string constr = System.Text.Encoding.ASCII.GetString()
            // MessageBox.Show(cip + str);
            log.WriteLog(cip + str, textBoxlog);
        }

        private  void ComnetDataReceiveddelegates(ref byte[] buf, uint length)
        {

            log.WriteLog(System.Text.Encoding.ASCII.GetString(buf), textBoxlog);
        }
      
        private void button7_Click_2(object sender, EventArgs e)
        {
          

        }
        public static Dictionary<string, long> GetFile(string path, Dictionary<string, long> FileList, string RelativePath)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                //int size = Convert.ToInt32(f.Length);
                long size = f.Length;
                FileList.Add(f.FullName, size);//添加文件路径到列表中
            }
            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                GetFile(d.FullName, FileList, RelativePath);
            }
            return FileList;
        }
        private void button8_Click_1(object sender, EventArgs e)
        {
            
        }

        private void buttonftpset_Click(object sender, EventArgs e)
        {
            Configure cf = new Configure();                   

            MessageBox.Show("OK");
        }

        private void pictureBoxworkshow_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
  
        private void buttonLOADTOMODELFILE_Click(object sender, EventArgs e)
        {
    
                
        }  

        private void buttonLOADTOBROTHER_Click(object sender, EventArgs e)
        {
     


        }

        private void textBoxPH_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttongotozeropoint_Click(object sender, EventArgs e)
        {
            HgMarkInterface.AxisCorrectOrigin(0);
            // buttongotozeropoint.Enabled = false;
            //double xy = HgMarkInterface.GetAxisCoor(0);

            //// HgMarkInterface.AxisMoveTo(0,-40);
            //HgMarkInterface.AxisMoveTo(0, -20);
            //HgMarkInterface.AxisMoveTo(0, 10);
        }

        private void buttongotosetpoint_Click(object sender, EventArgs e)
       {
         
       }

        private void buttonstatepoint_Click(object sender, EventArgs e)
        {   
                    
            HgMarkInterface.AxisMoveTo(0, 0);
            HgMarkInterface.AxisMoveTo(0, -15);

        }

        private void button7_Click_3(object sender, EventArgs e)
        {
       
    

        }

        private void buttonSETAXILALPOINT_Click(object sender, EventArgs e)
        {    
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
            HgMarkInterface.Openred();
        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            int m = DateTime.Now.Minute;
            int s = DateTime.Now.Second;
            int ms = DateTime.Now.Millisecond;
            HgMarkInterface.Mark(false, 0);

            int m2 = DateTime.Now.Minute;
            int s2 = DateTime.Now.Second;
            int ms2 = DateTime.Now.Millisecond;
            MessageBox.Show("打印时间=" + ((m2 * 60 * 1000 + s2 * 1000 + ms2) - (m * 60 * 1000 + s * 1000 + ms)).ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            HgMarkInterface.Stopred();
        }

        private void button11_Click(object sender, EventArgs e)
        {
           // double point = Convert.ToDouble(textBoxYpiont.Text);
            HgMarkInterface.AxisMoveTo(0, -1);
        }

        private void button12_Click(object sender, EventArgs e)
        {           

        }

    

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click_4(object sender, EventArgs e)
        {
            Configure cf = new Configure();

            cf.WriteConfig("ODBC", "sourcename", textBoxdatasourcename.Text);
            cf.WriteConfig("ODBC", "loginid", textBoxloginid.Text);
            cf.WriteConfig("ODBC", "password", textBoxpassword.Text);

            MessageBox.Show("OK");
        }

        private void button8_Click_3(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "模板文件(*.txt)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strEzdpathA = openFileDialog1.FileName;
                textBox1.Text = strEzdpathA;


        
           
            }


        }
    }
}
