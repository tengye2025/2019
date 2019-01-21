using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HGMark.Common.CommunicationAnalysis;
using System.Runtime.InteropServices;
namespace HGMark.Common.CmdLib
{
    #region 协议说明

/*
 * cmd format like that :  frame head +........+ ck + frame end
 //

 */


    #endregion


    public  class CmdFactory
    {

        static private SequenceGenerator sr = new SequenceGenerator();
        static private List<byte> listbuf = new List<byte>();
        static public void CreateCmdBagFrame(out CmdManage cm, ushort cmd,byte channel,byte ack,byte err,ref byte []buf,ushort buflength,pCheckSum checksum)
        {
            listbuf.Clear();
            CmdDefined.MachineSendDataST_none cmdstframe = new CmdDefined.MachineSendDataST_none();
            cmdstframe.stx = 0xAF90;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.stx, 2));
            cmdstframe.cmd = (ushort)cmd;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.cmd, 2));
            cmdstframe.sequence = sr.GenerateSeqNo();
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.sequence, 1));
            cmdstframe.ack = ack;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.ack , 1));
            cmdstframe.err = err;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.err, 1));
            cmdstframe.channel = channel;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.channel, 1));
            cmdstframe.datalength = buflength;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.datalength, 2));
            cmdstframe.pdatabuf = buf;
            listbuf.AddRange(buf);

            byte[] sendbufwithoutcketx = listbuf.ToArray();
            byte[] sendbuf = new byte[14 + buflength];

            Array.Copy(sendbufwithoutcketx, sendbuf, sendbufwithoutcketx.Length);

            byte []crc =new byte[2];
            checksum(ref sendbuf, ref crc);
            ushort checksume = (ushort)((crc[0] & 0x00ff) | ((crc[1] & 0x00ff) << 8));
            cmdstframe.checksum = checksume;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.checksum, 2));
            cmdstframe.etx = 0xEB81;
            listbuf.AddRange(CommonUtil.StructToBytes(cmdstframe.etx, 2));

            byte[] sendbuferts = listbuf.ToArray();
            cm = new CmdManage();
            cm.SetTimeout(5000);
            cm.LoadSendData(ref sendbuferts);
          
        }





    }




}
