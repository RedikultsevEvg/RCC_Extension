using RDBLL.Common.ErrorProcessing.Messages;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Sections;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service.DsOperations.Mappings;
using RDBLL.Common.Service.DsOperations.Mappings.Factories;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    internal class DataSetProcessor : IDataSetProcessor
    {
        /// <summary>
        /// Сохранение свойств сущности в датасет
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="entity">Объект сущности, которая сохраняется</param>
        /// <param name="columnMappings">Коллекция маппингов столбцов</param>
        public void SavePropertiesToDataSet(DataRow dataRow, object entity, IEnumerable<ColumnMapping> columnMappings)
        {
            foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
            {
                string propertyName = propertyInfo.Name;
                foreach (ColumnMapping columnMapping in columnMappings)
                {
                    string targetPropertyName = columnMapping.PropertyName;
                    if (propertyName == targetPropertyName)
                    {
                        SavePropertyToRow(dataRow, propertyInfo, entity, targetPropertyName);
                    }
                }
            }
        }

        /// <summary>
        /// Назначение свойств сущности из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="entity">Объект сущности, свойства которой заполняются</param>
        /// <param name="columnMappings">Коллекция маппингов столбцов</param>
        public void LoadPropertiesFromDataSet(DataRow dataRow, object entity, IEnumerable<ColumnMapping> columnMappings)
        {
            foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
            {
                string propertyName = propertyInfo.Name;
                foreach (ColumnMapping columnMapping in columnMappings)
                {
                    string targetPropertyName = columnMapping.PropertyName;
                    if (propertyName == targetPropertyName)
                    {
                        LoadPropertyFromRow(dataRow, propertyInfo, entity);
                    }
                }
            }
        }

        private void LoadPropertyFromRow(DataRow dataRow, PropertyInfo propertyInfo, object entity)
        {
            //Наименование свойства
            string propertyName = propertyInfo.Name;
            //Тип свойства
            Type propertyType = propertyInfo.PropertyType;
            //Наименование типа свойства
            string propertyTypeName = propertyType.Name;
            //Если тип является точкой
            if (propertyTypeName == typeof(Point2D).Name)
            {
                Point2D point = GetPointFromProperty(dataRow, propertyInfo);
                propertyInfo.SetValue(entity, point);
            }
            else if (propertyTypeName == typeof(ICircle).Name)
            {
                ICircle circle = GetCircleFromProperty(dataRow, propertyInfo);
                propertyInfo.SetValue(entity, circle);
            }
            else if (propertyTypeName == typeof(ConcreteUsing).Name)
            {
                //ConcreteUsing concrete = new ConcreteUsing(false);
                //propertyInfo.SetValue(entity, concrete);
            }
            else if (propertyTypeName == typeof(ReinforcementUsing).Name)
            {
                //ReinforcementUsing reinforcement = new ReinforcementUsing(false);
                //propertyInfo.SetValue(entity, reinforcement);
            }
            //Если тип является строкой
            else if (propertyTypeName == typeof(string).Name)
            {
                string value = dataRow.Field<string>(propertyName);
                propertyInfo.SetValue(entity, value);
            }
            else //Если тип является типом значения
            {
                ValueType value;
                if (propertyTypeName == typeof(double).Name)
                {
                    value = dataRow.Field<double>(propertyName);
                }
                else if (propertyTypeName == typeof(int).Name)
                {
                    value = dataRow.Field<int>(propertyName);
                }
                else if (propertyTypeName == typeof(bool).Name)
                {
                    value = dataRow.Field<bool>(propertyName);
                }
                else
                {
                    throw new Exception(CommonMessages.TypeIsUknown);
                }
                propertyInfo.SetValue(entity, value);
            }
        }

        private void SavePropertyToRow(DataRow dataRow, PropertyInfo propertyInfo, object entity, string columnName)
        {
            Type type = propertyInfo.PropertyType;
            if (type == typeof(ValueType))
            {
                ValueType value = propertyInfo.GetValue(entity) as ValueType;
                dataRow.SetField(columnName, value);
            }            
            else if (type == typeof(string))
            {
                string value = propertyInfo.GetValue(entity) as string;
                dataRow.SetField(columnName, value);
            }
            else if (type == typeof(Point2D))
            {
                Point2D point = entity as Point2D;
                SavePoint2DToDataRow(dataRow, propertyInfo, columnName,  point);
            }
        }

        private void SavePoint2DToDataRow(DataRow dataRow, PropertyInfo propertyInfo, string columnName, Point2D entity)
        {
            Point2D point = propertyInfo.GetValue(entity) as Point2D;
            dataRow.SetField($"{columnName}X", point.X);
            dataRow.SetField($"{columnName}Y", point.Y);
        }

        private static ICircle GetCircleFromProperty(DataRow row, PropertyInfo propertyInfo)
        {
            string name = propertyInfo.Name;
            ICircle circle = new CircleSection();
            double x = row.Field<double>($"{name}CenterX");
            double y = row.Field<double>($"{name}CenterY");
            circle.Center = new Point2D(x, y);
            circle.Diameter = row.Field<double>("CircleDiameter");
            return circle;
        }

        private static Point2D GetPointFromProperty(DataRow row, PropertyInfo propertyInfo)
        {
            //Наименование свойства
            string propertyName = propertyInfo.Name;
            double x = row.Field<double>(propertyName + "X");
            double y = row.Field<double>(propertyName + "Y");
            Point2D point = new Point2D(x, y);
            return point;
        }
    }
}
