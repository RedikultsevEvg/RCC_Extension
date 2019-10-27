using RDBLL.Common.Service;
using RDBLL.Entity.Common.NDM;
using System;

namespace RDBLL.Entity.SC.Column
{
    public class SteelBolt: ICloneable
    {
        public int Id { get; set; } //Код
        public int SteelBaseId { get; set; } //Код базы
        public SteelBase ColumnBase { get; set; } //Ссылка на базу
        public String Name { get; set; }
        public double Diameter { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public bool AddSymmetricX { get; set; } //Наличие симметричного участка относительно оси X
        public bool AddSymmetricY { get; set; } //Наличие симметричного участка по оси Y
        public NdmSteelArea SubPart { get; set; }

        //Constructors
        #region
        public SteelBolt(SteelBase steelColumnBase)
        {
            Id = ProgrammSettings.CurrentId;
            SteelBaseId = steelColumnBase.Id;
            ColumnBase = steelColumnBase;
            Name = "Новый болт";
            Diameter = 0.030;
            CenterX = 0.200;
            CenterY = 0.300;
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
