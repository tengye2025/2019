using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HGMark.XMLClassLibrary;
using WindowsFormsApplication1.MarkInterface;
namespace WindowsFormsApplication1
{
    public partial class FormWork : Form
    {
        public FormWork()
        {
            InitializeComponent();
        }
        public FormMidContains fmdi = null;
     
        private void button1_Click(object sender, EventArgs e)
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
                    HgMarkInterface.DrawModelFileInPicture(ref pictureBoxworkshow, mksindex);
                }

                HG.Configure.Configure cf = new HG.Configure.Configure();
                string   xyzpozations = cf.ReadConfig("ModelFilesPozation", textBoxmodelfilepath.Text, "");
                if (xyzpozations != "")
                {
                    string[] xyzarry = xyzpozations.Split('-');
                    textBoxxpoint.Text = xyzarry[0];
                    textBoxypoint.Text = xyzarry[1];
                    textBoxzpoint.Text = xyzarry[2];

                    XyzPozationSet(0x0000, (float)Convert.ToDouble(xyzarry[0]));//x
                    XyzPozationSet(0x0002, (float)Convert.ToDouble(xyzarry[1]));//y
                    XyzPozationSet(0x0004, (float)Convert.ToDouble(xyzarry[2]));//z
                }else
                {
                    XyzPozationSet(0x0000, (float)Convert.ToDouble(0));//x
                    XyzPozationSet(0x0002, (float)Convert.ToDouble(0));//y
                    XyzPozationSet(0x0004, (float)Convert.ToDouble(0));//z
                    MessageBox.Show("此模板没有位置记录");
                }


            }
        }

        private void buttonstartwork_Click(object sender, EventArgs e)
        {        
            if(textBoxstartmarkshuzi.Text == "")
            {
                 MessageBox.Show("输入开始数字");
                  return;
            }
            byte[] z = fmdi.m.ReadKeepRegister(0x000d, 0x0001, 0x01);
            if (z[z.Length - 1] == 0x01)
            {
                fmdi.m.WriteOneRegister(0x0008, 0x0001, 0x01);
                Thread.Sleep(300);
                fmdi.m.WriteOneRegister(0x0008, 0x0000, 0x01);
                //button2.Enabled = false;
                _workenvent.Set();

            }
            else
            {
                MessageBox.Show("请先完成三轴回零");
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            fmdi.m.WriteOneRegister(0x0007, 0x0001, 0x01);
            Thread.Sleep(400);
            fmdi.m.WriteOneRegister(0x0007, 0x0000, 0x01);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBoxxpoint.Text = GETpresevepoint(0x0010);
            textBoxypoint.Text = GETpresevepoint(0x0012);
            textBoxzpoint.Text = GETpresevepoint(0x0014);
        }

       private string  GETpresevepoint(ushort preaddress)
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

       private   void  XyzPozationSet(ushort ad,float value)
        {
            byte[] fbytearry = new byte[4];
            unsafe
            {
                float fl = value;
                byte* p = (byte*)&fl;
                for (byte a = 3; a >= 0; a--)
                {
                    fbytearry[a] = (byte)*p;
                    p = p + 1;
                    if (a == 0) break;
                }

            }
            fmdi.m.WriteMoreRegister(ad, 0x0002, 0x04, ref fbytearry, 0x01);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBoxxpoint.Text == "")
            {
                MessageBox.Show("坐标值错误");
                return;
            }
            HG.Configure.Configure cf = new HG.Configure.Configure();
            string pozations = textBoxxpoint.Text + "-" + textBoxypoint.Text + "-" + textBoxzpoint.Text;
            cf.WriteConfig("ModelFilesPozation", textBoxmodelfilepath.Text, pozations);
            MessageBox.Show("坐标值对应的模板保存完毕。");
        }

        private void buttonstopwork_Click(object sender, EventArgs e)
        {
            fmdi.m.WriteOneRegister(0x0006, 0x0001, 0x01);
            Thread.Sleep(400);
            fmdi.m.WriteOneRegister(0x0006, 0x0000, 0x01);
            MessageBox.Show("关机检查");
            fmdi.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fmdi.m.WriteOneRegister(0x0009, 0x0001, 0x01);
            Thread.Sleep(500);
            fmdi.m.WriteOneRegister(0x0009, 0x0000, 0x01);
            button2.Enabled = true;
        }

        private void FormWork_Load(object sender, EventArgs e)
        {
            Thread myThread = new Thread(new ParameterizedThreadStart(OnworkTreadproc));
            myThread.IsBackground = true;
            myThread.Start(this);
        }
        private ManualResetEvent _workenvent = new ManualResetEvent(false);
        private string printtextname = "1";
        void OnworkTreadproc(object c)
        {

            while(true)
            {
                _workenvent.WaitOne();
                if(HgMarkInterface.ReadIO(4))
                {
                    string strshuzi = textBoxstartmarkshuzi.Text;
                    HgMarkInterface.ChangeNameByTxtstring(ref printtextname, ref strshuzi, 0);
                    HgMarkInterface.DrawModelFileInPicture(ref pictureBoxworkshow, 0);
                    HgMarkInterface.Mark(false, 0);
                    textBoxstartmarkshuzi.Invoke((EventHandler)(delegate
                   {
                       textBoxstartmarkshuzi.Text = (Convert.ToInt32( textBoxstartmarkshuzi.Text ) + 1).ToString();
                   }));
                    HgMarkInterface.WriteIO(5, true);
                    Thread.Sleep(100);
                    HgMarkInterface.WriteIO(5,false);
                }

            }

        }

        private void buttonstop_Click(object sender, EventArgs e)
        {
        
        }

        private void button5_Click(object sender, EventArgs e)
        {
   

            if (button5.Text == "激光标刻暂停")
            {
                _workenvent.Reset();      
                fmdi.m.WriteOneRegister(0x000f, 0x0001, 0x01);
                button5.Text = "激光标刻开始";
            }
            else if (button5.Text == "激光标刻开始")
            {
                   _workenvent.Set();   
                   fmdi.m.WriteOneRegister(0x000f, 0x0000, 0x01);
                   button5.Text = "激光标刻暂停";
            }
        }

        private void textBoxmodelfilepath_TextChanged(object sender, EventArgs e)
        {

        }
    }
}