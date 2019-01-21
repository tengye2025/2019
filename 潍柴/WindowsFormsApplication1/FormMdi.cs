#region 命名空间引用
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.Register;
using HG.Configure;
using HG.MyJCZ;
using HG.LogRecord;
using System.Management;
using HG.Communication;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Timers;
//using HGMark.OPCUAClassLibrary;
//using ModbusClassLibrary;
using WindowsFormsApplication1.MarkInterface;
using CommNetSocketClass;
using FtpClass;
using System.Data.SqlClient;
using System.Data.Sql;
using ModbusClassLibrary;
#endregion
namespace WindowsFormsApplication1
{
    public struct MdiReferenceObjST
    {
      
    }
    public partial class FormMidContains : Form
    {
        private object _markedlistlock = new object();
        private object _iooutlock = new object();

        private ManualResetEvent _workenvent = new ManualResetEvent(false);

     // private MdiReferenceObjST mdiobjtosonfroms = new MdiReferenceObjST();

        public FormMidContains()
        {
            InitializeComponent();
          
        }
        #region 界面选择按钮回调
        private void GetChoseFormIndex(byte a)
        {
            byte nowshowindex = leftButtons.GetClickedButtonIndex;
            childrenformsarry[nowshowindex].Hide();
            childrenformsarry[a].Show();
        }
        #endregion
        private void SystemInit()
        {
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
                    Thread.Sleep(2);
                    int addp = (int)(0.29 * a);
                    SetProcessbarvalue(70 + addp);
                }
                CommunicationIint(false,false);   //通信
                CreatWorkThread(false);
                Thread.Sleep(50);
                SetProcessbarvalue(100);
                //   Thread.Sleep(1000);
                //   timer.Elapsed += new System.Timers.ElapsedEventHandler(Timerevent);
                //   timer.AutoReset = true;
                //   timer.Enabled = true;
            }
            else
            {
                this.Close();
            }    
        }
        /// <summary>
        /// 初始化系统设置界面
        /// </summary>
        private void INITWINDOWSFORM()
        {
          
        }
        #region 初始化控制卡
        /// <summary>
        /// 初始化控制卡
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

            if (HgMarkInterface.InitLaser() == HgMarkInterface.Lasererr.initfail)//初始化激光器
            {
              
                   if( mst.markcard == HgMarkInterface.MarkCardType.markjcz)
                  {
                      toolStripStatusLabelmarkcardstaus.Text = "JCZFIAL";
                  }
                  else if (mst.markcard == HgMarkInterface.MarkCardType.markmsi)
                  {
                      toolStripStatusLabelmarkcardstaus.Text = "MSIFIAL";
                  }
                 toolStripStatusLabelmarkcardstaus.BackColor = Color.Red; 
                MessageBox.Show("初始化控制卡失败");
                return;
            }
            else
            {
                  if( mst.markcard == HgMarkInterface.MarkCardType.markjcz)
                  {
                      toolStripStatusLabelmarkcardstaus.Text = "JCZOK";
                  }
                  else if (mst.markcard == HgMarkInterface.MarkCardType.markmsi)
                  {
                      toolStripStatusLabelmarkcardstaus.Text = "MSIOK";
                  }
            }
           
            string pathstr = string.Empty;
            if (mst.markcard == HgMarkInterface.MarkCardType.markjcz)
            {
                pathstr = Application.StartupPath + "\\" + "AUTOSAVE.ezd";
                if (!File.Exists(pathstr))
                {
                    MessageBox.Show("不存在" + pathstr);
                    return;
                }
                else
                {

                }
            }
            else   if (mst.markcard == HgMarkInterface.MarkCardType.markmsi)
            {            
                pathstr = Application.StartupPath + "\\" + "AUTOSAVE.msi";
                if (!File.Exists(pathstr))
                {
                    MessageBox.Show("不存在" + pathstr);
                    return;
                }
                else
                {

                }
            }
            int mmksfileindex = -1;
            mmksfileindex = HgMarkInterface.LoadtheModelFile(ref pathstr);
            if (mmksfileindex != -1)
            {
                toolStripStatusLabelmodelstaus.Text = "MODELFILELOADOK";

            }
            else
            {
                toolStripStatusLabelmodelstaus.Text = "MODELFILELOADFAIL";
                toolStripStatusLabelmodelstaus.BackColor = Color.Red;
            }
            HgMarkInterface.Resetaxis(true, true);

            if (mst.markcard == HgMarkInterface.MarkCardType.markmsi)
            {
                if (HgMarkInterface.MsiSetPreseveMode(1) == HgMarkInterface.Lasererr.setpresevemodefail)
                {
                    MessageBox.Show("MSI设置预存模式失败");
                    return;
                }
                else
                {
                    toolStripStatusLabelprestorestaus.Text = "OK";
                }
            }
            else
            {

            }

        }

        #endregion
        #region 使用权限
        private void StopWork()
        {
            HgMarkInterface.CloseLaser(1);
            MessageBox.Show("Stop Working", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
        #region 通信初始化
        private MyClient myclience = null;
        private MyServer myserver = null;
        private CommInterface mycom = null;
        private SerialPortInfo mycominfo = null;
        private   NetInterfaceinfo info = new NetInterfaceinfo();
        /// <summary>
        /// 通信初始化
        /// </summary>
        private void CommunicationIint(bool netenable,bool comenable)
        {
            if (netenable)
            {


                if (false)
                {
                    info.CorS = netApplyInterfaceChose.Clience;
                    info.Ip = "192.168.10.2";
                    info.Port = 502;
                    info.OnClienceConnectServerEvent = new ClienceConnectServerComneteventdelegate(this.ClienceConnectServerComnetevent);
                    info.OnGetBufBackFunction = new ComnetDataReceiveddelegate(this.GetDataFromSocketServer);
                    myclience = new MyClient(ref info);
                    try
                    {
                        if (myclience.Open())
                        {

                        }
                        else
                        {
                            MessageBox.Show("连接PLC服务器失败");
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("connect to PLC server fail" + ex.ToString());
                        this.Close();
                    }

                }
                else if (false)
                {
                    info.CorS = netApplyInterfaceChose.Server;
                    info.Ip = "";
                    info.Port = 502;
                    info.OnClienceConnectServerEvent = new ClienceConnectServerComneteventdelegate(this.ClienceConnectServerComnetevent);
                    info.OnGetBufBackFunction = new ComnetDataReceiveddelegate(this.GetDataFromSocketServer);
                    myserver = new MyServer(ref info);
                    try
                    {
                        if (myserver.Open())
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("server listen fail" + ex.ToString());
                        toolStripStatusLabelnetstaus.Text = "ServerListenFail";
                        toolStripStatusLabelnetstaus.BackColor = Color.Red;
                     
                    }
                }

            }
            else
            {
                toolStripStatusLabelnetstaus.Text = "null";
            }

            if (comenable)
            {


                mycominfo.PortName = "";
                mycominfo.BaudRate = 9600;

                (mycom as CommSerial).PortInfo = mycominfo;
                (mycom as CommSerial).SetDataReceived(SerialDataReceivedfromiobord);
                if (mycom.Open())
                {
                    toolStripsystemcontrlcomstasus.Text = "COMOK";
                }
                else
                {
                    MessageBox.Show("com  fail");
                    toolStripsystemcontrlcomstasus.Text = "COMFAIL";
                    return;
                }
            }
            else
            {
                toolStripsystemcontrlcomstasus.Text = "null";              
            }
      

        }
#endregion
        #region 通信回调函数
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
            string[] firstanalayarry = getstring.Split('-');

        }
        /// <summary>
        /// 网络连接动作相应
        /// </summary>
        /// <param name="strdata"></param>
        private void ClienceConnectServerComnetevent(ref string cip, ref string str)
        {
            string clenceip = cip;
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
        #endregion
        #region 创造工作线程
        private void CreatWorkThread(bool createnable)
        {
            if (createnable)
            {
                Thread myThread = new Thread(new ParameterizedThreadStart(OnworkFrameTreadproc));
                myThread.IsBackground = true;
                myThread.Start(this);
            }
            else
            {

            }
        }



        private void OnworkFrameTreadproc(object p)
        {
            while (true)
            {
                _workenvent.WaitOne();
                if (HgMarkInterface.ReadIO(9))
                {
               
                    HgMarkInterface.Mark(false, 0);
                }
                Thread.Sleep(10);
            }

        }
        #endregion
        #region 主界面关闭
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMidContains_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (info.CorS == netApplyInterfaceChose.Server)
            {
                myserver.Close();
            }
            else if (info.CorS == netApplyInterfaceChose.Clience)
            {
                
                myclience.Close();
            }
            m.ComClose();
        }
    #endregion

        private Form[] childrenformsarry = new Form[3];
        private void FormMidContains_Load(object sender, EventArgs e)
        {
            leftButtons.SetAutoBackClickedButtonIndexFunction(GetChoseFormIndex);
            int xw = this.ClientSize.Width;
            int yh = this.ClientSize.Height;
            paneltitle.Location = new System.Drawing.Point(0, 0);
            paneltitle.Size = new System.Drawing.Size(400, 40);

            panelsystemcontrlcomstasus.Location = new Point(400, 0);
            panelsystemcontrlcomstasus.Size = new System.Drawing.Size(xw - 400 + 3, 40);

            leftButtons.Location = new Point(0, 40);
            leftButtons.Size = new Size(98, yh - 40);

            statusStrip.Location = new Point(98, yh - 30);
            statusStrip.Size = new Size(xw - 98, 30);

            panel1.Location = new Point(99, 41);
            panel1.Size = new Size(xw - 98 - 2, yh - 30 - 40 - 2);
//formwork
            Form fwork = new FormWork();
            fwork.MdiParent = this;
            (fwork as FormWork).fmdi = this;
            fwork.FormBorderStyle = FormBorderStyle.None;
            fwork.ControlBox = false;
            fwork.Parent = this.panel1;
            fwork.Dock = DockStyle.Fill;
            fwork.BackColor = Color.White;
            this.LayoutMdi(MdiLayout.ArrangeIcons);
            childrenformsarry[0] = fwork;
//formdebu

            Form fdebug = new FormDebug();
            fdebug.MdiParent = this;
            (fdebug as FormDebug).fmdi = this;
            fdebug.FormBorderStyle = FormBorderStyle.None;
            fdebug.ControlBox = false;
            fdebug.Parent = this.panel1;
            fdebug.Dock = DockStyle.Fill;
            fdebug.BackColor = Color.White;
            this.LayoutMdi(MdiLayout.ArrangeIcons);
            childrenformsarry[1] = fdebug;
//formabout

            Form fabout = new FormExploitationInformation();
            fabout.MdiParent = this;

            fabout.FormBorderStyle = FormBorderStyle.None;
            fabout.ControlBox = false;
            fabout.Parent = this.panel1;
            fabout.Dock = DockStyle.Fill;
            fabout.BackColor = Color.White;
            this.LayoutMdi(MdiLayout.ArrangeIcons);
            childrenformsarry[2] = fabout;

            SystemInit();
           // Tcpmodbusset();
       
            leftButtons.SetShowFOrmIndex(0);

        }
        public void Tcpmodbusset()
        {         

                  ModbusClassLibrary.CommunicationCom.ModbusNetApplyInterfaceChose typse = ModbusClassLibrary.CommunicationCom.ModbusNetApplyInterfaceChose.Clience;          
                  m = new TCPModbusClass(ref typse);
                  if (m.ComOpen())
                  {
                      toolStripLabelplcstaus.Text = "SIEMENS PLC CONNECTED";
                  }
                  else
                  {
                      toolStripLabelplcstaus.Text = "SIEMENS PLC DISCONNECTED";
                      toolStripLabelplcstaus.BackColor = Color.Red;
                      MessageBox.Show("PLC链接失败");
                  }

        }
        public ModbusClassLibrary.TCPModbusClass m;

       
    }
}
