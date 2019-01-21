using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Timers;

namespace HG.MyJCZ
{
    public class MarkJcz : LoadDll
    {
        private static bool mIsInitLaser = false;
        private static IntPtr mPtr ;
        private static Thread threadInitLaser;
        private static string mMarkEzdFile = null;
        private static LmcErrCode mLastError;
        private static System.Timers.Timer mTimer = null;//= new System.Threading.Timer();


        /// <summary>
        /// 飞行打标
        /// </summary>
        /// <returns></returns>
      public static bool MarkFlyByInIOSignal()
     {
         if (!mIsInitLaser)
         {
             MessageBox.Show("Laser is not initialized");
             return false;
         }
         if (mMarkEzdFile == null)
         {
             MessageBox.Show("model file is not load");
             return false;
         }
         LmcErrCode Ret = LMC1_MarkFlyByStartSignal();
         if (Ret != LmcErrCode.LMC1_ERR_SUCCESS)
         {
             mLastError = Ret;
             return false;
         }
         else
         {
             mLastError = Ret;
             return true;
         }
     }


        /// <summary>
        /// 获取当前错误
        /// </summary>
        /// <returns></returns>
        public static LmcErrCode GetLastError()
        {
            return mLastError;
        }

        /// <summary>
        /// 初始化激光器
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static bool InitLaser(IntPtr hwnd)
        {
            mIsInitLaser = false;
            mPtr = hwnd;
            string strEzCadPath = Application.StartupPath + "\\";
            LmcErrCode Ret = LMC1_INITIAL(strEzCadPath, 0, hwnd);
   
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                mIsInitLaser = true;
                return true;
            }
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 关闭激光器
        /// </summary>
        public static void Close()
        {
            if (mIsInitLaser)
            {
                LmcErrCode Ret = LMC1_CLOSE();
                if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                {
                    mIsInitLaser = false;
                    return;
                }
                else
                {
                    mLastError = Ret;
                    return;
                }
            }
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public static string LoadEzdFile(ref string strFile, bool bDialog = false)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return " ";
            }
            if (bDialog)
            {
                OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                openFileDialog1.Filter = "Ezd(*.ezd)|*.ezd";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    strFile = openFileDialog1.FileName;
                   
                }
                else
                {
                    return " ";
                }
            }
            LmcErrCode Ret = LMC1_LOADEZDFILE(strFile);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                mMarkEzdFile = strFile;
                return strFile;
            }
            else
            {
                mLastError = Ret;
                return " ";
            }
        }

        /// <summary>
        /// 开始标刻
        /// </summary>
        /// <param name="bFlay"></param>
        /// <returns></returns>
        public static bool Mark(bool bFlay = false)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            if (mMarkEzdFile == null)
            {
                MessageBox.Show("model file is not load");
                return false;
            }
            int nFly = 0;
            if (bFlay)
            {
                nFly = 1;
            }
            LmcErrCode Ret = LMC1_MARK(nFly);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        public static bool MarkEntity(string EntityName)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            if (mMarkEzdFile == null)
            {
                MessageBox.Show("model file is not load");
                return false;
            }
            LmcErrCode Ret = LMC1_MARKENTITY(EntityName);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }
        }
        
        /// <summary>
        /// 停止标刻
        /// </summary>
        public static bool StopMark()
        {

            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return  false;
            }
            LmcErrCode Ret = LMC1_STOPMARK();
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false ;
            }
        }

        /// <summary>
        /// 读取端口信号
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        public static bool ReadPort(int nPort)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            int nState = 0;
            LmcErrCode Ret = LMC1_READPORT(ref nState);
            if (LmcErrCode.LMC1_ERR_SUCCESS != Ret)
            {
                mLastError = Ret;
                return false;
            }
            if (((nState >> nPort) & 0x01) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 写入端口信号
        /// </summary>
        /// <param name="nPort"></param>
        /// <param name="bState"></param>
        /// <returns></returns>
        public static bool WritePort(int nPort, bool bState)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            if (nPort < 0 || nPort > 15)
            {
                return false;
            }
            int nState = 0;
            LmcErrCode Ret = LMC1_GETOUTPORT(ref nState);
            if (LmcErrCode.LMC1_ERR_SUCCESS != Ret)
            {
                mLastError = Ret;
                return false;
            }

            int dbuff = 0;
            if (bState)
            {
                dbuff = 0x0001 << nPort;
                nState |= dbuff;
            }
            else
            {
                dbuff = ~(0x0001 << nPort);
                nState &= dbuff;
            }

            Ret = LMC1_WRITEPORT(nState);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }

        }

        /// <summary>
        /// 平移所有对象到相对位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool MoveAllEnt(double x, double y, double angle = 0)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            int nCount = LMC1_GETENTITYCOUNT();
            if (nCount < 1)
            {
                return false;
            }
            for (int i = 0; i < nCount; i++)
            {
                string strName = "";
                if (!GetEntityName(i, ref strName))
                {
                    return false;
                }
                if (!SetEntityName(i, "shanchu"))
                {
                    return false;
                }
                if (angle == 0)
                {
                    double centerx = 0, centery = 0;
                    GetEntCenter("shanchu", ref centerx, ref centery);
                    RotateEnt("shanchu", centerx, centery, angle);
                }
            
                if (!MoveEnt("shanchu", x, y))
                {
                    return false;
                }
                if (!SetEntityName(i, strName))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 显示所有对象名字到ListView中
        /// </summary>
        /// <param name="listView"></param>
        public static void ShowAllEntList(ListView listView)
        {
            int nCount = MarkJcz.GetEntCount();
            if (nCount <= 0)
            {
                return;
            }
            listView.BeginUpdate();  
            listView.Items.Clear();
            for (int i = 0; i < nCount; i++)
            {
                string strEntName = "";
                MarkJcz.GetEntityName(i, ref strEntName);

                // 添加一行  
                ListViewItem lvItem;
                lvItem = new ListViewItem();
                lvItem.Text = (i+1).ToString();
                listView.Items.Add(lvItem);

                // 添加信息  
                ListViewItem.ListViewSubItem lvSubItem;
                lvSubItem = new ListViewItem.ListViewSubItem();
                lvSubItem.Text = strEntName;
                lvItem.SubItems.Add(lvSubItem);
            }
            listView.EndUpdate();
        }
        
        /// <summary>
        /// 根据文本对象名字切换文本内容
        /// </summary>
        /// <param name="strEntName"></param>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static bool ChangeTextByName(ref string strEntName, ref string strText)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            if (mMarkEzdFile == null)
            {
                MessageBox.Show("model file is not load");
                return false;
            }
            LmcErrCode Ret = LMC1_CHANGETEXTBYNAME(strEntName, strText);

            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 显示图片到Picture控件中
        /// </summary>
        /// <param name="pictureBox"></param>
        public static void ShowPreviewBmp(System.Windows.Forms.PictureBox pictureBox)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return ;
            }
            int width = pictureBox.Size.Width;
            int height = pictureBox.Size.Height;
            IntPtr ptr = LMC1_GETPREVBITMAP2(width, height);
            pictureBox.Image = Bitmap.FromHbitmap(ptr);
            DeleteObject(ptr);

            //pictureBox.Invoke((EventHandler)(delegate
            //{
            //    //使用GetPrevBitmap2可以使图像显示出来
            //    IntPtr ptr = LMC1_GETPREVBITMAP2(width, height);
            //    pictureBox.Image = Bitmap.FromHbitmap(ptr);
            //    DeleteObject(ptr);
            //}));
        }

        /// <summary>
        /// 获取所有对象个数
        /// </summary>
        /// <returns></returns>
        public static int GetEntCount()
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return -1;
            }
            return LMC1_GETENTITYCOUNT();
        }

        /// <summary>
        /// 根据索引获取当前文本对象名字
        /// </summary>
        /// <param name="nEntityIndex"></param>
        /// <param name="strEntName"></param>
        /// <returns></returns>
        public static bool GetEntityName(int nEntityIndex, ref string strEntName)
        {
            char[] chEnt = new char[256];
            LmcErrCode Ret = LMC1_GETENTITYNAME(nEntityIndex, chEnt);

            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                strEntName = new string(chEnt);
                strEntName = strEntName.Replace(new String(new Char[] { '\0' }), "");
                return true;
            }
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 根据索引设置对象名字
        /// </summary>
        /// <param name="nEntityIndex"></param>
        /// <param name="strEntName"></param>
        /// <returns></returns>
        public static bool SetEntityName(int nEntityIndex, string strEntName)
        {
            LmcErrCode Ret = LMC1_SETENTITYNAME(nEntityIndex, strEntName);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                SaveEntLibToFile();
                return true;
            }
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 取得实体对象的位置
        /// </summary>
        /// <param name="strEntName"></param>
        /// <param name="dMinx"></param>
        /// <param name="dMiny"></param>
        /// <param name="dMaxx"></param>
        /// <param name="dMaxy"></param>
        /// <param name="dZ"></param>
        /// <returns></returns>
        public static bool GetEntSize(string strEntName, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref double dZ)
        {
            LmcErrCode Ret = LMC1_GETENTSIZE(strEntName, ref dMinx, ref dMiny, ref dMaxx, ref dMaxy, ref dZ);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 根据对象名字获取中心坐标
        /// </summary>
        /// <param name="strEntName"></param>
        /// <param name="dCenx"></param>
        /// <param name="dCeny"></param>
        /// <returns></returns>
        public static bool GetEntCenter(string strEntName, ref double dCenx, ref double dCeny)
        {
            dCenx = 0; dCeny = 0;
            double minx = 0, miny = 0, maxx = 0, maxy = 0, rot = 0;
            if (!GetEntSize(strEntName, ref minx, ref miny, ref maxx, ref maxy, ref rot))
            {
                return false;
            }
            dCenx = (maxx + minx) / 2;
            dCeny = (maxy + miny) / 2;
            return true;
        }

        public static bool GetEntCenter(string strEntName, ref double dCenx, ref double dCeny, ref double dWidth, ref double dHeight)
        {
            dCenx = 0; dCeny = 0;
            double minx = 0, miny = 0, maxx = 0, maxy = 0, z = 0;
            if (!GetEntSize(strEntName, ref minx, ref miny, ref maxx, ref maxy, ref z))
            {
                return false;
            }
            dCenx = (maxx + minx) / 2;
            dCeny = (maxy + miny) / 2;
            dWidth = maxx - minx;
            dHeight = maxy - miny;
            return true;
        }

        public static bool SetEntCenter(string strEntName, double dCenx, double dCeny, double dWidth, double dHeight)
        {
            double dx = 0, dy = 0;
            double minx = 0, miny = 0, maxx = 0, maxy = 0, z = 0;
            if (!GetEntSize(strEntName, ref minx, ref miny, ref maxx, ref maxy, ref z))
            {
                return false;
            }
            dx = (maxx + minx) / 2;
            dy = (maxy + miny) / 2;

            dCenx -= dx;
            dCeny -= dy;
            if (!MoveEnt(strEntName, dCenx, dCeny))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据对象名字旋转对象
        /// </summary>
        /// <param name="strEntName"></param>
        /// <param name="dCenx"></param>
        /// <param name="dCeny"></param>
        /// <param name="dAngle"></param>
        /// <returns></returns>
        public static bool RotateEnt(string strEntName, double dCenx, double dCeny, double dAngle)
        {
            LmcErrCode Ret = LMC1_ROTATEENT(strEntName, dCenx, dCeny, dAngle);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 平移实体对象
        /// </summary>
        /// <param name="pEntName"></param>
        /// <param name="dMovex"></param>
        /// <param name="dMovey"></param>
        /// <returns></returns>
        public static bool MoveEnt(string pEntName, double dMovex, double dMovey)
        {
            LmcErrCode Ret = LMC1_MOVEENT(pEntName, dMovex, dMovey);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        public static bool DeleteEnt(int index)
        {
            int nCount = GetEntCount();
            if (nCount > index)
            {
                SetEntityName(index, "shanchu");
                DeleteEnt("shanchu");
                SaveEntLibToFile();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除指定对象名字
        /// </summary>
        /// <param name="srEntName"></param>
        /// <returns></returns>
        private static bool DeleteEnt(string strEntName)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            LmcErrCode Ret = lmc1_DeleteEnt(strEntName);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                return true;
            }
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 弹出设置参数对话框
        /// </summary>
        public static void SetConfigForm()
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return ;
            }
            LmcErrCode Ret = LMC1_SETDEVCFG();
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                return ;
            }
            else
            {
                mLastError = Ret;
                return ;
            }
        }

        /// <summary>
        /// 根据笔号获取所有
        /// </summary>
        /// <param name="nPenNo"></param>
        /// <param name="nMarkLoop"></param>
        /// <param name="dMarkSpeed"></param>
        /// <param name="dPowerRatio"></param>
        /// <param name="dCurrent"></param>
        /// <param name="nFreq"></param>
        /// <param name="dQPulseWidth"></param>
        /// <param name="nStartTC"></param>
        /// <param name="nLaserOffTC"></param>
        /// <param name="nEndTC"></param>
        /// <param name="nPolyTC"></param>
        /// <param name="dJumpSpeed"></param>
        /// <param name="nJumpPosTC"></param>
        /// <param name="nJumpDistTC"></param>
        /// <param name="dEndComp"></param>
        /// <param name="dAccDist"></param>
        /// <param name="dPointTime"></param>
        /// <param name="bPulsePointMode"></param>
        /// <param name="nPulseNum"></param>
        /// <param name="dFlySpeed"></param>
        /// <returns></returns>
        public static bool GetPenParam(int nPenNo,//要设置的笔号(0-255)					 
            ref int nMarkLoop,//加工次数
            ref double dMarkSpeed,//标刻次数mm/s
            ref double dPowerRatio,//功率百分比(0-100%)	
            ref double dCurrent,//电流A
            ref int nFreq,//频率HZ
            ref double dQPulseWidth,//Q脉冲宽度us	
            ref int nStartTC,//开始延时us
            ref int nLaserOffTC,//激光关闭延时us 
            ref int nEndTC,//结束延时us
            ref int nPolyTC,//拐角延时us   //	
            ref double dJumpSpeed, //跳转速度mm/s
            ref int nJumpPosTC, //跳转位置延时us
            ref int nJumpDistTC,//跳转距离延时us	
            ref double dEndComp,//末点补偿mm
            ref double dAccDist,//加速距离mm	
            ref double dPointTime,//打点延时 ms						 
            ref bool bPulsePointMode,//脉冲点模式 
            ref int nPulseNum,//脉冲点数目
            ref double dFlySpeed)//流水线速度
        {
            LmcErrCode Ret = lmc1_GetPenParam(nPenNo, ref nMarkLoop,//加工次数
             ref dMarkSpeed,//标刻次数mm/s
             ref dPowerRatio,//功率百分比(0-100%)	
             ref dCurrent,//电流A
                ref nFreq,//频率HZ
            ref dQPulseWidth,//Q脉冲宽度us	
             ref nStartTC,//开始延时us
             ref nLaserOffTC,//激光关闭延时us 
                ref nEndTC,//结束延时us
             ref nPolyTC,//拐角延时us   //	
             ref dJumpSpeed, //跳转速度mm/s
             ref nJumpPosTC, //跳转位置延时us
            ref  nJumpDistTC,//跳转距离延时us	
             ref  dEndComp,//末点补偿mm
             ref  dAccDist,//加速距离mm	
             ref  dPointTime,//打点延时 ms						 
            ref  bPulsePointMode,//脉冲点模式 
            ref  nPulseNum,//脉冲点数目
             ref  dFlySpeed);//流水线速度

            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
                return true;
            else
            {
                mLastError = Ret;
                return false;
            }
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool SaveEntLibToFile(string fileName=null)
        {

            if (!mIsInitLaser)
            {
                mLastError = LmcErrCode.LMC1_ERR_NOINITIAL;
                return false;
            }
            if (fileName == null)
            {
                fileName = mMarkEzdFile;
            }
            LmcErrCode Ret = lmc1_SaveEntLibToFile(fileName);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                return true;
            }
            else
            {
                mLastError = Ret;
                return false;
            }


        }

        /// <summary>
        /// 定时器事件，触发红光
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void StartRedLight(Object source, ElapsedEventArgs e)
        {
            if (!mIsInitLaser)
            {
                mLastError = LmcErrCode.LMC1_ERR_NOINITIAL;
                return;
            }
            LmcErrCode Ret = LMC1_REDLIGHTMARK();
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                return;
            }
            else
            {
                mLastError = Ret;
                return;
            }

        }

        /// <summary>
        /// 开始红光
        /// </summary>
        /// <param name="nMs"></param>
        public static bool StartRed(int nMs = 100)
        {
            if (!mIsInitLaser)
            {
                MessageBox.Show("Laser is not initialized");
                return false;
            }
            if (mMarkEzdFile == null)
            {
                MessageBox.Show("model file is not load");
                return false;
            }
            if (mTimer != null)
            {
                StopRed();
            }
            mTimer = new System.Timers.Timer();

            // Hook up the Elapsed event for the timer.
            mTimer.Elapsed += new ElapsedEventHandler(StartRedLight);

            // Set the Interval to 2 seconds (2000 milliseconds).
            mTimer.Interval = nMs;
            mTimer.Enabled = true;
            mTimer.Start();
            return true;
        }

        /// <summary>
        /// 停止红光
        /// </summary>
        public static void StopRed()
        {
            if (mTimer != null)
            {
                mTimer.Stop();
                mTimer.Elapsed -= new ElapsedEventHandler(StartRedLight);
                mTimer.Dispose();
                mTimer = null;
            }
        }

     

        /// <summary>
        /// 编辑线程
        /// </summary>
        private static void threadInitHglaser()
        {
            Close();
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.CreateNoWindow = true;//设不显示窗口
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = Application.StartupPath + @"\HGLaser.exe";
            process.StartInfo.Arguments = mMarkEzdFile;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            try
            {
                process.Start();
            }
            catch (System.ComponentModel.Win32Exception we)
            {
                process.Dispose();
                MessageBox.Show(we.Message);
                return ;
            }
            process.WaitForExit();
            process.Dispose();
            if (!InitLaser(mPtr))
            {
                MessageBox.Show("Laser is not initialized!");
                return ;
            }
            return ;
        }

        /// <summary>
        /// 编辑Ezd模板，弹出Form
        /// </summary>
        /// <param name="mFilePath"></param>
        /// <returns></returns>
        public static bool EditMarkEzdForm(string strFilePath)
        {
            if (threadInitLaser != null)
            {
                //判断当前线程是否结束
                if (threadInitLaser.IsAlive)
                {
                    return false;
                }
            }
            if (!File.Exists(Application.StartupPath + "\\" + "HGLaser.exe"))
            {
                MessageBox.Show("The specified file does not exist");
                return false;
            }
            if (!File.Exists(strFilePath))
            {
                MessageBox.Show("The specified file does not exist");
                return false;
            }
            LoadEzdFile(ref strFilePath);
            mMarkEzdFile = strFilePath;
            threadInitLaser = new Thread(threadInitHglaser);
            threadInitLaser.Start();
            return true;
        }

        /// <summary>
        /// 根据索引获取当前数据信息
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="dSetPower"></param>
        /// <param name="nSetRate"></param>
        /// <param name="dSetCurrent"></param>
        /// <returns></returns>
        public static bool GetParamByName(int nIndex, ref double dPowerRatio, ref int nFreq, ref double dCurrent)
        {
            string strEntName = null;
            if (!GetEntityName(nIndex, ref strEntName))
            {
                return false;
            }
            if (!SetEntityName(nIndex, "shanchu"))
            {
                return false;
            }
            int nPen = lmc1_GetPenNumberFromEnt("shanchu");
            if (!SetEntityName(nIndex, strEntName))
            {
                return false;
            }
            if (nPen < 0)
            {
                return false;
            }
            int nMarkLoop = 0;//加工次数
            double dMarkSpeed = 0; ;//标刻次数mm/s
            //double dPowerRatio = 0;//功率百分比(0-100%)	
            //double dCurrent = 0;//电流A
            //int nFreq = 0;//频率HZ
            double dQPulseWidth = 0;//Q脉冲宽度us	
            int nStartTC = 0;//开始延时us
            int nLaserOffTC = 0;//激光关闭延时us 
            int nEndTC = 0;//结束延时us
            int nPolyTC = 0;//拐角延时us   //	
            double dJumpSpeed = 0; //跳转速度mm/s
            int nJumpPosTC = 0; //跳转位置延时us
            int nJumpDistTC = 0;//跳转距离延时us	
            double dEndComp = 0;//末点补偿mm
            double dAccDist = 0;//加速距离mm	
            double dPointTime = 0;//打点延时 ms						 
            bool bPulsePointMode = false;//脉冲点模式 
            int nPulseNum = 0;//脉冲点数目
            double dFlySpeed = 0;//流水线速度
            lmc1_GetPenParam(nPen, ref nMarkLoop, ref dMarkSpeed, ref dPowerRatio, ref dCurrent, ref nFreq, ref dQPulseWidth,
                ref nStartTC, ref nLaserOffTC, ref nEndTC, ref nPolyTC, ref dJumpSpeed, ref nJumpPosTC, ref nJumpDistTC, ref dEndComp
                , ref dAccDist, ref dPointTime, ref bPulsePointMode, ref nPulseNum, ref dFlySpeed);
            return true;
        }

        /// <summary>
        /// 根据对象索引设置功率，频率，电流
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="dSetPower"></param>
        /// <param name="nSetRate"></param>
        /// <param name="dSetCurrent"></param>
        /// <returns></returns>
        public static bool SetParamByName(int nIndex, double dSetPower, int nSetRate, double dSetCurrent)
        {
            if (dSetPower < 0 || dSetPower > 100)
            {
                MessageBox.Show("Power over the actual range");
                return false;
            }
            string strEntName = null;
            if (!GetEntityName(nIndex, ref strEntName))
            {
                return false;
            }
            if (!SetEntityName(nIndex, "shanchu"))
            {
                return false;
            }

            int nPen = lmc1_GetPenNumberFromEnt("shanchu");
            if (!SetEntityName(nIndex, strEntName))
            {
                return false;
            }
            if (nPen < 0)
            {
                return false;
            }
            int nMarkLoop = 0;//加工次数
            double dMarkSpeed = 0; ;//标刻次数mm/s
            double dPowerRatio = 0;//功率百分比(0-100%)	
            double dCurrent = 0;//电流A
            int nFreq = 0;//频率HZ
            double dQPulseWidth = 0;//Q脉冲宽度us	
            int nStartTC = 0;//开始延时us
            int nLaserOffTC = 0;//激光关闭延时us 
            int nEndTC = 0;//结束延时us
            int nPolyTC = 0;//拐角延时us   //	
            double dJumpSpeed = 0; //跳转速度mm/s
            int nJumpPosTC = 0; //跳转位置延时us
            int nJumpDistTC = 0;//跳转距离延时us	
            double dEndComp = 0;//末点补偿mm
            double dAccDist = 0;//加速距离mm	
            double dPointTime = 0;//打点延时 ms						 
            bool bPulsePointMode = false;//脉冲点模式 
            int nPulseNum = 0;//脉冲点数目
            double dFlySpeed = 0;//流水线速度
            lmc1_GetPenParam(nPen, ref nMarkLoop, ref dMarkSpeed, ref dPowerRatio, ref dCurrent, ref nFreq, ref dQPulseWidth,
                ref nStartTC, ref nLaserOffTC, ref nEndTC, ref nPolyTC, ref dJumpSpeed, ref nJumpPosTC, ref nJumpDistTC, ref dEndComp
                , ref dAccDist, ref dPointTime, ref bPulsePointMode, ref nPulseNum, ref dFlySpeed);

            lmc1_SetPenParam(nPen, nMarkLoop, dMarkSpeed, dSetPower, dSetCurrent, nSetRate, dQPulseWidth,
                 nStartTC, nLaserOffTC, nEndTC, nPolyTC, dJumpSpeed, nJumpPosTC, nJumpDistTC, dEndComp
                , dAccDist, dPointTime, bPulsePointMode, nPulseNum, dFlySpeed);

            return true;
        }

        public static bool GetParam(int nIndex, ref double x, ref double y, ref double w, ref double h, ref double a, ref int b, ref double c)
        {
            string strName = null;
            if (!GetEntityName(nIndex, ref strName))
            {
                return false;
            }
            if (!SetEntityName(nIndex, "shanchu"))
            {
                return false;
            }
            GetEntCenter("shanchu", ref x, ref y, ref w, ref h);
            if (!SetEntityName(nIndex, strName))
            {
                return false;
            }
            if (!GetParamByName(nIndex, ref a, ref b, ref c))
            {
                return false;
            }
            return true;
        }

        public static bool SetParam(int nIndex, double x, double y, double w, double h, double a, int b, double c)
        {
            string strName = null;
            if (!GetEntityName(nIndex, ref strName))
            {
                return false;
            }
            if (!SetEntityName(nIndex, "shanchu"))
            {
                return false;
            }
            SetEntCenter("shanchu", x, y, w, h);
            if (!SetEntityName(nIndex, strName))
            {
                return false;
            }
            if (!SetParamByName(nIndex, a, b, c))
            {
                return false;
            }
            return true;
        }
        //2016.6.15 0.12
        //public static bool axis..........(int axis, double GoalPos)
        /// <summary>
        /// 移动扩展轴
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="GoalPos"></param>
        /// <returns></returns>
        public static bool AxisMoveTo(int axis, double GoalPos)
        {
            LmcErrCode Ret = lmc1_AxisMoveTo(axis, GoalPos);
            if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
            {
                return true;
            }
            else
            {
                mLastError = Ret;
                return false;
            }

        }
        //复位扩展轴
        public static bool AxisReset(bool b0, bool b1)
        {
            LmcErrCode Ret = lmc1_Reset(b0, b1);
            return true;
        }
        //矫正扩展轴
        public static bool AxisCorrectOrigin(int axis)
        {
            LmcErrCode Ret =lmc1_AxisCorrectOrigin(axis); 
            return true;
        }
        //得到当前扩展轴坐标
        public static double GetAxisCoor(int axis)//
        {
            double dPos = lmc1_GetAxisCoor(axis);
            return dPos;
        }
        //
        public static int GetAxisCoorPulse(int axis)
        {
            int iPos  =lmc1_GetAxisCoorPulse(axis);
            return iPos;
        }
        public static bool AxisMoveToPulse(int axis, int nGaolPos)
        {
            lmc1_AxisMoveToPulse(axis,nGaolPos);
            return true;
        }


        


        //public static bool AxisMoveTo(int axis, double GoalPos)
        //{
        //    LmcErrCode Ret = lmc1_AxisMoveTo(axis, GoalPos);
        //    if (LmcErrCode.LMC1_ERR_SUCCESS == Ret)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        mLastError = Ret;
        //        return false;
        //    }

        //}
        
      
    }
}
