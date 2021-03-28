using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Entity.MeasureUnits;

namespace RDBLL.Common.Service
{
    /// <summary>
    /// Класс операций для сравнения величин
    /// </summary>
    public class Compare
    {
        /// <summary>
        /// Наименование проверки
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Наименование величины 1
        /// </summary>
        public string Val1Name { get; set; }
        /// <summary>
        /// Наименование величины 2
        /// </summary>
        public string Val2Name { get; set; }
        /// <summary>
        /// Наименование единицы измерения
        /// </summary>
        public string ValUnitName { get; set; }
        /// <summary>
        /// Величина 1
        /// </summary>
        public double Val1 { get; set; }
        /// <summary>
        /// Величина 2
        /// </summary>
        public double Val2 { get; set; }
        /// <summary>
        /// Дополнительный коэффициент
        /// </summary>
        public double Cofficient { get; set; }
        /// <summary>
        /// Правильный результат проверки величина 1 меньше величины 2
        /// </summary>
        public bool NeedLess{ get; set; }
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Compare()
        {
            NeedLess = true;
            Cofficient = 1;
        }
        /// <summary>
        /// Конструктор по индексу единицы измерения
        /// </summary>
        /// <param name="i">Индекс единицы измерения</param>
        public Compare(int i)
        {
            NeedLess = true;
            ValUnitName = MeasureUnitConverter.GetUnitLabelText(i);
            Cofficient = MeasureUnitConverter.GetCoefficient(i);
        }
        /// <summary>
        /// Результат сравнения в виде булева значения
        /// </summary>
        /// <returns></returns>
        public bool BoolResult()
        {
            if (NeedLess & (Val1 <= Val2)) return true;
            if (!NeedLess & (Val1 >= Val2)) return true;
            return false;
        }
        /// <summary>
        /// Результат сравнения в виде строки
        /// </summary>
        /// <returns></returns>
        public string CompareResult()
        {
            string s = $"{Name}:";
            Val1 = Val1 * Cofficient;
            Val2 = Val2 * Cofficient;
            s += $" {Val1Name} = {MathOperation.Round(Val1)} {ValUnitName}";
            if (Val1 < Val2) { s += $" <"; }
            if (Val1 == Val2) { s += $" ="; }
            if (Val1 > Val2) { s += $" >"; }
            s += $" {Val2Name} = {MathOperation.Round(Val2)} {ValUnitName}. ";
            if (BoolResult()) s += "Проверка выполнена"; else s += "Проверка не выполнена";
            return s;
        }
    }
}
