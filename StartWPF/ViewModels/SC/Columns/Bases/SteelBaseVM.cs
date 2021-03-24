using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.SC.Column;
using RDStartWPF.InfraStructure.Comands.Base;
using RDStartWPF.ViewModels.Base;
using RDStartWPF.Views.SC.Columns.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RDStartWPF.ViewModels.SC.Columns.Bases
{
    internal class SteelBaseVM : ViewModelBase
    {
        public SteelBase Base { get; set; }
        private CommandBase _ViewPartsCommand;
        public CommandBase ViewPartsCommand
        {
            get
            {
                return _ViewPartsCommand ??
                    (_ViewPartsCommand = new CommandBase(
                        newObject =>
                        {
                            WndSteelBasePart wndSteelBasePart = new WndSteelBasePart(Base);
                            wndSteelBasePart.ShowDialog();
                            if (wndSteelBasePart.DialogResult == true)
                            {
                                try
                                {
                                    Base.IsActual = false;
                                    DsOperation.DeleteRow(ProgrammSettings.CurrentDataSet, "SteelBaseParts", "ParentId", Base.Id);
                                    foreach (SteelBasePart steelBasePart in Base.SteelBaseParts)
                                    {
                                        steelBasePart.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                                    }
                                    ProgrammSettings.IsDataChanged = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Ошибка сохранения :" + ex);
                                }
                            }
                            else
                            {
                                Base.SteelBaseParts = GetEntity.GetSteelBaseParts(ProgrammSettings.CurrentDataSet, Base);
                            }
                        },
                        newObject => Base.Pattern is null));
            }
        }
        private CommandBase _ViewBoltsCommand;
        public CommandBase ViewBoltsCommand
        {
            get
            {
                return _ViewBoltsCommand ??
                    (_ViewBoltsCommand = new CommandBase(
                        newObject =>
                        {
                            wndSteelBaseBolts wndSteelBaseBolts = new wndSteelBaseBolts(Base);
                            wndSteelBaseBolts.ShowDialog();
                            if (wndSteelBaseBolts.DialogResult == true)
                            {
                                try
                                {
                                    Base.IsActual = false;
                                    DsOperation.DeleteRow(ProgrammSettings.CurrentDataSet, "SteelBaseParts", "ParentId", Base.Id);
                                    foreach (SteelBolt steelBolt in Base.SteelBolts)
                                    {
                                        steelBolt.SaveToDataSet(ProgrammSettings.CurrentDataSet, true);
                                    }
                                    ProgrammSettings.IsDataChanged = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Ошибка сохранения :" + ex);
                                }
                            }
                            else
                            {
                                Base.SteelBolts = GetEntity.GetSteelBolts(ProgrammSettings.CurrentDataSet, Base);
                            }
                            Base.SteelBaseParts = GetEntity.GetSteelBaseParts(ProgrammSettings.CurrentDataSet, Base);
                        },
                        newObject => Base.Pattern is null));
            }
        }
        public SteelBaseVM(SteelBase _base)
        {
            Base = _base;
        }
    }
}
