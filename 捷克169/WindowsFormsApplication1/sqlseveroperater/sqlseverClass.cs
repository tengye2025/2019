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
        /// 设置数据库连接 必须先设置
        /// </summary>
        /// <param name="consqlad"></param>
       static  public  void SQLSEVERINITSet(ref string consqlad)
       {
           string consql = consqlad;//"data source=192.168.10.28;initial catalog=dataty;user id=sa;password=1123581321;"+ "Connect Timeout = 4;";
           HGMark.DBClassLibrary.HGMarkDBHelper.Instance.SetConnectstring(consql);       
       }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consqlad"></param>
        static public void SQLSEVEROpen()
        {
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Open();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consqlad"></param>
        static public void SQLSEVERClose()
        {
            HGMark.DBClassLibrary.HGMarkDBHelper.Instance.Close();
        }
        /// <summary>
        /// 通过未打印标志位获取ID
        /// </summary>
        /// <returns></returns>
        static public int GetIDByPrintflag()
        {
             
            string sql = "select * from wcs_laser_labelprint where print_flag = '1'";  
            SqlDataReader sqldatareader;
            string id= string.Empty;
            if (HGMarkDBHelper.Instance.ExcuteReaderSqlStatement(ref sql, out sqldatareader))
            {
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                         id = sqldatareader["taskid"].ToString();
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
            sqldatareader.Close();
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

       
            string sql = "select * from wcs_laser_labelprint where print_flag = '1'";
            SqlDataReader sqldatareader;
            string data = string.Empty;
            if (HGMarkDBHelper.Instance.ExcuteReaderSqlStatement(ref sql, out sqldatareader))
            {
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                        data = sqldatareader["print_data"].ToString();
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
            sqldatareader.Close();
            if (data != string.Empty)
            {
                return data;
            }
            else
            {
                return data;
            }

           
        }

        static public string GetPrintData2ByPrintflag()
        {


            string sql = "select * from wcs_laser_labelprint where print_flag = '1'";
            SqlDataReader sqldatareader;
            string data = string.Empty;
            if (HGMarkDBHelper.Instance.ExcuteReaderSqlStatement(ref sql, out sqldatareader))
            {
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                        data = sqldatareader["print_data2"].ToString();
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
            sqldatareader.Close();
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
    
            string sql = "select * from wcs_laser_labelprint where print_flag = '1'";
            SqlDataReader sqldatareader;
            string data = string.Empty;
            if (HGMarkDBHelper.Instance.ExcuteReaderSqlStatement(ref sql, out sqldatareader))
            {
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                        data = sqldatareader["axial_type"].ToString();
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
            sqldatareader.Close();
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
            String sql = "update wcs_laser_labelprint set finish_date = '" + DateTime.Now.ToString() + "' where taskid = '" + id.ToString() + "'"; 
            int  getinit;
            if (HGMarkDBHelper.Instance.ExcuteNonQuerySqlStatement(ref sql, out getinit))
            {
                int aget = getinit;
            }
        }
        /// <summary>
        /// 更新打印标志位
        /// </summary>
        /// <param name="id"></param>
        static public void UpdatePrintflag(int id)
        {
            String sql = "update wcs_laser_labelprint set print_flag = '2' where taskid = '" + id.ToString() + "'";
            int getinit;  
            if (HGMarkDBHelper.Instance.ExcuteNonQuerySqlStatement(ref sql, out getinit))
            {
                int aget = getinit;
            }      
        }
    }
}
