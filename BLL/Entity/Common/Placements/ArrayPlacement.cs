using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Params;
using RDBLL.Common.Service;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Geometry;

namespace RDBLL.Entity.Common.Placements
{
    /// <summary>
    /// Абстрактный класс расположения элементов в виде массива (кругового, прямоугольного и т.д.)
    /// </summary>
    public abstract class ArrayPlacement : Placement
    {
        new public int ParamQuant { get { return 1; } }
        /// <summary>
        /// Угол поворота массива относительно родительского элемента
        /// </summary>
        public double ArrayAngle
        {
            get { return StoredParams[0 + ParamQuant].GetDoubleValue(); }
            set { StoredParams[0 + ParamQuant].SetDoubleValue(value); }
        }
        /// <summary>
        /// Величина защитного слоя
        /// </summary>
        public double CoveringLayer
        {
            get { return StoredParams[1 + ParamQuant].GetDoubleValue(); }
            set { StoredParams[1 + ParamQuant].SetDoubleValue(value); }
        }
        /// <summary>
        /// Точка центра массива
        /// </summary>
        public Point2D Center
        {
            get
            {
                _Center.X = StoredParams[2 + ParamQuant].GetDoubleValue();
                _Center.Y = StoredParams[3 + ParamQuant].GetDoubleValue();
                return _Center;
            }
        }
    
        private Point2D _Center;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ArrayPlacement() : base()
        {
            _Center = new Point2D();
            // Угол поворота массива относительно родительского элемента
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "ArrayAngle" });
            StoredParams[0 + ParamQuant].SetDoubleValue(0);
            // Угол поворота массива относительно родительского элемента
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "CoveringLayer" });
            StoredParams[1 + ParamQuant].SetDoubleValue(0);
            // Координата центра массива по X
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "CenterX" });
            StoredParams[2 + ParamQuant].SetDoubleValue(0);
            // Координата центра массива по Y
            StoredParams.Add(new StoredParam(this) { Id = ProgrammSettings.CurrentId, Name = "CenterY" });
            StoredParams[3 + ParamQuant].SetDoubleValue(0);
        }
        /// <summary>
        /// Устанавливает центр массива по координатам
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetCenter(double x, double y)
        {
            StoredParams[2 + ParamQuant].SetDoubleValue(x);
            StoredParams[3 + ParamQuant].SetDoubleValue(y);
            _Center.X = StoredParams[2 + ParamQuant].GetDoubleValue();
            _Center.Y = StoredParams[3 + ParamQuant].GetDoubleValue();
        }
        public void SetCenter(Point2D point)
        {
            SetCenter(point.X, point.Y);
        }
    }
}
