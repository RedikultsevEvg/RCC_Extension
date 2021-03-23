using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Entity.SC.Column.SteelBases.Factorys
{
    /// <summary>
    /// Процессор фабрики участков для наиболее характерных типов баз колонн
    /// </summary>
    public static class PartFactProc
    {
        public static void GetPartsType1(SteelBase steelBase, double baseWidth, double baseLength, double baseInternalLength)
        {
            double widthOuterPart = baseWidth / 2;
            double lengthOuterPart = (baseLength - baseInternalLength) / 2;
            double addY = baseInternalLength / 2;
            steelBase.SteelBaseParts = new ObservableCollection<SteelBasePart>();
            SteelBasePart part;
            //Участок левый верхний
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = "1";
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.CenterX = -widthOuterPart / 2;
            part.CenterY = (addY + lengthOuterPart / 2);
            part.FixRight = true;
            part.FixBottom = true;
            part.RightOffset = 0.008;
            part.BottomOffset = 0.008;
            //Участок правый верхний
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = "2";
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.CenterX = widthOuterPart / 2;
            part.CenterY = (addY + lengthOuterPart / 2);
            part.FixLeft = true;
            part.FixBottom = true;
            part.LeftOffset = 0.008;
            part.BottomOffset = 0.008;
            //Участок левый нижний
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = "3";
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.CenterX = -widthOuterPart / 2;
            part.CenterY = -(addY + lengthOuterPart / 2);
            part.FixRight = true;
            part.FixTop = true;
            part.RightOffset = 0.008;
            part.TopOffset = 0.008;
            //Участок правый нижний
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = "4";
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.CenterX = widthOuterPart / 2;
            part.CenterY = -(addY + lengthOuterPart / 2);
            part.FixLeft = true;
            part.FixTop = true;
            part.LeftOffset = 0.008;
            part.TopOffset = 0.008;

            //Участок левый средний
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = "5";
            part.Width = widthOuterPart;
            part.Length = baseInternalLength;
            part.CenterX = -widthOuterPart / 2;
            part.CenterY = 0;
            part.FixRight = true;
            part.FixTop = true;
            part.FixBottom = true;
            part.RightOffset = 0.008;
            part.TopOffset = 0.008;
            part.BottomOffset = 0.008;
            //Участок правый средний
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = "6";
            part.Width = widthOuterPart;
            part.Length = baseInternalLength;
            part.CenterX = widthOuterPart / 2;
            part.CenterY = 0;
            part.FixLeft = true;
            part.FixTop = true;
            part.FixBottom = true;
            part.LeftOffset = 0.008;
            part.TopOffset = 0.008;
            part.BottomOffset = 0.008;
        }
    }
}
