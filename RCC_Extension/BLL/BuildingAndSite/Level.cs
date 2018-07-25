using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.Geometry;
using RCC_Extension.BLL.WallAndColumn;
using System.Xml;


namespace RCC_Extension.BLL.BuildingAndSite
{
    public class Level :ICloneable
    {
        public string Name { get; set; }
        public Building Building { get; set; }

        public decimal FloorLevel { get; set; }
        public decimal Height { get; set; }
        public decimal TopOffset { get; set; }
        public int Quant { get; set; }

        public Point3D BasePoint { get; set; }
        public List<Wall> WallList { get; set; }
        public List<Column> ColumnList { get; set; }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement levelNode = xmlDocument.CreateElement("Level");
            XmlAttribute nameAttr = xmlDocument.CreateAttribute("Name");
            XmlText nameText = xmlDocument.CreateTextNode(Name);
            nameAttr.AppendChild(nameText);
            levelNode.Attributes.Append(nameAttr);
            XmlAttribute floorLevelAttr = xmlDocument.CreateAttribute("FloorLevel");
            XmlText floorLevelText = xmlDocument.CreateTextNode(Convert.ToString(FloorLevel));
            floorLevelAttr.AppendChild(floorLevelText);
            levelNode.Attributes.Append(floorLevelAttr);
            return levelNode;
        }

        public Level (Building building)
        {
            Name = "Этаж 1";
            Building = building;
            FloorLevel = 0;
            Height = 3000;
            TopOffset = -200;
            BasePoint = new Point3D(0, 0, 0);
            Quant = 1;
            WallList = new List<Wall>();
            building.LevelList.Add(this);
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
