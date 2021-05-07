using RDStartWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RDStartWPF.Views.Bases.Pages
{
    internal class PageBase<VM> : Page where VM : ViewModelBase
    {
        private VM _ViewModel;
        public PageBase (VM viewModel)
        {
            _ViewModel = viewModel;
        }
    }
}
