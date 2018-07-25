using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.WallAndColumn;
using System.Xml;


namespace RCC_Extension.BLL.BuildingAndSite
{
    public class Building : ICloneable
    {
        public string Name { get; set; }
        public List<Level> LevelList { get; set; }
        public List<WallType> WallTypeList { get; set; }

        public Building(BuildingSite buildingSite)
        {         
            Name = "Мое здание";
            LevelList = new List<Level>();
            WallTypeList = new List<WallType>();
            buildingSite.BuildingList.Add(this);
        }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement BuildingNode = xmlDocument.CreateElement("Building");
            XmlAttribute NameAttr = xmlDocument.CreateAttribute("Name");
            XmlText NameText = xmlDocument.CreateTextNode(Name);
            NameAttr.AppendChild(NameText);
            BuildingNode.Attributes.Append(NameAttr);
            foreach (Level obj in LevelList)
            {
                BuildingNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            return BuildingNode;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
