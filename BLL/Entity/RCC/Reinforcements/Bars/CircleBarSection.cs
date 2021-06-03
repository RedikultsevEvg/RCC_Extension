using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Placements;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Bars
{
    /// <summary>
    /// Класс сечения арматурного стержня
    /// </summary>
    public class CircleBarSection : IBarSection
    {
        #region Properties
        /// <summary>
        /// Код сечения
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name {get; set; }
        /// <summary>
        /// Ссылка на родительский элемент
        /// </summary>
        public IDsSaveable ParentMember { get; }
        /// <summary>
        /// Предварительная деформация
        /// </summary>
        public double Prestrain { get; set; }
        /// <summary>
        /// Расположение элементов
        /// </summary>
        public IParentPlacement ParentPlacement { get; set; }
        /// <summary>
        /// Ссылка на класс арматуры
        /// </summary>
        public ReinforcementUsing Reinforcement { get; set; }
        /// <summary>
        /// Форма стержня
        /// </summary>
        public ICircle Circle { get; set; }
        #endregion
        public void DeleteFromDataSet(DataSet dataSet)
        {
            
        }

        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void RegisterParent(IDsSaveable parent)
        {
            throw new NotImplementedException();
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }

        public void UnRegisterParent()
        {
            throw new NotImplementedException();
        }
    }
}
