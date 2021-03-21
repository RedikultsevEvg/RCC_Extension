using RDBLL.Common.Interfaces;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.RCC.BuildingAndSite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.Soils
{
    public class SoilSectionUsing : IHasParent, ICloneable
    {


        public int Id { get; set; }
        public string Name { get; set; }
        public IDsSaveable ParentMember { get; private set; }
        //Код выбранной скважины
        public int? SelectedId { get ; set ; }
        public SoilSection SoilSection
        { get
            {
                if (SelectedId != null)
                {
                    return BuildingProcessor.GetSoilSectionById(this, Convert.ToInt32(SelectedId));
                }
                return null;
            }

        }
        /// <summary>
        /// Коллекция всех скважин в текущем объекте
        /// </summary>
        public ObservableCollection<SoilSection> SoilSections
        {
            get
            {
                BuildingSite buildingSite = BuildingProcessor.GetBuildingSite(this);
                ObservableCollection<SoilSection> soilSections = buildingSite.SoilSections;
                return soilSections;
            }
        }
        //Удаляет запись из датасета
        public void DeleteFromDataSet(DataSet dataSet) => DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        //Возвращает имя таблицы
        public string GetTableName() { return "SoilSectionUsings"; }
        //Открывает запись из датасета
        public void OpenFromDataSet(DataSet dataSet) => OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));

        public void OpenFromDataSet(DataRow dataRow)
        {
            Id = dataRow.Field<int>("Id");
            Name = dataRow.Field<string>("Name");
            SelectedId = dataRow.Field<int>("SelectedId");
        }

        public void RegisterParent(IDsSaveable parent)
        {
            if (!(parent is IHasSoilSection)) { throw new Exception("Type of perent is not suitable for borehole"); }
            IHasSoilSection hasSoilSection = parent as IHasSoilSection;
            hasSoilSection.RegSSUsing(this);
            ParentMember = hasSoilSection;
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
            row.SetField("SelectedId", SelectedId);
        }

        public void UnRegisterParent()
        {
            IHasSoilSection hasSoilSection = ParentMember as IHasSoilSection;
            hasSoilSection.UnRegSSUsing(this);
            ParentMember = null;
        }
        //Создает копию объекта
        public object Clone()
        {
            SoilSectionUsing sectionUsing = this.MemberwiseClone() as SoilSectionUsing;
            sectionUsing.Id = ProgrammSettings.CurrentId;
            return sectionUsing;
        }

        public SoilSectionUsing(bool genId = false)
        {
            if (genId)Id = ProgrammSettings.CurrentId;
            Name = "Скважина";
        }
    }
}
