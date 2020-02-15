using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDUIL.WPF_Windows.Foundations.Soils.ControlClasses
{
    public class SoilCard
    {
        public string Name { get; set; }
        public BuildingSite BuildingSite { get; set; }
        //public string SoilTypeName { get; set; }
        public string Description { get; set; }
        public string ToolTip { get; set; }
        public string ImageName { get; set; }

        public delegate void CommandDelegate(BuildingSite buildingSite);
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
                _commandDelegate.Invoke(BuildingSite);
            }

        }
    }
}
