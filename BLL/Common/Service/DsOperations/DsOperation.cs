using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using RDBLL.Common.Interfaces;
using System.Reflection;
using RDBLL.Common.Geometry;
using RDBLL.Common.Service.DsOperations.Mappings;
using RDBLL.Common.Service.DsOperations.Mappings.Factories;
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Entity.Common.Materials;
using RDBLL.Common.ErrorProcessing.Messages;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Common.Geometry.Sections;

namespace RDBLL.Common.Service.DsOperations
{
    public enum TableNames
    {
        Fondations,
        FoundationParts,
    }
    public enum FieldsNames
    {
        Id,
        Name,
        ParentId,
    }
    /// <summary>
    /// Базовые операции с датасетами
    /// </summary>
    public static class DsOperation
    {
        /// <summary>
        /// Добавляет столбец с кодом в таблицу
        /// </summary>
        /// <param name="dataTable">Таблица датасета</param>
        /// <param name="addNameColumn">Флаг необходимости добавления столбца с наименованием</param>
        public static List<DataColumn> AddIdColumn(DataTable dataTable, bool addNameColumn = false)
        {
            List<DataColumn> dataColumns = new List<DataColumn>();
            DataColumn IdColumn;
            IdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
            IdColumn.ColumnMapping = MappingType.Attribute;
            //IdColumn.Unique = true;
            dataTable.Columns.Add(IdColumn);
            dataColumns.Add(IdColumn);
            //dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["Id"] };
            if (addNameColumn & (! dataTable.Columns.Contains("Name"))) { dataColumns.Add(AddNameColumn(dataTable)); }
            return dataColumns;
        }
        /// <summary>
        /// Добавление внешного ключа
        /// </summary>
        /// <param name="parentDataTableName">Таблица данных</param>
        /// <param name="parentColumnName">Наименование столбца в дочерней таблице</param>
        /// <param name="childDataTable">Наименование дочерней таблицы</param>
        /// <param name="allowNull">Флаг допустимости Null</param>
        public static DataColumn AddFkIdColumn(string parentDataTableName, string parentColumnName, DataTable childDataTable, bool allowNull = false)
        {
            DataSet dataSet = childDataTable.DataSet;
            DataTable parentDataTable = dataSet.Tables[parentDataTableName];
            string columnName = parentColumnName;
            DataColumn FkIdColumn;
            FkIdColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            FkIdColumn.ColumnMapping = MappingType.Attribute;
            childDataTable.Columns.Add(FkIdColumn);
            ForeignKeyConstraint foreignKey;
            //Если допускается нулевое значение, то при удалении устанавливаем в нулл
            if (allowNull)
            {
                foreignKey = new ForeignKeyConstraint(parentDataTable.Columns["Id"], childDataTable.Columns[columnName])
                {
                    ConstraintName = parentDataTableName + childDataTable.TableName + "ForeignKey",
                    DeleteRule = Rule.SetNull,
                    UpdateRule = Rule.Cascade
                };
            }
            //Иначе при удалении удаляем каскадно
            else
            {
                foreignKey = new ForeignKeyConstraint(parentDataTable.Columns["Id"], childDataTable.Columns[columnName])
                {
                    ConstraintName = parentDataTableName + childDataTable.TableName + "ForeignKey",
                    DeleteRule = Rule.Cascade,
                    UpdateRule = Rule.Cascade
                };
            }
            childDataTable.Constraints.Add(foreignKey);
            dataSet.EnforceConstraints = true;
            dataSet.Relations.Add(parentDataTable.TableName + childDataTable.TableName, parentDataTable.Columns["Id"], childDataTable.Columns[columnName]);

            return FkIdColumn;
        }

        internal static IEnumerable<int> GetIdsFromParentId(DataTable dataTable, int parentId)
        {
            List<int> idS = new List<int>();
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parentId
                        select dataRow;
            foreach (var dataRow in query)
            {
                idS.Add(dataRow.Field<int>("Id"));
            }
            return idS;
        }

        public static DataColumn AddFkIdColumn(string parentColumnName, DataTable childDataTable, bool allowNull = false)
        {
            DataSet dataSet = childDataTable.DataSet;
            string columnName = parentColumnName;
            DataColumn FkIdColumn;
            FkIdColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            FkIdColumn.AllowDBNull = allowNull;
            FkIdColumn.ColumnMapping = MappingType.Attribute;
            childDataTable.Columns.Add(FkIdColumn);
            return FkIdColumn;
        }
        /// <summary>
        /// Добавляет стандартные столбцы
        /// </summary>
        /// <param name="dataTable">Ссылка на таблицу, в которую добавляются столбцы</param>
        /// <param name="parentTable">Наименование родительской таблицы</param>
        /// <returns></returns>
        public static List<DataColumn> AddCommonColumns(DataTable dataTable, string parentTable = null)
        {
            List<DataColumn> dataColumns = new List<DataColumn>();
            dataColumns.AddRange(AddIdColumn(dataTable, true));
            if (string.IsNullOrEmpty(parentTable))
            {
                DataColumn ParentIdColumn;
                ParentIdColumn = new DataColumn("ParentId", Type.GetType("System.Int32"));
                ParentIdColumn.ColumnMapping = MappingType.Attribute;
                dataTable.Columns.Add(ParentIdColumn);
                dataColumns.Add(ParentIdColumn);
            }
            else dataColumns.Add(AddFkIdColumn(parentTable, "ParentId", dataTable));
            return dataColumns;
        }
        public static void AddIntColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            dataTable.Columns.Add(NewColumn);
        }
        public static DataColumn AddIntColumn(DataTable dataTable, string columnName, int defaultValue)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            NewColumn.AllowDBNull = false;
            NewColumn.DefaultValue = defaultValue;
            dataTable.Columns.Add(NewColumn);
            return NewColumn;
        }
        public static DataColumn AddDoubleColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Double"));
            dataTable.Columns.Add(NewColumn);
            return NewColumn;
        }
        public static DataColumn AddDoubleColumn(DataTable dataTable, string columnName, double defaultValue)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Double"));
            NewColumn.AllowDBNull = false;
            NewColumn.DefaultValue = defaultValue;
            dataTable.Columns.Add(NewColumn);
            return NewColumn;
        }
        public static void AddBoolColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Boolean"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void AddBoolColumn(DataTable dataTable, string columnName, bool defaultValue)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Boolean"));
            NewColumn.AllowDBNull = false;
            NewColumn.DefaultValue = defaultValue;
            dataTable.Columns.Add(NewColumn);
        }
        /// <summary>
        /// Добавляет столбец типа String в таблицу датасета
        /// </summary>
        /// <param name="dataTable">Таблица датасета</param>
        /// <param name="columnName">Имя создаваемого столбца</param>
        public static void AddStringColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.String"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void AddByteColumn(DataTable dataTable, string columnName)
        {
            DataColumn NewColumn;
            NewColumn = new DataColumn(columnName, Type.GetType("System.Byte[]"));
            dataTable.Columns.Add(NewColumn);
        }
        public static void DeleteRow(DataSet dataSet, string dataTableName, int Id)
        {
            DeleteRow(dataSet, dataTableName, "Id", Id);
        }
        public static void DeleteRow(DataSet dataSet, string dataTableName, string keyFieldName, int Id)
        {
            DataTable dataTable = dataSet.Tables[dataTableName];
            DeleteRow(dataTable, keyFieldName, Id);
        }
        public static void DeleteRow(DataTable dataTable, string keyFieldName, int Id)
        {
            DataRow[] rows = dataTable.Select($"{keyFieldName}={Id}");
            int count = rows.Length;
            for (int j = count - 1; j >= 0; j--)
            {
                dataTable.Rows.Remove(rows[j]);
            }
        }
        /// <summary>
        /// Возвращает строку из датасета с указанным Id
        /// </summary>
        /// <param name="Id">Код элемента</param>
        /// <param name="createNew">Флаг необходимости создания нового элемента</param>
        /// <param name="dataTable">Ссылка на таблицу в которой ищется запись</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static DataRow CreateNewRow(int Id, bool createNew, DataTable dataTable)
        {
            DataRow row;
            //Если флаг создания новой записи установлен
            if (createNew)
            {
                row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            //Иначе
            else
            {
                try
                {
                    //Находим нужную запись

                    var oldRow = (from dataRow in dataTable.AsEnumerable()
                                  where dataRow.Field<int>("Id") == Id
                                  select dataRow).Single();
                    row = oldRow;
                }
                catch //Если нужная запись не находится
                {
                    //Добавляем ее
                    row = dataTable.NewRow();
                    dataTable.Rows.Add(row);
                }
            }
            return row;
        }
        /// <summary>
        /// Возвращает строку из датасета по Id
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// /// <param name="dataTableName">Имя таблицы</param>
        /// <param name="Id">Код элемента</param>
        /// <returns></returns>
        public static DataRow OpenFromDataSetById(DataSet dataSet, string dataTableName, int Id)
        {
            DataTable dataTable = dataSet.Tables[dataTableName];
            return OpenFromDataTableById(dataTable, Id);
        }
        public static DataRow OpenFromDataTableById(DataTable dataTable, int Id)
        {
            var row = (from dataRow in dataTable.AsEnumerable()
                       where dataRow.Field<int>("Id") == Id
                       select dataRow).Single();
            return row;
        }
        /// <summary>
        /// Возвращает строку из датасета по элементу (необходимо для отката изменений)
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static DataRow OpenFromDataSetById(DataSet dataSet, IDsSaveable entity)
        {
            return OpenFromDataSetById(dataSet, GetTableName(entity), entity.Id);
        }
        public static void SetId(DataRow dataRow, int id, string name = null, int? parentId = null)
        {
            dataRow.SetField("Id", id);
            if (name != null) dataRow.SetField("Name", name);
            if (parentId != null) dataRow.SetField("ParentId", parentId);
        }
        public static object GetCoolection(DataSet dataSet, string tableName, IDsSaveable parent)
        {
            DataTable dataTable = dataSet.Tables[tableName];
            var query = from dataRow in dataTable.AsEnumerable()
                        where dataRow.Field<int>("ParentId") == parent.Id
                        select dataRow;
            return query;
        }
        /// <summary>
        /// Устанавливает значение свойства в строку таблицы датасета
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="row">Ссылка на строку таблицы датасета</param>
        /// <param name="columnName">Наименование столбца</param>
        /// <param name="value">Устанавливаемое значение</param>
        public static void SetField<T>(DataRow row, string columnName, T value)
        {
            DataTable table = row.Table;
            if (!table.Columns.Contains(columnName))
            {
                DataColumn column = new DataColumn(columnName, typeof(T));
                table.Columns.Add(column);
            }
            row.SetField(columnName, value);
        }
        public static void Field<T>(DataRow row, ref T target, string columnName, T defaultValue)
        {
            DataTable table = row.Table;
            if (!table.Columns.Contains(columnName))
            {
                DataColumn column = new DataColumn(columnName, typeof(T));
                column.DefaultValue = defaultValue;
                table.Columns.Add(column);
            }
            target = row.Field<T>(columnName);
        }
        //public static ValueType GetField<T>(DataRow row, string columnName, T defaultValue)
        //{
        //    DataTable table = row.Table;
        //    if (!table.Columns.Contains(columnName))
        //    {
        //        DataColumn column = new DataColumn(columnName, typeof(T));
        //        column.DefaultValue = defaultValue;
        //        table.Columns.Add(column);
        //    }
        //    return row.Field<T>(columnName);
        //}
        /// <summary>
        /// Возвращает таблицу если она имеется в датасете или создает новую
        /// </summary>
        /// <param name="dataSet">Датасет, в который добавляется таблица</param>
        /// <param name="tableName">Наименование таблицы</param>
        /// <param name="parentTableName">Наименование родительской таблицы (необязательно)</param>
        /// <returns></returns>
        public static DataTable GetDataTable(DataSet dataSet, string tableName, string parentTableName = null)
        {
            DataTable dataTable;
            //Если датасет содержит нужную таблицу, то получаем ее
            if (dataSet.Tables.Contains(tableName)) { dataTable = dataSet.Tables[tableName]; }
            //Иначе создаем нужную таблицу
            else
            {
                dataTable = new DataTable(tableName);
                dataSet.Tables.Add(dataTable);
                AddCommonColumns(dataTable, parentTableName);
            }
            return dataTable;
        }
        /// <summary>
        /// Добавляет все свойства указанного класса как столбцы таблицы датасета
        /// </summary>
        /// <param name="dataTable">Таблица датасета</param>
        /// <param name="type">Тип, для которого необходимо добавить столбцы</param>
        public static void AddColumnsFromProperties(DataTable dataTable, Type type)
        {
            //Коллекция наименования столбцов, которые добавлять не нужно
            IEnumerable<string> exceptList = GetExceptColumnList();
            //Коллекция типов, которые нужно добавлять в таблицу
            IEnumerable<string> typeList = GetStringsFromTypes(GetColumnTypesToAddToTable());
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                //Наименование свойства
                string name = propertyInfo.Name;
                //Наименование типа свойства
                string propertyType = propertyInfo.PropertyType.Name;
                if (!(exceptList.Contains(name)) //Если наименование таблицы не входит в перечень исключенных
                    & (typeList.Contains(propertyType))) //И входит в перечень допустимых типов значения
                {
                    //Получаем коллекцию столбцов, которые надо добавить в таблицу
                    List<ColumnTemplate> columns = GetTypes(propertyInfo);
                    //Добавляем столбцы в таблицу
                    AddColumnFromTypes(dataTable, columns);
                }
            }
        }
        /// <summary>
        /// Возвращает таблицу с названием равным имени типа и наименованию родительской таблицы
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <param name="parentTableName">Наименование родительской таблицы</param>
        /// <returns></returns>
        public static DataTable AddTableToDataset(Type type)
        {
            //Получаем имя для новой таблицы в соответствие с типом
            string tableName = GetTableName(type);
            DataTable dataTable = new DataTable(tableName);
            //Добавляем столбцы из свойств типа
            AddColumnsFromProperties(dataTable, type);
            return dataTable;
        }
        /// <summary>
        /// Выполняет сериализацию объекта в строку датасета
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        public static void SetRowFields(DataRow row, IHasId obj)
        {
            //Коллекция наименования столбцов, которые добавлять не нужно
            IEnumerable<string> exceptList = GetExceptColumnList();
            //Коллекция типов, которые нужно добавлять в таблицу
            List<string> typeList = new List<string>();
            typeList.AddRange(GetStringsFromTypes(GetColumnSimpleTypes()));
            typeList.AddRange(GetStringsFromTypes(GetColumnComplexTypes()));
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                DataTable dataTable = row.Table;
                //Наименование свойства
                string name = propertyInfo.Name;
                //Наименование типа свойства
                string propertyTypeName = propertyInfo.PropertyType.Name;
                if (!(exceptList.Contains(name)) //Если наименование таблицы не входит в перечень исключенных
                    & (typeList.Contains(propertyTypeName))) //И входит в перечень допустимых типов значения
                {

                    if (propertyTypeName == typeof(Point2D).Name)
                    {
                        Point2D point = propertyInfo.GetValue(obj) as Point2D;
                        SetField(row, name + "X", point.X);
                        SetField(row, name + "Y", point.Y);
                    }
                    else if (propertyTypeName == typeof(ConcreteUsing).Name
                        ||
                        propertyTypeName == typeof(ReinforcementUsing).Name)
                    {
                        SetMaterialRow(row.Table.DataSet, propertyInfo, obj);
                    }
                    else if (propertyTypeName == typeof(ICircle).Name)
                    {
                        ICircle circle = propertyInfo.GetValue(obj) as ICircle;
                        SetField(row, name + "CenterX", circle.Center.X);
                        SetField(row, name + "CenterY", circle.Center.Y);
                        SetField(row, name + "Diameter", circle.Diameter);
                    }
                    else
                    {
                        object value = propertyInfo.GetValue(obj);
                        SetField(row, name, value);
                    }

                }
            }
        }
        private static void SetMaterialRow(DataSet dataSet, PropertyInfo propertyInfo, IHasId obj)
        {
            //Наименование типа свойства
            string propertyTypeName = propertyInfo.PropertyType.Name;

            MaterialUsing entity;
            if (propertyTypeName == typeof(ConcreteUsing).Name)
            {
                entity = propertyInfo.GetValue(obj) as ConcreteUsing;
            }
            else if (propertyTypeName == typeof(ReinforcementUsing).Name)
            {
                entity = propertyInfo.GetValue(obj) as ReinforcementUsing;
            }
            else
            {
                throw new Exception(CommonMessages.TypeIsUknown);
            }
            entity.SaveToDataSet(dataSet, true);
        }
        /// <summary>
        /// Устанавливает свойства объекта по строке датасета
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        public static void GetFieldsFromRow(DataRow row, IDsSaveable obj)
        {
            //Коллекция наименования столбцов, которые добавлять не нужно
            List<string> exceptList = new List<string> { "Id", "ParentMember", "Name", "Children", "ForcesGroups", "LoadCases", "IsActive" };
            //Коллекция типов, которые нужно добавлять в таблицу
            List<string> typeSimpleList = new List<string> { "Double", "String", "Int32", "Boolean" };
            //Коллекция сложных типов
            List<string> typeComplexList = new List<string> { "Point2D" };
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                DataTable dataTable = row.Table;
                //Наименование свойства
                string propertyName = propertyInfo.Name;
                //Тип свойства
                Type propertyType = propertyInfo.PropertyType;
                //Наименование типа свойства
                string propertyTypeName = propertyType.Name;
                if (!(exceptList.Contains(propertyName)) //Если наименование таблицы не входит в перечень исключенных
                    & (typeSimpleList.Contains(propertyTypeName) || typeComplexList.Contains(propertyTypeName))) //И входит в перечень допустимых типов значения
                {
                    if (typeSimpleList.Contains(propertyTypeName) & !dataTable.Columns.Contains(propertyName))
                    {
                        throw new Exception($"Table {dataTable.TableName} not contain required column {propertyName}");
                    }
                    ProcessObjectFromProperty(obj, row, propertyInfo);
                }
            }
        }

        private static void ProcessObjectFromProperty(IHasId obj, DataRow row, PropertyInfo propertyInfo)
        {
            //Наименование свойства
            string propertyName = propertyInfo.Name;
            //Тип свойства
            Type propertyType = propertyInfo.PropertyType;
            //Наименование типа свойства
            string propertyTypeName = propertyType.Name;
            //Если тип является точкой
            if (propertyTypeName == typeof(Point2D).Name)
            {
                Point2D point = GetPointFromProperty(row, propertyInfo);
                propertyInfo.SetValue(obj, point);
            }
            else if (propertyTypeName == typeof(ICircle).Name)
            {
                ICircle circle = GetCircleFromProperty(row, propertyInfo);
                propertyInfo.SetValue(obj, circle);
            }
            else if (propertyTypeName == typeof(ConcreteUsing).Name)
            {
                //ConcreteUsing concrete = new ConcreteUsing(false);
                //propertyInfo.SetValue(obj, concrete);
            }
            else if (propertyTypeName == typeof(ReinforcementUsing).Name)
            {
                //ReinforcementUsing reinforcement = new ReinforcementUsing(false);
                //propertyInfo.SetValue(obj, reinforcement);
            }
            //Если тип является строкой
            else if (propertyTypeName == typeof(string).Name)
            {
                string value = row.Field<string>(propertyName);
                propertyInfo.SetValue(obj, value);
            }
            else //Если тип является типом значения
            {
                ValueType value;
                if (propertyTypeName == typeof(double).Name)
                {
                    value = row.Field<double>(propertyName);
                }
                else if (propertyTypeName == typeof(int).Name)
                {
                    value = row.Field<int>(propertyName);
                }
                else if (propertyTypeName == typeof(bool).Name)
                {
                    value = row.Field<bool>(propertyName);
                }
                else
                {
                    throw new Exception(CommonMessages.TypeIsUknown);
                }
                propertyInfo.SetValue(obj, value);
            }
        }

        private static ICircle GetCircleFromProperty(DataRow row, PropertyInfo propertyInfo)
        {
            string name = propertyInfo.Name;
            ICircle circle = new CircleSection();
            double x = row.Field<double>($"{name}CenterX");
            double y = row.Field<double>($"{name}CenterY");
            circle.Center = new Point2D(x, y);
            circle.Diameter = row.Field<double>("CircleDiameter");
            return circle;
        }

        private static Point2D GetPointFromProperty(DataRow row, PropertyInfo propertyInfo)
        {
            //Наименование свойства
            string propertyName = propertyInfo.Name;
            double x = row.Field<double>(propertyName + "X");
            double y = row.Field<double>(propertyName + "Y");
            Point2D point = new Point2D(x, y);
            return point;
        }

        /// <summary>
        /// Возвращает наименование таблицы, в которую следует сохранять указанный тип объекта
        /// </summary>
        /// <param name="type">тип объекта</param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {
            IEnumerable<TableMapping> tableMappings = TableMappingFactory.GetTableMappings();
            string tableName = type.Name + "s";
            foreach (TableMapping mapping in tableMappings)
            {
                if (mapping.EntityName == type.Name)
                {
                    tableName = mapping.TableName;
                    break;
                }
            }
            return tableName;
        }
        /// <summary>
        /// Возвращает наименование таблицы, в которую следует сохранять объект
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetTableName(IHasId obj)
        {
            return GetTableName(obj.GetType());
        }
        /// <summary>
        /// Заполняет свойства объекта из датасета
        /// </summary>
        /// <param name="dataSet">Датасет</param>
        /// <param name="entity">Ссылка на объект</param>
        public static void OpenEntityFromDataSet(DataSet dataSet, IDsSaveable entity)
        {
            string tablename = GetTableName(entity);
            DataRow row = OpenFromDataSetById(dataSet, tablename, entity.Id);
            EntityOperation.SetProps(row, entity);
            GetFieldsFromRow(row, entity);
        }
        public static void OpenEntityFromRow(DataRow row, IHasId obj)
        {
            //Коллекция наименования столбцов, которые обрабатывать не нужно
            IEnumerable<string> exceptList = GetExceptColumnList();
            //Коллекция типов, которые нужно обрабатывать
            IEnumerable<string> typeSimpleList = GetStringsFromTypes(GetColumnSimpleTypes());
            IEnumerable<string> typeCompexList = GetStringsFromTypes(GetColumnComplexTypes());
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                //Наименование свойства
                string name = propertyInfo.Name;
                //Наименование типа свойства
                string propertyTypeName = propertyInfo.PropertyType.Name;
                if (!(exceptList.Contains(name)))
                {
                    if (typeSimpleList.Contains(propertyTypeName) || typeCompexList.Contains(propertyTypeName))
                    {
                        string columnName = GetMappingColumnName(name);
                        if ((typeSimpleList.Contains(propertyTypeName)) & (! row.Table.Columns.Contains(columnName)))
                        {
                            throw new Exception(CommonMessages.TableNotContainColumn);
                        }
                        ProcessObjectFromProperty(obj, row, propertyInfo);
                    }
                }
            }
        }

        private static string GetMappingColumnName(string name)
        {
            IEnumerable<ColumnMapping> columnMappings = ColumnMappingFactory.GetColumnMappings(); 
            string columnName = name;
            foreach (ColumnMapping mapping in columnMappings)
            {
                if (mapping.PropertyName == name)
                {
                    columnName = mapping.ColumnName;
                }
            }
            return columnName;
        }
        private static IEnumerable<Type> GetColumnSimpleTypes()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(double));
            types.Add(typeof(string));
            types.Add(typeof(int));
            types.Add(typeof(bool));
            return types;
        }
        private static IEnumerable<Type> GetColumnComplexTypes()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(Point2D));
            types.Add(typeof(ICircle));
            types.Add(typeof(IRectangle));
            types.Add(typeof(IShape));
            types.Add(typeof(ConcreteUsing));
            types.Add(typeof(ReinforcementUsing));
            types.Add(typeof(SteelUsing));
            return types;
        }
        private static IEnumerable<Type> GetColumnTypesToAddToTable()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(double));
            types.Add(typeof(string));
            types.Add(typeof(int));
            types.Add(typeof(bool));
            types.Add(typeof(Point2D));
            types.Add(typeof(ICircle));
            types.Add(typeof(IRectangle));
            types.Add(typeof(IShape));
            return types;
        }
        private static IEnumerable<string> GetStringsFromTypes(IEnumerable<Type> types)
        {
            List<string> strings = new List<string>();
            foreach(Type type in types)
            {
                strings.Add(type.Name);
            }
            return strings;
        }
        private static IEnumerable<string> GetExceptColumnList()
        {
            List<string> exceptList = new List<string>
            { "Id", "ParentMember", "Children", "ForcesGroups", "LoadCases", "IsActive"
            };
            return exceptList;
        }
        /// <summary>
        /// Возвращает коллекцию шаблонов столцов, которые необходимо создать по информации о свойстве типа
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private static List<ColumnTemplate> GetTypes(PropertyInfo propertyInfo)
        {
            string name = propertyInfo.Name;
            Type propertyType = propertyInfo.PropertyType;
            string propertyTypeName = propertyType.Name;

            List<ColumnTemplate> columns = new List<ColumnTemplate>();
            if (propertyTypeName == "Double" || propertyTypeName == "Int32")
            {
                columns.Add(new ColumnTemplate() { ColumnName = name, ColumnType = propertyType, DefaultValue = 0 });
            }
            else if (propertyTypeName == "String")
            {
                columns.Add(new ColumnTemplate() { ColumnName = name, ColumnType = propertyType, DefaultValue = "" });
            }
            else if (propertyTypeName == "Boolean")
            {
                columns.Add(new ColumnTemplate() { ColumnName = name, ColumnType = propertyType, DefaultValue = true });
            }
            else if (propertyTypeName == typeof(Point2D).Name)
            {
                columns.Add(new ColumnTemplate() { ColumnName = name + "X", ColumnType = typeof(double), DefaultValue = 0 });
                columns.Add(new ColumnTemplate() { ColumnName = name + "Y", ColumnType = typeof(double), DefaultValue = 0 });
            }
            else if (propertyTypeName == typeof(ICircle).Name)
            {
                columns.Add(new ColumnTemplate() { ColumnName = "CircleCenterX", ColumnType = typeof(double), DefaultValue = 0 });
                columns.Add(new ColumnTemplate() { ColumnName = "CircleCenterY", ColumnType = typeof(double), DefaultValue = 0 });
                columns.Add(new ColumnTemplate() { ColumnName = "CircleDiameter", ColumnType = typeof(double), DefaultValue = 0 });
            }
            else
            {
                throw new Exception("Type is unknown");
            }
            return columns;
        }
        /// <summary>
        /// Создает столцы в таблице датасета по коллекции шаблонов
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columns"></param>
        private static void AddColumnFromTypes (DataTable dataTable, List<ColumnTemplate> columns)
        {
            //Для каждого элемента коллекции создаем новый столбец в таблице
            foreach (ColumnTemplate column in columns)
            {
                if (! dataTable.Columns.Contains(column.ColumnName))
                {
                    DataColumn NewColumn;
                    NewColumn = new DataColumn(column.ColumnName, column.ColumnType);
                    NewColumn.AllowDBNull = false;
                    NewColumn.DefaultValue = column.DefaultValue;
                    dataTable.Columns.Add(NewColumn);
                }
            }
        }
        private static DataColumn AddNameColumn(DataTable dataTable)
        {
            DataColumn NameColumn;
            NameColumn = new DataColumn("Name", Type.GetType("System.String"));
            NameColumn.ColumnMapping = MappingType.Attribute;
            dataTable.Columns.Add(NameColumn);
            return NameColumn;
        }
    }
}
