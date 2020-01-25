using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;
using RDBLL.Common.Service;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Entity.RCC.Foundations;

namespace RDBLL.Forces
{
    /// <summary>
    /// Группа нагрузок, приложенных в одной точке
    /// </summary>
    public class ForcesGroup: ISavableToDataSet
    {
        #region Fields and properties
        /// <summary>
        /// Код группы нагрузок
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка. База стальной колонны к котрой относится группа нагрузок
        /// </summary>
        public List<SteelBase> SteelBases { get; set; }
        /// <summary>
        /// Обратная ссылка на коллекцию фундаментов
        /// </summary>
        public List<Foundation> Foundations { get; set; }
        /// <summary>
        /// Коллекция набора нагрузок
        /// </summary>
        public ObservableCollection<LoadSet> LoadSets { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Точка, к которой приложена группа нагрузок
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Точка, к которой приложена группа нагрузок
        /// </summary>
        public double CenterY { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ForcesGroup()
        {
            SteelBases = new List<SteelBase>();
            Foundations = new List<Foundation>();
            LoadSets = new ObservableCollection<LoadSet>();
        }
        /// <summary>
        /// Конструктор по стальной базе
        /// </summary>
        /// <param name="steelColumnBase"></param>
        public ForcesGroup(SteelBase steelColumnBase)
        {
            Id = ProgrammSettings.CurrentId;
            SteelBases = new List<SteelBase>();
            Foundations = new List<Foundation>();
            SteelBases.Add(steelColumnBase);
            LoadSets = new ObservableCollection<LoadSet>();
            Name = "Новая группа нагрузок";
        }
        /// <summary>
        /// Конструктор по фундаменту
        /// </summary>
        /// <param name="foundation"></param>
        public ForcesGroup(Foundation foundation)
        {
            Id = ProgrammSettings.CurrentId;
            SteelBases = new List<SteelBase>();
            Foundations = new List<Foundation>();
            Foundations.Add(foundation);
            LoadSets = new ObservableCollection<LoadSet>();
            Name = "Новая группа нагрузок";
        }
        #endregion
        #region Methods
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            DataRow row;
            //Данные по группам нагрузок
            dataTable = dataSet.Tables["ForcesGroups"];
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
            row.SetField("Name", Name);
            row.SetField("CenterX", CenterX);
            row.SetField("CenterY", CenterY);
            #endregion
            dataTable.AcceptChanges();
            //Данные по нагрузкам на стальные базы
            dataTable = dataSet.Tables["SteelBaseForcesGroups"];
            foreach (SteelBase steelBase in SteelBases)
            {
                if (createNew)
                {
                    row = dataTable.NewRow();
                    dataTable.Rows.Add(row);
                }
                else
                {
                    var tmpRow = (from dataRow in dataTable.AsEnumerable()
                              where dataRow.Field<int>("ForcesGroupId") == Id
                              select dataRow).Single();
                    row = tmpRow;
                }
                #region           
                row.SetField("SteelBaseId", steelBase.Id);
                row.SetField("ForcesGroupId", Id);
                #endregion
            }
            dataTable.AcceptChanges();
            //Данные по нагрузкам на фундамент
            dataTable = dataSet.Tables["FoundationForcesGroups"];
            foreach (Foundation foundation in Foundations)
            {
                if (createNew)
                {
                    row = dataTable.NewRow();
                    dataTable.Rows.Add(row);
                }
                else
                {
                    var tmpRow = (from dataRow in dataTable.AsEnumerable()
                                  where dataRow.Field<int>("ForcesGroupId") == Id
                                  select dataRow).Single();
                    row = tmpRow;
                }
                #region           
                row.SetField("FoundationId", foundation.Id);
                row.SetField("ForcesGroupId", Id);
                #endregion
            }
            dataTable.AcceptChanges();
            foreach (LoadSet loadSet in LoadSets) { loadSet.SaveToDataSet(dataSet, createNew);}
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

        /// <summary>
        /// Установка родителей неактуальными
        /// </summary>
        public void SetParentsNotActual()
        {
            foreach (SteelBase steelBase in SteelBases)
            {
                steelBase.IsActual = false;
                steelBase.IsLoadCasesActual = false;
            }
            foreach (Foundation foundation in Foundations)
            {
                foundation.IsLoadCasesActual = false;
            }
        }
        #endregion
    }
}
