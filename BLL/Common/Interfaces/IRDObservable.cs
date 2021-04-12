using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Interfaces
{
    public interface IRDObservable
    {
        void AddObserver(IRDObserver obj);
        void RemoveObserver(IRDObserver obj);
        void NotifyObservers();
        bool HasChild();
    }
}
