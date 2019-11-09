using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDUIL.WPF_Windows.ControlClasses
{
    public class ReportCard
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string ToolTip { get; set; }
        public string ImageName { get; set; }

        public delegate void CommandDelegate(string reportFileName);
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
                _commandDelegate.Invoke(FileName);
            }

        }
    }
}
