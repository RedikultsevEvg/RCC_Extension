using RDBLL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.Views.Common.Patterns.ControlClasses
{
    public class PatternCard : IHasParent
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get; set; }
        public IDsSaveable ParentMember { get; private set; }
        public string Description { get; set; }
        public string ToolTip { get; set; }
        public string ImageName { get; set; }


        public delegate void CommandDelegate(IDsSaveable dsSaveable);
        private CommandDelegate _commandDelegate;

        // Регистрируем делегат
        public void RegisterDelegate(CommandDelegate commandDelegate)
        {
            _commandDelegate = commandDelegate;
        }

        public void RunCommand()
        {
            if (!(_commandDelegate == null))
            {
                _commandDelegate.Invoke(ParentMember);
            }

        }

        public void RegisterParent(IDsSaveable parent)
        {
            ParentMember = parent;
        }

        public void UnRegisterParent()
        {
            ParentMember = null;
        }

        public string GetTableName()
        {
            throw new NotImplementedException();
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }
    }
}
