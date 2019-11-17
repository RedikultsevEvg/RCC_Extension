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
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["ForcesGroups"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[]
            { Id, Name,
                CenterX, CenterY
            };
            dataTable.Rows.Add(dataRow);

            dataTable = dataSet.Tables["SteelBaseForcesGroups"];
            foreach (SteelBase steelBase in SteelBases)
            {
                dataRow = dataTable.NewRow();
                dataRow.ItemArray = new object[]
                { steelBase.Id, this.Id
                };
                dataTable.Rows.Add(dataRow);
            }
            foreach (LoadSet loadSet in LoadSets)
            {
                loadSet.SaveToDataSet(dataSet);
            }
        }

        public void OpenFromDataSet(DataSet dataSet, int Id)
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
                foundation.IsLoadsActual = false;
            }
        }
        #endregion
    }
}
