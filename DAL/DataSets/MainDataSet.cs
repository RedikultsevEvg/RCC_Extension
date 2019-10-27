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
            #endregion
            dataTable = new DataTable("Generators");
            dataSet.Tables.Add(dataTable);
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            dataTable.Columns.Add(IdColumn);
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
            //Базы стальных колонн
            //DataTable SteelBases = new DataTable("SteelBases");
            //dataSet.Tables.Add(SteelBases);
            //DataColumn SteelBaseId = new DataColumn("Id", Type.GetType("System.Int32"));
            //DataColumn SteelBasePicture = new DataColumn("Picture", Type.GetType("System.Byte[]"));
            //DataColumn SteelBaseName = new DataColumn("Name", Type.GetType("System.String"));
            //DataColumn SteelBaseWidth = new DataColumn("Width", Type.GetType("System.Double"));
            //DataColumn SteelBaseLength = new DataColumn("Length", Type.GetType("System.Double"));
            //DataColumn SteelBaseArea = new DataColumn("Area", Type.GetType("System.Double"));
            //DataColumn SteelBaseWx = new DataColumn("Wx", Type.GetType("System.Double"));
            //DataColumn SteelBaseWy = new DataColumn("Wy", Type.GetType("System.Double"));

            //SteelBases.Columns.Add(SteelBaseId);
            //SteelBases.Columns.Add(SteelBasePicture);
            //SteelBases.Columns.Add(SteelBaseName);
            //SteelBases.Columns.Add(SteelBaseWidth);
            //SteelBases.Columns.Add(SteelBaseLength);
            //SteelBases.Columns.Add(SteelBaseArea);
            //SteelBases.Columns.Add(SteelBaseWx);
            //SteelBases.Columns.Add(SteelBaseWy);
            #endregion

            return dataSet;
        }
    }
}
