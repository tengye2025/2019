using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HGControlLibrary
{
    public delegate void AutoBackClickedButtonIndex(byte buttonid);
    public partial class LeftButtons: UserControl
    {
        public LeftButtons()
        {
            InitializeComponent();
        }

        private List<ToolStripButton> spbtlist = new List<ToolStripButton>();
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Resetbutton();
            toolStripButtonwork.ForeColor = Color.White;
            toolStripButtonwork.BackColor = System.Drawing.Color.FromArgb(3, 143, 244);        
            backchosebuttonidf(0);
            ClickedButtonIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {      
            toolStripLabel1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            toolStripLabel2.Text = DateTime.Now.ToString("HH-mm-ss");
        }


        private void UserControl1_Load(object sender, EventArgs e)
        {
            toolStripLabel1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            toolStripLabel2.Text = DateTime.Now.ToString("HH-mm-ss");
            spbtlist.Add(toolStripButtonwork);
            spbtlist.Add(toolStripButtonabout);
            spbtlist.Add(toolStripButtondebug);
           
        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Resetbutton();
            toolStripButtondebug.ForeColor = Color.White;
           toolStripButtondebug.BackColor = System.Drawing.Color.FromArgb(3, 143, 244);
         
           backchosebuttonidf(1);
           ClickedButtonIndex = 1;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Resetbutton();      
            toolStripButtonabout.ForeColor = Color.White;
            toolStripButtonabout.BackColor = System.Drawing.Color.FromArgb(3, 143, 244);
           
            backchosebuttonidf(2);
            ClickedButtonIndex = 2;
        }

        private void Resetbutton()
        {
            for (int a = 0; a < spbtlist.Count; a++)
            {
                spbtlist[a].ForeColor = Color.Black;
                spbtlist[a].BackColor = Color.White;
            }
        }
        private AutoBackClickedButtonIndex backchosebuttonidf;
        public void SetAutoBackClickedButtonIndexFunction(AutoBackClickedButtonIndex f)
        {
            backchosebuttonidf = new AutoBackClickedButtonIndex(f);
        }
        private byte ClickedButtonIndex = 0;
        public byte GetClickedButtonIndex
        {
            get { return ClickedButtonIndex; }
        }
        public void SetShowFOrmIndex(byte f)
        {
            switch (f)
            {

                case 0: Resetbutton();
                    toolStripButtonwork.ForeColor = Color.White;
                    toolStripButtonwork.BackColor = System.Drawing.Color.FromArgb(3, 143, 244);
                    backchosebuttonidf(0);break;

                case 1: Resetbutton();
                    toolStripButtondebug.ForeColor = Color.White;
                    toolStripButtondebug.BackColor = System.Drawing.Color.FromArgb(3, 143, 244);

                    backchosebuttonidf(1); break;
                case 2: Resetbutton();
                    toolStripButtonabout.ForeColor = Color.White;
                    toolStripButtonabout.BackColor = System.Drawing.Color.FromArgb(3, 143, 244);
                    backchosebuttonidf(2); break;
                default: break;
            }
            ClickedButtonIndex = f;
        }
    }
}
