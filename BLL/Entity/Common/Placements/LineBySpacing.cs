﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Params;
using RDBLL.Common.Service;
using System.Drawing;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Geometry;


namespace RDBLL.Entity.Common.Placements
{
    public class LineBySpacing : LinePlacement
    {
        new public int ParamQuant { get { return 7; } }
        /// <summary>
        /// Шаг элементов вдоль линии (максимальный)
        /// </summary>
        public double Spacing
        {
          get { return StoredParams[0 + ParamQuant].GetDoubleValue(); }
          set { StoredParams[0 + ParamQuant].SetDoubleValue( value); }
        }
        public LineBySpacing() : base ()
        {
            StoredParams.Add(new StoredParam() { Id = ProgrammSettings.CurrentId, Name = "Spacing" });
            StoredParams[0 + ParamQuant].SetDoubleValue(0.2);
        }

        /// <summary>
        /// Конструктор по входной строке
        /// </summary>
        /// <param name="s">Входная строка</param>
        public LineBySpacing(string s) : base(s) { }
        /// <summary>
        /// Возвращает коллекцию элементов
        /// </summary>
        /// <returns></returns>
        public override List<Point2D> GetElementPoints()
        {
            //Определяем количество элементов без учета крайних элементов
            int quant = Convert.ToInt32(Math.Ceiling(GetLength() / Spacing)) - 1;
            return GetElementPoints(quant);
        }
    }
}
