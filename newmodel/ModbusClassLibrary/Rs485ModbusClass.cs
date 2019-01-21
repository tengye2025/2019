using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusClassLibrary.CommunicationCom;
/// <summary>
/// RTU模式
/// </summary>
namespace ModbusClassLibrary
{
   public enum rsmodbuscom
   { 
        seriport,
        net
   }
    public class Rs485ModbusClass
    {
        private CommSeriport com;
        public Rs485ModbusClass(ref CommunicationCom.SerialPortInfo pe)
        {
            m_address = 0;
            com.PortInfo = pe;
            com.Open();
        }
        public Rs485ModbusClass()
        {
          
        }

        public void WriteData(ref byte []b)
       {
            com.Write(ref b);
       }

        public byte [] ReadData()
        {
            byte[] yu = new byte[2];
            return yu; 
        }
        protected byte m_address =0;
        public void SetMachineModbusAddress(byte address)
        {
            m_address = address;
        }
        protected List<byte> analysislist = new List<byte>();
        public void AnalysisTheReturnData(ref byte [] p)
        {
            analysislist.AddRange(p);
            byte[] arry1 = analysislist.ToArray();
            analysislist.Clear();
            for (int a = 0; a < arry1.Length; a++)
            {
                if(arry1[a] == m_address)
                {
                    for(int b= a+2;b<arry1.Length;b++)
                    {
                        byte[] bbyte = new byte[b - a+ 3];
                        for(int c = 0;c <(b - a +1);c++)
                        {
                            bbyte[c] = arry1[a+c];
                        }
                        byte[] crc = new byte[2];
                        GetCRC(ref bbyte, ref crc);
                        if(crc[0] == arry1[b+1] && crc[1] == arry1[b+2])
                        {
                            OnGetFrame(ref bbyte);
                            byte[] remainbytearry = new byte[arry1.Length - (b - a + 3)];
                            for(int d = 0; d < a; d++)
                            {
                                remainbytearry[d] = arry1[d];
                            }
                            for(int e = a; e< remainbytearry.Length;e++)
                            {
                                remainbytearry[e] = arry1[b+3];
                                b = b + 1;
                            }
                            analysislist.AddRange(remainbytearry);
                           //后续处理 
                        }
                        else
                        {

                        }

                    }

                }else
                {


                }


            }


        }

        private void OnGetFrame(ref byte []fbyte)
        {



        }
        public void GetCRC(ref byte[] message, ref byte[] CRC)
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

    }
}
