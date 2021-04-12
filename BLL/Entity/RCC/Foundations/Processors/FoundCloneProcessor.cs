using RDBLL.Common.Service;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Soils;
using RDBLL.Forces;
using RDBLL.Forces.Processors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Foundations.Processors
{
    /// <summary>
    /// Процессор для создания полной копии фундамента по исходному фундаменту
    /// </summary>
    public static class FoundCloneProcessor
    {
        public static void FoundationClone(Foundation source, Foundation found)
        {
            //Копируем нагрузки
            ForcesCloneProcessor.CloneForceCollection(source, found);
            //Копируем ступени
            DuplicateParts(source, found);
            //копируем материалы
            DuplicateMaterial(source, found);
            //Копируем скважину
            SoilSectionUsing soilSectionUsing = source.SoilSectionUsing.Clone() as SoilSectionUsing;
            soilSectionUsing.RegisterParent(found);
            found.IsActual = false;
            found.Result = new Results.FoundationResult();
        }
        /// <summary>
        /// Добавляет копию ступеней
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fromFound"></param>
        /// <param name="found"></param>
        private static void DuplicateParts(Foundation source, Foundation found)
        {
            //Создаем в приемнике новую коллекцию для ступеней
            found.Parts = new ObservableCollection<RectFoundationPart>();
            List<RectFoundationPart> parts = new List<RectFoundationPart>();
            foreach (RectFoundationPart part in source.Parts)
            {
                parts.Add(part.Clone() as RectFoundationPart);
            }
            foreach (RectFoundationPart part in parts)
            {
                part.ParentMember = found;
                found.Parts.Add(part);
            }
        }
        /// <summary>
        /// Добавляет копию материалов
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fromFound"></param>
        /// <param name="found"></param>
        private static void DuplicateMaterial(Foundation source, Foundation found)
        {
            found.BottomReinforcement = source.BottomReinforcement.Clone() as MaterialContainer;
            found.BottomReinforcement.RegisterParent(found);
            found.VerticalReinforcement = source.VerticalReinforcement.Clone() as MaterialContainer;
            found.VerticalReinforcement.RegisterParent(found);
            found.Concrete = source.Concrete.Clone() as ConcreteUsing;
            found.Concrete.RegisterParent(found);
        }
    }
}
