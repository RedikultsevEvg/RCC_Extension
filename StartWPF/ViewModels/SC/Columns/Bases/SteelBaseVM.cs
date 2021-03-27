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
using Winforms = System.Windows.Forms;

namespace RDStartWPF.ViewModels.SC.Columns.Bases
{
    internal class SteelBaseVM : ViewModelBase
    {
        public SteelBase Base { get; set; }
        //Команда получения частей базы
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
        //Команда получения болтов базы
        private CommandBase _ViewBoltsCommand;
        public CommandBase ViewBoltsCommand
        {
            get
            {
                return _ViewBoltsCommand ??
                    (_ViewBoltsCommand = new CommandBase(
                        newObject =>
                        {
                            //Показываем окно с болтами
                            ViewBolts(Base);
                        },
                        //Команда может выполняться только если у базы нет паттерна
                        newObject => Base.Pattern is null));
            }
        }
        //Команда удаления паттерна из базы
        private CommandBase _AntiPatCommand;
        public CommandBase AntiPatCommand
        {
            get
            {
                return _AntiPatCommand ??
                    (_AntiPatCommand = new CommandBase(
                        newObject =>
                        {
                            Winforms.DialogResult result = Winforms.MessageBox.Show("Удаление паттерна", "Удалить паттерн?",
                            Winforms.MessageBoxButtons.YesNo,
                            Winforms.MessageBoxIcon.Information,
                            Winforms.MessageBoxDefaultButton.Button1,
                            Winforms.MessageBoxOptions.DefaultDesktopOnly);

                            if (result == Winforms.DialogResult.Yes)
                            {
                                //Получаем части базы из паттерна
                                Base.Pattern.GetBaseParts();
                                //Удаляем паттерн
                                Base.Pattern = null;
                                ProgrammSettings.IsDataChanged = true;
                            }

                        },
                        //Команда может выполняться только если у базы есть паттерн
                        newObject => (Base.Pattern != null)
                        ));
            }
        }
        public SteelBaseVM(SteelBase _base)
        {
            Base = _base;
        }
        //метод показа болтов базы
        private void ViewBolts(SteelBase _base)
        {
            wndSteelBaseBolts wndSteelBaseBolts = new wndSteelBaseBolts(_base);
            wndSteelBaseBolts.ShowDialog();
            if (wndSteelBaseBolts.DialogResult == true)
            {
                try
                {
                    _base.IsActual = false;
                    DsOperation.DeleteRow(ProgrammSettings.CurrentDataSet, "SteelBolts", "ParentId", _base.Id);
                    foreach (SteelBolt steelBolt in _base.SteelBolts)
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
                _base.SteelBolts = GetEntity.GetSteelBolts(ProgrammSettings.CurrentDataSet, _base);
            }
        }
    }
}
