using RDBLL.Entity.Soils;
using RDStartWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.ViewModels.Soils.SoilSections
{
    internal class SoilSectionUsingVM : ViewModelBase
    {
        private SoilSectionUsing _SoilSectionUsing;
        public int? SelectedId
        {
            get
            {
                return _SoilSectionUsing.SelectedId;
            }
            set
            {
                _SoilSectionUsing.SelectedId = value;
                OnPropertyChanged("SelectedId");
            }
        }
        public ObservableCollection<SoilSection> SoilSections { get => _SoilSectionUsing.SoilSections; }

        public SoilSectionUsingVM()
        {
        }
        public SoilSectionUsingVM(SoilSectionUsing soilSectionUsing)
        {
            _SoilSectionUsing = soilSectionUsing;
        }
    }
}
