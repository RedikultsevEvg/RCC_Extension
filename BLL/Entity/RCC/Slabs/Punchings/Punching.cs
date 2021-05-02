using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDBLL.Entity.RCC.Slabs.Punchings
{
    public class Punching : IHasParent, ICloneable, IHasForcesGroups, IRectangle
    {
        /// <summary>
        /// Код элемента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование элемента
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Обратная ссылка на родительский элемент (уровень)
        /// </summary>
        public IDsSaveable ParentMember { get; private set; }
        /// <summary>
        /// Коллекция слоев продавливания
        /// </summary>
        public ObservableCollection<PunchingLayer> Layers { get; set; }
        /// <summary>
        /// Коллекция групп нагрузок
        /// </summary>
        public ObservableCollection<ForcesGroup> ForcesGroups { get; set; }
        /// <summary>
        /// Коллекция комбинаций загружений
        /// </summary>
        public ObservableCollection<LoadSet> LoadCases { get; set; }
        /// <summary>
        /// Ширина сечения колонны (размер в направлении X)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Длина сечения колон (размер в направлении Y)
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Точка центра, не используется
        /// </summary>
        public Point2D Center { get => new Point2D(0,0); set => throw new NotImplementedException(); }
        /// <summary>
        /// Величина защитного слоя для арматуры вдоль оси X
        /// </summary>
        public double CoveringLayerX { get; set; }
        /// <summary>
        /// Величина защитного слоя для арматуры вдоль оси Y
        /// </summary>
        public double CoveringLayerY { get; set; }
        /// <summary>
        /// Клонирование элемента
        /// </summary>
        /// <returns></returns>
        public Punching(bool GenId = false)
        {
            if (GenId) { Id = ProgrammSettings.CurrentId; }
            Layers = new ObservableCollection<PunchingLayer>();
        }
        public object Clone()
        {
            Punching punching = this.MemberwiseClone() as Punching;
            punching.Id = ProgrammSettings.CurrentId;
            return punching;
        }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public double GetArea()
        {
            throw new NotImplementedException();
        }

        public string GetTableName() { return "Punchings";}

        public void OpenFromDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void RegisterParent(IDsSaveable parent)
        {
            Level level = parent as Level;
            ParentMember = level;
            level.Children.Add(this);
        }

        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            try
            {
                DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
                #region setFields
                //DsOperation.SetField(row, "RelativeTopLevel", RelativeTopLevel);
                #endregion
                row.AcceptChanges();
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage("Ошибка сохранения элемента: " + Name, ex);
            }
        }

        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.Children.Remove(this);
            ParentMember = null;
        }
    }
}
