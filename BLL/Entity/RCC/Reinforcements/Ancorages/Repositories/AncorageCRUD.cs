using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Common.Service.DsOperations.Mappings;
using RDBLL.Entity.RCC.Reinforcements.Ancorages.Factories;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using RDBLL.Entity.RCC.Reinforcements.Bars.Storages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Ancorages.Repositories
{
    public class AncorageCRUD : IRepository<IAncorage>
    {
        private IDataTableProcessor _TableProcessor = new DataTableProcessor();
        private IDataRowProcessor _RowProcessor = new DataRowProcessor();
        private IDataSetProcessor _SetProcessor = new DataSetProcessor();
        private DataSet _DataSet;
        private DataTable _DataTable { get => _TableProcessor.GetTable(_DataSet, typeof(IAncorage)); }

        public AncorageCRUD(DataSet dataSet)
        {
            this._DataSet = dataSet;
        }

        public void Create(IAncorage obj)
        {
            DataRow row;
            try
            {
                row = _RowProcessor.FindDataRowById(_DataTable, obj.Id);
            }
            catch (Exception ex)
            {
                row = _RowProcessor.CreateDataRow(_DataTable, obj.Id);
            }
            IAncorage entity = obj as IAncorage;
            if (entity.ParentMember != null) FieldSetter.SetField(row, "ParentId", entity.ParentMember.Id);
            FieldSetter.SetField(row, "Name", obj.Name);
            entity.Concrete.SaveToDataSet(_DataSet, true);

            string childTableName = DsOperation.GetTableName(typeof(IBarSection));
            IRepository<IBarSection> repository = new BarCRUD(_DataSet);
            foreach (IBarSection barSection in entity.BarSections)
            {
                repository.Create(barSection);
                repository.Save();
            }
        }

        public void Delete(IAncorage obj)
        {
            _RowProcessor.DeleteDataRowById(_DataTable, obj.Id);
        }

        public IAncorage GetEntityById(int id)
        {
            DataRow dataRow = _RowProcessor.FindDataRowById(_DataTable, id);
            IAncorageLogic ancorageLogic = new AncorageLogic();
            IAncorage entity = new Ancorage(ancorageLogic, false);
            IEnumerable<ColumnMapping> mappings = GetColumnMappings();
            _SetProcessor.LoadPropertiesFromDataSet(dataRow, entity, mappings);
            return entity;
        }

        public void Save()
        {
            _DataTable.AcceptChanges();
        }
        /// <summary>
        /// Обновляет запись в хранилище по имеющейся сущности
        /// </summary>
        /// <param name="obj"></param>
        public void Update(IAncorage obj)
        {
            DataRow row = DsOperation.OpenFromDataTableById(_DataTable, obj.Id);
            IAncorage entity = obj as IAncorage;
            if (entity.ParentMember != null) DsOperation.SetField(row, "ParentId", entity.ParentMember.Id);
            DsOperation.SetRowFields(row, entity);
        }
        /// <summary>
        /// Обновляет сущность по имеющейся записи в датасете
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateObject(IAncorage obj)
        {

        }

        private IEnumerable<ColumnMapping> GetColumnMappings()
        {
            List<ColumnMapping> mappings = new List<ColumnMapping>();
            ColumnMapping mapping;
            mapping = new ColumnMapping() { PropertyName = "Name", ColumnName = "Name" };
            mappings.Add(mapping);
            mapping = new ColumnMapping() { PropertyName = "LongLoadRate", ColumnName = "LongLoadRate" };
            mappings.Add(mapping);
            return mappings;
        }
    }
}
