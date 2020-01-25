using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.ComponentModel;
using System.Data;

namespace RDBLL.Entity.Soils
{
    /// <summary>
    /// Класс слоя грунта
    /// </summary>
    public class SoilLayer : ISavableToDataSet

    {
        /// <summary>
        /// Код слоя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код модели грунта
        /// </summary>
        public int SoilId { get; set; }
        /// <summary>
        /// Ссылка на модель грунта
        /// </summary>
        public Soil Soil { get; set; }
        /// <summary>
        /// Отметка верха слоя
        /// </summary>
        public double TopLevel { get; set; }

        #region SaveToDataset
        /// <summary>
        /// Сохраняет класс в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["SoilSections"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
                { Id, SoilId, TopLevel };
            dataTable.Rows.Add(dataRow);
        }
        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
