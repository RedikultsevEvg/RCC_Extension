using RDBLL.Common.Geometry;
using RDBLL.Common.Geometry.Mathematic;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Geometry.Points;
using RDBLL.Common.Interfaces.Placements;
using RDBLL.Common.Params;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.PlacementEhs
{
    /// <summary>
    /// Базовый класс расположения для элементов
    /// </summary>
    public abstract class PlacementEhBase : ParametriсBase, IPlacementEh
    {
        #region Properties
        /// <summary>
        /// Коллекция вложенных расположений
        /// </summary>
        public List<IPlacementEh> Placements { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        /// <param name="genId">Флаг необходимости генерации Id</param>
        public PlacementEhBase(bool genId = false) : base(genId)
        {
            Placements = new List<IPlacementEh>();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Возвращает коллекцию точек расположения элементов
        /// </summary>
        /// <returns></returns>
        public List<Point2DRot> GetPoints()
        {
            List<Point2DRot> points = new List<Point2DRot>();
            List<Point2DRot> ownPoints = GetOwnPoints();
            foreach (Point2DRot ownPoint in ownPoints)
            {
                List<Point2DRot> childPoints = new List<Point2DRot>();
                childPoints.AddRange(GetChildPoints());
                //to do доделать трансформации
                //GeometryProcessor.RotatePoints(childPoints, ownPoint.RotZ);
                //GeometryProcessor.MovePoints(childPoints, ownPoint.X, ownPoint.Y);
                points.AddRange(childPoints);
            }
            return points;
        }
        /// <summary>
        /// Возвращает коллекцию точек вложенных расположений
        /// </summary>
        /// <returns></returns>
        protected List<Point2DRot> GetChildPoints()
        {
            List<Point2DRot> points = new List<Point2DRot>();
            foreach (IPlacementEh placement in Placements)
            {
                List<Point2DRot> childPoints = placement.GetPoints();
                points.AddRange(childPoints);
            }
            return points;
        }
        /// <summary>
        /// Возвращает коллекцию точек расположения без учета вложенных элементов
        /// </summary>
        /// <returns></returns>
        public abstract List<Point2DRot> GetOwnPoints();
        #endregion
    }
}
