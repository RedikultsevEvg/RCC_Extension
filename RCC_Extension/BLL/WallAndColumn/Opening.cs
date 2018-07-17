using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RCC_Extension.BLL.WallAndColumn
{
    public class Opening : ICloneable
    {
        public string Name { get; set; } //Наименование отверстия
        public string Purpose { get; set; } //Назначение отверстия
        public decimal Width { get; set; } //Ширина отверстия
        public decimal Height { get; set; } //Высота отверстия

        public decimal Left { get; set; } //Привязка от левого края (координата по X)
        public decimal Bottom { get; set; } //Привязка снизу (координата Y)

        public bool AddEdgeLeft { get; set; } //Устанавливать обрамление слева
        public bool AddEdgeRight { get; set; } //Справа
        public bool AddEdgeTop { get; set; } //Сверху
        public bool AddEdgeBottom { get; set; } //Снизу

        public bool MoveVert { get; set; }
        public int QuantVertLeft { get; set; } //Количество вертикальных стержней обрамления, если 0 - Стержни не учащаются
        public int QuantVertRight { get; set; } //Количество вертикальных стержней обрамления, если 0 - Стержни не учащаются

        public decimal Area()
        {
            //Возвращает площадь проема
            return Width * Height;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int AddEdgeQuant(int Type, decimal Spacing)
        {
            //Возвращает количество элементов обрамления с соответствующей стороны проема
            //Type
            //1 - обрамление слева
            //2 - обрамление справа
            //3 - обрамление сверху
            //4 - обрамление снизу
            int Quant=0;
            if (Type == 1 || AddEdgeLeft) Quant = Convert.ToInt32(Math.Ceiling(Height / Spacing))-1;
            if (Type == 2 || AddEdgeRight) Quant = Convert.ToInt32(Math.Ceiling(Height / Spacing)) - 1;
            if (Type == 3 || AddEdgeTop) Quant = Convert.ToInt32(Math.Ceiling(Width / Spacing)) - 1;
            if (Type == 4 || AddEdgeRight) Quant = Convert.ToInt32(Math.Ceiling(Width / Spacing)) - 1;
            return Quant;
        }
    }
}
