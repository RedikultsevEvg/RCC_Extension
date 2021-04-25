using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.SC.Column.SteelBases;
using RDBLL.Entity.SC.Column.SteelBases.Processors;
using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.Views.Common.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.ViewModels.SC.Columns.Bases
{
    internal class SteelBasePartGroupPgVM : ViewModelBase
    {
        private SteelBasePartGroup _Item { get; set; }
        private CommandBase _ViewProtocol;
        /// <summary>
        /// Имя элемента
        /// </summary>
        public string Name
        {
            get => _Item.Name;
            set
            {
                string s = _Item.Name;
                if (SetProperty(ref s, value))
                {
                    //if (string.IsNullOrEmpty(s)) Error += "Имя элемента не может быть пустым";
                    _Item.Name = s;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Высота участков
        /// </summary>
        public double Height
        {
            get => _Item.Height * 1000.0;
            set
            {
                double h = _Item.Height;
                if (SetProperty(ref h, value / 1000.0))
                {
                    _Item.Height = h;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Давление на участки
        /// </summary>
        public double Pressure
        {
            get => _Item.Pressure;
            set
            {
                double h = _Item.Pressure;
                if (SetProperty(ref h, value))
                {
                    _Item.Pressure = h;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Сталь для участков
        /// </summary>
        public SteelUsing Steel
        {
            get => _Item.Steel;
        }
        public CommandBase ViewProtocol
        {
            get
            {
                return _ViewProtocol ??
                    (_ViewProtocol = new CommandBase(
                        newObject =>
                        {
                            SteelBasePartGroupProcessor.SolvePartGroup(_Item);
                            WndReportList wnd = new WndReportList(_Item.ReportList);
                            wnd.ShowDialog();
                        },
                        //Команда может выполняться только если нет ошибок
                        newObject => _Item.SteelBaseParts.Count() > 0)
                    );
            }
        }
        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "Name":
                        {
                            if (string.IsNullOrEmpty(Name))
                            {
                                error = "Имя элемента не может быть пустым";
                            }
                        }
                        break;
                    case "Height":
                        {
                            if (Height / 1000.0 < 0.005 || Height / 1000.0 > 0.2)
                            {
                                error = "Неверная толщина элемента";
                            }
                        }
                        break;
                    case "Pressure":
                        {
                            double maxPressure = 200000000;
                            if (Pressure < 0)
                            {
                                error = "Давление должно быть больше нуля";
                            }
                            else if (Pressure > maxPressure)
                            {
                                error = $"Давление не должно превышать {maxPressure * MeasureUnitConverter.GetCoefficient(3)}{Measures.Stress}";
                            }
                        }
                        break;
                }
                Error = error;
                return error;
            }
        }
        /// <summary>
        /// Конструктор по группе участков
        /// </summary>
        /// <param name="partGroup"></param>
        public SteelBasePartGroupPgVM(SteelBasePartGroup partGroup)
        {
            _Item = partGroup;
        }
    }
}
