using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            XmlElement SiteNode = xmlDocument.CreateElement("BuildingSite");
            XmlAttribute NameAttr = xmlDocument.CreateAttribute("Name");
            XmlText NameText = xmlDocument.CreateTextNode(Name);
            NameAttr.AppendChild(NameText);
            SiteNode.Attributes.Append(NameAttr);
            foreach (Building obj in BuildingList)
            {
                SiteNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            return SiteNode;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
