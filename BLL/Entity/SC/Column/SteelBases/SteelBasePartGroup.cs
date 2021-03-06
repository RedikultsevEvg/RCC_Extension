﻿using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.SC.Column.SteelBases.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases
{
    /// <summary>
    /// Класс для группы участков стальной базы
    /// </summary>
    public class SteelBasePartGroup : IHasParent, ICloneable, IHasSteel, IHasHeight
    {

        #region Properties
        /// <summary>
        /// Код элемента
        /// </summary>
        public int Id { get; set ; }
        /// <summary>
        /// Обратная ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get ; set ; }
        /// <summary>
        /// Признак актуальности расчета
        /// </summary>
        public bool IsActual { get; set; }
        /// <summary>
        /// Коллекция участков
        /// </summary>
        public ObservableCollection<SteelBasePart> SteelBaseParts { get; set; }
        /// <summary>
        /// Использование класса стали
        /// </summary>
        public SteelUsing Steel { get; set; }
        /// <summary>
        /// Высота (толщина) участков
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Давление на участки
        /// </summary>
        public double Pressure { get; set; }
        /// <summary>
        /// Коллекция строк для протокола расчета
        /// </summary>
        public List<string> ReportList { get; set; }
        #endregion
        #region Constructor
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="genId"></param>
        public SteelBasePartGroup (bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
            IsActual = false;
            SteelBaseParts = new ObservableCollection<SteelBasePart>();
            MatFactProc.GetMatType(this, MatType.SteelBasePartGroup);
        }
        #endregion
        /// <summary>
        /// Клонирование элемента
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            SteelBasePartGroup newObject = this.MemberwiseClone() as SteelBasePartGroup;
            newObject.Id = ProgrammSettings.CurrentId;
            newObject.SteelBaseParts = new ObservableCollection<SteelBasePart>();
            foreach (SteelBasePart part in SteelBaseParts)
            {
                SteelBasePart newPart = part.Clone() as SteelBasePart;
                newPart.UnRegisterParent();
                newPart.RegisterParent(newObject);
            }
            newObject.Steel = this.Steel.Clone() as SteelUsing;
            return newObject;
        }
        /// <summary>
        /// Удаление объекта из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            //Участки являются простым элементом, удаляем просто так
            DsOperation.DeleteRow(dataSet, "SteelBaseParts", "ParentId", Id);
            //Болт не является простым элементом, поэтому удалять его просто из таблицы нельзя
            EntityOperation.DeleteEntity(dataSet, this);
        }
        /// <summary>
        /// Возвращает имя таблицы
        /// </summary>
        /// <returns></returns>
        public string GetTableName() { return "SteelBasePartGroups"; }
        /// <summary>
        /// Открывает запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Открывает строку из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            EntityOperation.SetProps(dataRow, this);
            double pressure = 0;
            DsOperation.Field(dataRow, ref pressure, "Pressure", 0);
            Pressure = pressure;
            IsActual = false;
        }
        /// <summary>
        /// Регистрирует родителя
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            Level level = parent as Level;
            ParentMember = level;
            level.Children.Add(this);
        }
        /// <summary>
        /// Сохраняет запись в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            DsOperation.SetField(row, "Pressure", Pressure);
            row.AcceptChanges();
            foreach (SteelBasePart steelBasePart in SteelBaseParts)
            {
                steelBasePart.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Удаляет ссылку на родителя
        /// </summary>
        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.Children.Remove(this);
        }
    }
}
