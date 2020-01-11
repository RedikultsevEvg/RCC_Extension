using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;

namespace RDBLL.Entity.Common.NDM
{
    public class ForceCurvature
    {
        /// <summary>
        /// Набор нагрузок
        /// </summary>
        public LoadSet LoadSet { get; set; }
        /// <summary>
        /// Сумммарные нормативные усилия для кривизны
        /// </summary>
        public SumForces CrcSumForces { get; set; }
        /// <summary>
        /// Суммарные расчетные усилия для определения кривизны
        /// </summary>
        public SumForces DesignSumForces { get; set; }
        /// <summary>
        /// Кривизна для второй группы предельных состояний
        /// </summary>
        public Curvature CrcCurvature { get; set; }
        /// <summary>
        /// Кривизна для первой группы предельных состояний
        /// </summary>
        public Curvature DesignCurvature { get; set; }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ForceCurvature()
        {

        }
        /// <summary>
        /// Конструктор по комбинации нагрузок
        /// </summary>
        /// <param name="loadSet"></param>
        public ForceCurvature(LoadSet loadSet)
        {
            this.LoadSet = loadSet;
            this.CrcSumForces = new SumForces(loadSet, false);
            this.DesignSumForces = new SumForces(loadSet, true);
        }
        /// <summary>
        /// Конструктор по всем параметрам
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="sumForces"></param>
        /// <param name="concreteCurvature"></param>
        public ForceCurvature(LoadSet loadSet, SumForces sumForces, Curvature concreteCurvature)
        {
            this.LoadSet = loadSet;
            this.DesignSumForces = sumForces;
            this.DesignCurvature = concreteCurvature;
        }
       
        /// <summary>
        /// Конструктор по набору нагрузок и значению кривизны
        /// Предполагается одинаковая кривизна для стали и бетона
        /// </summary>
        /// <param name="loadSet"></param>
        /// <param name="designCurvature"></param>
        public ForceCurvature(LoadSet loadSet, Curvature designCurvature)
        {
            this.LoadSet = loadSet;
            this.DesignSumForces = new SumForces(loadSet);
            this.DesignCurvature = designCurvature;
        }
    }
}
