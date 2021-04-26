using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column.SteelBases.Factories
{
    public enum MatType
    {
        SteelBase,
        SteelBasePartGroup
    }
    public static class MatFactProc
    {
        public static void GetMatType(IDsSaveable obj, MatType type)
        {
            switch (type)
            {
                case MatType.SteelBase:
                    {
                        IHasSteel steelObject = obj as IHasSteel;
                        SteelUsing steel = GetSteelUsing(obj);
                        steel.Purpose = "BaseSteel";
                        steelObject.Steel = steel;
                        IHasConcrete concreteObject = obj as IHasConcrete;
                        ConcreteUsing concrete = new ConcreteUsing(obj);
                        concrete.Name = "Бетон";
                        concrete.Purpose = "Filling";
                        concrete.SelectedId = ProgrammSettings.ConcreteKinds[0].Id;
                        concrete.AddGammaB1();
                        concreteObject.Concrete = concrete;
                        return;
                    }
                case MatType.SteelBasePartGroup:
                    {
                        IHasSteel steelObject = obj as IHasSteel;
                        SteelUsing steel = GetSteelUsing(obj);
                        steel.Purpose = "BasePartGroupSteel";
                        steelObject.Steel = steel;
                        return;
                    }
            }
        }
        private static SteelUsing GetSteelUsing(IDsSaveable obj)
        {
            SteelUsing steel = new SteelUsing(obj);
            steel.Name = "Сталь";
            steel.SelectedId = ProgrammSettings.SteelKinds[0].Id;

            SafetyFactor safetyFactor = new SafetyFactor(true);
            safetyFactor.Name = "Коэффициент, учитывающий толщину элемента";
            safetyFactor.PsfFst = 0.9;
            safetyFactor.PsfFstTens = 0.9;
            safetyFactor.PsfFstLong = 0.9;
            safetyFactor.PsfFstLongTens = 0.9;
            safetyFactor.RegisterParent(steel);
            steel.SafetyFactors.Add(safetyFactor);

            safetyFactor = new SafetyFactor(true);
            safetyFactor.Name = "Коэффициент условий работы стальной базы";
            safetyFactor.PsfFst = 1.15;
            safetyFactor.PsfFstTens = 1.15;
            safetyFactor.PsfFstLong = 1.15;
            safetyFactor.PsfFstLongTens = 1.15;
            safetyFactor.RegisterParent(steel);
            steel.SafetyFactors.Add(safetyFactor);

            return steel;
        }
    }
}
