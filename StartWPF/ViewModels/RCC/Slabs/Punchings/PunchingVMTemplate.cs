

using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.InfraStructure.Common.CommonOperations;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.ViewModels.Base.Interfaces;

//---------------------------------------------------------------------------------------
//Этот файл сгенерирован автоматически. Не вносите в данный файл какие либо изменения
//---------------------------------------------------------------------------------------


partial class PunchingVMTemplate : ViewModelBase
{
    Punching _Punching;

        /// <summary>
    /// Код элемента
    /// </summary>
            public System.Int32 Id
        {
            get => _Punching.Id;
            set
            {
            System.Int32 d = _Punching.Id;
            if (SetProperty(ref d, value)) { _Punching.Id = d; }
            }
        }   
        /// <summary>
    /// Наименование элемента
    /// </summary>
            public System.String Name
        {
            get => _Punching.Name;
            set
            {
            System.String d = _Punching.Name;
            if (SetProperty(ref d, value)) { _Punching.Name = d; }
            }
        }   
        public System.Double Width
        {
            get => _Punching.Width;
            set
            {
            System.Double d = _Punching.Width;
            if (SetProperty(ref d, value)) { _Punching.Width = d; }
            }
        }   
        public System.Double Length
        {
            get => _Punching.Length;
            set
            {
            System.Double d = _Punching.Length;
            if (SetProperty(ref d, value)) { _Punching.Length = d; }
            }
        }   
        public System.Double CoveringLayerX
        {
            get => _Punching.CoveringLayerX;
            set
            {
            System.Double d = _Punching.CoveringLayerX;
            if (SetProperty(ref d, value)) { _Punching.CoveringLayerX = d; }
            }
        }   
        public System.Double CoveringLayerY
        {
            get => _Punching.CoveringLayerY;
            set
            {
            System.Double d = _Punching.CoveringLayerY;
            if (SetProperty(ref d, value)) { _Punching.CoveringLayerY = d; }
            }
        }   
        public System.Boolean SeveralLayers
        {
            get => _Punching.SeveralLayers;
            set
            {
            System.Boolean d = _Punching.SeveralLayers;
            if (SetProperty(ref d, value)) { _Punching.SeveralLayers = d; }
            }
        }   
        public System.Boolean LeftEdge
        {
            get => _Punching.LeftEdge;
            set
            {
            System.Boolean d = _Punching.LeftEdge;
            if (SetProperty(ref d, value)) { _Punching.LeftEdge = d; }
            }
        }   
        public System.Boolean RightEdge
        {
            get => _Punching.RightEdge;
            set
            {
            System.Boolean d = _Punching.RightEdge;
            if (SetProperty(ref d, value)) { _Punching.RightEdge = d; }
            }
        }   
        public System.Boolean TopEdge
        {
            get => _Punching.TopEdge;
            set
            {
            System.Boolean d = _Punching.TopEdge;
            if (SetProperty(ref d, value)) { _Punching.TopEdge = d; }
            }
        }   
        public System.Boolean BottomEdge
        {
            get => _Punching.BottomEdge;
            set
            {
            System.Boolean d = _Punching.BottomEdge;
            if (SetProperty(ref d, value)) { _Punching.BottomEdge = d; }
            }
        }   
        public System.Double LeftEdgeDist
        {
            get => _Punching.LeftEdgeDist;
            set
            {
            System.Double d = _Punching.LeftEdgeDist;
            if (SetProperty(ref d, value)) { _Punching.LeftEdgeDist = d; }
            }
        }   
        public System.Double RightEdgeDist
        {
            get => _Punching.RightEdgeDist;
            set
            {
            System.Double d = _Punching.RightEdgeDist;
            if (SetProperty(ref d, value)) { _Punching.RightEdgeDist = d; }
            }
        }   
        public System.Double TopEdgeDist
        {
            get => _Punching.TopEdgeDist;
            set
            {
            System.Double d = _Punching.TopEdgeDist;
            if (SetProperty(ref d, value)) { _Punching.TopEdgeDist = d; }
            }
        }   
        public System.Double BottomEdgeDist
        {
            get => _Punching.BottomEdgeDist;
            set
            {
            System.Double d = _Punching.BottomEdgeDist;
            if (SetProperty(ref d, value)) { _Punching.BottomEdgeDist = d; }
            }
        }   
        public System.Boolean IsActive
        {
            get => _Punching.IsActive;
            set
            {
            System.Boolean d = _Punching.IsActive;
            if (SetProperty(ref d, value)) { _Punching.IsActive = d; }
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
                }
                Error = error;
            return error;
        }
    }
}