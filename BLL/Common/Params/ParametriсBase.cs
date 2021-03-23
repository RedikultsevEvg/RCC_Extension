﻿using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Common.Params
{
    public abstract class ParametriсBase : IHasParent, IHasStoredParams
    {
        public IDsSaveable ParentMember { get; private set; }

        public int Id { get; set ; }
        public string Name { get; set; }

        public List<StoredParam> StoredParams { get; private set; }

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
            row.SetField<string>("Type", Type);
        }

        public void UnRegisterParent()
        {
            ParentMember = null;
        }
    }
}
