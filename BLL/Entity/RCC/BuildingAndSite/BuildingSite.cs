using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.WallAndColumn;
using System.Collections.ObjectModel;


namespace RDBLL.Entity.RCC.BuildingAndSite
{
    public class BuildingSite :ICloneable
    {
        public string Name { get; set; }
        public List<Building> BuildingList { get; set; }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("BuildingSite");
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Name", Name);
            foreach (Building obj in BuildingList)
            {
                xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            return xmlNode;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public BuildingSite()
        {
            Name = "Мой объект";
            BuildingList = new List<Building>();
        }

        public BuildingSite(XmlNode xmlNode)
        {
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "Name") Name = obj.Value;
            }
            BuildingList = new List<Building>();
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == "Building") BuildingList.Add(new Building(this, childNode));
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Building : ICloneable
    {
        public string Name { get; set; }
        public BuildingSite BuildingSite { get; set; }
        public ObservableCollection<Level> LevelList { get; set; }
        public ObservableCollection<WallType> WallTypeList { get; set; }
        public ObservableCollection<OpeningType> OpeningTypeList { get; set; }
        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("Building");
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Name", Name);
            foreach (WallType obj in WallTypeList)
            {
                xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            foreach (OpeningType obj in OpeningTypeList)
            {
                xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            foreach (Level obj in LevelList)
            {
                xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            return xmlNode;
        }
        public Building(BuildingSite buildingSite)
        {
            Name = "Мое здание";
            BuildingSite = buildingSite;
            LevelList = new ObservableCollection<Level>();
            WallTypeList = new ObservableCollection<WallType>();
            OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        public Building(BuildingSite buildingSite, XmlNode xmlNode)
        {
            BuildingSite = buildingSite;
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "Name") Name = obj.Value;
            }
            WallTypeList = new ObservableCollection<WallType>();
            OpeningTypeList = new ObservableCollection<OpeningType>();
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == "WallType") WallTypeList.Add(new WallType(this, childNode));
                if (childNode.Name == "OpeningType") OpeningTypeList.Add(new OpeningType(this, childNode));
            }
            LevelList = new ObservableCollection<Level>();
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == "Level") LevelList.Add(new Level(this, childNode));
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
