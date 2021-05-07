using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDStartWPF.Views.Common.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.InfraStructure.Common.CommonOperations
{
    internal static class CommonWindowOperation
    {
        public static void ShowForceWindow(IHasForcesGroups hasForces)
        {
            wndForces wndForces = new wndForces(hasForces.ForcesGroups[0]);
            wndForces.ShowDialog();
            if (wndForces.DialogResult == true)
            {
                try
                {
                    hasForces.ForcesGroups[0].DeleteFromDataSet(ProgrammSettings.CurrentDataSet);
                    hasForces.ForcesGroups[0].SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                    ProgrammSettings.IsDataChanged = true;
                }
                catch (Exception ex)
                {
                    CommonErrorProcessor.ShowErrorMessage($"Ошибка сохранения в элементе: {hasForces.Name}", $"Тип элемента: {hasForces.GetType().Name}, \n Код элемента: Id={hasForces.Id}, \n Имя элемента: {hasForces.Name}", ex);
                }
            }
            else
            {
                hasForces.ForcesGroups = GetEntity.GetParentForcesGroups(ProgrammSettings.CurrentDataSet, hasForces);
            }
        }
    }
}
