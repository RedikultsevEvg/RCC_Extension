using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RDStartWPF.ViewModels.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private ICommand _Command;

        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
