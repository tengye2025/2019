namespace HG_EVENT
{
    //网络部分
    public delegate void ClientDataReceivedEventHandler(string strPre, string strInfo);

    public delegate void ClientConnectEventHandler(string strPre);

    public delegate void ClientDisConnectEventHandler(string strPre);
    public delegate void ClientReceiveFromserver(string strPre);
}