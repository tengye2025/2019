using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace HGMark.Common.CmdLib
{
  public  class CmdDefined
    {
       [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Cmdstructlist
       {   
        public const ushort CMD_fr = 0x0000;
        public const ushort CMD_tw = 0x0001;
        public const ushort CMD_tr = 0x0002;
       }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]  
      
        public  struct MachineSendDataST_none
        {           
            public ushort stx;        
            public ushort cmd;              
            public byte   sequence;         
            public byte   ack;           
            public byte   err;      
            public byte   channel;//通道          
            public ushort datalength;         
            public byte   []pdatabuf;            
            public ushort checksum;         
            public ushort etx;
        }
        public struct MachineSendDataST
        {
            public ushort stx;
            public ushort cmd;
            public byte sequence;
            public byte ack;
            public byte err;
            public byte channel;//通道          
            public ushort datalength;
            [MarshalAs(UnmanagedType.ByValArray,SizeConst =1)]
            public byte[] pdatabuf;
            public ushort checksum;
            public ushort etx;
        }
      
    }
}
