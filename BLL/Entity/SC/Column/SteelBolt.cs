using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Linq;
using DAL.Common;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBolt: ICloneable, ISavableToDataSet
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код базы
        /// </summary>
        public int SteelBaseId { get; set; }
        /// <summary>
        /// Ссылка на базу
        /// </summary>
        public SteelBase SteelBase { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Диаметр
        /// </summary>
        public double Diameter { get; set; }
        /// <summary>
        /// Положение центра X
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Положение центра Y
        /// </summary>
        public double CenterY { get; set; }
        /// <summary>
        /// Наличие симметричного участка относительно оси X
        /// </summary>
        public bool AddSymmetricX { get; set; }
        /// <summary>
        /// Наличие симметричного участка по оси Y
        /// </summary>
        public bool AddSymmetricY { get; set; } 
        /// <summary>
        /// Участки НДМ
        /// </summary>
        public NdmCircleArea SubPart { get; set; }

        #region Constructors
        public SteelBolt()
        {
        }

        public SteelBolt(SteelBase steelBase)
        {
            Id = ProgrammSettings.CurrentId;
            SteelBaseId = steelBase.Id;
            SteelBase = steelBase;
            Name = "Новый болт";
            Diameter = 0.030;
            CenterX = 0.200;
            CenterY = 0.300;
            AddSymmetricX = true;
            AddSymmetricY = true;
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
            row.SetField("Id", Id);
            row.SetField("SteelBaseId", SteelBaseId);
            row.SetField("Name", Name);
            row.SetField("Diameter", Diameter);
            row.SetField("CenterX", CenterX);
            row.SetField("CenterY", CenterY);
            row.SetField("AddSymmetricX", AddSymmetricX);
            row.SetField("AddSymmetricY", AddSymmetricY);
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
            SteelBaseId = dataRow.Field<int>("SteelBaseId");
            Name = dataRow.Field<string>("Name");
            Diameter = dataRow.Field<double>("Diameter");
            CenterX = dataRow.Field<double>("CenterX");
            CenterY = dataRow.Field<double>("CenterY");
            AddSymmetricX = dataRow.Field<bool>("AddSymmetricX");
            AddSymmetricY = dataRow.Field<bool>("AddSymmetricY");
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
            SteelBase.IsActual = false;
            SteelBase.IsBoltsActual = false;
        }
        #endregion
        public object Clone()
        {
            SteelBolt steelBolt = this.MemberwiseClone() as SteelBolt;
            return steelBolt;
        }
    }
}
