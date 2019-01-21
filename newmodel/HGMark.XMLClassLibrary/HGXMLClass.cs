using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace HGMark.XMLClassLibrary
{
    public class HGXMLClass
    {
        public  static void CreatXmlTree(ref string path)
        {
              //XElement xElement = new XElement(
              //     new XElement("HGMarkSystemParam",
              //     new XElement("MarkCardType",
              //               new XElement("CardType", "JCZ")         
              //         ),
              //  new XElement("NetInformation",
              //               new XElement("ConnectType", "C"),
              //               new XElement("IP", "192.68.0.1"),
              //               new XElement("Port", "5000")
              //         )
              //         )
              // );
            XElement xElement = new XElement(
                 new XElement("HGMarkSystemParam",""
             
            
                     )
             );
           XmlWriterSettings settings = new XmlWriterSettings();
           settings.Encoding = new UTF8Encoding(false);
           settings.Indent = true;
           XmlWriter xw = XmlWriter.Create(path, settings);
           xElement.Save(xw);
           xw.Flush();
           xw.Close();
        }

        public static void CreatXmlTree(ref XElement xElement,ref string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            XmlWriter xw = XmlWriter.Create(path, settings);
            xElement.Save(xw);
            xw.Flush();
            xw.Close();
        }

        public static void CreateNode(string xmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);           
            var root = xmlDoc.DocumentElement;
            XmlNode newNode = xmlDoc.CreateNode(XmlNodeType.Element, "NewBook", "");
            newNode.InnerText = "WPF";  
                   
            root.AppendChild(newNode);
            xmlDoc.Save(xmlPath);
        }
       public static void CreateAttribute(string xmlPath)
       {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            var root = xmlDoc.DocumentElement;//取到根结点
            XmlElement node = (XmlElement)xmlDoc.SelectSingleNode("HGMarkSystemParam/NewBook");
            node.SetAttribute("Name", "WPF");
            xmlDoc.Save(xmlPath);
       }

}
}
