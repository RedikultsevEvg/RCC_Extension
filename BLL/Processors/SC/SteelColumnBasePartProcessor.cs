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


namespace RDBLL.Processors.SC
{
    delegate void EchoDelegate(String S);

    public static class SteelColumnBasePartProcessor
    {
        public static double[] GetResult(SteelBasePart basePart, double maxStress)
        {
            /*Алгоритм расчета основан на подходе из учебника Белени по
             * таблицам Галеркина
             * Момент определяется в зависимости от количества сторон, по которым имеется опора
             * Если опора имеется только по одной стороне, то считается что участок консольный
             * Иначе считаетася, что участок оперт шарнирно
             */
            //double maxStress = GetMinStressLinear(basePart);
            double[] result = new double[2] { 0, 0 };
            double thickness = basePart.ColumnBase.Thickness;
            double Wx = thickness * thickness / 6;
            double maxMoment = 0;
            int countFixSides = 0;
            
            #region Определение количества сторон, по которым имеются опоры
            if (basePart.FixLeft) { countFixSides++; }
            if (basePart.FixRight) { countFixSides++; }
            if (basePart.FixTop) { countFixSides++; }
            if (basePart.FixBottom) { countFixSides++; }
            #endregion
            //Участок отрывается, напряжения равны нулю
            if (maxStress < 0)
            {
                result[0] = 0;
                result[1] = 0;
                //MessageBox.Show("Неверное приложение нагрузки - полный отрыв базы", "Ошибка");
                return result;
            }
            switch (countFixSides)
            {
                case 0://Если ни одна из опор не задана, то это ошибка
                    MessageBox.Show("Неверное закрепление сторон", "Ошибка");
                    return result;
                case 1://Если опора только по одной стороне, считаем как консоль
                    maxMoment = CalcStreessOneSide(maxStress, basePart);
                    break;
                case 2://Если опора с двух сторон
                    maxMoment = CalcStreessTwoSide(maxStress, basePart);
                    break;
                case 3://Если опора с трех сторон 
                    maxMoment = CalcStreessThreeSide(maxStress, basePart);
                    break;
                case 4://Если опора с 4-х сторон
                    maxMoment = CalcStreessFourSide(maxStress, basePart);
                    break;
            }
            result[0] = maxMoment;
            result[1] = maxMoment / Wx;
            return result;
        }

        public static double[] GetResult(SteelBasePart basePart)
        {
            return GetResult(basePart, GetGlobalMinStressLinear(basePart) * (-1D));
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
        /// Вычисляет изгибающий момент в участке плите для участка, опертого по одной стороне
        /// </summary>
        /// <param name="maxStress">Максимальное напряжение по участку</param>
        /// <param name="basePart">Экземпляр участка</param>
        /// <param name="echoDelegate">Делегат, в который передается результат расчета</param>
        /// <returns></returns>
        private static double CalcStreessOneSide(double maxStress, SteelBasePart basePart)
        {
            double maxMoment;
            double width = basePart.Width;
            double length = basePart.Length;
            //Если опора слева или справа
            //echoDelegate("Для участка задана опора по одной стороне, участок расчитывается как консоль");
            if (basePart.FixLeft || basePart.FixRight)
            {
                maxMoment = maxStress * width * width / 2;
            }
            //Если опора сверху или снизу
            else
            {
                maxMoment = maxStress * length * length / 2;
            }
            return maxMoment;
        }
        /// <summary>
        /// Вычисляет изгибающий момент в участке плите для участка, опертого по одной стороне
        /// </summary>
        /// <param name="maxStress">Максимальное напряжение по участку</param>
        /// <param name="basePart">Экземпляр участка</param>
        /// <param name="echoDelegate">Делегат, в который передается результат расчета</param>
        /// <returns></returns>
        private static double CalcStreessTwoSide(double maxStress, SteelBasePart basePart)
        {
            #region Исходные списки для интерполяции коэффициентов
            //Для участков, опертых по 2-м и 3-м сторонам
            List<double> xValues23 = new List<double>() { 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.2, 1.4, 2 };
            List<double> yValues23 = new List<double>() { 0.06, 0.074, 0.088, 0.097, 0.107, 0.112, 0.120, 0.126, 0.132 };
            #endregion
            double maxMoment;
            double width = basePart.Width;
            double length = basePart.Length;
            //Если опора слева и справа
            if (basePart.FixLeft && basePart.FixRight)
            {
                //echoDelegate("Для участка заданы две опоры по противоположным сторонам, опоры считаются шарнирными");
                maxMoment = maxStress * width * width / 8;
            }
            else
            {
                //Если опора снизу и сверху
                if (basePart.FixTop && basePart.FixBottom)
                {
                    maxMoment = maxStress * length * length / 8;
                }
                //Иначе плита оперта по двум смежным сторонам, неважно каким
                else
                {
                    //echoDelegate("Для участка заданы две опоры по смежным сторонам, опоры считаются шарнирными");
                    double koeff_a1 = Math.Sqrt(width * width + length * length);
                    double koeff_b1 = width * length * 0.25 / koeff_a1;
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
                    maxMoment = maxStress * koeff_a1 * koeff_a1 * koeff_betta;
                }

            }
            return maxMoment;
        }
        /// <summary>
        /// Вычисляет изгибающий момент в участке плите для участка, опертого по трем сторонам
        /// </summary>
        /// <param name="maxStress">Максимальное напряжение по участку</param>
        /// <param name="basePart">Экземпляр участка</param>
        /// <param name="echoDelegate">Делегат, в который передается результат расчета</param>
        /// <returns></returns>
        private static double CalcStreessThreeSide(double maxStress, SteelBasePart basePart)
        {
            /*Для участка, опертого по трем сторонам в учебнике есть некоторая нелогичность
            * При соотношении сторон менее 0,5 происходит резкий скачок в определении момента
            *Так как данный скачок в запас несущей способности, то решили оставить так*/
            #region Исходные списки для интерполяции коэффициентов
            //Для участков, опертых по 2-м и 3-м сторонам
            List<double> xValues23 = new List<double>() { 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.2, 1.4, 2 };
            List<double> yValues23 = new List<double>() { 0.06, 0.074, 0.088, 0.097, 0.107, 0.112, 0.120, 0.126, 0.132 };
            #endregion
            double maxMoment;
            double width = basePart.Width;
            double length = basePart.Length;
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

            if (ratio < 0.5)
            {
                koeff_betta = 0.5;
                maxMoment = maxStress * koeff_b1 * koeff_b1 * koeff_betta;
            }
            else
            {
                koeff_betta = MathOperation.InterpolateList(xValues23, yValues23, ratio);
                maxMoment = maxStress * koeff_a1 * koeff_a1 * koeff_betta;
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
        private static double CalcStreessFourSide(double maxStress, SteelBasePart basePart)
        {
            #region Исходные списки для интерполяции коэффициентов
            //Для участков, опертых по 4-м сторонам
            List<double> xValues4 = new List<double>() { 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2 };
            List<double> yValues4 = new List<double>() { 0.048, 0.055, 0.063, 0.069, 0.075, 0.081, 0.086, 0.091, 0.094, 0.098, 0.1 };
            #endregion
            double maxMoment;
            double width = basePart.Width;
            double length = basePart.Length;
            //echoDelegate("Для участка заданы четыре опоры, опоры считаются шарнирными");
            double koeff_b;
            double koeff_a;
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
            maxMoment = maxStress * koeff_a * koeff_a * koeff_betta;
            return maxMoment;
        }
        #endregion
        /// <summary>
        /// Получает коллекцию всех участков базы с учетом симметрии
        /// </summary>
        /// <param name="steelColumnBase"></param>
        /// <returns></returns>
        public static List<SteelBasePart> GetSteelBasePartsFromColumnBase(SteelColumnBase steelColumnBase)
        {
            List<SteelBasePart> steelBaseParts = new List<SteelBasePart>(); 
            foreach (SteelBasePart steelBasePart in steelColumnBase.SteelBaseParts)
            {
                List<SteelBasePart> locSteelBaseParts = GetSteelBasePartsFromPart(steelBasePart);
                foreach (SteelBasePart locSteelBasePart in locSteelBaseParts)
                {
                    steelBaseParts.Add(locSteelBasePart);
                }
            }
            return steelBaseParts;
        }
        /// <summary>
        /// Поллучает коллекцию участков по заданному с учетом симметрии
        /// </summary>
        /// <param name="steelBasePart"></param>
        /// <returns></returns>
        public static List<SteelBasePart> GetSteelBasePartsFromPart(SteelBasePart steelBasePart)
        {
            List<SteelBasePart> steelBaseParts = new List<SteelBasePart>();
            steelBaseParts.Add(steelBasePart);
            if (steelBasePart.AddSymmetricX)
            {
                SteelBasePart newSteelBasePart = (SteelBasePart)(steelBasePart.Clone());
                newSteelBasePart.Name = steelBasePart.Name + 'X';
                newSteelBasePart.ColumnBase = steelBasePart.ColumnBase;
                newSteelBasePart.Center[0] = (1.0) * steelBasePart.Center[0];
                newSteelBasePart.Center[1] = (-1.0) * steelBasePart.Center[1];
                newSteelBasePart.FixTop = steelBasePart.FixBottom;
                newSteelBasePart.FixBottom = steelBasePart.FixTop;
                steelBaseParts.Add(newSteelBasePart);
            }
            if (steelBasePart.AddSymmetricY)
            {
                SteelBasePart newSteelBasePart = (SteelBasePart)(steelBasePart.Clone());
                newSteelBasePart.Name = steelBasePart.Name + 'Y';
                newSteelBasePart.ColumnBase = steelBasePart.ColumnBase;
                newSteelBasePart.Center[0] = (-1.0) * steelBasePart.Center[0];
                newSteelBasePart.Center[1] = (1.0) * steelBasePart.Center[1];
                newSteelBasePart.FixLeft = steelBasePart.FixRight;
                newSteelBasePart.FixRight = steelBasePart.FixLeft;
                steelBaseParts.Add(newSteelBasePart);
            }
            if (steelBasePart.AddSymmetricX & steelBasePart.AddSymmetricY)
            {
                SteelBasePart newSteelBasePart = (SteelBasePart)(steelBasePart.Clone());
                newSteelBasePart.Name = steelBasePart.Name + "XY";
                newSteelBasePart.ColumnBase = steelBasePart.ColumnBase;
                newSteelBasePart.Center[0] = (-1.0) * steelBasePart.Center[0];
                newSteelBasePart.Center[1] = (-1.0) * steelBasePart.Center[1];
                newSteelBasePart.FixTop = steelBasePart.FixBottom;
                newSteelBasePart.FixBottom = steelBasePart.FixTop;
                newSteelBasePart.FixLeft = steelBasePart.FixRight;
                newSteelBasePart.FixRight = steelBasePart.FixLeft;
                steelBaseParts.Add(newSteelBasePart);
            }
            return steelBaseParts;
        }
        /// <summary>
        /// Получает коллекцию элементарных участков для участка базы
        /// </summary>
        /// <param name="steelBasePart"></param>
        public static void GetSubParts(SteelBasePart steelBasePart)
        {
            steelBasePart.SubParts = new List<NdmConcreteArea>();
            double elementSize = 0.02;
            int numX = Convert.ToInt32(steelBasePart.Width / elementSize);
            int numY = Convert.ToInt32(steelBasePart.Length / elementSize);
            //Шаг элементарных участков (совпадает с соответствующим размером участка)
            double stepX = steelBasePart.Width / numX;
            double stepY = steelBasePart.Length / numY;

            double startCenterX = steelBasePart.Center[0] - steelBasePart.Width / 2 + stepX / 2;
            double startCenterY = steelBasePart.Center[1] - steelBasePart.Length / 2 + stepY / 2;

            for (int i = 0; i < numX; i++)
            {
                for (int j = 0; j < numY; j++)
                {
                    NdmConcreteArea subPart = new NdmConcreteArea();
                    subPart.Width = stepX;
                    subPart.Length = stepY;
                    subPart.ConcreteArea.CenterX = startCenterX + stepX * i;
                    subPart.ConcreteArea.CenterY = startCenterY + stepY * j;
                    steelBasePart.SubParts.Add(subPart);
                }
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
            //RectCrossSection baseRect = new RectCrossSection(basePart.ColumnBase.Width, basePart.ColumnBase.Length);
            //MassProperty massProperty = RectProcessor.GetRectMassProperty(baseRect);
            List<double> dxList = new List<double>();
            List<double> dyList = new List<double>();
            dxList.Add(basePart.Center[0] + basePart.Width / 2);
            dxList.Add(basePart.Center[0] - basePart.Width / 2);
            dyList.Add(basePart.Center[1] + basePart.Length / 2);
            dyList.Add(basePart.Center[1] - basePart.Length / 2);
            foreach (double dx in dxList)
            {
                foreach (double dy in dyList)
                {
                    stresses.Add(LoadSetProcessor.StressInBarSection(loadCase, massProperty, dx, dy));
                }
            }
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
        public static double GetGlobalMinStressLinear(SteelBasePart basePart, SteelColumnBase columnBase)
        {
            List<double> stresses = new List<double>();
            RectCrossSection baseRect = new RectCrossSection(basePart.ColumnBase.Width, basePart.ColumnBase.Length);
            MassProperty massProperty = RectProcessor.GetRectMassProperty(baseRect);
            foreach (LoadSet loadCase in columnBase.LoadCases)
            {
                stresses.Add(GetMinStressLinear(basePart, massProperty, loadCase));
            }
            return stresses.Min();
        }
        public static double GetGlobalMinStressLinear(SteelBasePart basePart)
        {
            SteelColumnBase columnBase = basePart.ColumnBase;
            return GetGlobalMinStressLinear(basePart, columnBase);
        }
        public static double GetMinStressNonLinear(SteelBasePart basePart, Curvature curvature)
        {
            List<double> stresses = new List<double>();
            foreach (NdmConcreteArea ndmConcreteArea in basePart.SubParts)
            {
                NdmArea ndmArea = ndmConcreteArea.ConcreteArea;
                stresses.Add(NdmAreaProcessor.GetStrainFromCuvature(ndmArea, curvature)[1]);
            }
            return stresses.Min();
        }
    }
}
