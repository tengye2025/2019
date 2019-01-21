using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HGMark.Common.CommunicationDispatch;
using HGMark.Common.CmdLib;
using HGMark.Common.CommunicationFunction;
namespace HGMark.Common
{
    public  sealed  class CommunicationApp
    {
        private static readonly CommunicationApp _instance;//单件模式
        private CmdDispatcher _cmddispatcher;
        private static NetInterfaceinfo netinfo;

        public static NetInterfaceinfo NetInfo
        {
            get { return netinfo; }
            set { netinfo = value; }
        }
        public static  CommunicationApp instance
        {
            get { return _instance; }
        }

        static CommunicationApp()
        {
            _instance = new CommunicationApp();
        }

        private  CommunicationApp()
        {
        
        }

        public void SetComPragram(ref NetInterfaceinfo info)
        {
            NetInfo = info;
        }

        public void Init()
        {
            _cmddispatcher = new CmdDispatcher(ref netinfo);
            _cmddispatcher.Initframe(); 
        }

        public void SetPassivegetdatafromothersevent(ref PassiveGetDataFromOthersDelegate p)
        {
            _cmddispatcher.SetPassivegetdatafromothersevent(ref p);
        }

        public bool Open()
        {
            return _cmddispatcher.OPenTheChannelCom();
        }

        public byte [] PUT_cmd()
        {
            byte [] bud = new byte[2]{1,2};
           return   _cmddispatcher.putcmdintocmd(CmdDefined.Cmdstructlist.CMD_fr, ref bud,(ushort)bud.Length);
        }


    }
}
