﻿using RDBLL.Common.Interfaces;
using RDBLL.Entity.SC.Column;
using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.ViewModels.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.ViewModels.SC.Columns.Bases
{
    internal class SteelBasePartsPgVM : ViewModelBase, IHasCollection
    {
        private IDsSaveable _Parent;
        public ObservableCollection<SteelBasePart> Collection { get; set; }
        public bool IsCenterVisible { get; private set; }
        private SteelBasePart _SelectedItem;
        private CommandBase _AddCommand;
        private CommandBase _RemoveCommand;
        public SteelBasePart SelectedItem
            {
            get
            {
                return _SelectedItem;
            }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
        public CommandBase AddCommand
        {
            get
            {
                return _AddCommand ??
                    (_AddCommand = new CommandBase(newObject =>
                    {
                        SteelBasePart part = new SteelBasePart(_Parent);
                    }));
            }
        }

        public CommandBase RemoveCommand
        {
            get
            {
                return _RemoveCommand ??
                    (_RemoveCommand = new CommandBase(oldObject =>
                    {
                        SteelBasePart Item = oldObject as SteelBasePart;
                        if (Item != null)
                        {
                            Item.UnRegisterParent();
                        }
                    },
                    oldObject => Collection.Count > 0));
            }
        }

        public override string this[string columnName] => throw new NotImplementedException();

        /// <summary>
        /// Конструктор вьюмодели
        /// </summary>
        /// <param name="parts">Коллекция частей</param>
        /// <param name="parent">Ссылка на родительский элемент</param>
        /// <param name="showCenter">Флаг показа координат центра</param>
        public SteelBasePartsPgVM(ObservableCollection<SteelBasePart> parts, IDsSaveable parent, bool showCenter = true)
        {
            _Parent = parent;
            Collection = parts;
            IsCenterVisible = showCenter;
            if (Collection.Count() > 0) { _SelectedItem = Collection[0]; }
        }
    }
}
