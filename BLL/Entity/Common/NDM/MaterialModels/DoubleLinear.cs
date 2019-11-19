using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Entity.Common.NDM.Interfaces;

namespace RDBLL.Entity.Common.NDM.MaterialModels
{
    /// <summary>
    /// Модель двухлинейного материала
    /// </summary>
    public class DoubleLinear : IMaterialModel
    {
        /// <summary>
        /// Начальный модуль упругости, Па
        /// </summary>
        public double ElasticModulus { get; set; }
        /// <summary>
        /// Список констант, интерпретация констант определяется моделью
        /// </summary>
        public List<MaterialConstant> ListOfConsnstants { get; set; }

        public DoubleLinear(List<double> list)
        //double copmressionStrength, double copmressionStrain, double copmressionUltStrain, double tensionStrength, double tensionStrain, double tensionUltStrain)
        {
            double copmressionStrength = list[0];
            double copmressionStrain = list[1];
            double copmressionUltStrain = list[2];
            double tensionStrength = list[3];
            double tensionStrain = list[4];
            double tensionUltStrain = list[5];

            ElasticModulus = 1e10;
            ListOfConsnstants = new List<MaterialConstant>();
            MaterialConstant NewConstant;
            NewConstant = new MaterialConstant { Name = "CompressionStrength", ConstantValue = copmressionStrength };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "copmressionStrain", ConstantValue = copmressionStrain };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "copmressionUltStrain", ConstantValue = copmressionUltStrain };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "TensionStrength", ConstantValue = tensionStrength };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "TensionStrain", ConstantValue = tensionStrain };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "TensionUltStrain", ConstantValue = tensionUltStrain };
            ListOfConsnstants.Add(NewConstant);
        }
        /// <summary>
        /// Возвращает напряжения
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Напряжения, Па</returns>
        public double GetStress(double strain)
        {
            double Rc = ListOfConsnstants[0].ConstantValue;
            double Epsc = ListOfConsnstants[1].ConstantValue;
            double UltEpsc = ListOfConsnstants[2].ConstantValue;
            double Rt = ListOfConsnstants[3].ConstantValue;
            double Epst = ListOfConsnstants[4].ConstantValue;
            double UltEpst = ListOfConsnstants[5].ConstantValue;
            double compressionModulus = Rc / Epsc;
            double tensionModulus = Rt / Epst;

            //Если деформации сжимающие
            if (strain < 0)
            {
                if (strain < UltEpsc || Rc == 0 || UltEpsc == 0) //Усли деформации больше предельных 
                { return 0; }
                else if (strain > Epsc)
                { return compressionModulus * strain;}
                else { return Rc; }
            }
            else //Деформации растягивающие
            {
                if (strain > UltEpst || Rt == 0 || UltEpst == 0)
                { return 0; }
                else if (strain < Epst)
                { return tensionModulus * strain; }
                else { return Rt; }
            }

        }
        /// <summary>
        /// Возвращает секущий модуль упругости
        /// </summary>
        /// <param name="strain">Деформации, д.ед.</param>
        /// <returns>Секущий модуль упругости, Па</returns>
        public double GetSecantModulus(double strain)
        {
            if (strain == 0) { return ElasticModulus; }
            else { return GetStress(strain) / strain; }
        }
    }
}
