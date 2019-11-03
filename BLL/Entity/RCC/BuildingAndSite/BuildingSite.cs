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



namespace RDBLL.Entity.RCC.BuildingAndSite
{
    public class BuildingSite :ICloneable, ISavableToDataSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Building> Buildings { get; set; }
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["BuildingSites"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, 0, Name };
            dataTable.Rows.Add(dataRow);
            foreach (Building building in Buildings)
            {
                building.SaveToDataSet(dataSet);
            }
        }
        public void OpenFromDataSet(DataSet dataSet, int Id)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables["BuildingSites"];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (Convert.ToInt32(dataTable.Rows[i].ItemArray[0]) == Id)
                {
                    this.Id = Id;
                    this.Name = Convert.ToString(dataTable.Rows[i].ItemArray[2]);
                    this.Buildings=GetEntity.GetBuildings(dataSet, this);
                }
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public BuildingSite()
        {
            Id = ProgrammSettings.CurrentId;
            Name = "Мой объект";
            Buildings = new ObservableCollection<Building>();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Building : ICloneable, ISavableToDataSet
    {
        public int Id { get; set; }
        public int BuildingSiteId { get; set; }
        public string Name { get; set; }
        public BuildingSite BuildingSite { get; set; }
        public ObservableCollection<Level> Levels { get; set; }
        public ObservableCollection<WallType> WallTypeList { get; set; }
        public ObservableCollection<OpeningType> OpeningTypeList { get; set; }
        public Building()
        {
            Levels = new ObservableCollection<Level>();
            WallTypeList = new ObservableCollection<WallType>();
            OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        public Building(BuildingSite buildingSite)
        {
            Id = ProgrammSettings.CurrentId;
            BuildingSiteId = buildingSite.Id;
            Name = "Мое здание";
            BuildingSite = buildingSite;
            Levels = new ObservableCollection<Level>();
            WallTypeList = new ObservableCollection<WallType>();
            OpeningTypeList = new ObservableCollection<OpeningType>();
        }
        public Building(BuildingSite buildingSite, XmlNode xmlNode)
        {
        }
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["Buildings"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, BuildingSiteId, Name };
            dataTable.Rows.Add(dataRow);
            foreach (Level level in Levels)
            {
                level.SaveToDataSet(dataSet);
            }
        }
        public void OpenFromDataSet(DataSet dataSet, int Id)
        {
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
