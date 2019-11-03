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


namespace RDBLL.Entity.RCC.BuildingAndSite
{
    public class Level :ICloneable, ISavableToDataSet
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public string Name { get; set; }
        public double FloorLevel { get; set; }
        public double Height { get; set; }
        public double TopOffset { get; set; }
        public double BasePointX { get; set; }
        public double BasePointY { get; set; }
        public double BasePointZ { get; set; }
        public ObservableCollection<Wall> Walls { get; set; }
        public ObservableCollection<Column> Columns { get; set; }
        public ObservableCollection<SteelBase> SteelBases { get; set; }

        public double GetConcreteVolumeNetto()
        {
            double volume = 0;
            foreach (Wall obj in Walls)
            {
                volume += obj.GetConcreteVolumeNetto();
            }
            return volume;

        }
        public void SaveToDataSet(DataSet dataSet)
        {
            DataTable dataTable;
            DataRow dataRow;
            dataTable = dataSet.Tables["Levels"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { Id, BuildingId, Name, FloorLevel, Height, TopOffset, BasePointX, BasePointY, BasePointY };
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

        public Level ()
        {
            Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
        }

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
            FloorLevel = 0;
            Height = 3000;
            TopOffset = -200;
            Walls = new ObservableCollection<Wall>();
            SteelBases = new ObservableCollection<SteelBase>();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
