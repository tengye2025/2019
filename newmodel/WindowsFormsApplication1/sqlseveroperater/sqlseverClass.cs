using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using HGMark.DBClassLibrary;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace WindowsFormsApplication1.sqlseveroperater
{
    class sqlseverClass
    {
        /// <summary>
        /// 设置数据库连接
        /// </summary>
        /// <param name="consqlad"></param>
       static  public  void SQLSEVERINITSet(ref string consqlad)
       {
           string consql = consqlad;//"data source=192.168.10.28;initial catalog=dataty;user id=sa;password=1123581321;"+ "Connect Timeout = 4;";
           HGMark.DBClassLibrary.HGMarkDBHelper.Instance.SetConnectstring(consql);              
       }
        /// <summary>
        /// 通过未打印标志位获取ID
        /// </summary>
        /// <returns></returns>
        static public int GetIDByPrintflag()
        {
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Open();       
            string sql =   "select * from LabelPrint where PrintFlag = '1'";  
            SqlDataReader sqldatareader;
            string id= string.Empty;
            if (HGMarkDBHelper.Instance.ExcuteReaderSqlStatement(ref sql, out sqldatareader))
            {
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                         id = sqldatareader["ID"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("label print have no data,by select printflag = 1");
                }
            }
            else
            {
                
            }

            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Close(); 
            if (id != string.Empty)
            {
                return Convert.ToInt32(id);
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 通过未打印标志位获取数据
        /// </summary>
        /// <returns></returns>
        static public string GetPrintDataByPrintflag()
        {

            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Open();
            string sql = "select * from LabelPrint where PrintFlag = '1'";
            SqlDataReader sqldatareader;
            string data = string.Empty;
            if (HGMarkDBHelper.Instance.ExcuteReaderSqlStatement(ref sql, out sqldatareader))
            {
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                        data = sqldatareader["PrintData"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("label print have no data,by select printflag = 1");
                }
            }
            else
            {

            }

            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Close();
            if (data != string.Empty)
            {
                return data;
            }
            else
            {
                return data;
            }

           
        }
        /// <summary>
        /// 通过未打印标志位获取 轴型
        /// </summary>
        /// <returns></returns>
        static public string GetAxialTypeByPrintflag()
        {
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Open();
            string sql = "select * from LabelPrint where PrintFlag = '1'";
            SqlDataReader sqldatareader;
            string data = string.Empty;
            if (HGMarkDBHelper.Instance.ExcuteReaderSqlStatement(ref sql, out sqldatareader))
            {
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                        data = sqldatareader["AxialType"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("label print have no data,by select printflag = 1");
                }
            }
            else
            {

            }

            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Close();
            if (data != string.Empty)
            {
                return data;
            }
            else
            {
                return data;
            }
        }
        /// <summary>
        /// 更新 打印完成时间
        /// </summary>
        /// <param name="id"></param>
        static public void UpdateFinishDate(int id)
        {
            String sql = "update LabelPrint set FinishDate = '" + DateTime.Now.ToString() + "' where ID = '" + id.ToString() + "'"; 
            int  getinit;
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Open();
            if (HGMarkDBHelper.Instance.ExcuteNonQuerySqlStatement(ref sql, out getinit))
            {
                int aget = getinit;
            }
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Close();
        }
        /// <summary>
        /// 更新打印标志位
        /// </summary>
        /// <param name="id"></param>
        static public void UpdatePrintflag(int id)
        {
            String sql = "update LabelPrint set PrintFlag = '2' where ID = '" + id.ToString() + "'";
            int getinit;
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Open();
            if (HGMarkDBHelper.Instance.ExcuteNonQuerySqlStatement(ref sql, out getinit))
            {
                int aget = getinit;
            }
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Close();
        }
    }
}
