using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace HGMark.DBClassLibrary
{
    public enum DBType
    {
        no,
        access,
        sqlsever,
        mysql,
        oracle
    };
    /// <summary>
    /// 单件模式设计
    /// </summary>
    public sealed  class HGMarkDBHelper
    {
        private static  HGMarkDBHelper _instance;
        private static readonly object lockobj= new object();
        private DBFactory databasefactory;
   
        
        private HGMarkDBHelper()
        {
        
        }
        public static HGMarkDBHelper Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock (lockobj)
                    {
                        _instance = new HGMarkDBHelper();
                    }
                }
                return _instance;
            }           
        }
        /// <summary>
        /// 设置连接字符串 
        /// string connsql
        /// </summary>
        /// <param name="cp"></param>
        public void SetConnectstring(string cp)
        {  
            databasefactory = (DBFactory)(new SqlServerDBOperater());
            databasefactory.SetConnectStr(ref cp);        
        }
        public bool ExcuteNonQuerySqlStatement(ref string sqlStatement, out int getint)
        {
            return databasefactory.ExcuteNonQuerySqlStatement(ref sqlStatement, out getint);                 
        }

        public  bool ExcuteReaderSqlStatement(ref string sqlStatement, out SqlDataReader sqldatareader)
        {
            return databasefactory.ExcuteReaderSqlStatement(ref sqlStatement, out sqldatareader);     
        }
        public bool ExcuteScalarSqlStatement(ref string sqlStatement, out object getbackobject)
        {
            return databasefactory.ExcuteScalarSqlStatement(ref sqlStatement, out getbackobject);
        }

        public  void Open() //打开数据库连接  
        {
            databasefactory.Open();
        }
        public void Close() //关闭数据库连接   
        {
            databasefactory.Close();
        }
    }
}
