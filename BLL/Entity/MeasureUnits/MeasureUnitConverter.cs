using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Entity.MeasureUnits
{
    /// <summary>
    /// 
    /// </summary>
    public static class MeasureUnitConverter
    {
        /*
         * 0 - линейные единицы
         * 1 - усилия
         * 2 - изгибающие моменты
         * 3 - напряжения
         * 4 - геометрия, площадь
         * 5 - геометрия, момент сопротивления
         * 6 - геометрия, момент инерции
         * 7 - масса
         * 8 - плотность
         * 9 - объемный вес
         * 10 - размеры, площадь
         * 11 - размеры, объем
         * 12 - распределенная нагрузка на погонный метр
         * 13 - распределенная нагрузка на квадратный метр
         * 14 - коэффициент фильтрации
         */
        public static double GetCoefficient(int index)
        {
            return GetUnitLabel(index).AddKoeff;
        }

        public static string GetUnitLabelText(int index)
        {
            return GetUnitLabel(index).UnitName;
        }

        public static MeasureUnitLabel GetUnitLabel(int index)
        {
            var measureUnitLabels = from t in ProgrammSettings.MeasureUnits[index].UnitLabels where t.Id == ProgrammSettings.MeasureUnits[index].CurrentUnitLabelId select t;
            return measureUnitLabels.First();
        }
    }
}
