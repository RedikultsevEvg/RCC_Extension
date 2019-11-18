using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.RCC.WallAndColumn;
using RDBLL.Common.Service;
using System.Xml;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;
using System.Data;
using RDBLL.Common.Interfaces;
using RDBLL.Entity.RCC.Foundations;


namespace RDBLL.Entity.RCC.BuildingAndSite
{
    /// <summary>
    /// Уровень
    /// </summary>
    public class Level :ICloneable, ISavableToDataSet
    {
        /// <summary>
        /// Код уровня
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код здания
        /// </summary>
        public int BuildingId { get; set; }
        /// <summary>
        /// Обратная ссылка на здание
        /// </summary>
        public Building Building { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Отметка уровня
        /// </summary>
        public double Elevation { get; set; }
        /// <summary>
        /// Высота этажа
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Сдвижка сверху
        /// </summary>
        public double TopOffset { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public double BasePointX { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public double BasePointY { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public double BasePointZ { get; set; }
        /// <summary>
        /// Привязка базовой точки
        /// </summary>
        public ObservableCollection<Wall> Walls { get; set; }
        /// <summary>
        /// Коллекция колонн
        /// </summary>
        public ObservableCollection<Column> Columns { get; set; }
        /// <summary>
        /// Коллекция стальных баз
        /// </summary>
        public ObservableCollection<SteelBase> SteelBases { get; set; }
        /// <summary>
        /// Коллекция фундаментов
        /// </summary>
        public ObservableCollection<Foundation> Foundations { get; set; }

        /// <summary>
        /// Получение суммарного объема бетона
        /// </summary>
        /// <returns></returns>
        public double GetConcreteVolumeNetto()
        {
            double volume = 0;
            foreach (Wall obj in Walls)
            {
                volume += obj.GetConcreteVolumeNetto();
            }
            return volume;

        }
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["Levels"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, BuildingId, Name, Elevation, Height, TopOffset, BasePointX, BasePointY, BasePointY };
            dataTable.Rows.Add(dataRow);
            foreach (SteelBase steelBase in SteelBases)
            {
                steelBase.SaveToDataSet(dataSet);
            }
        }

        public void OpenFromDataSet(DataSet dataSet, int Id)
        {
            DataTable dataTable, childTable;
            dataTable = dataSet.Tables["Levels"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (Convert.ToInt32(dataTable.Rows[i].ItemArray[0]) == Id)
                {
                    this.Id = Id;
                    this.BuildingId = Convert.ToInt32(dataTable.Rows[i].ItemArray[1]);
                    this.Name = Convert.ToString(dataTable.Rows[i].ItemArray[2]);
                    childTable = dataSet.Tables["SteelBases"];
                    if (childTable != null)
                    {
                        for (int j = 0; j < childTable.Rows.Count; j++)
                        {
                            if (Convert.ToInt32(childTable.Rows[j].ItemArray[1]) == this.Id)
                            {
                                SteelBase newObject = new SteelBase(this);
                                newObject.OpenFromDataSet(dataSet, Convert.ToInt32(childTable.Rows[j].ItemArray[0]));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Level ()
        {
            Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
            Foundations = new ObservableCollection<Foundation>();
        }

        /// <summary>
        /// Конструктор по зданию
        /// </summary>
        /// <param name="building"></param>
        public Level (Building building)
        {
            if (Id == 0) { Id = ProgrammSettings.CurrentId; }
            else {
                this.Id = Id;
            }         
            BuildingId = building.Id;
            Name = "Этаж 1";
            Building = building;
            building.Levels.Add(this);
            Elevation = 0;
            Height = 3;
            TopOffset = -0.2;
            Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
            Foundations = new ObservableCollection<Foundation>();
        }

        /// <summary>
        /// Метод клонирования
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
