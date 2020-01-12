﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soil
{
    /// <summary>
    /// Класс слоя грунта
    /// </summary>
    public class SoilLayer
    {
        /// <summary>
        /// Код слоя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на модель грунта
        /// </summary>
        public SoilBase Soil { get; set; }
        /// <summary>
        /// Отметка верха слоя
        /// </summary>
        public double TopLevel { get; set; }
    }
}