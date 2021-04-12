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
using RDBLL.Common.Service.DsOperations;

namespace RDBLL.Forces
{
    /// <summary>
    /// Группа нагрузок, приложенных в одной точке
    /// </summary>
    public class ForcesGroup: IDsSaveable, IDuplicate
    {
        #region Fields and properties
        /// <summary>
        /// Код группы нагрузок
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Обратная ссылка на коллекцию владельцев
        /// </summary>
        public List<IHasForcesGroups> Owners { get; set; }
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
            Owners = new List<IHasForcesGroups>();
            LoadSets = new ObservableCollection<LoadSet>();
        }
        /// <summary>
        /// Конструктор по фундаменту
        /// </summary>
        /// <param name="foundation"></param>
        public ForcesGroup(IHasForcesGroups parent)
        {
            Id = ProgrammSettings.CurrentId;
            Owners = new List<IHasForcesGroups>();
            Owners.Add(parent);
            LoadSets = new ObservableCollection<LoadSet>();
            Name = "Новая группа нагрузок";
        }
        #endregion
        #region Methods
        #region IODataset
        /// <summary>
        /// Return name of table in dataset for CRUD operation
        /// </summary>
        /// <returns>Name of table</returns>
        public string GetTableName() { return "ForcesGroups"; }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            row.SetField("CenterX", CenterX);
            row.SetField("CenterY", CenterY);
            row.AcceptChanges();
            //Удаляем записи по вложенным элементам
            DeleteSubElements(dataSet);
            //И создаем все нагрузки заново
            if (createNew)
            {
                DataTable dataTable;
                //Данные по нагрузкам на владельцев
                dataTable = dataSet.Tables["ParentForcesGroups"];
                foreach (IHasForcesGroups hasForces in Owners)
                {
                    row = dataTable.NewRow();
                    row.SetField("Id", ProgrammSettings.CurrentId);
                    row.SetField("ParentId", hasForces.Id);
                    row.SetField("ForcesGroupId", Id);
                    dataTable.Rows.Add(row);
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
            DsOperation.DeleteRow(dataSet, "ParentForcesGroups", "ForcesGroupId", Id);
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }
        #endregion
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
                int countLoadSet = query.Count();
                //Если данная комбинация больше нигде не встречается
                if (countLoadSet == 0)
                {
                    //Получаем запись данной комбинации и удаляем ее
                    DsOperation.DeleteRow(dataSet, "ForceParameters", "LoadSetId", loadSetId);
                    DsOperation.DeleteRow(dataSet, "LoadSets", loadSetId);
                }

            }
        }
        #endregion
        #region IClone
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ForcesGroup forcesGroup = this.MemberwiseClone() as ForcesGroup;
            forcesGroup.Id = ProgrammSettings.CurrentId;
            forcesGroup.Owners = new List<IHasForcesGroups>();
            forcesGroup.LoadSets = new ObservableCollection<LoadSet>();
            //копируем лоадсеты
            foreach (LoadSet loadSet in LoadSets)
            {
                LoadSet newLoadSet = loadSet.Clone() as LoadSet;
                newLoadSet.ForcesGroups.Add(forcesGroup);
                forcesGroup.LoadSets.Add(newLoadSet);
            }
            return forcesGroup;
        }
        #endregion
    }
}
