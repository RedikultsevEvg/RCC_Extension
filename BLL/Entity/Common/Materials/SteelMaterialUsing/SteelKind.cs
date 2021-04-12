using RDBLL.Entity.Common.Materials.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials.SteelMaterialUsing
{
    public class SteelKind : IMaterialKind
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double FstStrength { get; set; }
        public double SndStrength { get; set; }
        public double ElasticModulus { get; set; }
        public double PoissonRatio { get => 0.3; }


        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public string GetTableName() { return "SteelKinds"; }

        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            throw new NotImplementedException();
        }
    }
}
