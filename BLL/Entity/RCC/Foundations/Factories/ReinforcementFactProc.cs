using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Common.Materials.RFExtenders;
using RDBLL.Entity.Common.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Factories
{
    public static class ReinforcementFactProc
    {
        public static void GetReinforcement(Foundation foundation)
        {
            #region Армирование подошвы
            MaterialContainer materialContainer = new MaterialContainer(foundation);
            materialContainer.Name = "Армирование подошвы";
            materialContainer.Purpose = "BtmRF";
            ReinforcementUsing rfX = GetBottomReinforcement(materialContainer, 0.07, "Вдоль оси X", "Along X-axes");
            ReinforcementUsing rfY = GetBottomReinforcement(materialContainer, 0.07, "Вдоль оси Y", "Along Y-axes");
            materialContainer.MaterialUsings.Add(rfX);
            materialContainer.MaterialUsings.Add(rfY);
            foundation.BottomReinforcement = materialContainer;
            #endregion 
            #region Армирование подколонника
            MaterialContainer verticalContainer = new MaterialContainer(foundation);
            verticalContainer.Name = "Вертикальное армирование";
            verticalContainer.Purpose = "UndColumn";
            foundation.VerticalReinforcement = verticalContainer;
            ReinforcementUsing rfVert = GetUndColumnRF(verticalContainer, 0.07, "Подколонник", "UndColumn");
            verticalContainer.MaterialUsings.Add(rfVert);
            foundation.VerticalReinforcement = verticalContainer;
            #endregion
        }

        private static ReinforcementUsing GetRF(MaterialContainer container, string rusName, string engName)
        {
            ReinforcementUsing rf = new ReinforcementUsing(container);
            rf.Name = rusName;
            rf.Purpose = engName;
            rf.Diameter = 0.012;
            rf.SelectedId = ProgrammSettings.ReinforcementKinds[0].Id;
            return rf;
        }
        private static ReinforcementUsing GetBottomReinforcement(MaterialContainer container, double coveringLayer, string rusName, string engName)
        {
            ReinforcementUsing rf = GetRF(container, rusName, engName);
            LineBySpacing placement = new LineBySpacing(true);
            placement.RegisterParent(rf);
            LineToSurfBySpacing extender = ExtenderFactory.GetCoveredArray(ExtenderType.CoveredLine) as LineToSurfBySpacing;
            rf.SetExtender(extender);
            rf.SetPlacement(placement);
            extender.CoveringLayer = coveringLayer;
            return rf;
        }
        private static ReinforcementUsing GetUndColumnRF(MaterialContainer container, double coveringLayer, string rusName, string engName)
        {
            ReinforcementUsing rf = GetRF(container, rusName, engName);
            RectArrayPlacement placement = new RectArrayPlacement(true);
            placement.OffSet = 0.05;
            placement.RegisterParent(rf);
            rf.SetExtender(ExtenderFactory.GetCoveredArray(ExtenderType.CoveredArray));
            rf.SetPlacement(placement);
            return rf;
        }
    }
}
