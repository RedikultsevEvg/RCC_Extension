using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using RDBLL.Common.Interfaces;
using System.Data;

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
        public NdmSteelArea SubPart { get; set; }

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
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["SteelBolts"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, SteelBaseId, Name, Diameter, CenterX, CenterY, AddSymmetricX, AddSymmetricY };
            dataTable.Rows.Add(dataRow);
        }

        public void OpenFromDataSet(DataSet dataSet, int Id)
        {

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
