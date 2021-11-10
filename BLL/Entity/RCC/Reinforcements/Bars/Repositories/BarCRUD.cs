using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Common.Service.DsOperations.Mappings;
using RDBLL.Entity.RCC.Reinforcements.Bars.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Bars.Storages
{
    public class BarCRUD : IRepository<IBarSection>
    {
        private IDataTableProcessor _TableProcessor = new DataTableProcessor();
        private IDataRowProcessor _RowProcessor = new DataRowProcessor();
        private IDataSetProcessor _SetProcessor = new DataSetProcessor();
        private DataSet _DataSet;

        public BarCRUD(DataSet dataSet)
        {
            this._DataSet = dataSet;
        }

        private DataTable DataTable { get => _TableProcessor.GetTable(_DataSet, typeof(IBarSection)); }

        public void Create(IBarSection obj)
        {
            DataRow row;
            try
            {
                row = _RowProcessor.FindDataRowById(DataTable, obj.Id);
            }
            catch (Exception ex)
            {
                row = _RowProcessor.CreateDataRow(DataTable, obj.Id);
            }
            IBarSection entity = obj as IBarSection;
            if (entity.ParentMember != null) FieldSetter.SetField(row, "ParentId", entity.ParentMember.Id);
            FieldSetter.SetField(row, "Name", obj.Name);
            FieldSetter.SetField(row, "Circle", obj.Circle);
            entity.Reinforcement.SaveToDataSet(_DataSet, true);
        }

        public void Delete(IBarSection obj)
        {
            _RowProcessor.DeleteDataRowById(DataTable, obj.Id);
        }

        public IBarSection GetEntityById(int id)
        {
            DataRow dataRow = _RowProcessor.FindDataRowById(DataTable, id);
            IBarSection entity = BarSectionFactory.GetBarSection(BarType.BarWithoutPlace);
            IEnumerable<ColumnMapping> mappings = GetColumnMappings();
            _SetProcessor.LoadPropertiesFromDataSet(dataRow, entity, mappings);
            return entity;
        }

        public void Save()
        {
            DataTable.AcceptChanges();
        }

        public void Update(IBarSection obj)
        {
            DataRow row = _RowProcessor.FindDataRowById(DataTable, obj.Id);

        }

        public void UpdateObject(IBarSection obj)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ColumnMapping> GetColumnMappings()
        {
            List<ColumnMapping> mappings = new List<ColumnMapping>();
            ColumnMapping mapping;
            mapping = new ColumnMapping() { PropertyName = "Circle", ColumnName = "Circle" };
            mappings.Add(mapping);
            mapping = new ColumnMapping() { PropertyName = "Prestrain", ColumnName = "Prestrain" };
            mappings.Add(mapping);
            mapping = new ColumnMapping() { PropertyName = "Reinforcement", ColumnName = "Reinforcement" };
            mappings.Add(mapping);
            return mappings;
        }
    }
}
