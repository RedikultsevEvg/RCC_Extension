using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.Geometry;
using RCC_Extension.BLL.WallAndColumn;
using RCC_Extension.BLL.Service;
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

        public decimal GetConcreteVolumeNetto()
        {
            decimal volume = 0;
            foreach (Wall obj in WallList)
            {
                volume += obj.GetConcreteVolumeNetto();
            }
            return volume;

        }

        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("Level");
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Name", Name);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "FloorLevel", Convert.ToString(FloorLevel));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Height", Convert.ToString(Height));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "TopOffset", Convert.ToString(TopOffset));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Quant", Convert.ToString(Quant));
            foreach (Wall obj in WallList)
            {
                xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            return xmlNode;
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
