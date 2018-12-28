using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.Register;
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
            ClassRegisterUserAuthority.GetUserCPUIDWithAuthority();
            ClassRegisterUserAuthority.GetUsertimesWithAuthority(st);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            ClassRegisterUserAuthority.GetUserCPUIDWithAuthorityResult re = ClassRegisterUserAuthority.GetUserCPUIDWithAuthority();
            if (re == ClassRegisterUserAuthority.GetUserCPUIDWithAuthorityResult.CheckCPUIDOK)
            {
                ClassRegisterUserAuthority.GetUsertimesWithAuthority(st);
            }
            else if (re == ClassRegisterUserAuthority.GetUserCPUIDWithAuthorityResult.CheckCPUIdCancel)
            {
                this.Close();
            }
            else
            {

            }

            timer1.Enabled = true;
        }
        public void st()
        {

        }
    }
}
