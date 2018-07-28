using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RCC_Extension.BLL.Service;

namespace RCC_Extension.BLL.BuildingAndSite
{
    public class BuildingSite :ICloneable
    {
        public string Name { get; set; }
        public List<Building> BuildingList { get; set; }

        public BuildingSite()
        {
            Name = "Мой объект";
            BuildingList = new List<Building>();
        }

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
    }
}
