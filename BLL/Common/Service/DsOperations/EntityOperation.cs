using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Params;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.Common.Placements;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Service
{
    /// <summary>
    /// Класс для операций с объектам в датасете
    /// </summary>
    public static class EntityOperation
    {
        /// <summary>
        /// Заполняет строку в соответствии с общими свойствами объекта
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        public static DataRow SaveEntity(DataSet dataSet, bool createNew, IDsSaveable entity)
        {
            DataTable dataTable = dataSet.Tables[entity.GetTableName()];
            DataRow row = DsOperation.CreateNewRow(entity.Id, createNew, dataTable);
            row.SetField("Id", entity.Id);
            if (entity.Name != null) row.SetField("Name", entity.Name);
            if (entity is IHasParent)
            {
                IHasParent child = entity as IHasParent;
                row.SetField("ParentId", child.ParentMember.Id);
            }
            if (entity is IHasForcesGroups)
            {
                IHasForcesGroups child = entity as IHasForcesGroups;
                foreach (ForcesGroup forcesGroup in child.ForcesGroups) { forcesGroup.SaveToDataSet(dataSet, createNew);}
            }
            if (entity is IHasSoilSection) //Сохранение использования скважины
            {
                IHasSoilSection child = entity as IHasSoilSection;
                child.SoilSectionUsing.SaveToDataSet(dataSet, createNew);
            }
            //Сохранение бетона
            if (entity is IHasConcrete)
            {
                IHasConcrete child = entity as IHasConcrete;
                if (child.Concrete != null) child.Concrete.SaveToDataSet(dataSet, createNew);
            }
            //Сохранение стали
            if (entity is IHasSteel)
            {
                IHasSteel child = entity as IHasSteel;
                if (child.Steel != null) child.Steel.SaveToDataSet(dataSet, createNew);
            }
            //Сохранение диаметра
            if (entity is ICircle) //Сохранение стали
            {
                ICircle obj = entity as ICircle;
                row.SetField("Diameter", obj.Diameter);
            }
            //Сохранение длины и ширины
            if (entity is IRectangle) //Сохранение стали
            {
                IRectangle obj = entity as IRectangle;
                row.SetField("Width", obj.Width);
                row.SetField("Length", obj.Length);
            }
            //Сохранение высоты
            if (entity is IHasHeight) //Сохранение стали
            {
                IHasHeight obj = entity as IHasHeight;
                row.SetField("Height", obj.Height);
            }
            //Сохранение расположения элементов
            if (entity is IHasPlacement)
            {
                IHasPlacement obj = entity as IHasPlacement;
                if (obj.Placement != null) obj.Placement.SaveToDataSet(dataSet, createNew);
            }
            //Сохранение хранимых параметров
            if (entity is IHasStoredParams)
            {
                IHasStoredParams obj = entity as IHasStoredParams;
                foreach (StoredParam param in obj.StoredParams) { param.SaveToDataSet(dataSet, createNew); }
            }

            return row;
        }
        public static void SetProps(DataRow row, IDsSaveable entity)
        {
            entity.Id = row.Field<int>("Id");
            entity.Name = row.Field<string>("Name");
            //Сохранение диаметра
            if (entity is ICircle) //Сохранение стали
            {
                ICircle obj = entity as ICircle;
                obj.Diameter = row.Field<double>("Diameter");
            }
            //Сохранение длины и ширины
            if (entity is IRectangle) //Сохранение стали
            {
                IRectangle obj = entity as IRectangle;
                obj.Width = row.Field<double>("Width");
                obj.Length = row.Field<double>("Length");
            }
            if (entity is IHasHeight) //Сохранение стали
            {
                IHasHeight obj = entity as IHasHeight;
                obj.Height = row.Field<double>("Height");
            }
        }
        public static void DeleteEntity (DataSet dataSet, IDsSaveable entity)
        {
            //Удаление объекта, имеющего нагрузки
            if (entity is IHasForcesGroups)
            {
                IHasForcesGroups child = entity as IHasForcesGroups;
                foreach (ForcesGroup forcesGroup in child.ForcesGroups)
                {
                    forcesGroup.DeleteFromDataSet(dataSet);
                }
            }
            //Удаление объекта, имеющего раскладку
            if (entity is IHasPlacement)
            {
                IHasPlacement child = entity as IHasPlacement;
                child.Placement.DeleteFromDataSet(dataSet);
            }
            //Удаление объекта, содержащего параметры
            if (entity is IHasStoredParams)
            {
                IHasStoredParams obj = entity as IHasStoredParams;
                foreach (StoredParam param in obj.StoredParams) { param.DeleteFromDataSet(dataSet); }
            }

            DsOperation.DeleteRow(dataSet, entity.GetTableName(), entity.Id);
        }
    }
}
