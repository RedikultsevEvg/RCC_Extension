using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCC_Extension.BLL.BuildingAndSite;
using RCC_Extension.BLL.Reinforcement;
using RCC_Extension.BLL.Service;
using System.Xml;

namespace RCC_Extension.BLL.WallAndColumn
{
    public class WallType :ICloneable
    {
        public String Name { get; set; }
        public Building building { get; set; }

        public decimal Thickness { get; set; }
        public decimal TopOffset { get; set; }
        public decimal BottomOffset { get; set; }

        public decimal BarTopOffset { get; set; }

        public bool RoundVertToBaseLength { get; set; }
        public decimal VertBaseLength { get; set; }

        public bool HorLapping { get; set; }
        public decimal HorLappingLength { get; set; }
        public decimal HorBaseLength { get; set; }
        
        public BarSpacingSettings VertSpacingSetting { get; set; }
        public BarSpacingSettings HorSpacingSetting { get; set; }


        public decimal GetHeight(Level level)
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

            building.WallTypeList.Add(this);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
