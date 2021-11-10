using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service.DsOperations
{
    internal class DataRowProcessor : IDataRowProcessor
    {
        public DataRow CreateDataRow(DataTable dataTable, int? id = null)
        {
            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);
            if (id == null)
            {
                id = ProgrammSettings.CurrentId;
            }
            FieldSetter.SetField(dataRow, "Id", id);
            return dataRow;
        }

        public void DeleteDataRowById(DataTable dataTable, int id)
        {
            DataRow dataRow = FindDataRowById(dataTable, id);
            dataRow.Delete();
        }

        public DataRow FindDataRowById(DataTable dataTable, int id)
        {

            try
            {
                var row = (from dataRow in dataTable.AsEnumerable()
                           where dataRow.Field<int>("Id") == id
                           select dataRow).Single();
                return row;
            }
            catch(Exception ex)
            {
                throw new Exception("Row is not found, error: " + ex);
            }
        }

        public IEnumerable<DataRow> FindDataRowsByParentId(DataTable dataTable, int parentId)
        {
            var rows = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("ParentId") == parentId
                       select dataRow);
            return rows;
        }
    }
}
