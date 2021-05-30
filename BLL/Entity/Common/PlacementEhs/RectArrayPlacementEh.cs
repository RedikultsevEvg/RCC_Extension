using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Geometry.Points;
using RDBLL.Common.Interfaces.Placements;
using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.PlacementEhs
{
    /// <summary>
    /// Класс прямоугольного расположения элементов
    /// </summary>
    public class RectArrayPlacementEh : PlacementEhBase, IRectangle, ICloneable
    {
        #region Properties
        /// <summary>
        /// Ширина (размер вдоль X)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина (размер вдоль Y)
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Величина отступа от указанного контура
        /// </summary>
        public double OffSet { get; set; }
        /// <summary>
        /// Точка расположения центра
        /// </summary>
        public Point2D Center { get; set; }
        /// <summary>
        /// Признак заполнения внутри массива
        /// </summary>
        public bool IsFilling { get; set; }
        /// <summary>
        /// Угол поворота расположения
        /// </summary>
        double Angle { get; set; }
        #endregion
        #region Constructors
        #endregion
        #region Methods
        /// <summary>
        /// Возвращает площадь прямоугольника
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            return (Width + 2 * OffSet) * (Length + 2 * OffSet);
        }

        public override List<Point2DRot> GetOwnPoints()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает глубокую копию элемента
        /// </summary>
        /// <returns></returns>
        //public override object Clone()
        //{
        //    RectArrayPlacementEh rectArray = this.MemberwiseClone() as RectArrayPlacementEh;
        //    return rectArray;
        //}

        public double GetPerimeter()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
