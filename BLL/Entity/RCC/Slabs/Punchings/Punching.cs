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
        public Point2D Center { get; set; }
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
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            Center = new Point2D();
        }
        public object Clone()
        {
            Punching punching = this.MemberwiseClone() as Punching;
            punching.Id = ProgrammSettings.CurrentId;
            if (ParentMember != null) { punching.RegisterParent(this.ParentMember); }
            punching.ForcesGroups = new ObservableCollection<ForcesGroup>();
            foreach (ForcesGroup load in ForcesGroups)
            {
                ForcesGroup newLoad = load.Clone() as ForcesGroup;
                newLoad.Owners.Add(punching);
                punching.ForcesGroups.Add(newLoad);
            }
            punching.Layers = new ObservableCollection<PunchingLayer>();
            foreach (PunchingLayer layer in Layers)
            {
                PunchingLayer newLayer = layer.Clone() as PunchingLayer;
                newLayer.RegisterParent(punching);
            }
            punching.Center = this.Center.Clone() as Point2D;
            return punching;
        }

        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                forcesGroup.DeleteFromDataSet(dataSet);
            }
            foreach (PunchingLayer layer in Layers)
            {
                layer.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, GetTableName(), Id);
        }

        public double GetArea()
        {
            throw new NotImplementedException();
        }

        public string GetTableName() { return "Punchings";}

        public void OpenFromDataSet(DataSet dataSet)
        {
            try
            {
                OpenFromDataSet(DsOperation.OpenFromDataSetById(dataSet, GetTableName(), Id));
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка получения элемента из базы данных. Элемент {this.GetType().Name}: " + Name, ex);
            }
        }

        public void OpenFromDataSet(DataRow dataRow)
        {
            EntityOperation.SetProps(dataRow, this);
        }

        public void RegisterParent(IDsSaveable parent)
        {
            if (ParentMember != null) { UnRegisterParent(); }
            Level level = parent as Level;
            ParentMember = level;
            level.Children.Add(this);
        }
        //Сохранение объекта в датасет
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            try
            {
                DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
                #region setFields
                DsOperation.SetField(row, "CoveringLayerX", CoveringLayerX);
                DsOperation.SetField(row, "CoveringLayerY", CoveringLayerY);
                foreach (PunchingLayer layer in Layers) { layer.SaveToDataSet(dataSet, createNew); }
                #endregion
                row.AcceptChanges();
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка сохранения элемента {this.GetType().Name}: " + Name, ex);
            }
        }

        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.Children.Remove(this);
            ParentMember = null;
        }
        //Сравнение объектов
        public override bool Equals(object obj)
        {
            if (!(obj is Punching)) return false;
            Punching punching = obj as Punching;
            if (punching.Width != this.Width) return false;
            else if (punching.Length != this.Length) return false;
            else if (punching.CoveringLayerX != this.CoveringLayerX) return false;
            else if (punching.CoveringLayerY != this.CoveringLayerY) return false;
            return true;
        }
    }
}
