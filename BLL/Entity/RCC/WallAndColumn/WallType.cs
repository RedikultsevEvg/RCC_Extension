using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Reinforcement;
using RDBLL.Common.Service;
using System.Xml;

namespace RDBLL.Entity.RCC.WallAndColumn
{
    public class WallType :ICloneable
    {
        public String Name { get; set; }
        public Building Building { get; set; }
        public double Thickness { get; set; }
        public double TopOffset { get; set; }
        public double BottomOffset { get; set; }
        public double BarTopOffset { get; set; }
        public bool RoundVertToBaseLength { get; set; }
        public double VertBaseLength { get; set; }
        public bool HorLapping { get; set; }
        public double HorLappingLength { get; set; }
        public double HorBaseLength { get; set; }
        public BarSpacingSettings VertSpacingSetting { get; set; }
        public BarSpacingSettings HorSpacingSetting { get; set; }

        public double GetHeight(Level level)
        {
            return level.Height + level.TopOffset + TopOffset + BottomOffset;
        }
        public XmlElement SaveToXMLNode(XmlDocument xmlDocument)
        {
            XmlElement xmlNode = xmlDocument.CreateElement("WallType");
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Name", Name);
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "Thickness", Convert.ToString(Thickness));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "TopOffset", Convert.ToString(TopOffset));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "BottomOffset", Convert.ToString(BottomOffset));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "BarTopOffset", Convert.ToString(BarTopOffset));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "RoundVertToBaseLength", Convert.ToString(RoundVertToBaseLength));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "VertBaseLength", Convert.ToString(VertBaseLength));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "HorLapping", Convert.ToString(HorLapping));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "HorLappingLength", Convert.ToString(HorLappingLength));
            XMLOperations.AddAttribute(xmlNode, xmlDocument, "HorBaseLength", Convert.ToString(HorBaseLength));
            XmlElement VertSpacingSettingNode = VertSpacingSetting.SaveToXMLNode(xmlDocument, "VertSpacingSetting");
            xmlNode.AppendChild(VertSpacingSettingNode);
            XmlElement HorSpacingSettingNode = HorSpacingSetting.SaveToXMLNode(xmlDocument, "HorSpacingSetting");
            xmlNode.AppendChild(HorSpacingSettingNode);
            return xmlNode;
        }
        public WallType(Building building)
        {
            Name = "Новый тип стены";
            Thickness = 200;
            TopOffset = 0;
            BottomOffset = 0;
            BarTopOffset = 600;
            RoundVertToBaseLength = false;
            VertBaseLength = 11700;
            HorLapping = true;
            HorLappingLength = 400;
            HorBaseLength = 11700;
            VertSpacingSetting = new BarSpacingSettings(1);
            HorSpacingSetting = new BarSpacingSettings(2);
            Building = building;
/*            building.WallTypeList.Add(this)*/;
        }
        public WallType(Building building, XmlNode xmlNode)
        {
            Building = building;
            foreach (XmlAttribute obj in xmlNode.Attributes)
            {
                if (obj.Name == "Name") Name = obj.Value;
                if (obj.Name == "Thickness") Thickness = Convert.ToDouble(obj.Value);
                if (obj.Name == "TopOffset") TopOffset = Convert.ToDouble(obj.Value);
                if (obj.Name == "BottomOffset") BottomOffset = Convert.ToDouble(obj.Value);
                if (obj.Name == "BarTopOffset") BarTopOffset = Convert.ToDouble(obj.Value);
                if (obj.Name == "RoundVertToBaseLength") RoundVertToBaseLength = Convert.ToBoolean(obj.Value);
                if (obj.Name == "VertBaseLength") VertBaseLength = Convert.ToDouble(obj.Value);
                if (obj.Name == "HorLapping") HorLapping = Convert.ToBoolean(obj.Value);
                if (obj.Name == "HorLappingLength") HorLappingLength = Convert.ToDouble(obj.Value);
                if (obj.Name == "HorBaseLength") HorBaseLength = Convert.ToDouble(obj.Value);
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    if (childNode.Name == "VertSpacingSetting") VertSpacingSetting = new BarSpacingSettings(childNode);
                    if (childNode.Name == "HorSpacingSetting") HorSpacingSetting = new BarSpacingSettings(childNode);
                }
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
