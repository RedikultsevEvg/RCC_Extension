using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Geometry;
using RDBLL.Common.Params;
using RDBLL.Common.Service;
using RDBLL.Common.Geometry.Mathematic;


namespace RDBLL.Entity.Common.Placements
{
    /// <summary>
    /// Клас расположения элементов в виде прямоугольного массива
    /// </summary>
    public class RectArrayPlacement : ArrayPlacement
    {
        new public int ParamQuant { get { return 5; } }
        /// <summary>
        /// Размер вдоль оси X
        /// </summary>
        public double SizeX
        {
            get { return StoredParams[0 + ParamQuant].GetDoubleValue(); }
            set { StoredParams[0 + ParamQuant].SetDoubleValue(value); }
        }
        /// <summary>
        /// Размер вдоль оси Y
        /// </summary>
        public double SizeY
        {
            get { return StoredParams[1 + ParamQuant].GetDoubleValue(); }
            set { StoredParams[1 + ParamQuant].SetDoubleValue(value); }
        }
        /// <summary>
        /// Количество вдоль оси X
        /// </summary>
        public int QuantityX
        {
            get { return StoredParams[2 + ParamQuant].GetIntValue(); }
            set { StoredParams[2 + ParamQuant].SetIntValue(value); }
        }
        /// <summary>
        /// Количество вдоль оси Y
        /// </summary>
        public int QuantityY
        {
            get { return StoredParams[3 + ParamQuant].GetIntValue(); }
            set { StoredParams[3 + ParamQuant].SetIntValue(value); }
        }
        /// <summary>
        /// Флаг заполнения массива внутренними элементами
        /// </summary>
        public bool FillArray
        {
            get { return StoredParams[3 + ParamQuant].GetBoolValue(); }
            set { StoredParams[3 + ParamQuant].SetBoolValue(value); }
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public RectArrayPlacement () : base()
        {
            // Размер вдоль оси X
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "SizeX" });
            StoredParams[0 + ParamQuant].SetDoubleValue(0.9);
            // Размер вдоль оси Y
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "SizeY" });
            StoredParams[1 + ParamQuant].SetDoubleValue(0.9);
            // Количество вдоль оси X
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "QuantityX" });
            StoredParams[2 + ParamQuant].SetIntValue(2);
            // Количество вдоль оси Y
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "QuantityY" });
            StoredParams[3 + ParamQuant].SetIntValue(2);
            // Флаг заполнения массива внутренними элементами
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "FillArray" });
            StoredParams[4 + ParamQuant].SetBoolValue(true);
        }
        /// <summary>
        /// Возвращает коллекцию точек, соответвующую расположению элементов
        /// </summary>
        /// <returns></returns>
        public override List<Point2D> GetElementPoints()
        {
            return GeometryProc.GetRectArrayPoints(Center, SizeX, SizeY, QuantityX, QuantityY, FillArray);
        }
    }
}
