using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Service;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Абстрактный класс несущего грунта
    /// </summary>
    public abstract class BearingSoil : Soil, ISavableToDataSet
    {
        /// <summary>
        /// Модуль деформации
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Модуль деформации по вторичной ветви нагружения
        /// </summary>
        public double SndElasticModulus { get; set; }
        /// <summary>
        /// Коэффициент Пуассона
        /// </summary>
        public double PoissonRatio { get; set; }
        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public BearingSoil(BuildingSite buildingSite) : base(buildingSite)
        {
        }
        /// <summary>
        /// Сохраняет запись в строку датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public override void SaveToDataSet(DataRow dataRow)
        {
            base.SaveToDataSet(dataRow);
            dataRow["ElasticModulus"] = ElasticModulus;
            dataRow["SndElasticModulus"] = SndElasticModulus;
            dataRow["PoissonRatio"] = PoissonRatio;
        }
        /// <summary>
        /// Обновляет запись по строке датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public override void OpenFromDataSet(DataRow dataRow)
        {
            base.OpenFromDataSet(dataRow);
            ElasticModulus = dataRow.Field<double>("ElasticModulus");
            SndElasticModulus = dataRow.Field<double>("SndElasticModulus");
            PoissonRatio = dataRow.Field<double>("PoissonRatio");
        }
    }

}
