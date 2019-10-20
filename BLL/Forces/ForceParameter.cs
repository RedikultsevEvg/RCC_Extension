﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;

namespace RDBLL.Forces
{
    /// <summary>
    /// Класс величины усилия
    /// </summary>
    public class ForceParameter : IEquatable<ForceParameter>
    {
        private int _kind_id;
        private ForceParamKind _forceParamKind;
        private double _crcValue;

        public int Id { get; set; } //Код усилия
        public double CrcValue //Величина нагрузки (численное значение)
        {
            get { return _crcValue; }
            set { _crcValue = value; }
        }
        public double CrcValueInCurUnit
        {
            get
            {
                MeasureUnitLabel measureUnitLabel = _forceParamKind.MeasureUnit.GetCurrentLabel();
                return _crcValue * measureUnitLabel.AddKoeff;
            }
            set
            {
                MeasureUnitLabel measureUnitLabel = _forceParamKind.MeasureUnit.GetCurrentLabel();
                _crcValue = value / measureUnitLabel.AddKoeff;
            }
        }

        public double DesignValue { get; set; } //Величина нагрузки (численное значение)
        public int Kind_id //Код вида усилия (например, продольная сила). Виды нагрузки жестко предустановлены в программе
        {
            get { return _kind_id; }
            set
            {
                _kind_id = value;
                try
                {
                    var tmpForceParamKind = from t in ProgrammSettings.ForceParamKinds where t.Id == _kind_id select t;
                    _forceParamKind = tmpForceParamKind.First();
                }
                catch { }
                
            }
        }
        public ForceParamKind ForceParamKind
        {
            get
            {
                return _forceParamKind;
            }
            set { _forceParamKind = value; }
        }

        //IEquatable
        public bool Equals(ForceParameter other)
        {
            if (this.Kind_id == other.Kind_id
                & Math.Round(this.CrcValue, 3) == Math.Round(other.CrcValue, 3)
                & Math.Round(this.DesignValue, 3) == Math.Round(other.DesignValue, 3)
                )
            {
                return true;
            }
            else { return false; }
        }
    }


}
