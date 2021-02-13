using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials.Interfaces;
using DAL.Common;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Базовый класс массива расположения армирования
    /// </summary>
    public abstract class RFSpacingBase : ISavableToDataSet, IDuplicate
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Коллекция параметров армирования
        /// </summary>
        public List<RFSpacingParameter> RFSpacingParameters { get; }
        /// <summary>
        /// Ссылка на родителя
        /// </summary>
        public ReinforcementUsing ParentMember { get; set; }
        private static string TableName { get { return "RFSpacings"; } }
        #region ISavableToDataSet
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataTable dataTable;
            dataTable = dataSet.Tables[TableName];
            DataRow row = DsOperation.CreateNewRow(Id, createNew, dataTable);
            #region setFields
            row.SetField("Id", Id);
            row.SetField("Name", Name);
            row.SetField("ParentId", ParentMember.Id);
            if (this is RFSmearedBySpacing) {row.SetField("Type", "SmSpacing");}
            else if (this is RFSmearedByQuantity) { row.SetField("Type", "SmQuantity"); }
            else { throw new Exception("Type of Spacing is unknown"); }
            row.SetField("ValuesString", GetStringFromParameters());
            #endregion
            dataTable.AcceptChanges();
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, TableName, Id));
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            string s = dataRow.Field<string>("ValuesString");
            SetParametersFromString(s);
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            DsOperation.DeleteRow(dataSet, TableName, Id);
        }
        #endregion
        #region Constructors
        public RFSpacingBase()
        {
            RFSpacingParameters = new List<RFSpacingParameter>();
        }
        public RFSpacingBase(ReinforcementUsing parentMember)
        {
            Id = ProgrammSettings.CurrentId;
            ParentMember = parentMember;
            parentMember.RFSpacing = this;
            RFSpacingParameters = new List<RFSpacingParameter>();
        }
        #endregion
        #region Methods
        private string GetStringFromParameters()
        {
            string s="";
            foreach (RFSpacingParameter parameter in RFSpacingParameters)
            {
                s += parameter.ParameterValue+"~/~";
            }
            return s;
        }
        public void SetParametersFromString(string s)
        {
            string[] parameters = s.Split(new string[] { "~/~" }, StringSplitOptions.None);
            int lCount = RFSpacingParameters.Count;
            int pCount = parameters.Count()-1;
            //Сравниваем количество полученных параметров со количеством в классе
            if (lCount == pCount)
            {
                for (int i = 0; i<pCount; i++ )
                {
                    //Добавляем значения параметров
                    RFSpacingParameters[i].ParameterValue = parameters[i];
                }
            }
            else throw new Exception("String of parameters is not agree with set of parameters");
        }

        public object Duplicate()
        {
            RFSpacingBase rfSpacing;
            if (this is RFSmearedBySpacing)
            {
                RFSmearedBySpacing rfSmeared = new RFSmearedBySpacing();
                rfSpacing = rfSmeared;
            }
            else if (this is RFSmearedByQuantity)
            {
                RFSmearedByQuantity rfSmeared = new RFSmearedByQuantity();
                rfSpacing = rfSmeared;
            }
            else throw new Exception("Type of Spacing is unknown");
            rfSpacing.Id = ProgrammSettings.CurrentId;
            rfSpacing.Name = Name;
            foreach (RFSpacingParameter Parameter in this.RFSpacingParameters)
            {
                rfSpacing.RFSpacingParameters.Add(Parameter.Duplicate() as RFSpacingParameter);
            }
            return rfSpacing;
        }
        #endregion
    }
}
