using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Common.Materials.Processors.Strength
{
    /// <summary>
    /// Процессор прочностных характеристик бетона и арматуры
    /// </summary>
    public static class StrengthProcessor
    {
        /// <summary>
        /// Возвращает расчетное сопротивление бетона
        /// </summary>
        /// <param name="concrete">Применение бетона</param>
        /// <param name="firstState">Флаг первой группы предельных состояний</param>
        /// <param name="inCompression">Флаг сжатия</param>
        /// <param name="fullLoad">Флаг полной нагрузки</param>
        /// <returns></returns>
        public static double GetConcreteStrength(ConcreteUsing concrete, bool firstState, bool inCompression, bool fullLoad)
        {
            double strength;
            ConcreteKind kind = concrete.MaterialKind as ConcreteKind;
            double coefficient = GetPartialCoefficient(concrete, firstState, inCompression, fullLoad);
            if (firstState) //Если для первой группы предельных состояния
            {
                if (inCompression) //Если при сжатии
                {
                    strength = kind.FstCompStrength;
                }
                else //иначе при растяжении
                {
                    strength = kind.FstTensStrength;
                }
            }
            else //Иначе для второй группы предельных состояния
            {
                if (inCompression)
                {
                    strength = kind.SndCompStrength;
                }
                else
                {
                    strength = kind.SndTensStrength;
                }
            }
            strength *= coefficient;
            return strength;
        }
        /// <summary>
        /// Возвращает суммарный коэффициент условий работы
        /// </summary>
        /// <param name="material">Материал</param>
        /// <param name="firstState">Флаг первой группы предельных состояний</param>
        /// <param name="inCompression">Флаг сжатия</param>
        /// <param name="fullLoad">Флаг полной нагрузки</param>
        /// <returns></returns>
        private static double GetPartialCoefficient(MaterialUsing material, bool firstState, bool inCompression, bool fullLoad)
        {
            double coefficient;
            if (firstState) //Если для первой группы предельных состояния
            {
                if (inCompression) //Если при сжатии
                {
                    if (fullLoad) //Если для полной нагрузки
                    {
                        coefficient = material.TotalSafetyFactor.PsfFst;
                    }
                    else //иначе для кратковременной нагрузки
                    {
                        coefficient = material.TotalSafetyFactor.PsfFstLong;
                    }
                }
                else //иначе при растяжении
                {
                    if (fullLoad)
                    {
                        coefficient = material.TotalSafetyFactor.PsfFstTens;
                    }
                    else
                    {
                        coefficient = material.TotalSafetyFactor.PsfFstLongTens;
                    }
                }
            }
            else //Иначе для второй группы предельных состояния
            {
                if (inCompression)
                {
                    if (fullLoad)
                    {
                        coefficient = material.TotalSafetyFactor.PsfSnd;
                    }
                    else
                    {
                        coefficient = material.TotalSafetyFactor.PsfSndLong;
                    }
                }
                else
                {
                    if (fullLoad)
                    {
                        coefficient = material.TotalSafetyFactor.PsfSndTens;
                    }
                    else
                    {
                        coefficient = material.TotalSafetyFactor.PsfSndLongTens;
                    }
                }
            }
            return coefficient;
        }
    }
}
