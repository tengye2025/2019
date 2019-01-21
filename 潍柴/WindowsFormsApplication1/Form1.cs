using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HGMark.XMLClassLibrary;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  string path = "D:\\123.xml";
          //  string rootstring = "set";
          //  ConfigHelper.CreateXmlFile(ref path,ref rootstring);
          //  string xpath = "set";
          //  ConfigHelper.CreateNode(ref path, ref xpath, "name", "tengye");
          ////HGXMLClass.CreatXmlTree(ref path);

            if (textBox1.Text == "1")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                //this.Close();
                MessageBox.Show("error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
