using System;
using System.Collections.Generic;//List
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HG_EVENT;

namespace HG_Socket
{
    public struct NetInfo
    {
        public NetworkStream netStream;
        public string strIP;
    }

    public class MyServer
    {
        private TcpListener m_Listener;//监听对象
        private bool bStopListen = false;
        public List<NetInfo> _streams = new List<NetInfo>();
        private List<TcpClient> _clients = new List<TcpClient>();
        private List<Thread> _threads = new List<Thread>();
        private Thread m_ConnectThread;
        private int m_nPort;
        private IPAddress m_Address;
        private Form m_parentForm = null;

        private event ClientDataReceivedEventHandler ClientDataReceived;

        public MyServer(Form FMain, ClientDataReceivedEventHandler delegatefc, string strIp = "127.0.0.1", int nPort = 9001)
        {
            m_parentForm = FMain;
            m_Address = IPAddress.Parse(strIp);
            m_nPort = nPort;
            ClientDataReceived += delegatefc;
        }

        /// <summary>
        ///  监听线程
        /// </summary>
        public void ListenClient()
        {
            try
            {
                m_ConnectThread = new Thread(new ThreadStart(ConnectToClient));
                m_ConnectThread.IsBackground = true;
                m_ConnectThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConnectToClient()
        {
            try
            {
                m_Listener = new TcpListener(m_Address, m_nPort);
                m_Listener.Start();//开始监听
            }
            catch (SocketException se)
            {
                Close();
                MessageBox.Show(se.Message);
                return;
            }
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
                        ClientDataReceived("客户端已连接" + ipi.Address.ToString(), "");
                    }
                    else
                    {
                        MessageBox.Show("委托事件为空！");
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
                    MessageBox.Show("异常:" + ex.Message);
                }
            }
        }

        public bool SendData(string strip, string data)
        {
            foreach (NetInfo net in _streams)
            {
                if (net.strIP == strip)
                {
                    net.netStream.Write(Encoding.Default.GetBytes(data), 0, data.Length);
                    return true;
                }
            }
            return false;
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
                // a bad connection, couldn't get the NetworkStream
                if (netStream != null)
                    netStream.Close();
                MessageBox.Show(ex.Message);
                return;
            }
            if (netStream.CanRead)
            {
                myNetInfo.netStream = netStream;
                _streams.Add(myNetInfo);
            }
            byte[] receiveBuffer = new byte[1024];
            int bytesReceived = 0;
            try
            {
                while (!bStopListen && (bytesReceived = netStream.Read(receiveBuffer, 0, receiveBuffer.Length)) > 0)
                {
                    string rec = Encoding.Default.GetString(receiveBuffer, 0, bytesReceived);
                    ClientDataReceived(myNetInfo.strIP, rec);
                }
                _streams.RemoveAt(_streams.Count - 1);
                if (bytesReceived == 0)
                {
                    ClientDataReceived("客户端已断开" + myNetInfo.strIP.ToString(), "");
                }

            }
            catch (System.Exception)
            {
                return; //客户端断开了
            }
            finally
            {
                System.GC.Collect();
            }
        }

        public void Close()
        {
            if (m_Listener != null)
            {
                bStopListen = true;
                // First, close TCPListener
                m_Listener.Stop();
                // Wait for the server thread to terminate.
                m_ConnectThread.Abort();
                // close all client streams
                for (int i = 0; i < _streams.Count; i++)
                {
                    _streams[i].netStream.Flush();
                    _streams[i].netStream.Close();

                    _streams.Clear();
                }
                // close the client connection
                foreach (TcpClient client in _clients)
                    client.Close();
                _clients.Clear();

                // wait for all client threads to terminate
                foreach (Thread t in _threads)
                    t.Join();
                _threads.Clear();
            }
        }

    }
}