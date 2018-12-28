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
    public partial class FormCPUIDAuthority : Form
    {
        public FormCPUIDAuthority()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "jshglaser.202527")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("ERR", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void FormCPUIDAuthority_Load(object sender, EventArgs e)
        {
            textBox1.Focus();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
