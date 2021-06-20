using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Sections;
using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service
{
    public static class FieldGetter
    {
        public static object GetProperty(DataRow dataRow, Type type, string propertyName, string columnName)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            Type propertyType = propertyInfo.GetType();
            List<string> simpleTypeList = new List<string> { typeof(int).Name, typeof(double).Name, typeof(bool).Name, typeof(string).Name };
            if (propertyType == typeof(double)) { return dataRow.Field<double>(columnName);}
            else if (propertyType == typeof(int)) { return dataRow.Field<int>(columnName); }
            else if (propertyType == typeof(bool)) { return dataRow.Field<bool>(columnName); }
            else if (propertyType == typeof(string)) { return dataRow.Field<string>(columnName); }
            else if (propertyType == typeof(Point2D))
            {
                Point2D point = new Point2D();
                point.X = dataRow.Field<double>(columnName + "X");
                point.Y = dataRow.Field<double>(columnName + "Y");
                return point;
            }
            else if (propertyType is IShape)
            {
                Point2D center = GetProperty(dataRow, typeof(Point2D), propertyName+"_Center", columnName + "_Center") as Point2D;
                if (propertyType is ICircle)
                {
                    ICircle circle = new CircleSection();
                    circle.Center = center;
                    circle.Diameter = dataRow.Field<double>(columnName + "Diameter");
                    return circle;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
