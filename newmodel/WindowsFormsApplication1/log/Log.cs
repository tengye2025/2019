using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Threading;


namespace HG.LogRecord
{
    
    public class Log
    {
        private string mPathName = Application.StartupPath + "\\" + "log";
        private int mSum = 1;

        /// <summary>
        /// 设置日志存放路径
        /// </summary>
        public string PathName
        {
            set
            {
                mPathName = Application.StartupPath + "\\" + value; 
            }
        }

        public Log()
        {
            if(Directory.Exists(mPathName))
            {

            }else
            {
                Directory.CreateDirectory(mPathName);
            }
            Thread myThread = new Thread(new ParameterizedThreadStart(printflog));
            myThread.IsBackground = true;
            myThread.Start(this); 
        }
        private object _plock = new object();

        private void printflog(object p)
        {
            while (true)
            {
                StringBuilder strData = null;
                TextBox textboxlog = null;
                bool iscount = false;
                lock (_plock)
                {
              
                if (printstring.Count > 0)
                {
                    iscount = true;
                    strData = printstring[0].strb;
                    textboxlog = printstring[0].textboxs;
                    printstring.RemoveAt(0);
                }
                else
                {
                    iscount = false;
                }

               }
                if (iscount)
                {            
                    StringBuilder strFile = new StringBuilder();
                    strFile.AppendFormat("{0}\\{1}\\{2}\\", mPathName, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                    if (!Directory.Exists(strFile.ToString()))
                    {
                        Directory.CreateDirectory(strFile.ToString());
                    }
                    //将数据写入文件中
                    strFile.Append(DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                    using (StreamWriter swAppend = File.AppendText(strFile.ToString()))
                    {
                        swAppend.WriteLine(Convert.ToString(strData));
                        swAppend.Flush();
                        swAppend.Close();
                    }
                    mSum++;
                    if (textboxlog != null)
                    {
                        textboxlog.Invoke((EventHandler)(delegate
                        {
                            textboxlog.AppendText(Convert.ToString(strData) + "\r\n");
                        }));
                    }
                }
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strFile"></param>
        /// <param name="strData"></param>
        public void WriteLog(string strData, ref bool create)
        {
            //创建文件夹
            StringBuilder strFile = new StringBuilder();
            strFile.AppendFormat("{0}\\{1}\\{2}\\", mPathName, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            if (!Directory.Exists(strFile.ToString()))
            {
                Directory.CreateDirectory(strFile.ToString());
                create = true;
            }
            //将数据写入文件中
            strFile.Append(DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            using (StreamWriter swAppend = File.AppendText(strFile.ToString()))
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("[{0}][{1}][{2}]{3}", mSum, DateTime.Now, DateTime.Now.Millisecond.ToString("d4"), strData);
                swAppend.WriteLine(Convert.ToString(str));
                swAppend.Flush();
                swAppend.Close();
            }
            mSum++;

        }
         private  struct printlogST
        {
            public StringBuilder strb;
            public TextBox textboxs;

        }
         private List<printlogST> printstring = new List<printlogST>();
        private static readonly object obj_Log = new object();//日志文件读写同步锁
        public void WriteLog(string strData,TextBox textbox1)
        {
            try
            {
                Monitor.Enter(obj_Log);
                printlogST prt = new printlogST();
                prt.textboxs = textbox1;
                mSum++;
                StringBuilder str = new StringBuilder();
                str.AppendFormat("[{0}][{1}][{2}]{3}", mSum, DateTime.Now, DateTime.Now.Millisecond.ToString("d4"), strData);
                prt.strb = str;
                lock (_plock)
                {
                    printstring.Add(prt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            finally
            {
                Monitor.Exit(obj_Log);
            }
        }

    }
}
