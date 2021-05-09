using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.InfraStructure.Common.CommonOperations;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.ViewModels.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RDStartWPF.ViewModels.RCC.Slabs.Punchings
{
    internal class PunchingVM : ViewModelBase, IHasParentVM
    {
        private Punching _Punching;
        public ViewModelBase ParentVM { get; set; }

        public string Name
        {
            get => _Punching.Name;
            set
            {
                string s = _Punching.Name;
                if (SetProperty(ref s, value))
                {
                    //if (string.IsNullOrEmpty(s)) Error += "Имя элемента не может быть пустым";
                    _Punching.Name = s;
                }
            }
        }
        public double Width
        {
            get => _Punching.Width;
            set
            {
                double d = _Punching.Width;
                if (SetProperty(ref d, value)) { _Punching.Width = d; }
            }
        }
        public double Length
        {
            get => _Punching.Length;
            set
            {
                double d = _Punching.Length;
                if (SetProperty(ref d, value)) { _Punching.Length = d; }
            }
        }
        public double CoveringLayerX
        {
            get => _Punching.CoveringLayerX;
            set
            {
                double d = _Punching.CoveringLayerX;
                if (SetProperty(ref d, value)) { _Punching.CoveringLayerX = d; }
            }
        }
        public double CoveringLayerY
        {
            get => _Punching.CoveringLayerY;
            set
            {
                double d = _Punching.CoveringLayerY;
                if (SetProperty(ref d, value)) { _Punching.CoveringLayerY = d; }
            }
        }
        //В будущем необходимо изменить работу с первым слоем на работу с любым слоем
        public double Height
        {
            get => (_Punching.Children[0] as PunchingLayer).Height;
            set
            {
                double d = (_Punching.Children[0] as PunchingLayer).Height;
                if (SetProperty(ref d, value))
                {
                    (_Punching.Children[0] as PunchingLayer).Height = d;
                }
            }
        }
        public ConcreteUsing Concrete
        {
            get => (_Punching.Children[0] as PunchingLayer).Concrete;
        }

        private CommandBase _EditForceCommand;
        public CommandBase EditForceCommand
        {
            get
            {
                return _EditForceCommand ??
                    (_EditForceCommand = new CommandBase(
                        newObject =>
                        {
                            CommonWindowOperation.ShowForceWindow(_Punching);
                        }
                        ));
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
                    case "Width":
                        {
                            if (Width < 0.05)
                            {
                                error = "Ширина колонны должна быть больше 50мм";
                            }
                        }
                        break;
                    case "Length":
                        {
                            if (Length < 0.05)
                            {
                                error = "Высота сечения колонны должна быть больше 50мм";
                            }
                        }
                        break;
                    case "CoveringLayerX":
                        {
                            if (CoveringLayerX < 0.0 || CoveringLayerX > Height)
                            {
                                error = "Неверная величина защитного слоя";
                            }
                        }
                        break;
                    case "CoveringLayerY":
                        {
                            if (CoveringLayerY < 0.0 || CoveringLayerY > Height)
                            {
                                error = "Неверная величина защитного слоя";
                            }
                        }
                        break;
                }
                Error = error;
                (ParentVM as ViewModelDialog).IsChildModified();
                return error;
            }
        }

        public PunchingVM(Punching punching)
        {
            _Punching = punching;
        }

    }
}
