using RDBLL.Entity.Soils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Builders
{
    public abstract class BuilderBase
    {
        //public BuilderBase() {}
        public virtual void AddCommnon(Foundation foundation)
        {
            foundation.Name = "Новый фундамент";
            foundation.RelativeTopLevel = -0.2;
            foundation.SoilRelativeTopLevel = -0.2;
            foundation.SoilVolumeWeight = 18000;
            foundation.ConcreteVolumeWeight = 25000;
            foundation.FloorLoad = 0;
            foundation.FloorLoadFactor = 1.2;
            foundation.ConcreteFloorLoad = 0;
            foundation.ConcreteFloorLoadFactor = 1.2;
            foundation.CompressedLayerRatio = 0.2;
        }
        public abstract void AddParts();
        public abstract void AddReinforcement();
        public abstract void AddConcrete();
        public abstract void AddLoads();
        public virtual void AddSoilSection(Foundation foundation)
        {
            //Использование скважины грунта
            SoilSectionUsing soilSectionUsing = new SoilSectionUsing(true);
            soilSectionUsing.RegisterParent(foundation);
        }
        public abstract Foundation GetFoundation(); 
    }
}
