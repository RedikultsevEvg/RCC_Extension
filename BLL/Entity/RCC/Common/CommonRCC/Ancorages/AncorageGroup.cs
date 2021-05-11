using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Materials;
using RDBLL.Entity.Common.Materials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Common.CommonRCC.Ancorages
{
    public class AncorageGroup : IHasParent, IHasConcrete
    {
        public IDsSaveable ParentMember => throw new NotImplementedException();

        public int Id { get; set; }
        public string Name { get; set; }
        public ConcreteUsing Concrete { get; set; }
        public ObservableCollection<AncorageBar> AncorageBars { get; set; }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public string GetTableName()
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void RegisterParent(IDsSaveable parent)
        {
            throw new NotImplementedException();
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }

        public void UnRegisterParent()
        {
            throw new NotImplementedException();
        }
    }
}
