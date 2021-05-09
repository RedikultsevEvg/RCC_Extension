using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.ViewModels.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RDStartWPF.ViewModels.Base
{
    internal class ViewModelDialog : ViewModelBase
    {
        public Window Window;
        public Action BeforeOk;

        public List<IHasParentVM> Children { get; private set; }

        private CommandBase _CloseOkAction;
        private CommandBase _CloseCancelAction;
        public CommandBase CloseOkAction
        {
            get
            {
                return _CloseOkAction ??
                    (_CloseOkAction = new CommandBase(
                        newObject =>
                        {
                            BeforeOkClose();
                            if (BeforeOk != null) BeforeOk.Invoke();
                            Window.DialogResult = true;
                            Window.Close();
                        },
                        //Команда может выполняться только если нет ошибок
                        newObject => string.IsNullOrEmpty(Error))
                    );
            }
        }
        public CommandBase CloseCancelAction
        {
            get
            {
                return _CloseCancelAction ??
                    (_CloseCancelAction = new CommandBase(
                        newObject =>
                        {
                            BeforeCancelClose();
                            Window.DialogResult = false;
                            Window.Close();
                        },
                        //Команда может выполняться в любом случае
                        newObject => true)
                    );
            }
        }

        public override string this[string columnName] =>  throw new NotImplementedException();

        /// <summary>
        /// Метод для выполнени необходимых действий перед закрытием окна
        /// </summary>
        /// <param name="obj"></param>
        public virtual void BeforeOkClose(object obj = null)
        {

        }
        /// <summary>
        /// Метод для выполнени необходимых действий перед закрытием окна по отмене
        /// </summary>
        /// <param name="obj"></param>
        public virtual void BeforeCancelClose(object obj = null)
        {

        }

        public void IsChildModified()
        {
            Error = string.Empty;
            foreach (IHasParentVM child in Children)
            {
                Error += (child as ViewModelBase).Error;
            }
        }

        public ViewModelDialog(Window window)
        {
            Window = window;
            Children = new List<IHasParentVM>();
        }


    }
}
