using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Management;
namespace WindowsFormsApplication1.Register
{
    sealed class ClassRegisterUserAuthority
    {
      public  enum GetUserCPUIDWithAuthorityResult
        {
             CheckCPUIDOK,
             CheckCPUIdFail,
             CheckCPUIdCancel
        }

      public enum GetUsertimesWithAuthorityResult
      {
          CheckusertimesOK,
          CheckusertimesFail,
          CheckusertimesCancel
      }
        public  ClassRegisterUserAuthority(object p)
        {      
     
        }
        public delegate void Stop();
        /// <summary>
        /// 获取客户使用时间权限
        /// </summary>
       static public GetUsertimesWithAuthorityResult GetUsertimesWithAuthority(Stop p)
       {
           try
           {
               string cPath = System.AppDomain.CurrentDomain.BaseDirectory;
               string[] files = Directory.GetFiles(cPath);
               bool ishavebat = false;
               foreach (string file in files)//循环文件
               {
                   if (file == cPath+"win32A2S.bat")//
                   {                   
                       ishavebat = true;
                       break;
                   }
               }
               if (ishavebat)
               {
                   FileStream readfile = new FileStream(cPath+"win32A2S.bat", FileMode.Open, FileAccess.Read);
                   BinaryReader r = new BinaryReader(readfile);
                   string screatcompany = r.ReadString();
                   Int32 settime = r.ReadInt32();           
                   Int32 usedtime = r.ReadInt32();
                   r.Close();
                   readfile.Close();
                   if (screatcompany == "jshglaser2025")
                   {
                       if (usedtime >= settime)
                       {
                           p();                
                           FormUserTimesAthority f = new FormUserTimesAthority();
                           f.label1.Text = "times register";
                           if (DialogResult.OK == f.ShowDialog())
                           {
                               string writescreat = f.textBox1.Text;
                               int m_settimes = Convert.ToInt32(writescreat.Substring(writescreat.LastIndexOf(".") + 1));
                               FileStream writefile = new FileStream(cPath + "win32A2S.bat", FileMode.Open, FileAccess.Write);
                               BinaryWriter w = new BinaryWriter(writefile);
                               w.Write("jshglaser2025");
                               w.Write(m_settimes);
                               w.Write(0);
                               w.Close();
                               writefile.Close();
                           }
                           else
                           {
                               return GetUsertimesWithAuthorityResult.CheckusertimesCancel;
                           }
                       }
                       else
                       {
                           FileStream writefile = new FileStream(cPath + "win32A2S.bat", FileMode.Open, FileAccess.Write);
                           BinaryWriter w = new BinaryWriter(writefile);

                           w.Write("jshglaser2025");
                           w.Write(settime);
                           usedtime++;
                           w.Write(usedtime);
                           w.Close();
                           writefile.Close();
                           return GetUsertimesWithAuthorityResult.CheckusertimesOK;
                       }
                   }
                   else
                   {        
                       MessageBox.Show("not correct bat register file", "err", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                   }
               }
               else
               {
                   FileStream writefile = new FileStream(cPath + "win32A2S.bat", FileMode.OpenOrCreate, FileAccess.Write);
                   BinaryWriter w = new BinaryWriter(writefile);
                   w.Write("jshglaser2025");
                   Int32 settime = 0;
                   w.Write(settime);
                   Int32 usedtime = 1;
                   w.Write(usedtime);
                   w.Close();
                   writefile.Close();
               }
           }
           catch (Exception es)
           {          
               MessageBox.Show("Not Fund A2S.BAT"+es.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);          
           }

           return GetUsertimesWithAuthorityResult.CheckusertimesFail;
       }
        /// <summary>
        /// 硬件注册权限
        /// </summary>
        /// <returns></returns>
       static public GetUserCPUIDWithAuthorityResult GetUserCPUIDWithAuthority()
       {
           try
           {            
               ManagementClass mc = new ManagementClass("Win32_Processor");
               ManagementObjectCollection moc = mc.GetInstances();
               string strCpuID = null;
               foreach (ManagementObject mo in moc)
               {
                   strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                   mo.Dispose();
                   break;
               }
               string cPath = System.AppDomain.CurrentDomain.BaseDirectory;
               string[] files = Directory.GetFiles(cPath);
               bool ishavebat = false;
               foreach (string file in files)            //循环文件
               {
                   if (file == cPath + "eCPUIDA2S.bat")  //
                   {                   
                       ishavebat = true;
                       break;
                   }
               }
               if(ishavebat)
               {
                   FileStream readfile = new FileStream(cPath + "eCPUIDA2S.bat", FileMode.Open, FileAccess.Read);
                   BinaryReader r = new BinaryReader(readfile);
                   string screatcpuid = r.ReadString();
                   r.Close();
                   readfile.Close();
                   if (screatcpuid == strCpuID)
                   {
                       return GetUserCPUIDWithAuthorityResult.CheckCPUIDOK;
                   }
                   else
                   {
                       FormCPUIDAuthority f = new FormCPUIDAuthority();
                       f.label1.Text = "CPUID register";
                       if (DialogResult.OK == f.ShowDialog())
                       {                   
                   string writescreat = f.textBox1.Text;
                   if (writescreat == "jshglaser.202527")
                   {
                       FileStream writefile = new FileStream(cPath + "eCPUIDA2S.bat", FileMode.Open, FileAccess.Write);
                       BinaryWriter w = new BinaryWriter(writefile);
                       w.Write(strCpuID);
                       w.Close();
                       writefile.Close();
                        string cPath2 = System.AppDomain.CurrentDomain.BaseDirectory;
                        string[] files2 = Directory.GetFiles(cPath2);
                         bool ishavebat2 = false;
                        foreach (string file in files2)//循环文件
                        {
                            if (file == cPath+"win32A2S.bat")//
                            {                   
                               ishavebat2 = true;
                               break;
                            }
                        }

                         if (ishavebat2)
                         {
                             File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "win32A2S.bat");
                         }
                         else
                         {

                         }
                   }
                   else
                   {                  
                       MessageBox.Show("GET CPUID PASS WORD ERR", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      
                   }

                       }
                       else
                       {
                           return GetUserCPUIDWithAuthorityResult.CheckCPUIdCancel;
                       }

                   }

               }
               else
               {               
                       FileStream writefile = new FileStream(cPath + "eCPUIDA2S.bat", FileMode.OpenOrCreate, FileAccess.Write);
                       BinaryWriter w = new BinaryWriter(writefile);
                       w.Write("hello");
                       w.Close();
                       writefile.Close();              
               }
           
           }
           catch
           {          
               MessageBox.Show("check the cpu id err", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return GetUserCPUIDWithAuthorityResult.CheckCPUIdCancel;
           }
           return GetUserCPUIDWithAuthorityResult.CheckCPUIdFail;
       }

    }
}
