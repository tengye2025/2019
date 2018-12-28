using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1.Register
{
    public partial class FormUserTimesAthority : Form
    {
        public FormUserTimesAthority()
        {
            InitializeComponent();
        }

        private void FormUserAthority_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.IndexOf("jshglaser.") != -1 && GetSetTtimeunlimited())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("err", "err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private bool GetSetTtimeunlimited()
        {
            if(   (24*60*60)<Convert.ToInt32(textBox1.Text.Substring(textBox1.Text.LastIndexOf(".") + 1))   && (10*365*24*60*60) > Convert.ToInt32(textBox1.Text.Substring(textBox1.Text.LastIndexOf(".") + 1)) )
            {
                return true;
            }
            else
            {
                MessageBox.Show("set user times err", "err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }
    }
}
