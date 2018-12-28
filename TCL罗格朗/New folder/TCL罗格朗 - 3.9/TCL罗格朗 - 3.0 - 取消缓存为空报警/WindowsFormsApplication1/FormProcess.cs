using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
   
    public partial class FormProcess : Form
    {
        public FormProcess(ref ProcessbarStruct p)
        {
            InitializeComponent();
            prrocessobjsources = p;
        }
        private ProcessbarStruct prrocessobjsources;
        public void Initaddprocess(ref ProcessbarStruct p)
        {
            prrocessobjsources = p;
        }
     
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void FormProcess_Load(object sender, EventArgs e)
        {
            Thread myThread = new Thread(new ParameterizedThreadStart(OnProcessTreadproc));
            myThread.IsBackground = true;
            myThread.Start(this);
        }

        private void OnProcessTreadproc(object p)
        {
            while (true)
            {
                Thread.Sleep(10);
                int processbarvalue = 0;
                lock (prrocessobjsources._lockprrocessobj)
                {
                    processbarvalue = prrocessobjsources.processbarvalue;
                }
                    progressBar1.BeginInvoke((EventHandler)(delegate
                      {
                          progressBar1.Value = processbarvalue;
                      }));
                if (processbarvalue >= 99)
                {
                    break;
                }
            }

            Form f = (Form)p;
             f.BeginInvoke((EventHandler)(delegate
                       {
                           f.Close();
                       }));
        }
    }

    public class ProcessbarStruct
    {
        public object _lockprrocessobj;
        public int processbarvalue;
        public ProcessbarStruct()
        {
            _lockprrocessobj = new object();
            processbarvalue = 0;
        }
    }
}
