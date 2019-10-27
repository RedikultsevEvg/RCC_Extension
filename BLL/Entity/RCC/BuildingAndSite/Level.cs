using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.RCC.WallAndColumn;
using RDBLL.Common.Service;
using System.Xml;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;


namespace RDBLL.Entity.RCC.BuildingAndSite
{
    public class Level :ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public decimal FloorLevel { get; set; }
        public decimal Height { get; set; }
        public decimal TopOffset { get; set; }
        public int Quant { get; set; }
        public Point3D BasePoint { get; set; }
        public ObservableCollection<Wall> Walls { get; set; }
        public ObservableCollection<Column> Columns { get; set; }
        public ObservableCollection<SteelBase> SteelBases { get; set; }

        public decimal GetConcreteVolumeNetto()
        {
            decimal volume = 0;
            foreach (Wall obj in Walls)
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
            foreach (Wall obj in Walls)
            {
                xmlNode.AppendChild(obj.SaveToXMLNode(xmlDocument));
            }
            return xmlNode;
        }

        public Level (Building building)
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Этаж 1";
            Building = building;
            FloorLevel = 0;
            Height = 3000;
            TopOffset = -200;
            BasePoint = new Point3D(0, 0, 0);
            Quant = 1;
            Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
            building.LevelList.Add(this);
        }

        public Level(Building building, XmlNode xmlNode)
        {
            Building = building;
            Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {          
                if (obj.Name == "Name") Name = obj.Value;
                if (obj.Name == "FloorLevel") FloorLevel = Convert.ToDecimal( obj.Value);
                if (obj.Name == "Height") Height = Convert.ToDecimal(obj.Value);
                if (obj.Name == "TopOffset") TopOffset = Convert.ToDecimal(obj.Value);
                BasePoint = new Point3D(0, 0, 0);
                if (obj.Name == "Quant") Quant = Convert.ToInt16(obj.Value);
            }
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name == "Wall") Walls.Add(new Wall(this, childNode));
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
