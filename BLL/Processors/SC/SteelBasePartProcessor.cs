using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Results.SC;
using RDBLL.Entity.SC.Column;
using RDBLL.Common.Geometry;
using System.Windows.Forms;
using RDBLL.Forces;
using RDBLL.Processors.Forces;
using RDBLL.Entity.Results.Forces;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.Common.NDM.Processors;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM.Interfaces;
using RDBLL.Entity.Common.NDM.MaterialModels;
using RDBLL.Entity.MeasureUnits;

namespace RDBLL.Processors.SC
{
    delegate void EchoDelegate(String S);

    /// <summary>
    /// Processor of parts of steelbase
    /// </summary>
    public static class SteelBasePartProcessor
    {
        /// <summary>
        /// Возвращает момент в пластине по моменту и толщине
        /// </summary>
        /// <param name="moment"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static double GetPlateStress(double moment, double thickness, List<string> reportList = null)
        {
            double Wx = thickness * thickness / 6;
            string unitWx = thickness * MeasureUnitConverter.GetCoefficient(0) + MeasureUnitConverter.GetUnitLabelText(0);
            unitWx += "^2/6=" + Wx * MeasureUnitConverter.GetCoefficient(5) + MeasureUnitConverter.GetUnitLabelText(5);
            if (reportList != null) { reportList.Add($"Момент сопротивления W=1м*" + unitWx); }
            double stress = moment / Wx;
            string unitStress = stress * MeasureUnitConverter.GetCoefficient(3) + MeasureUnitConverter.GetUnitLabelText(3);
            if (reportList != null) { reportList.Add($"Напряжение в плите Sigma=" + unitStress); }
            return stress;
        }
        /// <summary>
        /// Возвращает момент и напряжения для участка базы стальной колонны
        /// </summary>
        /// <param name="basePart">Участок базы стальной колонны</param>
        /// <param name="maxStress">Максимальное давление на участок</param>
        /// <returns>Массив: 0-максимальный момент, 1 - максимальные напряжения </returns>
        public static double GetMoment(SteelBasePart basePart, double maxStress, List<string> reportList = null)
        {
            /*Алгоритм расчета основан на подходе из учебника Белени по
             * таблицам Галеркина
             * Момент определяется в зависимости от количества сторон, по которым имеется опора
             * Если опора имеется только по одной стороне, то считается что участок консольный
             * Иначе считаетася, что участок оперт шарнирно
             */
            //double maxStress = GetMinStressLinear(basePart);
            double result = 0;
            double maxMoment = 0;
            int countFixSides = 0;

            if (reportList != null)
            {
                string unitStress = maxStress * MeasureUnitConverter.GetCoefficient(3) + MeasureUnitConverter.GetUnitLabelText(3);
                reportList.Add($"Максимальное давление на участок Sigma=" + unitStress);
            }
            #region Определение количества сторон, по которым имеются опоры
            if (basePart.FixLeft) { countFixSides++; }
            if (basePart.FixRight) { countFixSides++; }
            if (basePart.FixTop) { countFixSides++; }
            if (basePart.FixBottom) { countFixSides++; }
            #endregion
            //Участок отрывается, напряжения равны нулю
            if (maxStress < 0)
            {
                return maxMoment;
            }
            switch (countFixSides)
            {
                case 0://Если ни одна из опор не задана, то это ошибка
                    MessageBox.Show("Неверное закрепление сторон", "Ошибка");
                    if (reportList != null) { reportList.Add("Ошибка! Неверное закрепление сторон участка");}
                    return result;
                case 1://Если опора только по одной стороне, считаем как консоль
                    if (reportList != null) { reportList.Add("Закрепление участка предусмотрено по одной стороне"); }
                    if (reportList != null) { reportList.Add("Расчет производится как для консольного участка"); }
                    maxMoment = CalcStreessOneSide(maxStress, basePart, reportList);
                    break;
                case 2://Если опора с двух сторон
                    maxMoment = CalcStreessTwoSide(maxStress, basePart, reportList);
                    break;
                case 3://Если опора с трех сторон 
                    maxMoment = CalcStreessThreeSide(maxStress, basePart, reportList);
                    break;
                case 4://Если опора с 4-х сторон
                    maxMoment = CalcStreessFourSide(maxStress, basePart, reportList);
                    break;
            }
            return maxMoment;
        }
        /// <summary>
        /// Выводит сообщения на консоль
        /// </summary>
        /// <param name="S"></param>
        private static void EchoConsole (String S)
        {
            Console.WriteLine(S);
        }
        #region //Функции вычисления изгибающего момента в плите в зависимости от закрепления сторон
        /// <summary>
        /// Возвращает изгибающий момент в участке плите для участка, опертого по одной стороне
        /// </summary>
        /// <param name="maxStress">Максимальное напряжение по участку</param>
        /// <param name="basePart">Экземпляр участка</param>
        /// <param name="echoDelegate">Делегат, в который передается результат расчета</param>
        /// <returns></returns>
        private static double CalcStreessOneSide(double maxStress, SteelBasePart basePart, List<string> reportList = null)
        {
            string reportString;
            double maxMoment;
            double[] sizes = GetPartSizes(basePart);
            double width = sizes[0];
            double length = sizes[1];
            double actualLength;
            //Если опора слева или справа
            //echoDelegate("Для участка задана опора по одной стороне, участок расчитывается как консоль");
            if (basePart.FixLeft || basePart.FixRight)
            {
                actualLength = width;
            }
            //Если опора сверху или снизу
            else
            {
                actualLength = length;
            }
            maxMoment = maxStress * actualLength * actualLength / 2;
            string unitLength = actualLength * MeasureUnitConverter.GetCoefficient(0)+MeasureUnitConverter.GetUnitLabelText(0);
            reportString = "Расчетный вылет консоли L=" + unitLength;
            if (reportList != null) { reportList.Add(reportString); }
            string unitStress = maxStress * MeasureUnitConverter.GetCoefficient(3) + MeasureUnitConverter.GetUnitLabelText(3);
            string unitMoment = maxMoment * MeasureUnitConverter.GetCoefficient(2) + MeasureUnitConverter.GetUnitLabelText(2);
              reportString = $"Изгибающий момент M={unitStress}*{unitLength}^2/2={unitMoment}";
            if (reportList != null) { reportList.Add(reportString); }
            return maxMoment;
        }
        /// <summary>
        /// Вычисляет изгибающий момент в участке плите для участка, опертого по одной стороне
        /// </summary>
        /// <param name="maxStress">Максимальное напряжение по участку</param>
        /// <param name="basePart">Экземпляр участка</param>
        /// <param name="echoDelegate">Делегат, в который передается результат расчета</param>
        /// <returns></returns>
        private static double CalcStreessTwoSide(double maxStress, SteelBasePart basePart, List<string> reportList = null)
        {
            string reportString;
            string unitStress = maxStress * MeasureUnitConverter.GetCoefficient(3) + MeasureUnitConverter.GetUnitLabelText(3);
            #region Исходные списки для интерполяции коэффициентов
            //Для участков, опертых по 2-м и 3-м сторонам
            List<double> xValues23 = new List<double>() { 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.2, 1.4, 2 };
            List<double> yValues23 = new List<double>() { 0.06, 0.074, 0.088, 0.097, 0.107, 0.112, 0.120, 0.126, 0.132 };
            #endregion
            double maxMoment;
            double[] sizes = GetPartSizes(basePart);
            double width = sizes[0];
            double length = sizes[1];
            //Если опора слева и справа

            //Опирание по противоположным сторонам
            if ((basePart.FixLeft && basePart.FixRight) || (basePart.FixTop && basePart.FixBottom))
            {
                double actualLength = 0;
                if (basePart.FixLeft && basePart.FixRight)
                {
                    reportString = $"Опирание по двум противоположным сторонам: слева и справа";
                    if (reportList != null) { reportList.Add(reportString); }
                    //echoDelegate("Для участка заданы две опоры по противоположным сторонам, опоры считаются шарнирными");
                    actualLength = width;
                }
                else
                {
                    //Если опора снизу и сверху
                    if (basePart.FixTop && basePart.FixBottom)
                    {
                        reportString = $"Опирание по двум противоположным сторонам: снизу и сверху";
                        if (reportList != null) { reportList.Add(reportString); }
                        actualLength = length;
                    }

                }
                maxMoment = maxStress * actualLength * actualLength / 8;
                string unitLength = actualLength * MeasureUnitConverter.GetCoefficient(0) + MeasureUnitConverter.GetUnitLabelText(0);
                string unitMoment = maxMoment * MeasureUnitConverter.GetCoefficient(2) + MeasureUnitConverter.GetUnitLabelText(2);
                reportString = $"Изгибающий момент M={unitStress}*{unitLength}^2/8={unitMoment}";
                if (reportList != null) { reportList.Add(reportString); }
            }
            //Иначе - опирание по двум противоположным сторонам
            else
            {
                if (reportList != null) { reportList.Add("Опирание по двум смежным сторонам"); }
                //echoDelegate("Для участка заданы две опоры по смежным сторонам, опоры считаются шарнирными");
                double koeff_a1 = Math.Sqrt(width * width + length * length);
                if (reportList != null) { reportList.Add( $"Коэффициент Alpha1 = {koeff_a1}"); }
                double koeff_b1 = width * length * 0.25 / koeff_a1;
                if (reportList != null) { reportList.Add($"Коэффициент Betta1 = {koeff_b1}"); }
                double ratio = koeff_b1 / koeff_a1;
                double koeff_betta;

                if (ratio > 2)
                {
                    koeff_betta = 0.133;
                }
                else
                {

                    if (ratio < 0.5) { koeff_betta = 0.06; }
                    else { koeff_betta = MathOperation.InterpolateList(xValues23, yValues23, ratio); }
                }
                if (reportList != null) { reportList.Add($"Коэффициент Betta = {koeff_betta}"); }
                maxMoment = maxStress * koeff_a1 * koeff_a1 * koeff_betta;
                string unitMoment = maxMoment * MeasureUnitConverter.GetCoefficient(2) + MeasureUnitConverter.GetUnitLabelText(2);
                reportString = $"Изгибающий момент M={unitStress}*{koeff_a1}^2*{koeff_betta}={unitMoment}";
                if (reportList != null) { reportList.Add(reportString); }
            }
            return maxMoment;
        }
        /// <summary>
        /// Возвращает изгибающий момент в участке плите для участка, опертого по трем сторонам
        /// </summary>
        /// <param name="maxStress">Максимальное напряжение по участку</param>
        /// <param name="basePart">Экземпляр участка</param>
        /// <param name="echoDelegate">Делегат, в который передается результат расчета</param>
        /// <returns></returns>
        private static double CalcStreessThreeSide(double maxStress, SteelBasePart basePart, List<string> reportList = null)
        {
            string reportString;
            string unitStress = maxStress * MeasureUnitConverter.GetCoefficient(3) + MeasureUnitConverter.GetUnitLabelText(3);
            /*Для участка, опертого по трем сторонам в учебнике есть некоторая нелогичность
            * При соотношении сторон менее 0,5 происходит резкий скачок в определении момента
            *Так как данный скачок в запас несущей способности, то решили оставить так*/
            #region Исходные списки для интерполяции коэффициентов
            //Для участков, опертых по 2-м и 3-м сторонам
            List<double> xValues23 = new List<double>() { 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.2, 1.4, 2 };
            List<double> yValues23 = new List<double>() { 0.06, 0.074, 0.088, 0.097, 0.107, 0.112, 0.120, 0.126, 0.132 };
            #endregion
            double maxMoment;
            double[] sizes = GetPartSizes(basePart);
            double width = sizes[0];
            double length = sizes[1];
            double koeff_a1;
            double koeff_b1;
            #region //Если закрепления слева и справа
            //echoDelegate("Для участка заданы три опоры, опоры считаются шарнирными");
            if (basePart.FixLeft && basePart.FixRight)
            {
                koeff_a1 = width;
                koeff_b1 = length;
            }
            //иначе закрепления сверху и снизу
            else
            {
                koeff_a1 = length;
                koeff_b1 = width;
            }
            #endregion
            double ratio = koeff_b1 / koeff_a1;
            double koeff_betta;
            if (reportList != null) { reportList.Add("Опирание по трем сторонам"); }
            if (ratio < 0.5)
            {
                koeff_betta = 0.5;
                maxMoment = maxStress * koeff_b1 * koeff_b1 * koeff_betta;

                string unitMoment = maxMoment * MeasureUnitConverter.GetCoefficient(2) + MeasureUnitConverter.GetUnitLabelText(2);
                reportString = $"Изгибающий момент M={unitStress}*{koeff_b1}^2*{koeff_betta}={unitMoment}";
                if (reportList != null) { reportList.Add(reportString); }
            }
            else if (ratio >2)
            {
                koeff_betta = 0.125;
                maxMoment = maxStress * koeff_a1 * koeff_a1 * koeff_betta;

                string unitMoment = maxMoment * MeasureUnitConverter.GetCoefficient(2) + MeasureUnitConverter.GetUnitLabelText(2);
                reportString = $"Изгибающий момент M={unitStress}*{koeff_a1}^2*{koeff_betta}={unitMoment}";
                if (reportList != null) { reportList.Add(reportString); }
            }
            else 
            {
                koeff_betta = MathOperation.InterpolateList(xValues23, yValues23, ratio);
                maxMoment = maxStress * koeff_a1 * koeff_a1 * koeff_betta;

                string unitMoment = maxMoment * MeasureUnitConverter.GetCoefficient(2) + MeasureUnitConverter.GetUnitLabelText(2);
                reportString = $"Изгибающий момент M={unitStress}*{koeff_a1 * MeasureUnitConverter.GetCoefficient(0) + MeasureUnitConverter.GetUnitLabelText(0)}^2*{koeff_betta}={unitMoment}";
                if (reportList != null) { reportList.Add(reportString); }

                //echoDelegate($"Отношение сторон ratio =  + {Convert.ToString(ratio)}");
                //echoDelegate($"Вспомогательный коэффициент koeff_a1 = {Convert.ToString(koeff_a1)}");
                //echoDelegate($"Вспомогательный коэффициент koeff_betta = {Convert.ToString(koeff_betta)}");
            }
            return maxMoment;
        }
        /// <summary>
        /// Вычисляет изгибающий момент в участке плите для участка, опертого по четырем сторонам
        /// </summary>
        /// <param name="maxStress">Максимальное напряжение по участку</param>
        /// <param name="basePart">Экземпляр участка</param>
        /// <param name="echoDelegate">Делегат, в который передается результат расчета</param>
        /// <returns></returns>
        private static double CalcStreessFourSide(double maxStress, SteelBasePart basePart, List<string> reportList = null)
        {
            string reportString;
            string unitStress = maxStress * MeasureUnitConverter.GetCoefficient(3) + MeasureUnitConverter.GetUnitLabelText(3);
            #region Исходные списки для интерполяции коэффициентов
            //Для участков, опертых по 4-м сторонам
            List<double> xValues4 = new List<double>() { 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2 };
            List<double> yValues4 = new List<double>() { 0.048, 0.055, 0.063, 0.069, 0.075, 0.081, 0.086, 0.091, 0.094, 0.098, 0.1 };
            #endregion
            double maxMoment;
            double[] sizes = GetPartSizes(basePart);
            double width = sizes[0];
            double length = sizes[1];
            //echoDelegate("Для участка заданы четыре опоры, опоры считаются шарнирными");
            double koeff_b;
            double koeff_a;

            if (reportList != null) { reportList.Add("Опирание по четырем сторонам"); }

            if (width > length)
            {
                koeff_b = width;
                koeff_a = length;
            }
            else
            {
                koeff_b = length;
                koeff_a = width;
            }
            double ratio = koeff_b / koeff_a;
            double koeff_betta;
            //echoDelegate($"Отношение сторон ratio =  + {Convert.ToString(ratio)}");

            if (ratio > 2)
            {
                koeff_betta = 0.125;
                //echoDelegate("Соотношение сторон превышает 2, расчет ведется по двум сторонам");
                //echoDelegate($"Вспомогательный коэффициент koeff_betta = {Convert.ToString(koeff_betta)}");
            }
            else
            {
                koeff_betta = MathOperation.InterpolateList(xValues4, yValues4, ratio);
                //echoDelegate("Соотношение сторон не превышает 2, расчет ведется для плиты опертой по контуру");
                //echoDelegate($"Вспомогательный коэффициент koeff_betta = {Convert.ToString(koeff_betta)}");
            }
            if (reportList != null) { reportList.Add($"Длинная сторона L= {koeff_b * MeasureUnitConverter.GetCoefficient(0) + MeasureUnitConverter.GetUnitLabelText(0)}"); }
            if (reportList != null) { reportList.Add($"Короткая сторона B= {koeff_a * MeasureUnitConverter.GetCoefficient(0) + MeasureUnitConverter.GetUnitLabelText(0)}"); }
            if (reportList != null) { reportList.Add($"Соотношение сторон L/B= {ratio}"); }
            if (reportList != null) { reportList.Add($"Коэффициент Betta = {koeff_betta}"); }
            maxMoment = maxStress * koeff_a * koeff_a * koeff_betta;
            string unitMoment = maxMoment * MeasureUnitConverter.GetCoefficient(2) + MeasureUnitConverter.GetUnitLabelText(2);
            reportString = $"Изгибающий момент M={unitStress}*{koeff_a * MeasureUnitConverter.GetCoefficient(0) + MeasureUnitConverter.GetUnitLabelText(0)}^2*{koeff_betta}={unitMoment}";
            if (reportList != null) { reportList.Add(reportString); }
            return maxMoment;
        }
        #endregion
        /// <summary>
        /// Возвращает коллекцию элементарных участков для участка базы
        /// </summary>
        /// <param name="steelBasePart">Участок базы стальной колонны</param>
        /// <param name="Rc">Расчетное сопротивление сжатию</param>
        public static void GetSubParts(SteelBasePart steelBasePart, double Rc = 0)
        {
            double elemSize = 0.01;
            if (Rc == 0)
            {
                IMaterialModel materialModel = new LinearIsotropic(1e+10, 1, 0);
                steelBasePart.SubParts = NdmAreaProcessor.MeshRectangle(materialModel, steelBasePart.Width, steelBasePart.Length, steelBasePart.Center.X, steelBasePart.Center.Y, elemSize);
            }
            else
            {
                List<double> constantList = new List<double> { Rc * (-1D), -0.0015, -0.0035, 0, 0.0015, 0.0035 };
                IMaterialModel materialModel = new DoubleLinear(constantList);
                steelBasePart.SubParts = NdmAreaProcessor.MeshRectangle(materialModel, steelBasePart.Width, steelBasePart.Length, steelBasePart.Center.X, steelBasePart.Center.Y, elemSize);
            }

        }
        /// <summary>
        /// Вычисляет минимальное напряжение на участке по линейно упругой теории сопротивления материалов
        /// </summary>
        /// <param name="basePart">Участок базы колонны</param>
        /// /// <param name="massProperty">Геометрическая характеристика сечения</param>
        /// <param name="loadCase"></param>
        /// <returns>Минимальное напряжение на участке</returns>
        public static double GetMinStressLinear(SteelBasePart basePart, MassProperty massProperty, LoadSet loadCase)
        {
            List<double> stresses = new List<double>();
            //List<double> dxList = new List<double>();
            //List<double> dyList = new List<double>();
            //dxList.Add(basePart.CenterX + basePart.Width / 2);
            //dxList.Add(basePart.CenterX - basePart.Width / 2);
            //dyList.Add(basePart.CenterY + basePart.Length / 2);
            //dyList.Add(basePart.CenterY - basePart.Length / 2);
            //foreach (double dx in dxList)
            //{
            //    foreach (double dy in dyList)
            //    {
            //        stresses.Add(LoadSetProcessor.StressInBarSection(loadCase, massProperty, dx, dy));
            //    }
            //}
            return stresses.Min();
        }
        public static double GetGlobalMinStressLinear(SteelBasePart basePart, MassProperty massProperty, List<LoadSet> loadCases)
        {
            List<double> stresses = new List<double>();
            foreach (LoadSet loadCase in loadCases)
            {
                stresses.Add(GetMinStressLinear(basePart, massProperty, loadCase));
            }
            return stresses.Min();
        }
        //public static double GetGlobalMinStressLinear(SteelBasePart basePart, SteelBase columnBase)
        //{
        //    List<double> stresses = new List<double>();
        //    RectCrossSection baseRect = new RectCrossSection(basePart.ParentMember.Width, basePart.ParentMember.Length);
        //    MassProperty massProperty = RectProcessor.GetRectMassProperty(baseRect);
        //    foreach (LoadSet loadCase in columnBase.LoadCases)
        //    {
        //        stresses.Add(GetMinStressLinear(basePart, massProperty, loadCase));
        //    }
        //    return stresses.Min();
        //}

        //public static double GetGlobalMinStressLinear(SteelBasePart basePart)
        //{
        //    SteelBase columnBase = basePart.ParentMember as SteelBase;
        //    return GetGlobalMinStressLinear(basePart, columnBase);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basePart"></param>
        /// <param name="curvature"></param>
        /// <returns></returns>
        public static double GetMinStressNonLinear(SteelBasePart basePart, Curvature curvature)
        {
            List<double> stresses = new List<double>();
            foreach (NdmRectangleArea ndmConcreteArea in basePart.SubParts)
            {
                stresses.Add(NdmAreaProcessor.GetStrainFromCuvature(ndmConcreteArea, curvature)[1]);
            }
            return stresses.Min();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basePart"></param>
        /// <param name="curvatures"></param>
        /// <returns></returns>
        public static double GetGlobalMinStressNonLinear(SteelBasePart basePart, List<Curvature> curvatures)
        {
            List<double> stresses = new List<double>();
            foreach (Curvature curvature in curvatures)
            {
                stresses.Add(GetMinStressNonLinear(basePart, curvature));
            }
            return stresses.Min();
        }
        /// <summary>
        /// Возвращает минимальное (с учетом знака) давление на участок базы стальной колонны
        /// </summary>
        /// <param name="basePart"></param>
        /// <returns></returns>
        public static double GetGlobalMinStressNonLinear(SteelBasePart basePart)
        {
            List<double> stresses = new List<double>();
            SteelBase steelBase = basePart.ParentMember as SteelBase;
            foreach (ForceDoubleCurvature forceCurvature in steelBase.ForceCurvatures)
            {
                stresses.Add(GetMinStressNonLinear(basePart, forceCurvature.DesignCurvature));
            }
            return stresses.Min();
        }
        private static double[] GetPartSizes(SteelBasePart basePart)
        {
            double width = basePart.Width;
            if (basePart.FixLeft) width -= basePart.LeftOffset;
            if (basePart.FixRight) width -= basePart.RightOffset;
            double length = basePart.Length;
            if (basePart.FixTop) length -= basePart.TopOffset;
            if (basePart.FixBottom) length -= basePart.BottomOffset;
            return new double[] { width, length };
        }
    }
}
