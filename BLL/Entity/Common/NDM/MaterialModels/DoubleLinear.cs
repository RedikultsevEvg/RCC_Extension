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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elasticModulus"></param>
        /// <param name="copmressionStrength"></param>
        /// <param name="copmressionUltStrain"></param>
        /// <param name="tensionStrength"></param>
        /// <param name="tensionUltStrain"></param>
        public DoubleLinear(double elasticModulus, double copmressionStrength, double copmressionUltStrain, double tensionStrength, double tensionUltStrain)
        {
            ElasticModulus = elasticModulus;
            ListOfConsnstants = new List<MaterialConstant>();
            MaterialConstant NewConstant;
            NewConstant = new MaterialConstant { Name = "CompressionStrength", ConstantValue = copmressionStrength };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "copmressionUltStrain", ConstantValue = copmressionUltStrain };
            ListOfConsnstants.Add(NewConstant);
            NewConstant = new MaterialConstant { Name = "TensionStrength", ConstantValue = tensionStrength };
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
            double elasticModulus = ElasticModulus;
            double Rc = ListOfConsnstants[0].ConstantValue;
            double Epsc = ListOfConsnstants[1].ConstantValue;
            double Rt = ListOfConsnstants[2].ConstantValue;
            double Epst = ListOfConsnstants[3].ConstantValue;

            //Если деформации сжимающие
            if (strain < 0)
            {
                if (strain < Epsc || Rc == 0 || Epsc == 0) //Усли деформации больше предельных 
                { return 0; }
                else if (strain < (Rc/elasticModulus))
                { return elasticModulus * strain;}
                else { return Rc; }
            }
            else //Деформации растягивающие
            {
                if (strain > Epst || Rt == 0 || Epst == 0)
                { return 0; }
                else if (strain > (Rt / elasticModulus))
                { return elasticModulus * strain; }
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
