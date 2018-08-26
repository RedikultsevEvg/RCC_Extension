using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RDBLL.Common.Service
{
    class XMLOperations
    {
        public static void AddAttribute (XmlElement node, XmlDocument xmlDocument, String attrName, String attrValue)
        {
            XmlAttribute nameAttr = xmlDocument.CreateAttribute(attrName);
            XmlText nameText = xmlDocument.CreateTextNode(attrValue);
            nameAttr.AppendChild(nameText);
            node.Attributes.Append(nameAttr);
        }
    }
}
