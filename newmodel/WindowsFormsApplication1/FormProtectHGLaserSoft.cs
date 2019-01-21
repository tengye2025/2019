using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using HG.Configure;
using WindowsFormsApplication1.Screat;
namespace WindowsFormsApplication1
{
    public partial class FormProtectHGLaserSoft : Form
    {
        public FormProtectHGLaserSoft()
        {
            InitializeComponent();
            ManagementClass mc = new ManagementClass("Win32_BaseBoard");
            ManagementObjectCollection moc = mc.GetInstances();
            string strID = null;
            foreach (ManagementObject mo in moc)
            {
                strID = mo.Properties["SerialNumber"].Value.ToString();
                break;
            }

            label2.Text = strID;
            textBox1.Focus();
         
        }

        private void button1_Click(object sender, EventArgs e)
        {        
           // string scret = (Convert.ToInt32(label2.Text.Substring(0, 4)) + 2025).ToString();
            Configure cf = new Configure();
            cf.FileName = "_SCreat.ini";   
            string aesedstring =   cf.ReadConfig("SETCREAT", "Screataes", "");
            string scstring = ClassAES.Decrypt(aesedstring, "1123581321123456");

            if (textBox1.Text == scstring)
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:
            if (message.Length > 2)
            {
                ushort CRCFull = 0xFFFF;
                byte CRCHigh = 0xFF, CRCLow = 0xFF;
                char CRCLSB;
                for (int i = 0; i < (message.Length) - 2; i++)
                {
                    CRCFull = (ushort)(CRCFull ^ message[i]);
                    for (int j = 0; j < 8; j++)
                    {
                        CRCLSB = (char)(CRCFull & 0x0001);
                        CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);
                        if (CRCLSB == 1)
                            CRCFull = (ushort)(CRCFull ^ 0xA001);
                    }
                }
                CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
                CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
            }
            else
            {
                return;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           // button1.Focus();
        }
    }
}
