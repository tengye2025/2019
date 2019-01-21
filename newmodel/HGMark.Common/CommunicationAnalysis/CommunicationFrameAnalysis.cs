using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HGMark.Common.CommunicationAnalysis
{

    /// <summary>
    /// 解析模式
    /// </summary>
public enum AnalyticModel
{
    None = 0,
	StxandEtx =1,
	LengthandCk =2
};

    /// <summary>
    /// 帧元素结构体
    /// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct  ProtocolElement
{
public	byte  []Data;
public	byte  DataLen;
public	byte  Enabled;
public	Int16 FrontOffset;
public  Int16 BackOffset;

};
/// <summary>
/// 校验函数委托
/// </summary>
/// <param name="buf"></param>
/// <param name="bufLen"></param>
/// <returns></returns>
public    delegate void pCheckSum(ref byte []buf, ref byte []crc);
    /// <summary>
    /// 回调响应函数委托
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="bufLen"></param>
public    delegate void pOnGetFrame(ref byte []buf, uint bufLen);

// 帧类型
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public  struct  ProtocolDataStxEtxandLen
{
public	ProtocolElement   stx;                // 起始位
public	ProtocolElement   etx;                // 结束位
public  ProtocolElement   len;                // 数据长度
public	ProtocolElement   ck;                 // 校验
public  uint              frameheadlength;    // 帧头长度
public	byte              []buf;              // 缓冲区
public  uint              bufLen;             // 缓冲区长度
public  uint              datCount;           // 缓冲区内有效字节长度
public	pCheckSum         CheckSum;           // 校验函数
public	pOnGetFrame       OnGetFrame;         // 获取帧成功后回调函数
public	AnalyticModel     model;              // 解析模式
};



    /// <summary>
    /// 帧解析类
    /// </summary>
 sealed public class CommunicationFrameAnalysis
{

/// <summary>
/// 初始化帧对象
/// </summary>
/// <param name="p"></param>
/// <param name="buf"></param>
/// <param name="buflength"></param>
/// <param name="checksum"></param>
/// <param name="ongetframe"></param>
static public  void InitTheFrame(out ProtocolDataStxEtxandLen p,ref byte []buf,ref uint buflength,ref pCheckSum checksum,ref pOnGetFrame ongetframe) 
{
    Debug.Assert(buf !=null);
    Debug.Assert(buflength >0);
    Debug.Assert(checksum != null);
    Debug.Assert(ongetframe != null);

    p = new ProtocolDataStxEtxandLen();
    p.buf = buf;
    p.bufLen = buflength;
    p.CheckSum = checksum;
    p.OnGetFrame = ongetframe;
    p.datCount = 0;
    p.model = AnalyticModel.None;
}


/// <summary>
/// 设置帧头
/// </summary>
/// <param name="p"></param>
/// <param name="stx"></param>
/// <param name="stxlength"></param>
static public  void SetFrameStx(ref ProtocolDataStxEtxandLen p,ref ushort stx ,ref byte stxlength)
{

    Debug.Assert(stx != 0x0000);
    Debug.Assert(stxlength > 0);  
    p.stx.Data = CommonUtil.ObjectToBytes(stx, stxlength);
    p.stx.DataLen = stxlength;
    p.stx.Enabled = 1;
    p.stx.FrontOffset = 0;
    p.stx.BackOffset = -1;//unuseful  

}
    /// <summary>
    /// 设置帧尾
    /// </summary>
    /// <param name="p"></param>
    /// <param name="stx"></param>
    /// <param name="stxlength"></param>
static public void SetFrameEtx(ref ProtocolDataStxEtxandLen p, ref ushort etx, ref byte etxlength)
{
    Debug.Assert(etx != 0x0000);
    Debug.Assert(etxlength > 0);
    Debug.Assert(p.len.Enabled == 0);
    p.etx.Data = CommonUtil.ObjectToBytes(etx, etxlength);
    p.etx.DataLen = etxlength;
    p.etx.Enabled = 1;
    p.etx.FrontOffset = -1;  
    p.etx.BackOffset = 0;  
    p.model = AnalyticModel.StxandEtx;

}
    /// <summary>
    /// 设置校验
    /// </summary>
    /// <param name="p"></param>
    /// <param name="cx"></param>
    /// <param name="cxlength"></param>
static public void SetFrameCk(ref ProtocolDataStxEtxandLen p, ref byte cxlength)
{
    Debug.Assert(cxlength > 0);
    p.ck.DataLen = cxlength;
    p.ck.Enabled = 1;
    p.ck.BackOffset = p.etx.DataLen;
    p.ck.FrontOffset = -1;

}
    /// <summary>
    /// 设置数据长度元素
    /// </summary>
    /// <param name="p"></param>
    /// <param name="lenlength"></param>
    /// <param name="frameheadlength"></param>
static public void SetFrameLen(ref ProtocolDataStxEtxandLen p, ref byte lenlength, ref short lenFrontOffset, ref uint frameheadlength)
{
    Debug.Assert(frameheadlength > 0);
    Debug.Assert(lenlength == 2);
    Debug.Assert(p.etx.Enabled == 0);

    p.len.DataLen = lenlength;
    p.len.Enabled = 1;
    p.len.FrontOffset = lenFrontOffset;
    p.len.BackOffset = -1;
    p.frameheadlength = frameheadlength;
    p.etx.Enabled = 0;
    p.model = AnalyticModel.LengthandCk;

}
/// <summary>
/// 把数据放入帧中
/// </summary>
/// <param name="p"></param>
/// <param name="buf"></param>
/// <param name="buflength"></param>
static public void PutBufDataIntoTheFrame(ref ProtocolDataStxEtxandLen p,ref byte []buf, ref uint buflength)
{
    Debug.Assert(buf !=null);
    Debug.Assert(buflength > 0);

    if (buflength + p.datCount > p.bufLen)
    {
        Debug.Assert(false);
        return;
    }
    else
    {
        for(int a = 0;a <buflength;a++)
        {
            p.buf[p.datCount + a] = buf[a];
        }
        p.datCount = p.datCount + buflength;

    }


}
   /// <summary>
   /// 解析帧对象
   /// </summary>
   /// <param name="p"></param>
static public void AnalyzeTheFrameData(ref ProtocolDataStxEtxandLen p)
{

    Debug.Assert(p.model != AnalyticModel.None);
    if (p.model == AnalyticModel.StxandEtx)
    {
        Debug.Assert(p.stx.DataLen > 0);
        Debug.Assert(p.etx.DataLen > 0);
        Debug.Assert(p.ck.DataLen > 0);
        Debug.Assert(p.CheckSum != null);
        Debug.Assert(p.OnGetFrame != null);

        for (uint a = 0; a < p.datCount; a++)
        {
            if (ArryMemcmp(ref p.stx.Data, 0, ref p.buf, a, p.stx.DataLen) == true)//find stx
            {

                for (uint b = a + p.stx.DataLen; b < p.datCount; b++)
                {
                    if (ArryMemcmp(ref p.etx.Data, 0, ref p.buf, b, p.etx.DataLen) == true)//find etx
                    {

                        if ((b - a - p.etx.DataLen) > p.stx.DataLen)
                        {
                           
                            byte[] workareabuf = new byte[b - a+p.etx.DataLen];

                            for (uint m = 0; m < b-a - p.ck.BackOffset; m++)
                            {
                                workareabuf[m] = p.buf[a + m];
                            }

                            byte []crcf = new byte[p.ck.DataLen];

                            p.CheckSum(ref workareabuf, ref crcf);  
                       
                            byte[] crcs = new byte[p.ck.DataLen];

                            for (uint i = 0; i < p.ck.DataLen; i++)
                            {
                                if ( (p.ck.BackOffset > 0) && (p.ck.FrontOffset <0))
                                {
                                    crcs[i] = p.buf[b - p.ck.BackOffset + i];
                                }
                                else
                                {
                                    crcs[i] = p.buf[a + p.ck.FrontOffset + i];
                                }
                            }
                            if (ArryMemcmp(ref crcf, 0, ref crcs, 0, p.ck.DataLen) == true)
                            {
                                byte[] workareaongetframebackbuf = new byte[b - a + p.etx.DataLen];
                                for (uint m = 0; m < b - a + p.etx.DataLen; m++)
                                {
                                    workareaongetframebackbuf[m] = p.buf[a + m];
                                }
                                p.OnGetFrame(ref workareaongetframebackbuf, b - a + p.etx.DataLen);//on back function
                             
                                byte []pbufremainarry = new byte[p.datCount - (b-a+p.etx.DataLen)];
                                for (uint o = 0; o < (p.datCount -(b-a +p.etx.DataLen)); o++)
                                {
                                    pbufremainarry[o] = p.buf[b + p.etx.DataLen + o];
                                }

                                for (uint j = 0; j < a+p.datCount; j++)
                                {
                                    p.buf[j] = 0;
                                }
                                uint sizeremainarry =  (uint)pbufremainarry.Length;
                                for (uint k = 0; k < sizeremainarry; k++)
                                {
                                    p.buf[k] = pbufremainarry[k];
                                }
                                p.datCount = p.datCount - (b - a + p.etx.DataLen);
                                break;
                            }
                            else
                            {

                             }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }                  
               }
            }
            else
            {

            }
        }

    }
    else if (p.model == AnalyticModel.LengthandCk)
    {
        Debug.Assert(p.stx.DataLen > 0);
        Debug.Assert(p.len.DataLen > 0);
        Debug.Assert(p.ck.DataLen > 0);
        Debug.Assert(p.CheckSum != null);
        Debug.Assert(p.OnGetFrame != null);
        Debug.Assert(p.frameheadlength > 3);

        for (uint a = 0; a < p.datCount; a++)
        {
            if (ArryMemcmp(ref p.stx.Data, 0, ref p.buf, a, p.stx.DataLen) == true)//find stx
            {                      
                    ushort datalen = 0;
                    if(p.len.FrontOffset>0 && p.len.BackOffset <0)
                    {
                        datalen = (ushort)((((p.buf[a + p.len.FrontOffset]) & 0x00ff) << 8) | ((p.buf[a + p.len.FrontOffset + 1]) & 0x00ff));
                     }
                    else
                    {

                    }
                   
                   byte[] withoutckframebuf = new byte[p.frameheadlength + datalen+p.ck.DataLen];
                   for (int m = 0; m < p.frameheadlength + datalen; m++)
                   {
                       withoutckframebuf[m] = p.buf[a + m];
                   }
                   byte[] crc = new byte[p.ck.DataLen];
                   p.CheckSum(ref withoutckframebuf, ref crc);
                   if (ArryMemcmp(ref crc, 0, ref p.buf, a +  p.frameheadlength + datalen , p.ck.DataLen) == true)
                   {           
                       byte[] workareaongetframebackbuf = new byte[p.frameheadlength + datalen + p.ck.DataLen];
                       for (int n = 0; n < p.frameheadlength + datalen + p.ck.DataLen; n++)
                       {
                           workareaongetframebackbuf[n] = p.buf[a + n];
                       }
                       p.OnGetFrame(ref workareaongetframebackbuf, p.frameheadlength + datalen + p.ck.DataLen);//on back function
                       byte[] pbufremainarry = new byte[p.datCount - (p.frameheadlength + datalen+p.ck.DataLen)];
                       for (uint o = 0; o < (p.datCount - (p.frameheadlength + datalen + p.ck.DataLen)); o++)
                       {
                           pbufremainarry[o] = p.buf[a+p.frameheadlength + datalen + p.ck.DataLen + o];
                       }

                       for (uint j = 0; j < a + p.datCount; j++)
                       {
                           p.buf[j] = 0;
                       }

                       uint sizeremainarry = (uint)pbufremainarry.Length;
                       for (uint k = 0; k < sizeremainarry; k++)
                       {
                           p.buf[k] = pbufremainarry[k];
                       }
                       p.datCount = p.datCount - (p.frameheadlength + datalen + p.ck.DataLen);
                       break;
                   }
                   else
                   {

                   }
            }
            else
            {

            }
        }
    }
    else
    {
        Debug.Assert(false);
    }

}


static private bool ArryMemcmp(ref byte[] bufa, uint sbufaindex, ref byte[] bufb, uint sbufbindex, byte AountComparison)
{
    for (int a = 0; a < AountComparison; a++)
    {
        if (bufa[sbufaindex + a] == bufb[sbufbindex + a])
        {

        }
        else
        {
            return false;
        }
    }
    return true; 
}
static public void GetCRC(ref byte[] message, ref byte[] CRC)
{
    Debug.Assert(message.Length > 2);
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
