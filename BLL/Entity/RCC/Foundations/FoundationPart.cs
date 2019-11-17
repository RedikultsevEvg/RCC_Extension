using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RDBLL.Common.Interfaces;
using System.Data;

namespace RDBLL.Entity.RCC.Foundations
{
    /// <summary>
    /// Класс части (ступени) фундамента
    /// </summary>
    public class FoundationPart
    {
        /// <summary>
        /// Код части фундамента
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
        /// Ширина (размер вдоль оси X)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина (размер вдоль оси Y)
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        public double Heigth { get; set; }
        /// <summary>
        /// Положение центра ступени по X
        /// </summary>
        public double CenterX { get; set; }
        /// <summary>
        /// Положение центра ступени по Y
        /// </summary>
        public double CenterY { get; set; }

    }
}
