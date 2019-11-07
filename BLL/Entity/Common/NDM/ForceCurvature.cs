using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;

namespace RDBLL.Entity.Common.NDM
{
    /// <summary>
    /// Класс для хранения набора нагрузок, суммарных усилий для определения кривизны
    /// и двух кривизн - для бетона и арматуры
    /// </summary>
    public class ForceCurvature
    {
        /// <summary>
        /// Набор нагрузок
        /// </summary>
        public LoadSet LoadSet { get; set; }
        /// <summary>
        /// Суммарные усилия для определения кривизны
        /// </summary>
        public SumForces SumForces { get; set; }
        /// <summary>
        /// Кривизна для бетона
        /// </summary>
        public Curvature ConcreteCurvature { get; set; }
        /// <summary>
        /// Кривизна для стали
        /// </summary>
        public Curvature SteelCurvature { get; set; }
        /// <summary>
        /// Конструктор по всем параметрам
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="sumForces"></param>
        /// <param name="concreteCurvature"></param>
        /// <param name="steelCurvature"></param>
        public ForceCurvature(LoadSet loadSet, SumForces sumForces, Curvature concreteCurvature, Curvature steelCurvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = sumForces;
            this.ConcreteCurvature = concreteCurvature;
            this.SteelCurvature = steelCurvature;
        }
        /// <summary>
        /// Конструктор по наборну нагрузок и двум кривизнам
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="concreteCurvature"></param>
        /// <param name="steelCurvature"></param>
        public ForceCurvature(LoadSet loadSet, Curvature concreteCurvature, Curvature steelCurvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = new SumForces(loadSet);
            this.ConcreteCurvature = concreteCurvature;
            this.SteelCurvature = steelCurvature;
        }
        /// <summary>
        /// Конструктор по набору нагрузок и значению кривизны
        /// Предполагается одинаковая кривизна для стали и бетона
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="curvature"></param>
        public ForceCurvature(LoadSet loadSet, Curvature curvature)
        {
            this.LoadSet = loadSet;
            this.SumForces = new SumForces(loadSet);
            this.ConcreteCurvature = curvature;
            this.SteelCurvature = curvature;
        }
    }
}
