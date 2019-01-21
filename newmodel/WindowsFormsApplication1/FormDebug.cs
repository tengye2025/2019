#region 命名空间引用
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HG.Configure;
using HG.MyJCZ;
using HG.LogRecord;
using System.Management;
using HG.Communication;
using System.IO.Ports;
using System.IO;
using System.Threading;
using WindowsFormsApplication1.Register;
using System.Timers;
using ModbusClassLibrary;
using WindowsFormsApplication1.MarkInterface;
using CommNetSocketClass;
using FtpClass;
using System.Data.SqlClient;
using System.Data.Sql;
#endregion
namespace WindowsFormsApplication1
{
    public partial class FormDebug : Form
    {

        public FormDebug()
        {
            InitializeComponent();
        }
        public FormMidContains fmdi = null;





        private void buttonXpointset_Click(object sender, EventArgs e)
        {

            //byte[] fbytearry = new byte[4];
            //unsafe
            //{

            //    float fl =(float) Convert.ToDouble(textBoxxpointset.Text);
            //    byte* p = (byte*)&fl;
            //    for (byte a = 3; a >= 0; a--)
            //    {
            //        fbytearry[a] = (byte)*p;
            //        p = p + 1;
            //        if (a == 0) break;
            //    }

            //}
            //fmdi.m.WriteMoreRegister(0x0000, 0x0002, 0x04, ref fbytearry, 0x01);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }



        private void buttonygobackzero_Click(object sender, EventArgs e)
        {
            fmdi.m.WriteOneRegister(0X0005, 0X0001, 0X01);
        }

        private void button9_Click(object sender, EventArgs e)
        {


        }

        private void button7_Click_4(object sender, EventArgs e)
        {

        }

        private void buttonypointadd_Click(object sender, EventArgs e)
        {



        }

        private void buttonypointincrease_Click(object sender, EventArgs e)
        {

        }

        private void buttonyroughdebug_Click(object sender, EventArgs e)
        {
            byte[] fbytearry = new byte[4];
            unsafe
            {
                float fl = (float)Convert.ToDouble(textBoxdafaultlocatoin.Text);
                byte* p = (byte*)&fl;
                for (byte a = 3; a >= 0; a--)
                {
                    fbytearry[a] = (byte)*p;
                    p = p + 1;
                    if (a == 0) break;
                }

            }
            fmdi.m.WriteMoreRegister(0x0002, 0x0002, 0x04, ref fbytearry, 0x01);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            byte[] po = fmdi.m.ReadKeepRegister(0x0012, 0x0002, 0x01);
            byte[] pos = new byte[4];
            for (int a = 0; a < 4; a++)
            {
                pos[a] = po[po.Length - 1 - a];
            }
            float locatopm = 1.0f;
            unsafe
            {

                byte* ml = (byte*)&locatopm;
                for (int b = 0; b < 4; b++)
                {

                    *ml = pos[b];
                    ml = ml + 1;
                }

            }

            textBoxdafaultlocatoin.Text = locatopm.ToString();

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }
        private object nowloactionlock = new object();
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            if(f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {         
            // fmdi.m.WriteOneRegister(0x0008, 0x0002, 0x01);
            groupBoxxyzdebug.Visible = true;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000d, 0X0001, 0X01);
            fmdi.m.WriteOneRegister(0X000d, 0X0000, 0X01);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000c, 0X0000, 0X01);
        }

        private void buttonypointadd_MouseDown(object sender, MouseEventArgs e)
        {

            fmdi.m.WriteOneRegister(0X0006, 0X0002, 0X01);

        }

        private void buttonypointadd_MouseUp(object sender, MouseEventArgs e)
        {

            fmdi.m.WriteOneRegister(0X0006, 0X0000, 0X01);

        }

        private void buttonypointadd_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void buttonypointadd_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void buttonypointadd_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void buttonypointincrease_MouseDown(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X0006, 0X0001, 0X01);
        }

        private void buttonypointincrease_MouseUp(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X0006, 0X0000, 0X01);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Resettools();

        }

        private void Resettools()
        {


            //buttonstartdebug.Enabled = false;
            //checkBox1.Enabled = false;
            //checkBox2.Enabled = false;
            //groupBoxydebug.Enabled = false;
            //groupBoxbeltdebug.Enabled = false;
        }

        private void buttonreset_Click(object sender, EventArgs e)
        {

        }







        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void buttonxlacationget_Click(object sender, EventArgs e)
        {
            byte[] po = fmdi.m.ReadKeepRegister(0x0010, 0x0002, 0x01);
            byte[] pos = new byte[4];
            for (int a = 0; a < 4; a++)
            {
                pos[a] = po[po.Length - 1 - a];
            }
            float locatopm = 1.0f;
            unsafe
            {

                byte* ml = (byte*)&locatopm;
                for (int b = 0; b < 4; b++)
                {

                    *ml = pos[b];
                    ml = ml + 1;
                }

            }

            textBoxxdafaultlacation.Text = locatopm.ToString();
        }

        private void buttonxlacationset_Click(object sender, EventArgs e)
        {
            byte[] fbytearry = new byte[4];
            unsafe
            {
                float fl = (float)Convert.ToDouble(textBoxxdafaultlacation.Text);
                byte* p = (byte*)&fl;
                for (byte a = 3; a >= 0; a--)
                {
                    fbytearry[a] = (byte)*p;
                    p = p + 1;
                    if (a == 0) break;
                }

            }
            fmdi.m.WriteMoreRegister(0x0000, 0x0002, 0x04, ref fbytearry, 0x01);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            byte[] po = fmdi.m.ReadKeepRegister(0x0014, 0x0002, 0x01);
            byte[] pos = new byte[4];
            for (int a = 0; a < 4; a++)
            {
                pos[a] = po[po.Length - 1 - a];
            }
            float locatopm = 1.0f;
            unsafe
            {

                byte* ml = (byte*)&locatopm;
                for (int b = 0; b < 4; b++)
                {

                    *ml = pos[b];
                    ml = ml + 1;
                }

            }

            textBoxzdafalutlacation.Text = locatopm.ToString();

        }

        private void button16_Click(object sender, EventArgs e)
        {
            byte[] fbytearry = new byte[4];
            unsafe
            {
                float fl = (float)Convert.ToDouble(textBoxzdafalutlacation.Text);
                byte* p = (byte*)&fl;
                for (byte a = 3; a >= 0; a--)
                {
                    fbytearry[a] = (byte)*p;
                    p = p + 1;
                    if (a == 0) break;
                }

            }
            fmdi.m.WriteMoreRegister(0x00004, 0x0002, 0x04, ref fbytearry, 0x01);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000b, 0X0001, 0X01);
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000b, 0X0000, 0X01);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000b, 0X0002, 0X01);
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000b, 0X0000, 0X01);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000a, 0X0001, 0X01);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000a, 0X0000, 0X01);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000a, 0X0002, 0X01);
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000a, 0X0000, 0X01);
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000c, 0X0001, 0X01);
        }

        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000c, 0X0000, 0X01);
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000c, 0X0002, 0X01);
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            fmdi.m.WriteOneRegister(0X000c, 0X0000, 0X01);
        }

        private void button12_Click_1(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void buttonloadmodelfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            if (HgMarkInterface.MarkCardInformationSt.markcard == HgMarkInterface.MarkCardType.markjcz)
            {
                openFileDialog1.Filter = "模板文件(*.ezd)|*.*";
            }
            else if (HgMarkInterface.MarkCardInformationSt.markcard == HgMarkInterface.MarkCardType.markmsi)
            {
                openFileDialog1.Filter = "模板文件(*.mks)|*.*";
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strEzdpathA = openFileDialog1.FileName;
                textBoxmodelfilepath.Text = strEzdpathA;
                int mksindex = HgMarkInterface.LoadtheModelFile(ref strEzdpathA);
                if (mksindex < 0)
                {
                    MessageBox.Show("加载模板失败");
                    return;
                }
                else
                {
                    HgMarkInterface.DrawModelFileInPicture(ref pictureBox1, mksindex);
                }
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            textBoxxpoint.Text = GETpresevepoint(0x0010);
            textBoxypoint.Text = GETpresevepoint(0x0012);
            textBoxzpoint.Text = GETpresevepoint(0x0014);
        }

        private string GETpresevepoint(ushort preaddress)
        {
            byte[] po = fmdi.m.ReadKeepRegister(preaddress, 0x0002, 0x01);
            byte[] pos = new byte[4];
            for (int a = 0; a < 4; a++)
            {
                pos[a] = po[po.Length - 1 - a];
            }
            float locatopm = 1.0f;
            unsafe
            {

                byte* ml = (byte*)&locatopm;
                for (int b = 0; b < 4; b++)
                {

                    *ml = pos[b];
                    ml = ml + 1;
                }

            }
            return locatopm.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBoxxpoint.Text == "")
            {
                MessageBox.Show("坐标值错误");
                return;
            }
            HG.Configure.Configure cf = new HG.Configure.Configure();
            string pozations = textBoxxpoint.Text + "-" + textBoxypoint.Text + "-" + textBoxzpoint.Text;
            cf.WriteConfig("ModelFilesPozation", textBoxmodelfilepath.Text, pozations);
            MessageBox.Show("坐标值对应的模板保存完毕。");
            groupBoxxyzdebug.Visible = false;
        }
    }
}