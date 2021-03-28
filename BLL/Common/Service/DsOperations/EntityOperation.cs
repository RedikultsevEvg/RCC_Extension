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
            string tableName = entity.GetTableName();
            DataTable dataTable;
            //Если датасет содержит нужную таблиц, то получаем ее
            if (dataSet.Tables.Contains(tableName)) { dataTable = dataSet.Tables[tableName]; }
            //Иначе создаем нужную таблицу
            else
            {
                dataTable = new DataTable(tableName);
                dataSet.Tables.Add(dataTable);
            }
            DataRow row = DsOperation.CreateNewRow(entity.Id, createNew, dataTable);
            DsOperation.SetField(row, "Id", entity.Id);
            if (! string.IsNullOrEmpty(entity.Name)) DsOperation.SetField(row, "Name", entity.Name);
            //Сохраняем родителя
            if (entity is IHasParent)
            {
                IHasParent child = entity as IHasParent;
                DsOperation.SetField(row, "ParentId", child.ParentMember.Id);
            }
            //Сохраняем детей
            if (entity is IHasChildren)
            {
                IHasChildren parent = entity as IHasChildren;
                foreach (IHasParent child in parent.Children) { child.SaveToDataSet(dataSet, createNew);}
            }
            //Сохраняем нагрузки
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
            //Сохранение центра фигуры
            if (entity is IShape) 
            {
                IShape obj = entity as IShape;
                DsOperation.SetField(row, "CenterX", obj.Center.X);
                DsOperation.SetField(row, "CenterY", obj.Center.Y);
            }
            //Сохранение диаметра
            if (entity is ICircle)
            {
                ICircle obj = entity as ICircle;
                DsOperation.SetField(row, "Diameter", obj.Diameter);
            }
            //Сохранение длины и ширины
            if (entity is IRectangle)
            {
                IRectangle obj = entity as IRectangle;
                DsOperation.SetField(row, "Width", obj.Width);
                DsOperation.SetField(row, "Length", obj.Length);
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
                row.SetField<string>("Type", obj.Type);
            }

            return row;
        }
        public static void SetProps(DataRow row, IDsSaveable entity)
        {
            entity.Id = row.Field<int>("Id");
            entity.Name = row.Field<string>("Name");
            if (entity is IShape)
            {
                IShape obj = entity as IShape;
                double dX = 0;
                DsOperation.Field(row, ref dX, "CenterX", 0);
                double dY = 0;
                DsOperation.Field(row, ref dY, "CenterY", 0);
                obj.Center = new Geometry.Point2D(dX, dY);
            }
            //Сохранение диаметра
            if (entity is ICircle)
            {
                ICircle obj = entity as ICircle;
                obj.Diameter = row.Field<double>("Diameter");
            }
            //Сохранение длины и ширины
            if (entity is IRectangle)
            {
                IRectangle obj = entity as IRectangle;
                obj.Width = row.Field<double>("Width");
                obj.Length = row.Field<double>("Length");
            }
            if (entity is IHasHeight)
            {
                IHasHeight obj = entity as IHasHeight;
                obj.Height = row.Field<double>("Height");
            }
            if (entity is IHasStoredParams)
            {
                IHasStoredParams obj = entity as IHasStoredParams;
                obj.Type = row.Field<string>("Type");
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
