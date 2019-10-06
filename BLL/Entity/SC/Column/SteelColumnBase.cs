﻿using System;
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
        public int Id { get; set; } //Код базы
        public int LevelId { get; set; } //Код базы
        public Level Level { get; set; } //Ссылка на уровень
        public String Name { get; set; } //Наименование
        public bool IsActual { get; set; } //Признак актуальности расчета
        public double Width { get; set; } //Ширина базы, м
        public double Length { get; set; } //Длина базы, м
        public double Thickness { get; set; } //Толщина, м
        //public double WidthBoltDist { get; set; } //Расстояние между болтами по ширине, м
        //public double LengthBoltDist { get; set; } //Расстояние между болтами по длине, м
        public double Koeff_WorkCond { get; set; } //Коэффициент условий работы
        public double SteelStrength { get; set; } //Расчетное сопротивление базы
        public double ConcreteStrength { get; set; } //Прочность бетона подливки
        public double BoltPrestressForce { get; set; } //Усилия преднатяжения болта
        public ObservableCollection<ForcesGroup> LoadsGroup { get; set; } //Коллекция групп нагрузок
        public ObservableCollection<SteelBasePart> SteelBaseParts { get; set; } //Коллекция участков
        public List<SteelBasePart> ActualSteelBaseParts { get; set; } //Коллекция участков с учетом симметрии
        public ObservableCollection<SteelBolt> SteelBolts { get; set; } //Коллекция болтов
        public List<SteelBolt> ActualSteelBolts { get; set; } //Коллекция болтов с учетом симметрии
        public List<BarLoadSet> LoadCases { get; set; } //Коллекция комбинаций
        public bool IsLoadCasesActual { get; set; }
        #endregion

        //Constructors
        #region
        public void SetDefault()
        {
            Name = "Новая база";
            Width = 0.6;
            Length = 0.9;
            Thickness = 0.06;
            Koeff_WorkCond = 1.1;
            SteelStrength = 245000000;
            ConcreteStrength = 1000000;
            BoltPrestressForce = 0;
            LoadsGroup = new ObservableCollection<ForcesGroup>();
            LoadsGroup.Add(new ForcesGroup(this));
            SteelBaseParts = new ObservableCollection<SteelBasePart>();
            //ActualSteelBaseParts = new List<SteelBasePart>();
            SteelBolts = new ObservableCollection<SteelBolt>();
            //ActualSteelBolts = new List<SteelBolt>();
            //LoadCases = new List<BarLoadSet>();
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
