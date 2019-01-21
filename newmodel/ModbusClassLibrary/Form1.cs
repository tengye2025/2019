using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModbusClassLibrary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModbusClassLibrary.CommunicationCom.ModbusNetApplyInterfaceChose lp = ModbusClassLibrary.CommunicationCom.ModbusNetApplyInterfaceChose.Clience;
            TCPModbusClass mm = new TCPModbusClass(ref lp);
            byte[] bmap = new byte[7];
            //   01 00 00 00 00 06 01
            bmap[0] = 0x01;
            bmap[1] = 0x00;
            bmap[2] = 0x00;
            bmap[3] = 0x00;
            bmap[4] = 0x00;
            bmap[5] = 0x06;
            bmap[6] = 0x01;
         //   mm.SetMBAP(ref bmap);
          //  ushort oa = 0x0000;
           // ushort rv = 0x0100;
          //  mm.WriteOneRegister(oa, rv);

        }
    }
}
