using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
namespace HGMark.DBClassLibrary
{
     class SqlServerDBOperater : DBFactory
    {

        private SqlConnection sqlconnect; //数据库连接    
        private string connectstr;//数据库连接字符
        public virtual void SetConnectStr(ref string strConnection)
        {
            connectstr = strConnection;
        }

           ~SqlServerDBOperater()                    // 析构函数
            {
                Console.WriteLine("~SqlServerDBOperater()析构函数");
            }
        public virtual void Open()
        {
            this.sqlconnect = new SqlConnection(connectstr);            
                sqlconnect.Open();
            
        }
        public virtual void Close()
        {
            sqlconnect.Close();
        }


        public virtual DataTable exeSqlForDataTable(ref string QueryString)
        {
            if (sqlconnect.State == ConnectionState.Closed)
            {
                sqlconnect.Open();
            }
            DataTable dt = null;   
            try
            {
             
                SqlDataAdapter myda = new SqlDataAdapter(QueryString, sqlconnect); // 实例化适配器
                dt = new DataTable(); // 实例化数据表
                myda.Fill(dt); // 保存数据                
            }
            catch
            {
              
            }
            return dt;
        }

        public virtual bool ExcuteReaderSqlStatement(ref string sqlStatement, out SqlDataReader SqlDataReader)
        {
            if (sqlconnect.State == ConnectionState.Closed)
            {
                sqlconnect.Open();
            }
         
            try
            {            
                SqlCommand cmd = new SqlCommand(sqlStatement, sqlconnect);
                SqlDataReader = cmd.ExecuteReader();
             
                return true;           
            }
            catch (Exception ex)
            {   
                MessageBox.Show(ex.ToString());
                SqlDataReader = null;           
                return false;         
            }
       
        }

        public virtual bool ExcuteScalarSqlStatement(ref string sqlStatement, out object getbackobject)
        {
            if (sqlconnect.State == ConnectionState.Closed)
            {
                sqlconnect.Open();
            }

            try
            {                
                SqlCommand cmd = new SqlCommand(sqlStatement, sqlconnect);
                getbackobject = cmd.ExecuteScalar();
           
                return true;
            }
            catch(Exception ex)
            {
              
                MessageBox.Show(ex.ToString());             
                getbackobject = new object();        
                return false;
            }
  
        }

        public virtual bool ExcuteNonQuerySqlStatement(ref string sqlStatement, out int getint)
        {
            if (sqlconnect.State == ConnectionState.Closed)
            {
                sqlconnect.Open();
            }
   
            try
            {              
                SqlCommand cmd = new SqlCommand(sqlStatement, sqlconnect);
                getint = cmd.ExecuteNonQuery();
             
                return true;
            }
            catch(Exception ex)
            {
              
                MessageBox.Show(ex.ToString());           
                getint = 0;         
                return false;
            }

        }


    }
}



