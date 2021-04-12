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
    public class ForceDoubleCurvature : ForceCurvature
    {
        /// <summary>
        /// Кривизна дополнительная
        /// </summary>
        public Curvature SecondDesignCurvature { get; set; }
        /// <summary>
        /// Конструктор по всем параметрам
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="sumForces"></param>
        /// <param name="firstCurvature"></param>
        /// <param name="secondCurvature"></param>
        public ForceDoubleCurvature(LoadSet loadSet, SumForces sumForces, Curvature firstCurvature, Curvature secondCurvature) :base(loadSet, sumForces, firstCurvature)
        {
           SecondDesignCurvature = secondCurvature;
        }
        /// <summary>
        /// Конструктор по наборну нагрузок и двум кривизнам
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="firstCurvature"></param>
        /// <param name="secondCurvature"></param>
        public ForceDoubleCurvature(LoadSet loadSet, Curvature firstCurvature, Curvature secondCurvature) : base(loadSet, firstCurvature)
        {
            SecondDesignCurvature = secondCurvature;
        }
        /// <summary>
        /// Конструктор по набору нагрузок и значению кривизны
        /// Предполагается одинаковая кривизна для стали и бетона
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="curvature"></param>
        public ForceDoubleCurvature(LoadSet loadSet, Curvature curvature) : base(loadSet, curvature)
        {
            SecondDesignCurvature = curvature;
        }
    }
}
