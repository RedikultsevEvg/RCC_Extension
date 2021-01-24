using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.Materials.Interfaces;
using RDBLL.Common.Interfaces;
using System.Data;
using DAL.Common;
using RDBLL.Common.Service;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Класс использования арматуры в конструкции
    /// </summary>
    public class ReinforcementUsing : IMaterialKind
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Код  выбранного вида арматуры
        /// </summary>
        public int ReinforcementKindId { get; set; }
        /// <summary>
        /// Ссылка на выбранный вид арматуры
        /// </summary>
        public ReinforcementKind ReinforcementKind { get; set; }
        /// <summary>
        /// Список расположения арматуры
        /// </summary>
        public List<ReinforcementSpacing> ReinforcementSpacings {get;set;}
        private MaterialUsing ParentMaterial { get; set; }
        #region Constructors
        public ReinforcementUsing()
        {
            ReinforcementSpacings = new List<ReinforcementSpacing>();
            
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables["Reinforcementusings"];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
            row.SetField("ReinforcementKindId", ReinforcementKindId);
            row.SetField("MaterialUsingId", ParentMaterial.Id);
            #endregion
            dataTable.AcceptChanges();
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
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, "Reinforcementusing", Id);
            throw new NotImplementedException();
        }
        #endregion
        #region Method
        public void RegisterParent(MaterialUsing materialUsing)
        {
            ParentMaterial = materialUsing;
        }
        public void RenewKind()
        {
            ReinforcementKind = MaterialProcessor.GetMaterialKindById("Reinforcement", ReinforcementKindId) as ReinforcementKind;
        }
        #endregion
    }
}
