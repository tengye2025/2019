using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpcUaHelper;
using System.Windows.Forms;
namespace HGMark.OPCUAClassLibrary
{
    public class OPCUAClassLibraryHelper
    {
        private static OpcUaClient opcuaclilent = new OpcUaClient();
        public static void DisConnected()
        {
            opcuaclilent.Disconnect();
        }

        public static void ConnectServer(string serverpath)
        {
            opcuaclilent.ConnectServer(serverpath);

        }
        public static void ConnectServer(string identification,string password,string serverpath)
        {
            opcuaclilent.UserIdentity   = new Opc.Ua.UserIdentity(identification, password);
            opcuaclilent.ConnectServer(serverpath);
        }
        public static void ShowForm()
        {
            using (FormBrowseServer form = new FormBrowseServer())
            {
                form.ShowDialog();
            }

        }
       
        public static T ReadNodeInfo<T>(string NODE)
        {
            T value = opcuaclilent.ReadNode<T>(NODE);
            return value;
        }


    }
}
