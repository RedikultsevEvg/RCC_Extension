﻿using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces;
using RDBLL.Common.Interfaces.IOInterfaces;
using RDBLL.Common.Interfaces.Shapes;
using RDBLL.Common.Service;
using RDBLL.Common.Service.DsOperations;
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Entity.RCC.Slabs.Punchings.Results;
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
    /// <summary>
    /// Класс расчета узла колонна-плиты на продавливание
    /// </summary>
    public class Punching : IHasParent, ICloneable, IHasForcesGroups, IRectangle, IHasChildren
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
        public ObservableCollection<IHasId> Children { get; set; }
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
        /// Флаг видимости нескольких слоев
        /// </summary>
        public bool SeveralLayers { get; set; }
        /// <summary>
        /// Признак необходимости учета края слева
        /// </summary>
        public bool LeftEdge { get; set; }
        /// <summary>
        /// Признак необходимости учета края справа
        /// </summary>
        public bool RightEdge { get; set; }
        /// <summary>
        /// Признак необходимости учета края сверху
        /// </summary>
        public bool TopEdge { get; set; }
        /// <summary>
        /// Признак необходимости учета края снизу
        /// </summary>
        public bool BottomEdge { get; set; }
        /// <summary>
        /// Расстояние до края слева
        /// </summary>
        public double LeftEdgeDist { get; set; }
        /// <summary>
        /// Расстояние до края справа
        /// </summary>
        public double RightEdgeDist { get; set; }
        /// <summary>
        /// Расстояние до края сверху
        /// </summary>
        public double TopEdgeDist { get; set; }
        /// <summary>
        /// Расстояние до края снизу
        /// </summary>
        public double BottomEdgeDist { get; set; }
        /// <summary>
        /// Признак актуальности расчета
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Свойство для хранения результата
        /// </summary>
        public PunchingResult Result { get; set; }
        /// <summary>
        /// Клонирование элемента
        /// </summary>
        /// <returns></returns>
        public Punching(bool GenId = false)
        {
            if (GenId) { Id = ProgrammSettings.CurrentId; }
            Children = new ObservableCollection<IHasId>();
            ForcesGroups = new ObservableCollection<ForcesGroup>();
            ForcesGroups.Add(new ForcesGroup(this));
            Center = new Point2D();
        }
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
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
            punching.Children = new ObservableCollection<IHasId>();
            foreach (PunchingLayer layer in Children)
            {
                PunchingLayer newLayer = layer.Clone() as PunchingLayer;
                newLayer.RegisterParent(punching);
            }
            punching.Center = this.Center.Clone() as Point2D;
            return punching;
        }
        /// <summary>
        /// Удалить объект из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void DeleteFromDataSet(DataSet dataSet)
        {
            foreach (ForcesGroup forcesGroup in ForcesGroups)
            {
                forcesGroup.DeleteFromDataSet(dataSet);
            }
            foreach (IHasParent child in Children)
            {
                child.DeleteFromDataSet(dataSet);
            }
            DsOperation.DeleteRow(dataSet, DsOperation.GetTableName(this), Id);
        }
        public double GetArea()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Открывает объект из датасета
        /// </summary>
        /// <param name="dataSet"></param>
        public void OpenFromDataSet(DataSet dataSet)
        {
            DsOperation.OpenEntityFromDataSet(dataSet, this);
        }
        /// <summary>
        /// Открывает объект из датасета
        /// </summary>
        /// <param name="row"></param>
        public void OpenFromDataSet(DataRow row)
        {
            EntityOperation.SetProps(row, this);
            DsOperation.GetFieldsFromRow(row, this);
        }
        /// <summary>
        /// Регистрация родительского элемента
        /// </summary>
        /// <param name="parent"></param>
        public void RegisterParent(IDsSaveable parent)
        {
            if (ParentMember != null) { UnRegisterParent(); }
            else if (!(parent is Level)) throw new Exception($"Parent type is not valid. Element type {GetType().Name}, Id = {Id}, Name={Name}");
            Level level = parent as Level;
            ParentMember = level;
            level.Children.Add(this);
        }
        /// <summary>
        /// Сохранение объекта в датасет
        /// </summary>
        public void SaveToDataSet(DataSet dataSet, bool createNew)
        {
            try
            {
                DataRow row = EntityOperation.SaveEntity(dataSet, createNew, this);
                DsOperation.SetRowFields(row, this);
                row.AcceptChanges();
            }
            catch (Exception ex)
            {
                CommonErrorProcessor.ShowErrorMessage($"Ошибка сохранения элемента {this.GetType().Name}: " + Name, ex);
            }
        }
        /// <summary>
        /// Удаление регистрации на родителе
        /// </summary>
        public void UnRegisterParent()
        {
            Level level = ParentMember as Level;
            level.Children.Remove(this);
            ParentMember = null;
        }
        /// <summary>
        /// Сравнение объектов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Punching)) return false;
            Punching punching = obj as Punching;
            if (punching.Id != this.Id) return false;
            else if (punching.Name != this.Name) return false;
            else if (punching.Width != this.Width) return false;
            else if (punching.Length != this.Length) return false;
            else if (punching.CoveringLayerX != this.CoveringLayerX) return false;
            else if (punching.CoveringLayerY != this.CoveringLayerY) return false;
            int count = this.Children.Count();
            if (count != punching.Children.Count()) return false;
            else
            {
                for (int i=0; i < count; i++)
                {
                    if (!Children[i].Equals(punching.Children[i])) return false;
                }
            }
            return true;
        }

        public double GetPerimeter()
        {
            throw new NotImplementedException();
        }
    }
}
