using RDBLL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RDBLL.DrawUtils.Interfaces
{
    public interface IDrawScatch
    {
        void DrawTopScatch(Canvas canvas, IDsSaveable item);
    }
}
