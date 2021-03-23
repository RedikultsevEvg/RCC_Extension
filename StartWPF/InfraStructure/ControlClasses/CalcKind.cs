using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDStartWPF.Infrasructure.ControlClasses
{
    public class CalcKind
    {
        public String KindName { get; set; }
        public String KindAddition { get; set; }
 
        public delegate void CommandDelegate();
        private CommandDelegate _commandDelegate;

        // Регистрируем делегат
        public void RegisterDelegate(CommandDelegate commandDelegate)
        {
            _commandDelegate = commandDelegate;
        }

        public void RunCommand()
        {
            if (! (_commandDelegate == null))
            {
                _commandDelegate.Invoke();
            }          
        }
    }
}
