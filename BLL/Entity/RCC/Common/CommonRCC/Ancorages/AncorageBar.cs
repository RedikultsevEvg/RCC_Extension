using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Shapes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Common.CommonRCC.Ancorages
{
    public class AncorageBar : IHasParent, ICircle
    {
        public IDsSaveable ParentMember => throw new NotImplementedException();

        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Diameter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point2D Center { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public double GetArea()
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
