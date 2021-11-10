using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    public static class FieldSetter
    {
        /// <summary>
        /// Устанавливает значение свойства в строку таблицы датасета
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="dataRow">Ссылка на строку таблицы датасета</param>
        /// <param name="columnName">Наименование столбца</param>
        /// <param name="value">Устанавливаемое значение</param>
        public static void SetField<T>(DataRow dataRow, string columnName, T value)
        {
            DataTable table = dataRow.Table;
            //Если необходимого столбца нет в датасете, добавляем его
            if (!table.Columns.Contains(columnName))
            {
                if (value is ValueType || value is string)
                {
                    DataColumn column = new DataColumn(columnName, typeof(T));
                    table.Columns.Add(column);
                }              
            }
            //Если свойство является типом значения или строкой, просто сохраняем его
            if (value is ValueType || value is string)
            {
                dataRow.SetField(columnName, value);
            }
            //Если свойство является точкой, то сохраняем ее в столбцы
            else if (value is Point2D)
            {
                Point2D point = value as Point2D;
                SetField(dataRow, columnName + "X", point.X);
                SetField(dataRow, columnName + "Y", point.Y);
            }
            //Если свойство является формой
            else if (value is IShape)
            {
                IShape shape = value as IShape;
                SetField(dataRow, columnName + "Center", shape.Center);
                //Если свойство является кругом
                if (value is ICircle)
                {
                    ICircle circle = value as ICircle;
                    SetField(dataRow, columnName + "Diameter", circle.Diameter);
                }
            }
        }
    }
}
