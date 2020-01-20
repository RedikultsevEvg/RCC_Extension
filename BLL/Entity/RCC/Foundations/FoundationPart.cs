﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Common.Service;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Entity.RCC.Foundations
{
    public abstract class FoundationPart
    {
        /// <summary>
        /// Код ступени фундамента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Код фундамента
        /// </summary>
        public int FoundationId { get; set; }
        /// <summary>
        /// Обратная ссылка на фундамент
        /// </summary>
        public Foundation Foundation { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Положение центра ступени по X
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Положение центра ступени по Y
        /// </summary>
        public double CenterY { get; set; }
        /// <summary>
        /// Наименование линейных единиц измерения
        /// </summary>
        public string LinearMeasure { get { return MeasureUnitConverter.GetUnitLabelText(0); } }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public FoundationPart()
        {

        }

        /// <summary>
        /// Конструктор по фундаменты
        /// </summary>
        /// <param name="foundation"></param>
        public FoundationPart(Foundation foundation)
        {
            Id = ProgrammSettings.CurrentId;
            FoundationId = foundation.Id;
            Foundation = foundation;
            if (foundation.Parts.Count == 0)
            {
                Name = "Подколонник";
                Height = 1;
            }
            else
            {
                Name = "Ступень " + foundation.Parts.Count;
                Height = 0.3;
            }
            CenterX = 0;
            CenterY = 0;
        }
    }
}
