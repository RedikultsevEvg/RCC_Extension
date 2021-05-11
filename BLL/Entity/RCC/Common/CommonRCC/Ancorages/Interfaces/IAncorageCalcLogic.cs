using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Common.CommonRCC.Ancorages.Interfaces
{
    public interface IAncorageCalcLogic
    {
        //1. Вычисление длины анкеровки арматуры при растяжении/сжатии
        double GetAncorageLength();
        //2. Вычисление одинарной длины нахлеста при растяжении/сжатии
        double GetSimpleLappingLength();
        //3. Вычисление двойной длины нахлеста при растяжении/сжатии
        double GetDoubleLappingLength();
        //4. Вычисление величины разбежки для нахлеста
        double GetLappingGapLength();
    }
}
