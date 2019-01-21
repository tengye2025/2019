using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Windows.Forms;
using System.IO;
namespace HGMark.XMLClassLibrary
{
    /// <summary>  
    /// 对exe.Config文件中的appSettings段进行读写配置操作  
    /// 注意：调试时，写操作将写在vhost.exe.config文件中  
    /// </summary>  
 public   class ConfigHelper
    {
        static private string _configName;
        static private string _fullPath;

        static public string Name
        {
            get
            {
                if (_configName == null)
                {
                    _configName = string.Empty;
                }
                return _configName;
            }
            set
            {
                _configName = value;
                _fullPath = Application.StartupPath + "\\" + _configName;
            }
        }

        /// <summary>  
        /// 写入值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void SetValue(string key, string value)
        {
            try
            {
                //增加的内容写在appSettings段下 </>  
                File.SetAttributes(_fullPath, FileAttributes.Archive);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_fullPath);
                XmlNode root = xmlDoc.SelectSingleNode("appSettings");
                XmlNode node = root.SelectSingleNode(key);
                if (null == node)
                {
                    node = xmlDoc.CreateNode(XmlNodeType.Element, key, null);
                    node.InnerText = value;
                    root.InsertAfter(node, root.FirstChild);
                }
                else
                {
                    node.InnerText = value;
                }
                xmlDoc.Save(_fullPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>  
        /// 读取指定key的值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string GetValue(string key)
        {
            string conn = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_fullPath);

                XmlNode root = xmlDoc.SelectSingleNode("appSettings");

                if (root != null)
                {
                    XmlNode node = root.SelectSingleNode(key);
                    if (node != null)
                    {
                        conn = node.InnerText;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return conn;
        }
      /// <summary>
      /// 创造一个xml
      /// </summary>
      /// <param name="rootstr">根节点</param>
        public static void CreateXmlFile(ref string pathfilename,ref string rootstr)
        {
            if (File.Exists(pathfilename))
            {
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            //创建类型声明节点  
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            //创建根节点  
          //  XmlNode newNode = xmlDoc.CreateNode(XmlNodeType.Element, rootstr, null);
            XmlNode root = xmlDoc.CreateElement(rootstr,"");
           // newNode.InnerText = "WPF";
            xmlDoc.AppendChild(root);

            try
            {
                xmlDoc.Save(pathfilename);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

 
         /// <summary>
         /// 创建一个节点
         /// </summary>
         /// <param name="xmlfilepath">xml文档</param>
         /// <param name="xpath">创建位置</param>
         /// <param name="name">元素名</param>
         /// <param name="value">值</param>
        public static void CreateNode(ref string xmlfilepath, ref string xpath, string name, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlfilepath);

            XmlNode root = xmlDoc.SelectSingleNode(xpath);
        

            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            (node as XmlElement).SetAttribute("batch", "123");
            node.InnerText = value;
        
            root.AppendChild(node);
            xmlDoc.Save(xmlfilepath);       

        }

        public static void Delete(string xmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            var root = xmlDoc.DocumentElement;//取到根结点

            var element = xmlDoc.SelectSingleNode("set/name");
            root.RemoveChild(element);
            xmlDoc.Save(xmlPath);
        }


    }
}
