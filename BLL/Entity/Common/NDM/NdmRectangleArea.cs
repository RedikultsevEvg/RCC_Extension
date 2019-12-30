using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.MaterialModels;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM
{
    /// <summary>
    /// Класс элементарного участка прямоугольной формы
    /// </summary>
    public class NdmRectangleArea :NdmArea
    {
        private double _Width;
        private double _Length;

        /// <summary>
        /// Ширина участка
        /// </summary>
        public double Width
        { get { return _Width; }
            set
            {
                _Width = value;
                Area = _Width * _Length;
            }
        }
        /// <summary>
        /// Длина участка
        /// </summary>
        public double Length
        {
            get { return _Length; }
            set
            {
                _Length = value;
                Area = _Width * _Length;
            }
        }

        public NdmRectangleArea(IMaterialModel materialModel)
        {
            MaterialModel = materialModel;
            //ConcreteArea = new NdmArea(new LinearIsotropic(1e+10, 1, 0));
        }
        //public NdmRectangleArea(List<double> list)
        //{
        //    ConcreteArea = new NdmArea(new DoubleLinear(list));
        //    //ConcreteArea = new NdmArea(new LinearIsotropic(1e+10, 1, 0));
        //}
    }
}
