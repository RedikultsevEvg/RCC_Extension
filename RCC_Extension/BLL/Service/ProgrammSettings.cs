using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.BuildingAndSite;
using System.Xml;


namespace RCC_Extension.BLL.Service
{
    public class ProgrammSettings
    {
        public static BuildingSite buildingSite { get; set; }
        public static Building building { get; set; }
        public static String FilePath { get; set; }

        public static void SaveProjectToFile(String FileName)
        {
            XmlTextWriter textWritter = new XmlTextWriter(FileName, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("Project");
            textWritter.Close();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(FileName);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            XmlElement xmlSite = buildingSite.SaveToXMLNode(xmlDocument);
            xmlRoot.AppendChild(xmlSite);
            xmlDocument.Save(FileName);
        }
    }
}
