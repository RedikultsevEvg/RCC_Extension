using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL.DataSets
{
    public static class MainDataSet
    {
        public static DataSet GetNewDataSet()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable;
            DataColumn IdColumn, FkIdColumn, NameColumn, NewColumn;
            #region Generators
            dataTable = new DataTable("Generators");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            #endregion
            #region Version
            dataTable = new DataTable("Versions");
            dataSet.Tables.Add(dataTable);
            DataColumn version = new DataColumn("Version", Type.GetType("System.Int32"));
            dataTable.Columns.Add(version);
            #endregion
            #region ProgrammSetting
            dataTable = new DataTable("MeasurementUnits");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            #endregion
            #region BuildingSites
            dataTable = new DataTable("BuildingSites");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            FkIdColumn = new DataColumn("ParentId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            dataTable.Columns.Add(NameColumn);
            #endregion
            #region Buildings
            dataTable = new DataTable("Buildings");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            FkIdColumn = new DataColumn("BuildingSiteId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            dataTable.Columns.Add(NameColumn);
            #endregion
            #region Levels
            dataTable = new DataTable("Levels");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            FkIdColumn = new DataColumn("BuildingId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            dataTable.Columns.Add(NameColumn);
            NewColumn = new DataColumn("FloorLevel", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Height", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("TopOffset", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("BasePointX", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("BasePointY", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("BasePointZ", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            #endregion
            #region SteelBases
            dataTable = new DataTable("SteelBases");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
            FkIdColumn = new DataColumn("LevelId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            FkIdColumn = new DataColumn("SteelClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            FkIdColumn = new DataColumn("ConcreteClassId", Type.GetType("System.Int32"));
            dataTable.Columns.Add(FkIdColumn);
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            dataTable.Columns.Add(NameColumn);
            NewColumn = new DataColumn("SteelStrength", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("ConcreteStrength", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("IsActual", Type.GetType("System.Boolean"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Width", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Length", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Thickness", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("WorkCondCoef", Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            #endregion

            return dataSet;
        }
    }
}
