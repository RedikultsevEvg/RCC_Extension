﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Params;
using RDBLL.Common.Service;
using System.Drawing;
using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;

namespace RDBLL.Entity.Common.Placements
{
    /// <summary>
    /// Класс элементов, распределенных по линии
    /// </summary>
    public abstract class LinePlacement : Placement
    {
        public int ParamQuant { get { return 1; } }
        public Point2D StartPoint
        { 
            get
            {
                _StartPoint.X = StoredParams[0 + ParamQuant].GetDoubleValue();
                _StartPoint.Y = StoredParams[1 + ParamQuant].GetDoubleValue();
                return _StartPoint;
            }
            set
            {
                _StartPoint = value;
                StoredParams[0 + ParamQuant].SetDoubleValue(_StartPoint.X);
                StoredParams[1 + ParamQuant].SetDoubleValue(_StartPoint.Y);
            }
        }
        public Point2D EndPoint
        {
            get
            {
                _EndPoint.X = StoredParams[2 + ParamQuant].GetDoubleValue();
                _EndPoint.Y = StoredParams[3 + ParamQuant].GetDoubleValue();
                return _EndPoint;
            }
            set
            {
                _EndPoint = value;
                StoredParams[2 + ParamQuant].SetDoubleValue(_EndPoint.X);
                StoredParams[3 + ParamQuant].SetDoubleValue(_EndPoint.Y);
            }
        }
        public bool AddStart
        {
            get {return StoredParams[4 + ParamQuant].GetBoolValue();}
            set { StoredParams[4 + ParamQuant].SetBoolValue(value); }
        }
        public bool AddEnd
        {
            get { return StoredParams[5 + ParamQuant].GetBoolValue(); }
            set { StoredParams[5 + ParamQuant].SetBoolValue(value); }
        }

        private Point2D _StartPoint;
        private Point2D _EndPoint;

        public LinePlacement(bool genId = false) : base(genId)
        {
            Type = "LinePlacement";
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "StartX" });
            //StoredParams[0 + ParamQuant].SetDoubleValue(0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "StartY" });
            //StoredParams[1 + ParamQuant].SetDoubleValue(0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "EndX" });
            //StoredParams[2 + ParamQuant].SetDoubleValue(0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "EndY" });
            //StoredParams[3 + ParamQuant].SetDoubleValue(0);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "AddStart" });
            //StoredParams[4 + ParamQuant].SetBoolValue(true);
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "AddStart" });
            //StoredParams[5 + ParamQuant].SetBoolValue(true);
            StartPoint = new Point2D(0, 0.05);
            EndPoint = new Point2D(0, 0.05);
            AddStart = true;
            AddEnd = true;
        }
        /// <summary>
        /// Возвращает
        /// </summary>
        /// <param name="quant"></param>
        /// <returns></returns>
        public List<Point2D> GetElementPoints(int quant)
        {    
            return GeometryProc.GetInternalPoints(StartPoint, EndPoint, quant, AddStart, AddEnd);
        }

        public double GetLength() {return GeometryProc.GetDistance(StartPoint, EndPoint);}
    }
}
