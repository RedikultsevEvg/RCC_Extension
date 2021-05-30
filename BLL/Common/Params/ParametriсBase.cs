using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Params
{
    /// <summary>
    /// Базовый абстрактный класс параметрических объектов
    /// </summary>
    public abstract class ParametriсBase : IHasParent, IHasStoredParams, ICloneable
    {
        public IDsSaveable ParentMember { get; private set; }

        public int Id { get; set ; }
        public string Name { get; set; }

        public List<StoredParam> StoredParams { get; set; }

        public virtual string Type { get; set; }

        public ParametriсBase(bool genId = false)
        {
            if (genId) { Id = ProgrammSettings.CurrentId; }
            StoredParams = new List<StoredParam>();
        }
        public void DeleteFromDataSet(DataSet dataSet) {EntityOperation.DeleteEntity(dataSet, this);}

        public string GetTableName() => "ParametricObjects";

        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataRow dataRow) {EntityOperation.SetProps(dataRow, this);}

        public void RegisterParent(IDsSaveable parent) { ParentMember = parent; }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            row.AcceptChanges();
        }

        public void UnRegisterParent()
        {
            ParentMember = null;
        }

        public object Clone()
        {
            ParametriсBase newObj = this.MemberwiseClone() as ParametriсBase;
            newObj.StoredParams = new List<StoredParam>();
            foreach (StoredParam param in StoredParams)
            {
                StoredParam newParam = param.Clone() as StoredParam;
                newParam.RegisterParent(newObj);
                newObj.StoredParams.Add(newParam);
            }
            return newObj;
        }
    }
}
