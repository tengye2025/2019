using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Modbus;

using System.Threading;
namespace WindowsFormsApplication1
{
    static class Program
    {               
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool ISOLNYONERUN;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "onlyonerun", out ISOLNYONERUN);
            if (ISOLNYONERUN)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            else
            {
                MessageBox.Show("程序已经打开","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Thread.Sleep(2000);
                System.Environment.Exit(1);
            }
         
        }
    }
}
