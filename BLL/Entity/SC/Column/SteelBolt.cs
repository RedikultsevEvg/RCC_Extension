using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBolt: ICloneable
    {
        public int Id { get; set; } //Код
        public int SteelBaseId { get; set; } //Код базы
        public SteelColumnBase SteelColumnBase { get; set; } //Ссылка на базу
        public String Name { get; set; }
        public double Diameter { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }
        public bool AddSymmetricX { get; set; } //Наличие симметричного участка относительно оси X
        public bool AddSymmetricY { get; set; } //Наличие симметричного участка по оси Y

        //Constructors
        #region
        public SteelBolt(SteelColumnBase steelColumnBase)
        {
            SteelBaseId = steelColumnBase.Id;
            Name = "Новый болт";
            Diameter = 0.030;
            CoordX = 0.200;
            CoordY = 0.300;
            AddSymmetricX = true;
            AddSymmetricY = true;
        }
        #endregion

        public object Clone()
        {
            SteelBolt steelBolt = this.MemberwiseClone() as SteelBolt;
            return steelBolt;
        }
    }
}
