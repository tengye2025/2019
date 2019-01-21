using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HG.Configure;
using WindowsFormsApplication1.Screat;

namespace WindowsFormsApplication1
{
    public partial class FormSetScreat : Form
    {
        public FormSetScreat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Configure cf = new Configure();
                cf.FileName = "_SCreat.ini";

                string scstring = ClassAES.Encrypt(textBox1.Text, "1123581321123456");
                cf.WriteConfig("SETCREAT", "Screataes", scstring);
                MessageBox.Show("OK");
                this.Close();
            }
            else
            {
                MessageBox.Show("密码不能为空");
            }

        }
    }
}
