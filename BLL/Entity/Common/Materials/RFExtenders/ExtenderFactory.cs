using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials.RFExtenders
{
    /// <summary>
    /// Перечисление для фабрики экстендеров
    /// </summary>
    public enum ExtenderType
    {
        /// <summary>
        /// 
        /// </summary>
        CoveredLine,
        CoveredArray,
        CoveredArrayCommon
    }
    public static class ExtenderFactory
    {
        public static RFExtender GetCoveredArray(ExtenderType type)
        {
            switch (type)
            {
                case (ExtenderType.CoveredLine):
                    {
                        RFExtender extender = new LineToSurfBySpacing();
                        return extender;
                    }
                case (ExtenderType.CoveredArray) :
                {
                    CoveredArray coveredArray = new CoveredArray();
                    coveredArray.VisibleCover = true;
                    coveredArray.VisibleCenter = false;
                    coveredArray.VisibleSizes = false;
                    coveredArray.VisibleFillArray = false;
                    return coveredArray;
                }
                case (ExtenderType.CoveredArrayCommon):
                    {
                        CoveredArray coveredArray = new CoveredArray();
                        coveredArray.VisibleCover = false;
                        coveredArray.VisibleCenter = true;
                        coveredArray.VisibleSizes = true;
                        coveredArray.VisibleFillArray = true;
                        return coveredArray;
                    }
                default: return new CoveredArray();
            }

        }
    }
}
