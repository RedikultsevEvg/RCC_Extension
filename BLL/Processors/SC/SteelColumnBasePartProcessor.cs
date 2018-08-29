﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Results.SC;
using RDBLL.Entity.SC.Column;
using RDBLL.Common.Geometry;
using System.Windows.Forms;

namespace RDBLL.Processors.SC
{
    public class SteelColumnBasePartProcessor
    {
        public static ColumnBasePartResult GetResult(SteelBasePart basePart)
        {
            #region
            List<double> xValues23 = new List<double>() { 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.2, 1.4, 2 };
            List<double> yValues23 = new List<double>() { 0.06, 0.074, 0.088, 0.097, 0.107, 0.112, 0.120, 0.126, 0.132 };
            List<double> xValues4 = new List<double>() { 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2 };
            List<double> yValues4 = new List<double>() { 0.048, 0.055, 0.063, 0.069, 0.075, 0.081, 0.086, 0.091, 0.094, 0.098, 0.1 };
            #endregion
            SteelColumnBaseProcessor columBaseProcessor = new SteelColumnBaseProcessor();
            ColumnBaseResult baseResult = columBaseProcessor.GetResult(basePart.ColumnBase);
            double maxStress = baseResult.MinStress*(-1.0);
            ColumnBasePartResult result = new ColumnBasePartResult();
            double width = basePart.Width;
            double length = basePart.Length;
            double thickness = basePart.ColumnBase.Thickness;
            double Wx = thickness * thickness / 6;
            double maxMoment = 0;
            int countFixSides = 0;
            if (basePart.FixLeft) { countFixSides++; }
            if (basePart.FixRight) { countFixSides++; }
            if (basePart.FixTop) { countFixSides++; }
            if (basePart.FixBottom) { countFixSides++; }
            if (countFixSides == 0)
            {
                MessageBox.Show("Неверное закрепление сторон", "Ошибка");
                return result;
            }
            //База целиком отрывается
            if (maxStress < 0) 
            {
                result.MaxMoment = 0;
                result.MaxStress = 0;
                return result;
            }
            //Если опора только по одной стороне, считаем как консоль
            if (countFixSides == 1)
            {
                //Если опора слева или справа
                if (basePart.FixLeft || basePart.FixRight)
                {
                    maxMoment = maxStress * width * width / 2;
                }
                //Если опора сверху или снизу
                if (basePart.FixTop || basePart.FixBottom)
                {
                    maxMoment = maxStress * length * length / 2;
                }
            }
            //Если опора с двух сторон
            if (countFixSides == 2)
            {
                //Если опора слева и справа
                if (basePart.FixLeft && basePart.FixRight)
                {
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
                        double koeff_a1 = Math.Sqrt(width*width + length*length);
                        double koeff_b1 = width*length*0.25/koeff_a1;
                        double ratio = koeff_b1 / koeff_a1;
                        double koeff_betta;

                        if (ratio > 2)
                        {
                            koeff_betta = 0.133;
                        }
                        else
                        {

                            if (ratio < 0.5) {koeff_betta = 0.06; }
                            else { koeff_betta = MathOperation.InterpolateList(xValues23, yValues23, ratio); }
                        }
                        maxMoment = maxStress * koeff_a1 * koeff_a1 * koeff_betta;
                    }

                }
                   
                }
            //Если опора с трех сторон    
            if(countFixSides == 3)
                {
                    double koeff_a1;
                    double koeff_b1;
                #region //Если закрепления слева и справа
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
                    //Console.WriteLine("Три стороны - консоль");
                    //Console.WriteLine("maxMoment = " + Convert.ToString(maxMoment));
                    //Console.ReadLine();
                }
                else
                {
                    koeff_betta = MathOperation.InterpolateList(xValues23, yValues23, ratio);
                    maxMoment = maxStress * koeff_a1 * koeff_a1 * koeff_betta;
                    //Console.WriteLine("ratio = " + Convert.ToString(ratio));
                    //Console.WriteLine("Три стороны - П-образная");
                    //Console.WriteLine("koeff_a1 = " + Convert.ToString(koeff_a1));
                    //Console.WriteLine("koeff_betta = " + Convert.ToString(koeff_betta));
                    //Console.WriteLine("maxMoment = " + Convert.ToString(maxMoment));
                    //Console.ReadLine();
                }

                
            }
            //Если опора с 4-х сторон
            if (countFixSides == 4)
            {
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
                if (ratio > 2) { koeff_betta = 0.125; Console.WriteLine("Четыре стороны - по двум сторонам"); }
                else
                {
                    koeff_betta = MathOperation.InterpolateList(xValues4, yValues4, ratio);
                    //Console.WriteLine("Четыре стороны - по контуру");
                }
                maxMoment = maxStress * koeff_a * koeff_a * koeff_betta;
                
                //Console.WriteLine("maxMoment = " + Convert.ToString(maxMoment));
                //Console.ReadLine();
            }
            result.MaxMoment = maxMoment;
            result.MaxStress = maxMoment / Wx;
            //Console.WriteLine("Time = " + DateTime.Now);
            //Console.WriteLine("maxMoment = " + Convert.ToString(maxMoment));
            //Console.WriteLine("MaxStress = " + Convert.ToString(result.MaxStress));
            //Console.ReadLine();
            return result;
        }
    }
}
