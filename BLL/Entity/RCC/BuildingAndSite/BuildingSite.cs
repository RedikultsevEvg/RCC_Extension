using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.WallAndColumn;
using System.Collections.ObjectModel;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Entity.Soils;
using RDBLL.Entity.MeasureUnits;
using System.ComponentModel;
using System.Windows;
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Entity.RCC.BuildingAndSite
{
    /// <summary>
    /// Класс строительного объекта
    /// </summary>
    public class BuildingSite :ICloneable, IDsSaveable
    {
        /// <summary>
        /// Код строительного объекта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Коллекция зданий
        /// </summary>
        public ObservableCollection<Building> Children { get; private set; }
        /// <summary>
        /// Коллекция грунтов строительного объекта
        /// </summary>
        public ObservableCollection<Soil> Soils { get; set; }
        /// <summary>
        /// Коллекция геологических разрезов
        /// </summary>
        public ObservableCollection<SoilSection> SoilSections { get; set; }
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "BuildingSites"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            row.Table.AcceptChanges();
            //Добавляем вложенные элементы
            foreach (Soil soil in Soils)
            {
                soil.SaveToDataSet(dataSet, createNew);
            }
            foreach (SoilSection soilSection in SoilSections)
            {
                soilSection.SaveToDataSet(dataSet, createNew);
            }
            //Здание надо добавлять позже грунтов, так как грунты нужны для фундаментов
            foreach (Building building in Children)
            {
                building.SaveToDataSet(dataSet, createNew);
            }
        }
        /// <summary>
        /// Открытие из датасета
        /// </summary>
        /// <param name="dataSet"></param>
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
            this.Id = dataRow.Field<int>("Id");
            this.Name = dataRow.Field<string>("Name");
        }
        /// <summary>
        /// Удаляет запись из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (SoilSection section in SoilSections)
            {
                section.DeleteFromDataSet(dataSet);
            }
            foreach (Soil soil in Soils)
            {
                soil.DeleteFromDataSet(dataSet);
            }
            //EntityOperation.DeleteEntity(dataSet, this);
            foreach (IDsSaveable child in Children)
            {
                child.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
        /// <summary>
        /// Возвращает грунт по умолчанию
        /// </summary>
        /// <returns></returns>
        public Soil AddDefaultSoil()
        {
            if (Soils.Count > 0) { return Soils[0]; } //Если в коллекции грунтов есть хотя бы один грунт, то возвращаем его
            else
            {
                MessageBox.Show("Для объекта не задано ни одного грунта", "Будет создан грунт по умолчанию");
                Soil soil = new ClaySoil(this);
                soil.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                ProgrammSettings.IsDataChanged = true;
                return soil;
            }
        }
        /// <summary>
        /// Возвращает скважину по умолчанию
        /// </summary>
        /// <param name="building"></param>
        /// <returns></returns>
        public SoilSection AddDefaultSoilSection(Building building)
        {
            SoilLayer AddSoilLayer(SoilSection soilSection)
            {
                Soil soil = AddDefaultSoil();
                SoilLayer soilLayer = new SoilLayer(soil, soilSection, building);
                return soilLayer;
            }
            ProgrammSettings.IsDataChanged = true;
            if (SoilSections.Count > 0)  //Если в коллекции есть хоть одна скважина, возвращаем ее
            {
                SoilSection section = SoilSections[0];
                if (section.SoilLayers.Count == 0)
                {
                    MessageBox.Show("Для скважины не задано ни одного грунта", "Будет создан грунт по умолчанию");
                    SoilLayer soilLayer = AddSoilLayer(section);
                    //Сохраняем слой в датасет, так как скважина уже есть
                    soilLayer.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                }
                return section;
            }
            else //Если ни одной скважины в коллекции нет, то добавляем ее
            {
                MessageBox.Show("Для объекта не задано ни одной скважины", "Будет создана скважина по умолчанию");
                SoilSection section = new SoilSection(this);
                AddSoilLayer(section);
                //Сохраняем в датасет новую скважину
                section.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                return section;
            }
        }
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public BuildingSite(bool genId = false)
        {
            if (genId) Id = ProgrammSettings.CurrentId;
            Name = "Мой объект";
            Children = new ObservableCollection<Building>();
            Soils = new ObservableCollection<Soil>();
            SoilSections = new ObservableCollection<SoilSection>();
        }

    }
}
