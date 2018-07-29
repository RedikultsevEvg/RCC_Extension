using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.BuildingAndSite;
using System.Xml;
using System.IO;
using Microsoft.Win32;


namespace RCC_Extension.BLL.Service
{
    public class ProgrammSettings
    {
        public static BuildingSite BuildingSite { get; set; }
        public static String FilePath { get; set; }
        public static bool IsDataChanged { get; set; }

        public static void InicializeNew()
        {
            BuildingSite = new BuildingSite();
            BuildingSite.BuildingList.Add(new Building(BuildingSite));
            IsDataChanged = false;
        }
        public static void ClearAll()
        {
            BuildingSite.BuildingList.Clear();
        }
        public static bool OpenProjectFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true) FilePath = openFileDialog.FileName; else return false;
            ClearAll();
            InicializeNew();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(FilePath);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            foreach (XmlNode childNode in xmlRoot.ChildNodes)
            {
                if (childNode.Name == "BuildingSite") BuildingSite = new BuildingSite(childNode);
            }
            return true;
        }

        public static bool SaveProjectToFile(bool InNewFile)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (FilePath == "" || FilePath == null || InNewFile)
            {
                if (saveFileDialog.ShowDialog() == true)
                    FilePath = saveFileDialog.FileName;
                else return false;
            }     
            XmlTextWriter textWritter = new XmlTextWriter(FilePath, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("Project");
            textWritter.Close();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(FilePath);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            XmlElement xmlSite = BuildingSite.SaveToXMLNode(xmlDocument);
            xmlRoot.AppendChild(xmlSite);
            xmlDocument.Save(FilePath);
            IsDataChanged = false;
            return true;
        }
    }
}
