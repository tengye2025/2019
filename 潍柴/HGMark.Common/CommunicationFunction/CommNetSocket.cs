using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HGMark.Common.CommunicationFunction
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
        private ComnetDataReceiveddelegate Getbuf;//
        private ClienceConnectServerComneteventdelegate ClienTConnectserverevent;

        public ClienceConnectServerComneteventdelegate OnClienceConnectServerEvent
        {
            get { return ClienTConnectserverevent; }
            set { ClienTConnectserverevent = value; }
        }



        public ComnetDataReceiveddelegate OnGetBufBackFunction
        {
            get { return Getbuf; }
            set { Getbuf = value; }
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
    public struct NetWorkStreamST //数据流结构体
    {
        public NetworkStream networkstream;
        public string thisnetworkstreamIP;
        public int port;
    }
    public class MyServer: mCommInterface
    {
        private TcpListener m_Listener;//监听对象
        private bool bStopListen = false;
        public List<NetWorkStreamST> networkstreamlist = new List<NetWorkStreamST>();
        private List<TcpClient> clientlist = new List<TcpClient>();
        private List<Thread> clientthreadslist = new List<Thread>();
        private Thread m_ConnectThread;
        private int m_nPort;
        private IPAddress m_Address;
        private NetInterfaceinfo info;
        private event ComnetDataReceiveddelegate ClientDataReceived;
        private event ClienceConnectServerComneteventdelegate ClienceConnectedEvent;
        public  MyServer(ref NetInterfaceinfo PINFO)
        {
            info = PINFO;
        }

        public override bool Open()
        {
            m_Address = IPAddress.Parse(info.Ip);
            m_nPort = info.Port;
            if (info.OnGetBufBackFunction == null | info.OnClienceConnectServerEvent == null)
            {
                return false;
            }
            else
            {
                ClientDataReceived = info.OnGetBufBackFunction;
                ClienceConnectedEvent = info.OnClienceConnectServerEvent;
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

                System.Windows.Forms.MessageBox.Show(p);
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
                    TcpClient Client = m_Listener.AcceptTcpClient();//监听对象，获取客户端

                    //开始创建连接对象
                    Thread Clientthread = new Thread(new ParameterizedThreadStart(ReceiveDataFromClient));
                    Clientthread.IsBackground = true;
                    Clientthread.Priority = ThreadPriority.AboveNormal;
                    Clientthread.Name = "client handle";
                    Clientthread.Start(Client);
                    clientthreadslist.Add(Clientthread);
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

        public  override  void Write(string ip, ref byte[] buffer)
        {
            SendData(ip, ref buffer);
        }
        private bool SendData(string ip, ref byte[] data)
        {
            foreach (NetWorkStreamST net in networkstreamlist)
            {
                if (net.thisnetworkstreamIP == ip)
                {
                    net.networkstream.Write(data, 0, data.Length);
                    return true;
                }
            }
            return false;
        }


        private void ReceiveDataFromClient(object clientObject)
        {


            ClienceConnectServerComneteventdelegate ClienceConnectedEventw = new ClienceConnectServerComneteventdelegate(info.OnClienceConnectServerEvent);
            ComnetDataReceiveddelegate ClientDataReceivedw = new ComnetDataReceiveddelegate(info.OnGetBufBackFunction);
            TcpClient client = clientObject as TcpClient;
            clientlist.Add(client);

            IPAddress ipaddress = (client.Client.RemoteEndPoint as IPEndPoint).Address;
            int port = (client.Client.RemoteEndPoint as IPEndPoint).Port;
            NetWorkStreamST myNetWorkStreamInfo = new NetWorkStreamST();
            myNetWorkStreamInfo.thisnetworkstreamIP = ipaddress.ToString();
            myNetWorkStreamInfo.port = port;

            NetworkStream netStream = null;
            try
            {
                netStream = client.GetStream();//获取流               
            }
            catch (Exception ex)
            {
                string p = ex.ToString();
                System.Windows.Forms.MessageBox.Show(p);
                if (netStream != null)
                {
                    netStream.Close();
                }
                return;
            }

            if (netStream.CanRead)
            {
                myNetWorkStreamInfo.networkstream = netStream;
                networkstreamlist.Add(myNetWorkStreamInfo);//流列表
                string strcon = "ClientConnected";
                ClienceConnectedEventw(ref myNetWorkStreamInfo.thisnetworkstreamIP, ref strcon);
            }
            byte[] receiveBuffer = new byte[1024];
            uint bytesReceived = 0;
            try
            {
                while (true)
                {
                    if (!bStopListen)
                    {

                    }
                    else
                    {
                        break;
                    }
                    bytesReceived = (uint)netStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                    if (bytesReceived > 0)
                    {
                        byte[] newreceiveBuffer = new byte[bytesReceived];
                        for (int a = 0; a < bytesReceived; a++)
                        {
                            newreceiveBuffer[a] = receiveBuffer[a];
                        }
                        ClientDataReceivedw(ref newreceiveBuffer, (uint)newreceiveBuffer.Length);
                    }
                    else
                    {
                        if (bytesReceived == 0)
                        {
                            string strcon = "ClientDisConnected";
                            ClienceConnectedEventw(ref myNetWorkStreamInfo.thisnetworkstreamIP, ref strcon);
                        }
                        else
                        {

                        }
                        netStream.Close();
                        client.Close();
                        networkstreamlist.Remove(myNetWorkStreamInfo);
                        clientlist.Remove(client);
                        break;
                    }
                }


            }
            catch (System.Exception ex)
            {
                string p = ex.ToString();
                System.Windows.Forms.MessageBox.Show(p);
                netStream.Close();
                client.Close();
                networkstreamlist.Remove(myNetWorkStreamInfo);
                clientlist.Remove(client);
                return;
            }
            finally
            {
                netStream.Close();
                client.Close();
                networkstreamlist.Remove(myNetWorkStreamInfo);
                clientlist.Remove(client);
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
                    for (int i = 0; i < networkstreamlist.Count; i++)
                    {
                        networkstreamlist[i].networkstream.Flush();
                        networkstreamlist[i].networkstream.Close();
                        networkstreamlist.Clear();
                    }

                    foreach (TcpClient client in clientlist)
                    {
                        client.Close();
                    }
                    clientlist.Clear();
                    foreach (Thread t in clientthreadslist)
                    {
                        t.Join();
                    }
                    clientthreadslist.Clear();
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
    public class MyClient: mCommInterface
    {
        private TcpClient client;

        private int m_nPort;

        private NetworkStream m_NetStream;
        private IPAddress m_Address;
        public bool m_bConnect = false;
        private event ComnetDataReceiveddelegate ServerDataReceived;  
        private event ClienceConnectServerComneteventdelegate DisConnectToServerEvent;
        private NetInterfaceinfo info;

        public MyClient(ref NetInterfaceinfo pinfo)
        {
            client = new TcpClient();
            info = pinfo;
        }

        public override bool Open()
        {
            m_Address = IPAddress.Parse(info.Ip);
            m_nPort = info.Port;
            ServerDataReceived = info.OnGetBufBackFunction;
            DisConnectToServerEvent = info.OnClienceConnectServerEvent;
            if (!ConnectToServer())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ConnectToServer()
        {
            try
            {
                try
                {
                    client.Connect(m_Address, m_nPort);
                }
                catch (Exception e)
                {
                    string p = e.ToString();
                    return false;
                }
                m_NetStream = client.GetStream();
                m_bConnect = true;
                Thread myThread = new Thread(new ParameterizedThreadStart(RecFromServer));
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
            if (!client.Connected)
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
                    int bytesReceived = m_NetStream.Read(strReadbuf, 0, strReadbuf.Length);//
                    m_NetStream.Flush();
                    buflength = (uint)bytesReceived;
                    if (bytesReceived <= 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception)
                {
                    return -2;
                }
            }

            return 0;
        }

        private void RecFromServer(object p)
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
                else if (readresult == -1 || readresult == -2)
                {
                    byte[] buferr = System.Text.Encoding.ASCII.GetBytes("DisConnected");
                    ServerDataReceived(ref buferr, 0);
                    string strerrinfo = "DisConnected";
                    string strerrstr =  " ";
                    DisConnectToServerEvent(ref strerrinfo,ref strerrstr);
                    Close();
                    break;
                }
            }

        }

        public override void Write(ref byte[] buffer)
        {
            WriteData(ref buffer);
        }

        public  bool WriteData(ref byte[] strSend)
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
                if (client != null)
                {
                    client.Close();
                    client = null;
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
