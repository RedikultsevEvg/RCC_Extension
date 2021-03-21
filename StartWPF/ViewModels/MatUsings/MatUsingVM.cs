﻿using StartWPF.InfraStructure.Comands.Base;
using StartWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StartWPF.Views.MatUsings;
using System.Collections.ObjectModel;
using RDBLL.Entity.Common.Materials;

namespace StartWPF.ViewModels.MatUsings
{
    internal class MatUsingVM : ViewModelBase
    {
        private static CommandBase _OpenPSF;
        public static CommandBase OpenPSF
        {
            get
            {
                return _OpenPSF ??
                    (
                     _OpenPSF = new CommandBase(obj =>
                     {
                         MaterialUsing parentMember = obj as MaterialUsing;
                         WndSafetyFactors wndSafetyFactors = new WndSafetyFactors(parentMember);
                         wndSafetyFactors.ShowDialog();
                     }));
            }
        }
    }
}