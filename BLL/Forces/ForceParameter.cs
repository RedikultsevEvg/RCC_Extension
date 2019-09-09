﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Forces
{
    /// <summary>
    /// Класс величины усилия
    /// </summary>
    public class ForceParameter :IEquatable<ForceParameter>
    {
        private int _kind_id;
        private ForceParamKind _forceParamKind;

        public int Id { get; set; } //Код усилия
        public double Value { get; set; } //Величина нагрузки (численное значение)
        public int Kind_id //Код вида усилия (например, продольная сила). Виды нагрузки жестко предустановлены в программе
        {
            get { return _kind_id; }
            set
            {
                _kind_id = value;
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
                & Math.Round(this.Value, 3) == Math.Round(other.Value, 3))
            {
                return true;
            }
            else { return false; }
        }
    }


}
