using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Linq;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBolt: ICloneable, ISavableToDataSet
    {
        public int Id { get; set; } //Код
        public int SteelBaseId { get; set; } //Код базы
        public SteelBase SteelBase { get; set; } //Ссылка на базу
        public String Name { get; set; }
        public double Diameter { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public bool AddSymmetricX { get; set; } //Наличие симметричного участка относительно оси X
        public bool AddSymmetricY { get; set; } //Наличие симметричного участка по оси Y
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
        #region Methods
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            dataTable = dataSet.Tables["SteelBolts"];
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
