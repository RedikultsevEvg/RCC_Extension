using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.WallAndColumn;
using RCC_Extension.BLL.Service;
using System.Xml;


namespace RCC_Extension.BLL.BuildingAndSite
{
    public class Building : ICloneable
    {
        public string Name { get; set; }
        public List<Level> LevelList { get; set; }
        public List<WallType> WallTypeList { get; set; }
        public List<OpeningType> OpeningTypeList { get; set; }

        public Building(BuildingSite buildingSite)
        {         
            Name = "Мое здание";
            LevelList = new List<Level>();
            WallTypeList = new List<WallType>();
            OpeningTypeList = new List<OpeningType>();
            buildingSite.BuildingList.Add(this);
        }

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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
