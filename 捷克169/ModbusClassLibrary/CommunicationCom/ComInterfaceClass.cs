using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusClassLibrary.CommunicationCom
{    /// <summary>
     /// 接口类
     /// </summary>
    public class ComInterface
    {
        private enum comway
        {
            seriport = 0,
            netserver,
            netclience,
            none
        }
        private comway _com = comway.none;
        private mCommInterface comsp = null;
        public ComInterface(SerialPortInfo info)
        {
            comsp = new CommSeriport();
            (comsp as CommSeriport).PortInfo = info;
            _com = comway.seriport;
        }

        public ComInterface(NetInterfaceinfo info)
        {
            try
            {
                if (info.CorS == netApplyInterfaceChose.Server)
                {
                    comsp = new MyServer();
                    (comsp as MyServer).info = info;
                    _com = comway.netserver;
                }

                if (info.CorS == netApplyInterfaceChose.Clience)
                {
                    comsp = new MyClient();
                    (comsp as MyClient).info = info;
                    _com = comway.netclience;

                }
            }
            catch (Exception ex)
            {
              //  System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        public bool Open()
        {
            if (_com == comway.none)
            {
                return false;
            }
            if (comsp != null)
            {
                if (comsp.Open())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool Close()
        {
            comsp.Close();
            return true;
        }
        private bool IsOpen()
        {
            return true;
        }

        public void Write(ref byte[] buffer)
        {
            comsp.Write(ref buffer);
        }
        public void Read(out byte[] buffer)
        {
            buffer = new byte[1];
        }

    }



    public class mCommInterface
    {

        public virtual bool Open()
        {
            return true;
        }
        public virtual bool Close()
        {
            return true;
        }
        public virtual bool IsOpen()
        {
            return true;
        }
        public virtual void Write(ref byte[] buffer)
        {

        }

        public virtual void Read(out byte[] buffer)
        {
            buffer = new byte[1];
        }

        public virtual void Write(string ip, ref byte[] buffer)
        {

        }




    }
}
