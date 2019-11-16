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

namespace RDBLL.Forces
{
    /// <summary>
    /// Группа нагрузок, приложенных в одной точке
    /// </summary>
    public class ForcesGroup: ISavableToDataSet
    {
        public int Id { get; set; }
        public List<SteelBase> SteelBases { get; set; } //Обратная ссылка. База стальной колонны к котрой относится группа нагрузок
        public ObservableCollection<LoadSet> LoadSets { get; set; } //Коллекция набора нагрузок
        public string Name { get; set; } //Наименование
        public double CenterX { get; set; } //Точка, к которой приложена группа нагрузок
        public double CenterY { get; set; } //Точка, к которой приложена группа нагрузок

        #region Constructors
        public ForcesGroup()
        {
            SteelBases = new List<SteelBase>();
            LoadSets = new ObservableCollection<LoadSet>();
        }
        /// <summary>
        /// Конструктор создает экземпляр класса группы нагрузок
        /// </summary>
        /// <param name="steelColumnBase"></param>
        public ForcesGroup(SteelBase steelColumnBase)
        {
            Id = ProgrammSettings.CurrentId;
            SteelBases = new List<SteelBase>();
            SteelBases.Add(steelColumnBase);
            LoadSets = new ObservableCollection<LoadSet>();
            Name = "Новая группа нагрузок";
        }
        #endregion
        #region Methods
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

        }

        public void SetParentsNotActual()
        {
            foreach (SteelBase steelBase in SteelBases)
            {
                steelBase.IsActual = false;
                steelBase.IsLoadCasesActual = false;
            }
        }
        #endregion
    }
}
