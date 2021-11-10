using RDBLL.Common.ErrorProcessing.Messages;
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Reinforcements.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Ancorages
{
    /// <summary>
    /// Класс расчета длин анкеровки
    /// </summary>
    public class Ancorage : IAncorage
    {
        /// <summary>
        /// Код элемента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование элемента
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Обратная ссылка на родительский элемент
        /// </summary>
        public IHasId ParentMember {get; private set;}
        /// <summary>
        /// Использование бетона
        /// </summary>
        public ConcreteUsing Concrete { get; set; }
        /// <summary>
        /// Коллекция сечений арматурных стержней
        /// </summary>
        public List<IBarSection> BarSections { get; set; }
        /// <summary>
        /// Отношение длительной нагрузки к полной
        /// </summary>
        public double LongLoadRate { get; set; }
        /// <summary>
        /// Логика расчета длин анкеровки
        /// </summary>
        public IAncorageLogic AncorageLogic { get; private set; }
        #region Constructors
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logic">Логика расчета длин анкеровки</param>
        /// <param name="genId">Флаг необходимости генерации Id</param>
        public Ancorage(IAncorageLogic logic, bool genId = false)
        {
            if (genId)
            {
                Id = ProgrammSettings.CurrentId;
            }
            AncorageLogic = logic;
            BarSections = new List<IBarSection>();
        }
        #endregion
        /// <summary>
        /// Добавляет обратную ссылку на родителя
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IHasId parent)
        {
            if (parent is Level)
            {
                Level level = parent as Level;
                level.Children.Add(this);
                ParentMember = parent;
            }
            else
            {
                throw new Exception(CommonMessages.TypeIsUknown);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void UnRegisterParent()
        {
            if (ParentMember != null)
            {
                if (ParentMember is Level)
                {
                    Level level = ParentMember as Level;
                    level.Children.Remove(this);
                    ParentMember = null;
                }
                else
                {
                    throw new Exception(CommonMessages.TypeIsUknown);
                }
            }
        }
    }
}
