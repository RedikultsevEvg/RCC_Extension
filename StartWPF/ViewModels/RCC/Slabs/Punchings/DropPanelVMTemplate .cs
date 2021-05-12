
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.RCC.Slabs.Punchings;
using RDBLL.Entity.RCC.Slabs.Punchings.DropPanels;
using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.InfraStructure.Common.CommonOperations;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.ViewModels.Base.Interfaces;

//---------------------------------------------------------------------------------------
//Этот файл сгенерирован автоматически. Не вносите в данный файл какие либо изменения
//---------------------------------------------------------------------------------------

partial class DropPanelRectVMTemplate : ViewModelBase
{
    DropPanelRect _DropPanelRect;

        public System.Double Width
        {
            get => _DropPanelRect.Width;
            set
            {
            System.Double d = _DropPanelRect.Width;
            if (SetProperty(ref d, value)) { _DropPanelRect.Width = d; }
            }
        }   
        public System.Double Length
        {
            get => _DropPanelRect.Length;
            set
            {
            System.Double d = _DropPanelRect.Length;
            if (SetProperty(ref d, value)) { _DropPanelRect.Length = d; }
            }
        }   
        /// <summary>
    /// Код элемента
    /// </summary>
            public System.Int32 Id
        {
            get => _DropPanelRect.Id;
            set
            {
            System.Int32 d = _DropPanelRect.Id;
            if (SetProperty(ref d, value)) { _DropPanelRect.Id = d; }
            }
        }   
        /// <summary>
    /// Наименование элемента
    /// </summary>
            public System.String Name
        {
            get => _DropPanelRect.Name;
            set
            {
            System.String d = _DropPanelRect.Name;
            if (SetProperty(ref d, value)) { _DropPanelRect.Name = d; }
            }
        }   
        public System.Double Height
        {
            get => _DropPanelRect.Height;
            set
            {
            System.Double d = _DropPanelRect.Height;
            if (SetProperty(ref d, value)) { _DropPanelRect.Height = d; }
            }
        }   
        public System.Boolean UseParentControl
        {
            get => _DropPanelRect.UseParentControl;
            set
            {
            System.Boolean d = _DropPanelRect.UseParentControl;
            if (SetProperty(ref d, value)) { _DropPanelRect.UseParentControl = d; }
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