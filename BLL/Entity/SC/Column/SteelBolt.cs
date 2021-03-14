using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Linq;
using DAL.Common;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBolt: ICloneable, IHasParent
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ссылка на базу
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public String Name { get; set; }

        public BoltUsing BoltUsing { get; set; }

        /// <summary>
        /// Участки НДМ
        /// </summary>
        public NdmCircleArea SubPart { get; set; }

        #region Constructors
        public SteelBolt(bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
        }

        public SteelBolt(SteelBase steelBase)
        {
            Id = ProgrammSettings.CurrentId;
            RegisterParent(steelBase);
            Name = "Новый болт";
        }
        #endregion
        #region IODataSet
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "SteelBolts"; }
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables[GetTableName()];
            if (createNew)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            else
            {
                var tmpRow = (from dataRow in dataTable.AsEnumerable()
                              where dataRow.Field<int>("Id") == Id
                              select dataRow).Single();
                row = tmpRow;
            }
            #region
            SetEntity.SetRow(row, this);
            BoltUsing.SaveToDataSet(dataSet, createNew);
            #endregion
            dataTable.AcceptChanges();
        }
        public void OpenFromDataSet(DataSet dataSet)
        {
            DataTable dataTable = dataSet.Tables[GetTableName()];
            var row = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("Id") == Id
                       select dataRow).Single();
            OpenFromDataSet(row);
        }
        /// <summary>
        /// Обновляет запись в соответствии со строкой датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        #region Methods
        public void SetParentNotActual()
        {
            //ParentMember.IsActual = false;
        }
        #endregion
        public object Clone()
        {
            SteelBolt steelBolt = this.MemberwiseClone() as SteelBolt;
            return steelBolt;
        }

        public void RegisterParent(IDsSaveable parent)
        {
            SteelBase steelBase = parent as SteelBase;
            ParentMember = steelBase;
            steelBase.SteelBolts.Add(this);
        }

        public void UnRegisterParent()
        {
            SteelBase steelBase = ParentMember as SteelBase;
            steelBase.SteelBolts.Remove(this);
            ParentMember = null;
        }
    }
}
