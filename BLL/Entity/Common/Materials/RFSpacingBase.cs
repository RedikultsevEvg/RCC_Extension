using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Interfaces;
using System.Data;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials.Interfaces;

namespace RDBLL.Entity.Common.Materials
{
    /// <summary>
    /// Базовый класс массива расположения армирования
    /// </summary>
    public abstract class RFSpacingBase : ISavableToDataSet
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
        private ReinforcementUsing ParentMember {get;}
        #region ISavableToDataSet
        /// <summary>
        /// Сохранение в датасет
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="createNew"></param>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
        }
        /// <summary>
        /// Открыть из датасета
        /// </summary>
        /// <param name="dataRow"></param>
        public void OpenFromDataSet(DataRow dataRow)
        {
        }
        /// <summary>
        /// Удалить из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
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
    }
}
