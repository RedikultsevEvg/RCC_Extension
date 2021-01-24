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
    /// Класс вида (класса) бетон
    /// </summary>
    public class ConcreteKind :IMaterialKind
    {
        /// <summary>
        /// Код класса бетона
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование класса
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Расчетное сопротивление сжатию для 1-й группы предельных состояний
        /// </summary>
        public double FstCompStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление сжатию для 2-й группы предельных состояний
        /// </summary>
        public double SndCompStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление растяжению для 1-й группы предельных состояний
        /// </summary>
        public double FstTensStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление растяжению для 2-й группы предельных состояний
        /// </summary>
        public double SndTensStrength { get; set; }
        /// <summary>
        /// Относительные деформации условных пластических деформаций при сжатии
        /// </summary>
        public double Epsb1Comp { get; set; }
        /// <summary>
        /// Относительные деформации при временном сопротивлении сжатию
        /// </summary>
        public double EpsbMaxComp { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Epsb2Comp { get; set; }
        /// <summary>
        /// Относительные деформации условных пластических деформаций при растяжении
        /// </summary>
        public double Epsb1Tens { get; set; }
        /// <summary>
        /// Относительные деформации при временном сопротивлении растяжению
        /// </summary>
        public double EpsbMaxTens { get; set; }
        /// <summary>
        /// Относительные деформации при разрушении при сжатии
        /// </summary>
        public double Epsb2Tens { get; set; }
        /// <summary>
        /// модуль упругости
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Коэффициент Пуассона
        /// </summary>
        public double PoissonRatio { get; set; }
        /// <summary>
        /// Коэффициент ползучести
        /// </summary>
        public double FiBCr { get; set; }

        #region IODataset
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
