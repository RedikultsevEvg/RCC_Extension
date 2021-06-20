using RDBLL.Common.ErrorProcessing.Messages;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Sections;
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Reinforcements.Bars.Storages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Reinforcements.Bars.Factories
{
    public enum BarType
    {
        BarWithoutPlace
    }
    /// <summary>
    /// Фабрика арматурных стержней
    /// </summary>
    public class BarSectionFactory
    {
        public static IBarSection GetBarSection (BarType type)
        {
            if (type == BarType.BarWithoutPlace)
            {
                return BarSectionWithoutPlacement();
            }
            else
            {
                throw new Exception(CommonMessages.TypeIsUknown);
            }
        }

        private static IBarSection BarSectionWithoutPlacement()
        {
            double ds = 0.012;
            ICircle circle = new CircleSection() { Center = new Point2D(), Diameter = ds };
            IBarSection barSection = new CircleBarSection(circle, true);
            ReinforcementUsing reinforcement = new ReinforcementUsing(true);
            reinforcement.SelectedId = ProgrammSettings.ReinforcementKinds[0].Id;
            barSection.Reinforcement = reinforcement;
            reinforcement.RegisterParent(barSection);
            return barSection;
        }
    }
}
