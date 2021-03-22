using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;
using RDBLL.Common.Interfaces;
using System.Data;
using System.Linq;
using RDBLL.Entity.Common.Placements;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Interfaces.Materials;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBolt: ICloneable, IHasParent, ICircle, IHasSteel, IHasPlacement
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
        /// <summary>
        /// Диаметр болта
        /// </summary>
        public double Diameter { get; set; }
        /// <summary>
        /// Класс стали
        /// </summary>
        public SteelUsing Steel { get; set; }
        /// <summary>
        /// Размещение
        /// </summary>
        public Placement Placement { get; set; }

        //public BoltUsing BoltUsing { get; set; }
        public MeasureUnitList Measures { get => new MeasureUnitList(); }

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
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            row.AcceptChanges();
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
            EntityOperation.SetProps(dataRow, this);
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            EntityOperation.DeleteEntity(dataSet, this);
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
            steelBolt.Id = ProgrammSettings.CurrentId;
            steelBolt.SetPlacement(this.Placement.Clone() as Placement);
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

        public double GetArea()
        {
            return Math.PI * Diameter * Diameter / 4;
        }

        public void SetPlacement(Placement placement)
        {
            this.Placement = placement;
        }
    }
}
