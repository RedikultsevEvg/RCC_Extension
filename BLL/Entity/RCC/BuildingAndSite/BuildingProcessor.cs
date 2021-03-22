using RDBLL.Common.Interfaces;
using RDBLL.Entity.Soils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.BuildingAndSite
{
    public static class BuildingProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject">Объект, от которого начинается поиск строительного объекта</param>
        /// <returns></returns>
        public static BuildingSite GetBuildingSite(IHasParent parentObject)
        {
            BuildingSite buildingSite = null;
            //Мы не знаем, какого уровня у нас родитель,
            //поэтому двигаемся вверх по родителям пока не достигнем строительного объекта
            while (!(parentObject is Building))
            {
                parentObject = parentObject.ParentMember as IHasParent;
                if (parentObject is Building) buildingSite = parentObject.ParentMember as BuildingSite;
            }
            return buildingSite;
        }
        /// <summary>
        /// Возвращает скважину из коллекции строительного объекта по ее коду
        /// </summary>
        /// <param name="parentObject">Объект, от которого начинается поиск строительного объекта</param>
        /// <param name="id">Код скважины</param>
        /// <returns></returns>
        public static SoilSection GetSoilSectionById(IHasParent parentObject, int id)
        {
            BuildingSite buildingSite = GetBuildingSite(parentObject);
            return GetSoilSectionById(buildingSite, id); 
        }

        public static SoilSection GetSoilSectionById(BuildingSite buildingSite, int id)
        {
            ObservableCollection<SoilSection> SoilSections = buildingSite.SoilSections;
            var section = (from soilSection in SoilSections.AsEnumerable()
                           where soilSection.Id == id
                           select soilSection).Single();
            return section;
        }
    }
}
