using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using System.Data.OleDb;
using System.Data;
using System.IO;
using ADOX;
namespace HGMark.DataACCessClassLibrary
{
    public class DataACCessClass
    {
            private static string strTest;
            private static string _accessPath = Application.StartupPath + @"\DataBase.accdb";

            public static string AccessPath
            {
                get { return _accessPath; }
                set { _accessPath = value; }
            }
            protected static string constring = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _accessPath + ";JET OLEDB:Database Password=admin;";

            //protected static string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _accessPath + ";JET OLEDB:Database Password=abc;";


            protected static OleDbConnection Connection;

            #region 构造函数
            public  DataACCessClass()
            {

            }
            ~DataACCessClass()
            {
                Close();
            }
            #endregion
            #region 本项目Access数据库初始化
            /// <summary>
            /// 初始化数据库，创建数据库及表格
            /// </summary>
            public static void DatabaseInit()
            {
                //  Createaccess();//创建Access数据库
                //  CreateTabAuthor();//在数据库中创建用户权限表
                //  InsertValAuthor("Admin", "lzauto", 1);//插入管理员权限   
                //  CreateTabProductInkData();         
            }
            #endregion
            #region 创建Accessdata数据库

            /// <summary>
            /// 创建数据库，带密码
            /// </summary>
            public static void CreateAccess(string path, string databasename)
            {

                _accessPath = path + @"\" + databasename + ".accdb";
                constring = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _accessPath + ";JET OLEDB:Database Password=1;";

                if (!File.Exists(_accessPath))
                {
                    try
                    {
                        Catalog ca = new Catalog();
                        ca.Create(constring);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("创建数据库错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    }
                }
            }
            #endregion
            #region 创建表
            /// <summary>
            /// 创建数据表
            /// </summary>
            /// 
            private static string tablenames = null;
            private static string strsqls = null;
            public static void CreateTabStat(string strsql, string tablename)
            {
                tablenames = tablename;
                strsqls = strsql;
                try
                {
                    if (!IsExistTable(tablename))
                    {
                        // string strsql = string.Format("create table {0}(ID identity,BoxNumber string,CanNumberone string,CanNumbertwo string,CanNumberthree string,CanNumberfour string,CanNumberfive string,CanNumbersix string,CanNumberseven string,CanNumbereight string,CanNumbernine string,CanNumberten string,CanNumbereleven string,CanNumbertwelven string)", tablename);
                        string putinstrsql = string.Format(strsql, tablename);
                        ExecuteSQL(putinstrsql);
                        Setkey(tablename, "ID");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建 " + tablename + " 错误" + ex.Message, "警告", MessageBoxButtons.OK);
                }
            }


            #endregion
            #region 哈希表
            //Hashtable statTable = new Hashtable();
            //public static Hashtable CreateHas()
            //{
            //    DateTime _date = DateTime.Today;
            //    statTable.Add("生产日期", _date);
            //    statTable.Add("tatol", 1);
            //    statTable.Add("OK", 2);
            //    statTable.Add("NG", 3);
            //    statTable.Add("良率", 0.82);
            //    return statTable;
            //}
            //public static void Inserttab()
            //{
            //    Hashtable table = CreateHas();
            //    access.Insert("数据统计表", table);
            //}
            #endregion
            #region 删除新表
            /// <summary>
            /// 根据表面删除表
            /// </summary>
            /// <param name="tablename"></param>
            public static void deletetab(string tablename)
            {
                try
                {
                    string strSql = "drop table " + tablename;
                    ExecuteSQL(strSql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除 " + tablename + " 错误" + ex.Message, "警告", MessageBoxButtons.OK);
                }
            }

            #endregion
            #region 设置主键
            /// <summary>
            /// 设置主键
            /// </summary>
            /// <param name="tablename"></param>表名
            /// <param name="col"></param>主键名
            public static void Setkey(string tablename, string col)
            {
                try
                {
                    string strSql = string.Format("alter table {0} add primary key({1})", tablename, col);
                    ExecuteSQL(strSql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("操作数据库 " + tablename + " 设置主键错误" + ex.Message, "警告", MessageBoxButtons.OK);
                }
            }
            #endregion
            #region 插入记录操作
            /// <summary>
            /// 
            /// </summary>
            /// <param name="StationName"></param>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="seal"></param>
            /// <param name="adress"></param>
            public static void InsertValStat(string strSql)
            {

                string tablename = "notebook";
                try
                {
                    if (!IsExistTable(tablename))
                    {
                        CreateTabStat(tablenames, strsqls);
                    }
                    //string strSql = string.Format("insert into {0}([BoxNumber],[CanNumber]) values('{1}','{2}')", tablename, boxnumber, CanNumber);
                    ExecuteSQL(strSql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(tablename + " 存储记录错误" + ex.Message, "警告", MessageBoxButtons.OK);
                }

            }


            #endregion
            #region 修改操作
            public static void ModifyTabAuthor(string username, string password)
            {
                try
                {
                    string strSql = string.Format("update 用户权限表 set [Pwd]= '{0}' where [UserName] ='{1}'", password, username);
                    ExecuteSQL(strSql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("修改权限密码操作用户权限表数据库错误" + ex.Message, "警告", MessageBoxButtons.OK);
                }
            }
            #endregion
            #region 查询登录名和密码是否正确
            public static bool IsUserCorrect(string username, string password)
            {
                bool result = false;
                try
                {
                    string tablename = "用户权限表";
                    string strSql = string.Format("select *from {0} where UserName = '{1}' and Pwd = '{2}'", tablename, username, password);
                    result = ExecuteSQL(strSql);
                    return result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询用户名、密码操作用户权限表数据库错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    return result;
                }
            }
            #endregion
            #region 条件查询
            /// <summary>
            /// 返回参数username对应的用户等级
            /// </summary>
            /// <param name="username"></param>
            /// <returns></returns>
            public static bool SelectProcess(out DataSet ds, string sql)
            {

                ds = new DataSet();
                try
                {
                    Open();
                    string strAdd = sql;
                    OleDbDataAdapter oledbba = new OleDbDataAdapter(strAdd, Connection);
                    oledbba.Fill(ds);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("条件查询数据库错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    return false;
                }
                return true;
            }
            #endregion
            #region 删除操作
            /// <summary>
            /// 删除操作
            /// </summary>
            /// <param name="strSql"></param>SQL语句
            /// <returns></returns>
            public static bool DeleteUser(string strSql)
            {
                bool result = false;
                try
                {
                    result = ExecuteSQL(strSql);
                    return result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除操作数据库错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    return result;
                }
            }
            #endregion
            #region 连接数据库
            public static void ConncetionDatabase()
            {
                if (Connection == null)
                {
                    try
                    {
                        Connection = new OleDbConnection(constring);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据库连接错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    }
                }

                if (Connection != null)
                {
                    if (Connection.State.Equals(ConnectionState.Closed))
                    {
                        try
                        {
                            Connection = new OleDbConnection(constring);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("数据库连接错误" + ex.Message, "警告", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            #endregion
            #region 连接并打开数据库
            public static void Open()
            {
                if (Connection == null)
                {
                    try
                    {
                        Connection = new OleDbConnection(constring);
                        Connection.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据库连接并打开错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    }
                }

                if (Connection != null)
                {
                    if (Connection.State.Equals(ConnectionState.Closed))
                    {
                        try
                        {
                            Connection.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("数据库打开错误" + ex.Message, "警告", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            #endregion
            #region 关闭数据库
            public static void Close()
            {
                try
                {
                    if (Connection != null)
                    {
                        Connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库关闭错误" + ex.Message, "警告", MessageBoxButtons.OK);
                }
            }
            #endregion
            #region 执行SQL语句
            /// <summary>
            /// 执行SQL语句
            /// </summary>
            /// <param name="sql"></param>
            public static bool ExecuteSQL(string sql)
            {
                bool result;
                try
                {
                    Open();
                    OleDbCommand cmd = new OleDbCommand(sql, Connection);
                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("执行数据库语句 " + sql + "错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    result = false;
                }
                return result;
            }
            #endregion
            #region 返回表数据
            /// <summary>
            /// 返回对应表名的Dataset数据
            /// </summary>
            /// <param name="tablename"></param>
            /// <returns></returns>
            public static DataSet ReturnTable(string tablename)
            {
                var ds = new DataSet();
                Open();
                try
                {
                    string strAdd = string.Format("select * from {0}", tablename);
                    var adapter = new OleDbDataAdapter(strAdd, Connection);
                    adapter.Fill(ds, tablename);
                    //dataGridView1.AutoGenerateColumns = true;
                    //dataGridView1.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("返回 " + tablename + " Dataset数据错误" + ex.Message, "警告", MessageBoxButtons.OK);
                }
                return ds;
            }
            #endregion
            #region 返回所有表名
            /// <summary>
            /// 獲取數據庫中所有的表名
            /// </summary>
            /// <returns></returns>
            public static string[] GetTablenames()
            {
                Open();
                string[] tableNames;
                try
                {
                    DataTable schemaTable = Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    if (schemaTable == null)
                    {
                        return null;
                    }

                    tableNames = new string[schemaTable.Rows.Count];
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        //tableNames[i] = schemaTable.Rows[i][2].ToString();
                        tableNames[i] = schemaTable.Rows[i]["TABLE_NAME"].ToString();
                    }
                    /////
                    foreach (DataRow dr in schemaTable.Rows)
                    {
                        strTest = dr["TABLE_NAME"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询所有表名操作数据库错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    return null;
                }
                return tableNames;
            }
            #endregion
            #region 判断表是否存在
            /// <summary>
            /// 判断表是否存在
            /// </summary>
            /// <returns></returns>true表示存在
            public static bool IsExistTable(string tablename)
            {
                bool result = false;
                string[] tabelName = GetTablenames();
                foreach (var s in tabelName)
                {
                    if (s == tablename)
                    {
                        result = true;
                        break;
                    }
                }
                return result;
            }
            #endregion
            #region 返回指定表列名
            /// <summary>
            /// 獲取數據庫中指定表中的所有列名
            /// </summary>
            /// <returns></returns>
            public static string[] GetTableColNames(string tablename)
            {
                Open();
                string[] tableNames;
                try
                {
                    DataTable columnTable = Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tablename, null });
                    if (columnTable == null)
                    {
                        return null;
                    }
                    //string str = "";

                    tableNames = new string[columnTable.Rows.Count];
                    for (int i = 0; i < columnTable.Rows.Count; i++)
                    {
                      //tableNames[i] = columnTable.Rows[i][3].ToString();
                        tableNames[i] = columnTable.Rows[i]["COLUMN_NAME"].ToString();
                    }
                    /////
                    foreach (DataRow dr2 in columnTable.Rows)
                    {
                        strTest = dr2["COLUMN_NAME"].ToString();
                        //str += strTest + @",";
                    }
                    // MessageBox.Show(str);
                    foreach (DataColumn dc in columnTable.Columns)
                    {
                        strTest = dc.ColumnName;

                      //str += strTest + @",";
                    }
                    //MessageBox.Show(str);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询 " + tablename + " 中所有列名错误" + ex.Message, "警告", MessageBoxButtons.OK);
                    return null;
                }
                return tableNames;
            }
            #endregion
            /////////////////////////// 
        }
    }

