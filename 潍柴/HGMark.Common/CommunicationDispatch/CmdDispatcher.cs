using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HGMark.Common.CommunicationFunction;
using HGMark.Common.CmdLib;
using HGMark.Common.CommunicationAnalysis;
namespace HGMark.Common.CommunicationDispatch
{
 public delegate byte [] PassiveGetDataFromOthersDelegate(ref byte []buf,uint bytelength);
 public enum CmdBackDataErr
{
     outtime,
     sucessful
}
    sealed  public class comchannel
    {
        public ComInterface com = null;
        public Queue<CmdManage> sendcommandquence;
        public comchannel(ComInterface pcom, Queue<CmdManage> qe)
        {
            com = pcom;
            sendcommandquence = qe;
        }

        public CmdBackDataErr ImplementCmd(ref CmdManage cmd, out byte[] outgetbuf)
        {
            sendcommandquence.Enqueue(cmd);
            byte []buf;
            cmd.GetSendData(out buf);
            if (com != null)
            {
                com.Write(ref buf);
            }

            if(cmd.WaitForFinished() == CmdStatus.StatusTimeout)
            {
                outgetbuf = null;
                return CmdBackDataErr.outtime;
            }
           else
           {
               cmd.GetRecveBuf(out outgetbuf);
               return CmdBackDataErr.sucessful;
           }
                    
        }



    }
  public  class CmdDispatcher
    {
       
      private   comchannel[] channel;
      private   ProtocolDataStxEtxandLen proc;
      private   byte[] mbuf = new byte[1024];
      private PassiveGetDataFromOthersDelegate passivegetdatafromothersevent;
     public CmdDispatcher(ref NetInterfaceinfo netinfo)
     {
            try
            {  
                netinfo.OnGetBufBackFunction = new ComnetDataReceiveddelegate(Ongetnetframe);
                ComInterface p1 =   new ComInterface(netinfo);
                Queue<CmdManage> p2 =   new Queue<CmdManage>();

                channel = new comchannel[1];
                channel[0] = new comchannel(p1, p2);
                channel[0].com = new ComInterface(netinfo);
                channel[0].sendcommandquence = new Queue<CmdManage>();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
                     
     }


    private void Ongetnetframe(ref byte [] getbuf,uint buflength)
    {
        CommunicationFrameAnalysis.PutBufDataIntoTheFrame(ref proc, ref getbuf, ref buflength);
        CommunicationFrameAnalysis.AnalyzeTheFrameData(ref proc);
    }


    private void netanalaysisframe(ref byte []buf,uint buflen)
    {
      
        byte[] getworkeddata = passivegetdatafromothersevent(ref buf, buflen);
        channel[0].com.Write(ref getworkeddata);
      
    }
    public void SetPassivegetdatafromothersevent(ref PassiveGetDataFromOthersDelegate p)
    {
        passivegetdatafromothersevent = p;
    }
        
        public void Initframe()
        {
            pCheckSum ck = new pCheckSum(this.GetCRC);
            pOnGetFrame on = new pOnGetFrame(this.netanalaysisframe);
            uint buflen =(uint)mbuf.Length;
            CommunicationFrameAnalysis.InitTheFrame(out proc, ref mbuf, ref buflen, ref ck, ref on);
            ushort stx = 0xaf90;
            byte stxlength = sizeof(ushort);
            CommunicationFrameAnalysis.SetFrameStx(ref proc, ref stx, ref stxlength);
          
            byte len = sizeof(ushort);
            short offest = 3;
            uint framelemgth = 5;
            CommunicationFrameAnalysis.SetFrameLen(ref proc, ref len, ref offest, ref  framelemgth);
            byte cklength = sizeof(short);
            CommunicationFrameAnalysis.SetFrameCk(ref proc, ref cklength);        
        }
        public bool OPenTheChannelCom()
        {
            return channel[0].com.Open();           
        }

       public  byte []  putcmdintocmd(ushort _cmd,ref byte []bufv,ushort buflength)
       {
           CmdManage cm;       
           pCheckSum cks= new pCheckSum(GetCRC);
           CmdFactory.CreateCmdBagFrame(out cm, CmdDefined.Cmdstructlist.CMD_fr, 0, 0,0, ref bufv, buflength, cks);
           byte[] finbuf;
           CmdBackDataErr err =    channel[0].ImplementCmd(ref cm,out finbuf);
       if(err == CmdBackDataErr.sucessful)
       {
           return finbuf;
         //  CmdDefined.MachineSendDataST backcmd = (CmdDefined.MachineSendDataST)CommonUtil.ByteToStruct(finbuf, (int)(finbuf.Length), typeof(CmdDefined.MachineSendDataST));
          // System.Windows.Forms.MessageBox.Show("ok", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
       }
       else
       {
           return finbuf;
          // System.Windows.Forms.MessageBox.Show("err time out", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
       }
             
     }


        private void GetCRC(ref byte[] message, ref byte[] CRC)
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
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);//
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }


    }
}
