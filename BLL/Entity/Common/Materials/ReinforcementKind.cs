using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials.Interfaces;
using System.Data;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Reinforcement class
    /// </summary>
    public class ReinforcementKind :IMaterialKind
    {
        /// <summary>
        /// Код класс
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование класса
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Нормативное сопротивление сжатию
        /// </summary>
        public double CrcCompStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление сжатию для 1-й группы предельных состояний
        /// </summary>
        public double FstCompStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление сжатию для 2-й группы предельных состояний
        /// </summary>
        public double SndCompStrength { get; set; }
        /// <summary>
        /// Нормативное сопротивление растяжению
        /// </summary>
        public double CrcTensStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление растяжению для 1-й группы предельных состояний
        /// </summary>
        public double FstTensStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление растяжению для 2-й группы предельных состояний
        /// </summary>
        public double SndTensStrength { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Eps2Comp { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Eps2Tens { get; set; }
        /// <summary>
        /// модуль упругости
        /// </summary>
        public double ElasticModulus { get; set; }
        public double PoissonRatio { get => 0.3; }

        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "ReinforcementKinds"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
