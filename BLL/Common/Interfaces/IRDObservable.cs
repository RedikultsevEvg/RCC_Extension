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

        //#region IRDObservable
        //public void AddObserver(IRDObserver obj)
        //{
        //    Observers.Add(obj);
        //}
        //public void RemoveObserver(IRDObserver obj)
        //{
        //    Observers.Remove(obj);
        //}
        //public void NotifyObservers()
        //{
        //    foreach (IRDObserver observer in Observers)
        //    {
        //        observer.Update();
        //    }
        //}
        //public bool HasChild()
        //{
        //  bool result = false;
        //  if (Observers.Count > 0) return true;
        //  return result;
        //}
        //#endregion
    }
}
