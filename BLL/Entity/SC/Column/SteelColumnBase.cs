using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Forces;
using RDBLL.Entity.Results.SC;
using RDBLL.Entity.RCC.BuildingAndSite;
using System.Collections.ObjectModel;

namespace RDBLL.Entity.SC.Column
{
    /// <summary>
    /// База стальной колонны
    /// </summary>
    public class SteelColumnBase : ICloneable
    {
        //Properties
        #region 
        //public ColumnBaseResult ColumnBaseResult { get; set; }
        public Level Level { get; set; }
        public int SteelColumnBaseID { get; set; } //Код стальной базы
        public String Name { get; set; } //Наименование
        public double Width { get; set; } //Ширина базы, м
        public double Length { get; set; } //Длина базы, м
        public double Thickness { get; set; } //Толщина, м
        public double WidthBoltDist { get; set; } //Расстояние между болтами по ширине, м
        public double LengthBoltDist { get; set; } //Расстояние между болтами по длине, м
        public double Koeff_WorkCond { get; set; } //Коэффициент условий работы
        public double SteelStrength { get; set; } //Расчетное сопротивление базы
        public double ConcreteStrength { get; set; } //Прочность бетона подливки
        public double BoltPrestressForce { get; set; } //Усилия преднатяжения болта
        public ObservableCollection<ForcesGroup> LoadsGroup { get; set; } //Коллекция групп нагрузок
        public ObservableCollection<SteelBasePart> SteelBaseParts { get; set; } //Коллекция участков

        #endregion
        
        //Constructors
        #region
        public void SetDefault()
        {
            Name = "Новая база";
            Width = 0.6;
            Length = 0.9;
            Thickness = 0.06;
            WidthBoltDist = 0.15;
            LengthBoltDist = 0.60;
            Koeff_WorkCond = 1.1;
            SteelStrength = 245000000;
            ConcreteStrength = 1000000;
            BoltPrestressForce = 0;
            LoadsGroup = new ObservableCollection<ForcesGroup>();
            LoadsGroup.Add(new ForcesGroup(this));
            SteelBaseParts = new ObservableCollection<SteelBasePart>();
        }
        /// <summary>
        /// Создает базу стальной колонны по указанному уровню
        /// </summary>
        /// <param name="level">Уровень, по которому создается колонна</param>
        public SteelColumnBase(Level level)
        {
            SetDefault();
            Level = level;
            level.SteelColumnBaseList.Add(this);
        }

        /// <summary>
        /// Создает базу стальной колонны со значениями по умолчанию
        /// </summary>
        public SteelColumnBase()
        {
            SetDefault();
        }

        #endregion
        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
