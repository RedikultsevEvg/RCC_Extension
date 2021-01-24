using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс распределения арматуры
    /// </summary>
    public class ReinforcementSpacing :ISavableToDataSet
    {
        /// <summary>
        /// Код распределения
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Код типа распределения
        /// 1 - по шагу +1 стержень
        /// 2 - по шагу -1 стержень
        /// 3 - по шагу
        /// 4 - по количеству стержней
        /// </summary>
        public int SpacingType { get; set; }
        /// <summary>
        /// Шаг стержней
        /// </summary>
        public double Spacing { get; set; }
        /// <summary>
        /// Количество стержней
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Диаметр стержня
        /// </summary>
        public double Diameter { get; set; }
        /// <summary>
        /// Величина защитного слоя
        /// </summary>
        public double CoveringLayer { get; set; }
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }
        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
    }
}
