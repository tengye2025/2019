using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace HGMark.Common.CmdLib
{
    public enum CmdStatus
    {
        StatusBeforeSend = 0,
        StatusAfterSend,
        StatusReceiveComplete,
        StatusCommFail,
        StatusTimeout,
        StatusUnknown
    }
    /// <summary>
    /// 序列号产生类
    /// </summary>
    class SequenceGenerator
    {
        private object _syncObj;		 //同步对象
        private byte _baseSequence;    //序列号

        public SequenceGenerator()
        {
            _syncObj = new object();
            _baseSequence = 0;
        }
        public byte GenerateSeqNo()
        {
            lock (_syncObj)
            {
                _baseSequence++;
                if (_baseSequence == 0)
                {
                    _baseSequence = 1;
                }
                if (_baseSequence >200)
                {
                    _baseSequence = 1;
                }
                return _baseSequence;
            }
        }
    }

    public  class CmdManage
    {

        private AutoResetEvent _event;
        private ushort timeoutset = 0;

        private List<byte> sendbuf;
        private List<byte> recevbuf;

        public CmdManage()
        {
            _event = new AutoResetEvent(false);
            timeoutset = 0;  
            sendbuf = new List<byte>();
            recevbuf = new List<byte>();
        }

        public void SetTimeout(ushort t)
        {
            timeoutset  = t;
        }

      
        public CmdStatus WaitForFinished()
        {
            if (_event.WaitOne(timeoutset))
            {
                return CmdStatus.StatusReceiveComplete;
            }
            else
            {
                return CmdStatus.StatusTimeout;
            }
        }
        public void EvetSet()
        {
            _event.Set();
        }

        public void LoadSendData(ref byte []recvArr)
        {
            sendbuf.AddRange(recvArr);
        }

        public void GetSendData(out byte[] sendvArr)
        {
           sendvArr = sendbuf.ToArray();
        }
        public void ResetSendBuffer()
        {
            sendbuf.Clear();
        }

        public void LoadRecveData(ref byte[] recvArr)
        {
            recevbuf.AddRange(recvArr);
        }

        public void GetRecveBuf(out byte [] buf)
        {
            buf = recevbuf.ToArray(); 
        }

        public void ResetRevceBuffer()
        {
            recevbuf.Clear();
        }


    }
}
