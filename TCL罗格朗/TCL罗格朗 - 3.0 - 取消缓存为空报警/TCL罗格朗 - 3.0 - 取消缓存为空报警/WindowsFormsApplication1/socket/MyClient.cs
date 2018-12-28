using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using HG_EVENT;
using System.ComponentModel;
using System.Threading;
namespace HG_Socket
{
    public class MyClient
    {
        private TcpClient m_client; //客户端连接类

        private int m_nPort;//port

        private NetworkStream m_NetStream = null;//回用于发送和接收数据的数据流
        private ClientReceiveFromserver recevevent;
        private IPAddress m_Address;//IP
        private int m_nTimeOut;//超时
        public bool m_bConnect = false;//对外标志

        public MyClient(ClientReceiveFromserver rec,string strIP, int nPort, int nTimeOut = 1000)
        {
            m_nTimeOut = nTimeOut;
            m_Address = IPAddress.Parse(strIP);
            m_nPort = nPort;
            recevevent += rec;
        }

        public bool ConnectToServer()
        {
            try
            {
                //连接server
                m_client = new TcpClient();
                try
                {
                    m_client.Connect(m_Address, m_nPort);
                }
                catch
                {

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
   
        //调用之前可以先调用CheckbufferData判断是否有数据;
        public int ReadData(ref string strRead)
        {
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
                        byte[] receiveBuffer = new byte[1024];//接收数据缓冲区
                        int bytesReceived = 0;//接收字节数
                        bytesReceived = m_NetStream.Read(receiveBuffer, 0, receiveBuffer.Length);//
                        m_NetStream.Flush();
                        if (bytesReceived <= 0)
                        {
                            Close();
                            return -2;
                        }
                        strRead = System.Text.Encoding.Default.GetString(receiveBuffer);
                        strRead = strRead.TrimEnd('\0');
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
                string readdata = "";
                int readresult = ReadData(ref readdata);
                if (readresult == 0 && readdata != "")
                {
                    recevevent(readdata);
                }
                else if (readresult == -2 || readresult == -3)
                {
                    recevevent("DISCON");
                    break;
                }
            }

        }

        public bool WriteData(string strSend)
        {
            if (m_NetStream == null)
            {
                MessageBox.Show("Please confirm connect OK !");
                return false;
            }
            if (!m_NetStream.CanWrite)
            {
                MessageBox.Show("Please confirm connect OK !");
                return false;
            }
            try
            {
                byte[] SendBuffer = new byte[strSend.Length];//发送数据缓冲区
                SendBuffer = System.Text.Encoding.Default.GetBytes(strSend);
                m_NetStream.Flush();
                int n = SendBuffer.Length;
                m_NetStream.Write(SendBuffer, 0, SendBuffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public void Close()
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
        }
    }
}