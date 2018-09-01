using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDUIL.WPF_Windows.ControlClasses
{
    public class CalcType
    {
        public String TypeName { get; set; }
        public String ImageName { get; set; }
        public List<CalcKind> CalcKinds { get; set; }
        public delegate void AddCommandDelegate(List<CalcKind> calcKinds);
        #region
        private AddCommandDelegate _addCommandDelegate;

        // Регистрируем делегат
        public void RegisterDelegate(AddCommandDelegate commandDelegate)
        {
            _addCommandDelegate = commandDelegate;
        }
        #endregion
        public void RunCommand()
        {
            _addCommandDelegate.Invoke(CalcKinds);
        }

        public CalcType()
        {
            CalcKinds = new List<CalcKind>();
        }

    }
}
