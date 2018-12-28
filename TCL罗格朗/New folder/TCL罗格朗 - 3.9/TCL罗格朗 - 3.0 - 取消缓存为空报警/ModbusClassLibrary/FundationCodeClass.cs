using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ModbusClassLibrary
{
    /// <summary>
    /// modbus 指令管理类
    /// </summary>
    class FundationCodeClass
    {
        public byte fundationcodebyte = 0;
        private AutoResetEvent _event = new AutoResetEvent(false);
        private ushort timeoutset = 0;

        public void SetTimeout(ushort t)
        {
            timeoutset = t;
        }


        public bool WaitForFinished()
        {
            if (_event.WaitOne(timeoutset))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void EvetSet()
        {
            _event.Set();
        }

    }
}
