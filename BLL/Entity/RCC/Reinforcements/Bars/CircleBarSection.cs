using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Interfaces.Placements;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Reinforcements.Ancorages;
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
        public string Name { get; set; }
        /// <summary>
        /// Ссылка на родительский элемент
        /// </summary>
        public IHasId ParentMember { get; private set; }
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
        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="circle">Круглое сечение</param>
        /// <param name="genId">Флаг необходимости генерации кода</param>
        public CircleBarSection (ICircle circle, bool genId = false)
        {
            if (genId)
            {
                Id = ProgrammSettings.CurrentId;
            }
            Circle = circle;
        }

        public void RegisterParent(IHasId parent)
        {
            if (ParentMember != null)
            {
                UnRegisterParent();
            }
            IAncorage ancorage = parent as IAncorage;
            ancorage.BarSections.Add(this);
            ParentMember = ancorage;
        }

        public void UnRegisterParent()
        {
            if (ParentMember != null)
            {
                IAncorage ancorage = ParentMember as IAncorage;
                ancorage.BarSections.Remove(this);
            }
            ParentMember = null;
        }
        #endregion
    }
}
