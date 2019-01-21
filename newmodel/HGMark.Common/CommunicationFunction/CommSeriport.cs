using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace HGMark.Common.CommunicationFunction
{ 
    public  delegate void ComseriportDataReceiveddelegate(ref byte[] sender, Int32 length);
    class CommSeriport:mCommInterface
    {
     
        private SerialPort _sp;

		public SerialPortInfo PortInfo { get; set; }

		public Semaphore _semaphore;
		private object _syncRoot;

		private List<byte> _recvArr;
        private event ComseriportDataReceiveddelegate ongetbackbuffunction;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommSeriport()
		{
			_sp = new SerialPort();
			_sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
			_sp.ReadTimeout = 1;
			_sp.ReceivedBytesThreshold = 1;
			_semaphore = new Semaphore(0, 20);
			_syncRoot = new object();
			_recvArr = new List<byte>();
		}
      

		public override bool Open()
		{
			try
			{
				if (_sp.IsOpen)
				{
					_sp.DiscardInBuffer();
					_sp.DiscardOutBuffer();
					_sp.Close();
				}
				_sp.BaudRate = PortInfo.BaudRate;
				_sp.DataBits = PortInfo.DataBits;
				_sp.StopBits = PortInfo.StopBits;
				_sp.Parity = PortInfo.Parity;
				_sp.PortName = PortInfo.PortName;
                ongetbackbuffunction = PortInfo.Onbackbuffunction;
				_sp.Open();
			}
			catch (Exception ex)
			{
				//CommonUtil.ToLog("commErr.txt", ex);
                string mess = ex.ToString();
			}

			return _sp.IsOpen;
	 }

        public override bool Close()
		{
			if (_sp.IsOpen)
			{
				_sp.DiscardInBuffer();
				_sp.DiscardOutBuffer();
				_sp.Close();
			}

			return !_sp.IsOpen;
		}

        public override bool IsOpen()
		{
			return _sp.IsOpen;
		}
        /// <summary>
        /// 写入函数
        /// </summary>
        /// <param name="buffer">待写入的数组</param>
        public override void Write(ref byte[] buffer)
		{
			if(_sp.IsOpen)
			{
				_sp.Write(buffer, 0, buffer.Length);
			}
		}

		private void DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
		        int iByteLen = _sp.BytesToRead;
	            Byte[] recvByte = new byte[iByteLen];
		        Int32 reclenth =	_sp.Read(recvByte, 0, iByteLen);
                ongetbackbuffunction(ref recvByte, reclenth);
            //lock (_syncRoot)
            //{
            //    _recvArr.AddRange(recvByte);//
            //}
            //int count = _semaphore.Release();
		}

        public override void Read(out byte[] buffer)
        {
            _semaphore.WaitOne(); 
			lock (_syncRoot)
			{
                string ss = string.Format("Recv data length: {0}", _recvArr.Count);
                Console.WriteLine(ss);
				buffer = _recvArr.ToArray();			
				_recvArr.Clear();
			}
		}

    }
        /// <summary>
        /// 串口信息配置类
        /// </summary>
        public class SerialPortInfo
        {
            public SerialPortInfo()
            {
                portname = "COM1";
                baudrate = 9600;
                databits = 8;
                parity = Parity.None;
                stopbits = StopBits.One;
                isOpen = false;
            }
            private ComseriportDataReceiveddelegate onbackbuff;
            public ComseriportDataReceiveddelegate Onbackbuffunction
            {
                get { return onbackbuff;}

                set { onbackbuff = value;}
            }
            private string portname;
            public string PortName
            {
                get { return portname; }
                set { portname = value; }
            }

            private int baudrate;
            public int BaudRate
            {
                get { return baudrate; }
                set { baudrate = value; }
            }

            private int databits;
            public int DataBits
            {
                get { return databits; }
                set { databits = value; }
            }

            private Parity parity;
            public Parity Parity
            {
                get { return parity; }
                set { parity = value; }
            }

            private StopBits stopbits;
            public StopBits StopBits
            {
                get { return stopbits; }
                set { stopbits = value; }
            }

            private string deviceID;
            public string DeviceID
            {
                get { return deviceID; }
                set { deviceID = value; }
            }

            private bool isOpen;
            public bool IsOpen
            {
                get { return isOpen; }
                set { isOpen = value; }
            }
        }





}
