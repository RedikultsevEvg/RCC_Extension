using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Forces
{
    public class Force : ICloneable
    {
        //Properties
        #region 
        public List<ForceParameter> ForceParameters { get; set; } 
        public double Force_Nz { get; set; }
        public double Force_Mx { get; set; }
        public double Force_My { get; set; }
        public double Force_Qx { get; set; }
        public double Force_Qy { get; set; }
        #endregion
        #region Constructors
        public Force(int type)
        {
            if (type == 1)
            {
                ForceParameters = new List<ForceParameter>();
                ForceParameters.Add(new ForceParameter
                {
                    Value = -100000,
                    ForceParamKind = ProgrammSettings.ForceParamKinds[0]
                });
                ForceParameters.Add(new ForceParameter
                {
                    Value = 0,
                    ForceParamKind = ProgrammSettings.ForceParamKinds[1]
                });
                ForceParameters.Add(new ForceParameter
                {
                    Value = 0,
                    ForceParamKind = ProgrammSettings.ForceParamKinds[2]
                });
                ForceParameters.Add(new ForceParameter
                {
                    Value = 0,
                    ForceParamKind = ProgrammSettings.ForceParamKinds[3]
                });
                ForceParameters.Add(new ForceParameter
                {
                    Value = 0,
                    ForceParamKind = ProgrammSettings.ForceParamKinds[4]
                });
            }

        }
        #endregion
        //IClonable
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
