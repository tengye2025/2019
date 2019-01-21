using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace HGMark.DBClassLibrary
{
    /// <summary>
    /// 工厂模式
    /// </summary>
     interface  DBFactory
    {
        void Open();  //打开数据库连接    
        void Close(); //关闭数据库连接   
        void SetConnectStr(ref string strConnection);   
        DataTable exeSqlForDataTable(ref string sqlstasment);
        bool ExcuteReaderSqlStatement(ref string sqlStatement,out SqlDataReader sqldatareader);
        bool ExcuteScalarSqlStatement(ref string sqlStatement, out object getbackobject);
        bool ExcuteNonQuerySqlStatement(ref string sqlStatement, out int getint);
    }


}
