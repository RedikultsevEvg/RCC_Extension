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
    /// Класс скального грунта
    /// </summary>
    public class RockSoil :BearingSoil, IDsSaveable
    {
        /// <summary>
        /// Нормативное сопротивление
        /// </summary>
        public double CrcStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление по 1-й группе ПС
        /// </summary>
        public double FstDesignStrength { get; set; }
        /// <summary>
        /// Расчетное сопротивление по 2-й группе ПС
        /// </summary>
        public double SndDesignStrength { get; set; }

        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite">Строительный объект</param>
        public RockSoil(BuildingSite buildingSite) : base(buildingSite)
        {
            Description = "Гранит малопрочный, сильновыветрелый";
            CrcDensity = 2400;
            FstDesignDensity = 2300;
            SndDesignDensity = 2250;
            CrcParticularDensity = 2700;
            FstParticularDensity = 2700;
            SndParticularDensity = 2700;
            PorousityCoef = 0.3;
            ElasticModulus = 2e8;
            SndElasticModulus = 1e8;
            PoissonRatio = 0.25;
            CrcStrength = 2e7;
            FstDesignStrength = 1.8e7;
            SndDesignStrength = 1.6e7;
        }
        /// <summary>
        /// Сохраняет класс в строку датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public override void SaveToDataSet(DataRow dataRow)
        {
            base.SaveToDataSet(dataRow);
            dataRow.SetField<string>("Type", "RockSoil");
            dataRow["CrcStrength"] = CrcStrength;
            dataRow["FstDesignStrength"] = FstDesignStrength;
            dataRow["SndDesignStrength"] = SndDesignStrength;

        }
        /// <summary>
        /// Обновляет запись в соответствии с датасетом
        /// </summary>
        /// <param name="dataSet"></param>
        public override void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables["Soils"];
            var soilRow = (from dataRow in dataTable.AsEnumerable()
                           where dataRow.Field<int>("Id") == Id
                           select dataRow).Single();
            OpenFromDataSet(soilRow);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public override void OpenFromDataSet(DataRow dataRow)
        {
            base.OpenFromDataSet(dataRow);
            CrcStrength = dataRow.Field<double>("CrcStrength");
            FstDesignStrength = dataRow.Field<double>("FstDesignStrength");
            SndDesignStrength = dataRow.Field<double>("SndDesignStrength");
        }
    }
}
