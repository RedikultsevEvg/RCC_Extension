﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Forces
{
    //Клас признаков загружения
    public class LoadSet
    {
        #region 
        public String Name { get; set; } //Наименование
        public double PartialSafetyFactor { get; set; } //Коэффициент надежности по нагрузке
        public bool IsDeadLoad { get; set; } //Флаг постоянной нагрузка
        public bool IsCombination { get; set; } //Флаг комбинации
        public bool IsDesignLoad { get; set; } //Флаг расчетной нагрузки
        public bool BothSign { get; set; } //Флаг знакопеременной нагрузки
        #endregion
    }
}
