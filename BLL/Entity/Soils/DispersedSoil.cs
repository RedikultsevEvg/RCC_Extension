using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Interfaces;
using System.Data;


namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс дисперсного грунта
    /// </summary>
    public class DispersedSoil :BearingSoil, ISavableToDataSet
    {
        /// <summary>
        /// Нормативное значение угла внутреннего трения
        /// </summary>
        public double CrcFi { get; set; }
        /// <summary>
        /// Расчетное значение угла внутреннего трения для 1-й группы ПС
        /// </summary>
        public double FstDesignFi { get; set; }
        /// <summary>
        /// Расчетное значение угла внутреннего трения для 2-й группы ПС
        /// </summary>
        public double SndDesignFi { get; set; }
        /// <summary>
        /// Нормативное значение сцепления, Па
        /// </summary>
        public double CrcCohesion { get; set; }
        /// <summary>
        /// Расчетное значение сцепления для 1-й группы ПС
        /// </summary>
        public double FstDesignCohesion { get; set; }
        /// <summary>
        /// Расчетное значение сцепления для 2-й группы ПС
        /// </summary>
        public double SndDesignCohesion { get; set; }

        /// <summary>
        /// Конструктор по строительному объекту
        /// </summary>
        /// <param name="buildingSite"></param>
        public DispersedSoil(BuildingSite buildingSite) :base(buildingSite)
        {
            CrcDensity = 1950;
            FstDesignDensity = 1800;
            SndDesignDensity = 1900;
            ElasticModulus = 2e7;
            SndElasticModulus = 1e8;
            PoissonRatio = 0.3;
            CrcFi = 20;
            FstDesignFi = 17;
            SndDesignFi = 18;
            CrcCohesion = 20000;
            FstDesignCohesion = 17000;
            SndDesignCohesion = 18000;
        }

        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        public override void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables["Soils"];
            if (createNew) { row = dataTable.NewRow(); }
            else
            {
                var soil = (from dataRow in dataTable.AsEnumerable()
                            where dataRow.Field<int>("Id") == Id
                            select dataRow).Single();
                row = soil;
            }          
            SaveToDataSet(row);
            dataTable.Rows.Add(row);
        }
        public override void SaveToDataSet(DataRow dataRow)
        {
            base.SaveToDataSet(dataRow);
            dataRow["CrcFi"] = CrcFi;
            dataRow["FstDesignFi"] = FstDesignFi;
            dataRow["SndDesignFi"] = SndDesignFi;
            dataRow["CrcCohesion"] = CrcCohesion;
            dataRow["FstDesignCohesion"] = FstDesignCohesion;
            dataRow["SndDesignCohesion"] = SndDesignCohesion;
        }
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
            CrcFi = dataRow.Field<double>("CrcFi");
            FstDesignFi = dataRow.Field<double>("FstDesignFi");
            SndDesignFi = dataRow.Field<double>("SndDesignFi");
            CrcCohesion = dataRow.Field<double>("CrcCohesion");
            FstDesignCohesion = dataRow.Field<double>("FstDesignCohesion");
            SndDesignCohesion = dataRow.Field<double>("SndDesignCohesion");
        }

    }
}
