using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusClassLibrary.CommunicationCom;

using System.Runtime.InteropServices;
namespace ModbusClassLibrary
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RequestPDU
   {
        public byte functioncode;
        public ushort startaddress;
        public ushort coilnumber;

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct AnswerPDU
    {
        public byte functioncode;
        public byte bytenumber;
        public ushort *coilstats;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WriteOneCoilPDU
    {
        public byte functioncode;
        public ushort outaddress;
        public ushort outvalue;

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct AnswerOneCoilPDU
    {
        public byte functioncode;
        public ushort outaddress;
        public ushort outvalue;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WriteOneRegisterPDU
    {
        public byte   functioncode;
        public ushort registeraddress;
        public ushort registervalue;
       
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct AnswerOneRegisterPDU
    {
        public byte functioncode;
        public ushort registeraddress;
        public ushort registervalue;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct mWriteMoreRegisterPDU
    {
        public byte   functioncode;
        public ushort startaddress;
        public ushort registernumber;
        public byte   bytenumber;
        public byte*  buf;

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct mAnswermWriteMoreRegisterPDU
    {
        public byte   functioncode;
        public ushort startaddress;
        public ushort registernumber;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct mWriteMoreCoilPDU
    {
        public byte functioncode;
        public ushort startaddress;
        public ushort outnumber;
        public byte bytenumber;
        public byte* buf;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct mAnswermWriteMoreCoilPDU
    {
        public byte functioncode;
        public ushort startaddress;
        public ushort outnumber;
    }
    public class TCPModbusClass
    {
       private  ComInterface _com;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="netinfo"> 接口信息类</param>
        public  TCPModbusClass(ref ModbusNetApplyInterfaceChose type)
        {
            InitComInterface(ref type);
        }
         ~TCPModbusClass()
        {
            _com.Close();
        }
        private void InitComInterface(ref ModbusNetApplyInterfaceChose type)
        {
            NetInterfaceinfo netinfo = new NetInterfaceinfo();
            if (type == ModbusNetApplyInterfaceChose.Clience)
            {
                netinfo.Ongetbufbackfunction = new ComnetDataReceiveddelegate(DataReceiveddFunction);//接受数据的所触发的事件
                netinfo.Ip = "192.168.0.1";
                netinfo.Port = 502;
                netinfo.CorS = ModbusNetApplyInterfaceChose.Clience;
                _com = new ComInterface(netinfo);
                _com.Open();

            }else if(type == ModbusNetApplyInterfaceChose.Server)
            {
                netinfo.Ongetbufbackfunction = new ComnetDataReceiveddelegate(DataReceiveddFunction); //接受数据的所触发的事件
                netinfo.Ip = "192.168.0.1";
                netinfo.Port = 502;
                netinfo.CorS = ModbusNetApplyInterfaceChose.Server;
                netinfo.Onclienceconnectserverevent = new ClienceConnectServerComneteventdelegate(ClienceConnectServerComnetevent);//客户端连接触发的事件
                _com = new ComInterface(netinfo);
                _com.Open();
            }
        }
        public void ComClose()
        {
            if (_com.IsOpen())
            {
               _com.Close();
            }
        
        }
        public bool ComOpen()
        {
            if (!_com.IsOpen())
            {
                return _com.Open();
            }
            else
            {
                return true;
            }
        }
        private static int MODBUSFLG = 1;
        private List<byte> DataOrganizationList = new List<byte>();//数据组织中转  data organization 
        private List<FundationCodeClass> fundationlist = new List<FundationCodeClass>();

       private byte[] SetMBAP(ushort nextdatalength,byte machineaddress)
       {
                byte[] MBAP = new byte[7];
                MBAP[0] = (byte)((MODBUSFLG>>8) & 0x00ff );
                MBAP[1] = (byte)(MODBUSFLG & 0x00ff);
                MODBUSFLG = MODBUSFLG + 1;
                MBAP[2] = 0x00;
                MBAP[3] = 0x00;
                MBAP[4] = (byte)((nextdatalength >> 8) & 0x00ff);
                MBAP[5] = (byte)(nextdatalength & 0x00ff);
                MBAP[6] = machineaddress;
                return MBAP;
        }
        /// <summary>
        /// 收到数据回调函数
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="length"></param>
        public   void DataReceiveddFunction(ref byte[] buf, uint length)
        {
            byte[] ibuf = new byte[length];
            for(int  b = 0;b<length;b++)
            {
                ibuf[b] = buf[b];
            }

            bool issentorder = false;
            for(int a =0;a<fundationlist.Count;a++)
            {
                if(fundationlist[a].fundationcodebyte == buf[7])
                {
                    fundationlist[a].waitidelarry = ibuf;
                    fundationlist[a].EvetSet();
                    issentorder = true;
                    fundationlist.RemoveAt(a);
                }
            }
            if(!issentorder)//非 发送到 指令返回。是被动接收到，要相应的
            {

            }
        }
        /// <summary>
        /// 客户端连接到服务器,相应函数
        /// </summary>
        /// <param name="cip"></param>
        /// <param name="str"></param>
        public  void ClienceConnectServerComnetevent(ref string cip, ref string str)
        {


        }
        /// <summary>
        /// 读线圈 
        /// </summary>
        /// <param name="fc">0x01</param>
        /// <param name="bn"></param>
        /// <param name="cn"></param>
        public unsafe void ReadCoil(ushort sd,ushort cn,byte machineaddress)
        {
            byte[] pbyte = new byte[5];
            fixed (byte * pfirstptr = &pbyte[0])
            {
                RequestPDU * p = (RequestPDU *)pfirstptr;
                p->functioncode = 0x01;
                p->startaddress = sd;
                p->coilnumber = cn;
                DataOrganizationList.Clear();
                DataOrganizationList.AddRange(SetMBAP(6, machineaddress));
                DataOrganizationList.AddRange(pbyte);
                byte[] senddatabyte = DataOrganizationList.ToArray();
                FundationCodeClass fund = new FundationCodeClass();
                fund.fundationcodebyte = p->functioncode;
                fund.SetTimeout(2000);
                fundationlist.Add(fund);
                _com.Write(ref senddatabyte);
                if (fund.WaitForFinished())
                {

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("modbus out time");
                }
            }

        }
        /// <summary>
        /// 读离散输入
        /// </summary>
        /// <param name="fc">功能码</param>
        /// <param name="bn"></param>
        /// <param name="cn"></param>
        public unsafe void ReadDiscreteInput(ushort sd, ushort cn, byte machineaddress)
        {
            byte[] pbyte = new byte[5];
            fixed (byte* pfirstptr = &pbyte[0])
            {
                RequestPDU* p = (RequestPDU *)pfirstptr;
                p->functioncode = 0x02;
                p->startaddress = sd;
                p->coilnumber = cn;
                DataOrganizationList.Clear();
                DataOrganizationList.AddRange(SetMBAP(6, machineaddress));
                DataOrganizationList.AddRange(pbyte);
                byte[] senddatabyte = DataOrganizationList.ToArray();
                FundationCodeClass fund = new FundationCodeClass();
                fund.fundationcodebyte = p->functioncode;
                fund.SetTimeout(2000);
                fundationlist.Add(fund);
                _com.Write(ref senddatabyte);
                if (fund.WaitForFinished())
                {

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("modbus out time");
                }
            }

        }
        /// <summary>
        /// 读保持寄存器
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="sd"></param>
        /// <param name="cn"></param>
        public unsafe byte [] ReadKeepRegister(ushort sd, ushort cn, byte machineaddress)
        {
            ushort sds = (ushort)(((sd & 0x00ff) << 8) | ((sd & 0xff00) >> 8));
            ushort cns = (ushort)(((cn & 0x00ff) << 8) | ((cn & 0xff00) >> 8));

            byte[] pbyte = new byte[5];
            fixed (byte* pfirstptr = &pbyte[0])
            {
                RequestPDU* p = (RequestPDU *)pfirstptr;
                p->functioncode = 0x03;
                p->startaddress = sds;
                p->coilnumber = cns;
                DataOrganizationList.Clear();
                DataOrganizationList.AddRange(SetMBAP(6, machineaddress));
                DataOrganizationList.AddRange(pbyte);
                byte[] senddatabyte = DataOrganizationList.ToArray();
                FundationCodeClass fund = new FundationCodeClass();
                fund.fundationcodebyte = p->functioncode;
                fund.SetTimeout(2000);
                fundationlist.Add(fund);
                _com.Write(ref senddatabyte);
                if (fund.WaitForFinished())
                {

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("modbus out time");
                }
                return fund.waitidelarry;
            }


        }
        /// <summary>
        ///写单个线圈
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="sd"></param>
        /// <param name="cn"></param>
        public unsafe void WriteOneCoil( ushort oa, ushort ov,byte machineaddress)
        {
            byte[] pbyte = new byte[5];
            fixed (byte* pfirstptr = &pbyte[0])
            {
                WriteOneCoilPDU * p = (WriteOneCoilPDU*)pfirstptr;
                p->functioncode = 0x05;
                p->outaddress = oa;
                p->outvalue = ov;
                DataOrganizationList.Clear();
                DataOrganizationList.AddRange(SetMBAP(6, machineaddress));
                DataOrganizationList.AddRange(pbyte);
                byte[] senddatabyte = DataOrganizationList.ToArray();
                FundationCodeClass fund = new FundationCodeClass();
                fund.fundationcodebyte = p->functioncode;
                fund.SetTimeout(2000);
                fundationlist.Add(fund);
                _com.Write(ref senddatabyte);
                if (fund.WaitForFinished())
                {

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("modbus out time");
                }
            }
        }
        /// <summary>
        /// 写单个寄存器
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="sd"></param>
        /// <param name="cn"></param>
        public unsafe void WriteOneRegister( ushort ra, ushort rv,byte machineaddress)
        {
            ushort ras = (ushort)(((ra & 0x00ff) << 8) | ((ra & 0xff00) >> 8));
            ushort rvs = (ushort)(((rv & 0x00ff) << 8) | ((rv & 0xff00) >> 8));
            byte[] pbyte = new byte[5];
            fixed (byte* pfirstptr = &pbyte[0])
            {
                WriteOneRegisterPDU * p = (WriteOneRegisterPDU *)pfirstptr;
                p->functioncode = 0x06;
                p->registeraddress = ras;
                p->registervalue = rvs;
                DataOrganizationList.Clear();
                DataOrganizationList.AddRange(SetMBAP(6, machineaddress));
                DataOrganizationList.AddRange(pbyte);
                byte[] senddatabyte = DataOrganizationList.ToArray();
                FundationCodeClass fund = new FundationCodeClass();
                fund.fundationcodebyte = p->functioncode;
                fund.SetTimeout(20000);
                fundationlist.Add(fund);
                _com.Write(ref senddatabyte);
                if( fund.WaitForFinished())
                {

                }else
                {
                    System.Windows.Forms.MessageBox.Show("modbus out time");
                }
                    
            }
        }
        /// <summary>
        /// 写多个线圈
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="sd"></param>
        /// <param name="cn"></param>
        public unsafe void WriteMoreCoil(ushort sd, ushort on, byte bn, byte[] coilvalue, byte machineaddress)
        {

            byte[] pbyte = new byte[6 + bn];
            fixed (byte* pfirstptr = &pbyte[0])
            {
                mWriteMoreCoilPDU* p = (mWriteMoreCoilPDU *)pfirstptr;
                p->functioncode = 0x0f;
                p->startaddress = sd;
                p->outnumber = on;
                p->bytenumber = bn;
                byte* po = (byte*)&p->buf;
                for (int a = 0; a < coilvalue.Length; a++)
                {
                    *po = coilvalue[a];
                    po++;
                }

                DataOrganizationList.Clear();
                DataOrganizationList.AddRange(SetMBAP((ushort)(pbyte.Length+1), machineaddress));
                DataOrganizationList.AddRange(pbyte);
                byte[] senddatabyte = DataOrganizationList.ToArray();
                FundationCodeClass fund = new FundationCodeClass();
                fund.fundationcodebyte = p->functioncode;
                fund.SetTimeout(2000);
                fundationlist.Add(fund);
                _com.Write(ref senddatabyte);         
                if (fund.WaitForFinished())
                {

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("modbus out time");
                }

            }

        }
        /// <summary>
        /// 写多个寄存器
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="sd"></param>
        /// <param name="cn"></param>
        public unsafe void WriteMoreRegister(ushort sd, ushort rn, byte bytenumber, ref byte[] regitervalue, byte machineaddress)
        {

            ushort sds = (ushort)(((sd & 0x00ff) << 8) | ((sd & 0xff00) >> 8));
            ushort rns = (ushort)(((rn & 0x00ff) << 8) | ((rn & 0xff00) >> 8));

            byte[] pbyte = new byte[6 + bytenumber];
            fixed (byte* pfirstptr = &pbyte[0])
            {
                mWriteMoreRegisterPDU* p = (mWriteMoreRegisterPDU *)pfirstptr;
                p->functioncode =0x10;
                p->startaddress = sds;
                p->registernumber = rns;
                p->bytenumber = bytenumber;
                byte* po =(byte *) &p->buf;
                for(int a = 0;a <regitervalue.Length;a++)
                {
                    *po = regitervalue[a];
                     po++;
                }
                DataOrganizationList.Clear();
                DataOrganizationList.AddRange(SetMBAP((ushort)(pbyte.Length+1), machineaddress));
                DataOrganizationList.AddRange(pbyte);
                byte[] senddatabyte = DataOrganizationList.ToArray();
                FundationCodeClass fund = new FundationCodeClass();
                fund.fundationcodebyte = p->functioncode;
                fund.SetTimeout(2000);
                fundationlist.Add(fund);
                _com.Write(ref senddatabyte);
                if (fund.WaitForFinished())
                {

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("modbus out time");
                }

            }
        }


    }
}
