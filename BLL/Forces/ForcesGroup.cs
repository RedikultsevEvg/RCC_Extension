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
using Winforms = System.Windows.Forms;
using System.Windows;
using DAL.Common;

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
            //Удаляем записи по вложенным элементам
            DeleteSubElements(dataSet);
            //И создаем все нагрузки заново
            if (createNew)
            {
                //Данные по нагрузкам на стальные базы
                dataTable = dataSet.Tables["SteelBaseForcesGroups"];
                foreach (SteelBase steelBase in SteelBases)
                {
                    row = dataTable.NewRow();
                    dataTable.Rows.Add(row);
                    #region
                    row.SetField("Id", ProgrammSettings.CurrentId);
                    row.SetField("SteelBaseId", steelBase.Id);
                    row.SetField("ForcesGroupId", Id);
                    #endregion
                }
                dataTable.AcceptChanges();
                //Данные по нагрузкам на фундамент
                dataTable = dataSet.Tables["FoundationForcesGroups"];
                foreach (Foundation foundation in Foundations)
                {
                    row = dataTable.NewRow();
                    dataTable.Rows.Add(row);
                    #region
                    row.SetField("Id", ProgrammSettings.CurrentId);
                    row.SetField("FoundationId", foundation.Id);
                    row.SetField("ForcesGroupId", Id);
                    #endregion
                }
                dataTable.AcceptChanges();
            }
            foreach (LoadSet loadSet in LoadSets)
            {
                loadSet.SaveToDataSet(dataSet, true);
            }
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
            DeleteSubElements(dataSet);
            DsOperation.DeleteRow(dataSet, "SteelBaseForcesGroups","ForcesGroupId", Id);
            DsOperation.DeleteRow(dataSet, "FoundationForcesGroups", "ForcesGroupId", Id);
            DsOperation.DeleteRow(dataSet, "ForcesGroups", Id);
        }
        private void DeleteSubElements(DataSet dataSet)
        {
            DataTable dataTable;
            int count;
            DataRow[] rows;
            //Удаляем записи в комбинациях нагрузок
            dataTable = dataSet.Tables["ForcesGroupLoadSets"];
            rows = dataTable.Select("ForcesGroupId=" + Id);
            count = rows.Length;
            for (int i = count - 1; i >= 0; i--)
            {
                //Проверяем, встречается ли комбинация нагрузок еще где-то
                int loadSetId = rows[i].Field<int>("LoadSetId");
                dataTable.Rows.Remove(rows[i]);
                //Получаем коллекцию записей комбинаций в группах нагрузок где встречается данная комбинация
                DataTable adjDataTable = dataSet.Tables["ForcesGroupLoadSets"];
                var query = from adjDataRow in adjDataTable.AsEnumerable()
                            from dataRow in dataTable.AsEnumerable()
                            where adjDataRow.Field<int>("LoadSetId") == loadSetId
                            select dataRow;
                int countLoadSet = 0;
                foreach (var dataRow in query) { countLoadSet++; }
                //Если данная комбинация больше нигде не встречается
                if (countLoadSet == 0)
                {
                    //Получаем запись данной комбинации и удаляем ее
                    DsOperation.DeleteRow(dataSet, "ForceParameters", "LoadSetId", loadSetId);
                    DsOperation.DeleteRow(dataSet, "LoadSets", loadSetId);
                }

            }
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
