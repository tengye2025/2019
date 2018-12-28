using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ModbusClassLibrary.CommunicationCom
{

    public delegate void ComnetDataReceiveddelegate(ref byte[] buf, uint length);
    public delegate void ClienceConnectServerComneteventdelegate(ref string cip, ref string str);
    public delegate void InitiativeGetDataeventdelegate(ushort us, ref byte[] buf);
    public enum netApplyInterfaceChose
    {
        Clience,
        Server
    }

    public class NetInterfaceinfo
    {
        private string ip;
        private Int16 port;
        private netApplyInterfaceChose cors;
        private ComnetDataReceiveddelegate getbuf;//get data delegate
        private ClienceConnectServerComneteventdelegate clienceconnectserverevent;

        public ClienceConnectServerComneteventdelegate Onclienceconnectserverevent
        {
            get { return clienceconnectserverevent; }
            set { clienceconnectserverevent = value; }
        }

        public ComnetDataReceiveddelegate Ongetbufbackfunction
        {
            get { return getbuf; }
            set { getbuf = value; }
        }
        public Int16 Port
        {
            get { return port; }
            set { port = value; }
        }

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public netApplyInterfaceChose CorS
        {
            get { return cors; }
            set { cors = value; }
        }

    }
    /// <summary>
    //服务器端类
    /// </summary>
    public struct NetInfo
    {
        public NetworkStream netStream;
        public string strIP;
    }
    public class MyServer : mCommInterface
    {
        private TcpListener m_Listener;
        private bool bStopListen = false;
        public List<NetInfo> _streams = new List<NetInfo>();
        private List<TcpClient> _clients = new List<TcpClient>();
        private List<Thread> _threads = new List<Thread>();
        private Thread m_ConnectThread;
        private int m_nPort;
        private IPAddress m_Address;
        public NetInterfaceinfo info;
        private event ComnetDataReceiveddelegate ClientDataReceived;
        private event ClienceConnectServerComneteventdelegate clienceconnectedevent;
        public MyServer()
        {

        }

        public override bool Open()
        {
            m_Address = IPAddress.Parse(info.Ip);
            m_nPort = info.Port;
            if (info.Ongetbufbackfunction == null | info.Onclienceconnectserverevent == null)
            {
                return false;
            }
            else
            {
                ClientDataReceived = info.Ongetbufbackfunction;
                clienceconnectedevent = info.Onclienceconnectserverevent;
            }

            try
            {
                if (ListenClient())
                {

                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string p = ex.ToString();
                return false;
            }
        }
        /// <summary>
        ///  监听线程
        /// </summary>
        private bool ListenClient()
        {
            try
            {
                m_Listener = new TcpListener(m_Address, m_nPort);
                m_Listener.Start();
                m_ConnectThread = new Thread(new ThreadStart(ConnectToClient));
                m_ConnectThread.IsBackground = true;
                m_ConnectThread.Start();
                return true;
            }
            catch (Exception ex)
            {
                string p = ex.ToString();
             // System.Windows.Forms.MessageBox.Show(p);
                Close();
                return false;
            }
        }

        private void ConnectToClient()
        {
            try
            {
                while (!bStopListen)
                {
                    TcpClient Client = m_Listener.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart(ReceiveDataFromClient));
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.AboveNormal;
                    t.Name = "client handle";
                    t.Start(Client);
                    _threads.Add(t);
                    _clients.Add(Client);
                    IPEndPoint ipi = (IPEndPoint)Client.Client.RemoteEndPoint;
                    if (ClientDataReceived != null)
                    {

                    }
                    else
                    {

                    }
                }
            }
            catch (SocketException ex)
            {
                if (ex.Message.Contains("WSACancelBlockingCall"))
                {
                }
                else
                {

                }
            }
        }

        public override void Write(ref byte[] buffer)
        {
            SendData(GetClienceip(), ref buffer);
        }
        private bool SendData(string ip, ref byte[] data)
        {
            foreach (NetInfo net in _streams)
            {
                if (net.strIP == ip)
                {
                    net.netStream.Write(data, 0, data.Length);
                    return true;
                }
            }
            return false;
        }
        public string GetClienceip()
        {
            return _streams[0].strIP;
        }

        private void ReceiveDataFromClient(object clientObject)
        {
            TcpClient client = clientObject as TcpClient;
            Socket s = client.Client;
            IPEndPoint pp = (IPEndPoint)s.RemoteEndPoint;
            NetInfo myNetInfo = new NetInfo();
            myNetInfo.strIP = pp.Address.ToString();

            NetworkStream netStream = null;
            try
            {
                netStream = client.GetStream();
            }
            catch (Exception ex)
            {
                string p = ex.ToString();
                if (netStream != null)
                    netStream.Close();
                return;
            }
            if (netStream.CanRead)
            {
                myNetInfo.netStream = netStream;
                _streams.Add(myNetInfo);
                string strcon = "CON";
                clienceconnectedevent(ref myNetInfo.strIP, ref strcon);
            }
            byte[] receiveBuffer = new byte[1024];
            uint bytesReceived = 0;
            try
            {

                while (!bStopListen && (bytesReceived = (uint)netStream.Read(receiveBuffer, 0, receiveBuffer.Length)) > 0)
                {
                    byte[] newreceiveBuffer = new byte[bytesReceived];
                    for (int a = 0; a < bytesReceived; a++)
                    {
                        newreceiveBuffer[a] = receiveBuffer[a];
                    }
                    ClientDataReceived(ref newreceiveBuffer, (uint)newreceiveBuffer.Length);
                }
                _streams.RemoveAt(_streams.Count - 1);
                if (bytesReceived == 0)
                {
                    string strcon = "COFF";
                    clienceconnectedevent(ref myNetInfo.strIP, ref strcon);
                }
            }
            catch (System.Exception)
            {
                return;
            }
            finally
            {
                System.GC.Collect();
            }
        }

        public override bool Close()
        {
            if (m_Listener != null)
            {
                try
                {
                    bStopListen = true;
                    m_Listener.Stop();
                    m_ConnectThread.Abort();
                    for (int i = 0; i < _streams.Count; i++)
                    {
                        _streams[i].netStream.Flush();
                        _streams[i].netStream.Close();

                        _streams.Clear();
                    }
                    foreach (TcpClient client in _clients)
                        client.Close();
                    _clients.Clear();
                    foreach (Thread t in _threads)
                        t.Join();
                    _threads.Clear();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

    }
    /// <summary>
    /// 客户端类
    /// </summary>
    public class MyClient : mCommInterface
    {
        private TcpClient m_client;

        private int m_nPort;

        private NetworkStream m_NetStream;
        private IPAddress m_Address;
        public bool m_bConnect = false;
        private event ComnetDataReceiveddelegate ServerDataReceived;
        public NetInterfaceinfo info;

        public MyClient()
        {
            m_client = new TcpClient();
        }

        public override bool Open()
        {
            m_Address = IPAddress.Parse(info.Ip);
            m_nPort = info.Port;
            ServerDataReceived = info.Ongetbufbackfunction;

            if (!ConnectToServer())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ConnectToServer()
        {
            try
            {
                try
                {
                    m_client.Connect(m_Address, m_nPort);
                }
                catch (Exception e)
                {
                    string p = e.ToString();
                    return false;
                }
                m_NetStream = m_client.GetStream();
                m_bConnect = true;
                Thread myThread = new Thread(new ParameterizedThreadStart(recfromserver));
                myThread.IsBackground = true;
                myThread.Start(this);
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }
        public override bool IsOpen()
        {
            return m_bConnect;
        }

        public int ReadData(out byte[] strReadbuf, out uint buflength)
        {
            strReadbuf = new byte[1024];
            buflength = 0;
            if (!m_client.Connected)
            {
                return -1;
            }
            if (m_NetStream == null)
            {
                return -1;
            }

            if (m_NetStream.CanRead)
            {
                try
                {
                    if (m_client.ReceiveBufferSize > 0)
                    {
                        int bytesReceived = m_NetStream.Read(strReadbuf, 0, strReadbuf.Length);//
                        m_NetStream.Flush();
                        buflength = (uint)bytesReceived;
                        if (bytesReceived <= 0)
                        {
                            Close();
                            return -2;
                        }
                    }
                }
                catch (Exception)
                {
                    return -3;
                }
            }
            return 0;
        }

        private void recfromserver(object p)
        {
            while (true)
            {
                byte[] buf;
                uint buflength;
                int readresult = ReadData(out buf, out buflength);
                if (readresult == 0)
                {
                    ServerDataReceived(ref buf, buflength);
                }
                else if (readresult == -2 || readresult == -3)
                {
                    break;
                }
            }

        }

        public override void Write(ref byte[] buffer)
        {
            WriteData(ref buffer);
        }

        public bool WriteData(ref byte[] strSend)
        {
            if (m_NetStream == null)
            {

                return false;
            }
            if (!m_NetStream.CanWrite)
            {

                return false;
            }
            try
            {
                m_NetStream.Flush();
                m_NetStream.Write(strSend, 0, strSend.Length);
            }
            catch (Exception ex)
            {
                string p = ex.ToString();
                return false;
            }
            return true;
        }

        public override bool Close()
        {
            try
            {
                if (m_NetStream != null)
                {
                    m_NetStream.Close();
                    m_NetStream = null;
                }
                if (m_client != null)
                {
                    m_client.Close();
                    m_client = null;
                }
                m_bConnect = false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


}
