using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDBLL.Common.Service;

namespace RDBLL.Entity.SC.Column.SteelBases.Factories
{
    /// <summary>
    /// Процессор фабрики участков для наиболее характерных типов баз колонн
    /// </summary>
    public static class PartFactProc
    {
        /// <summary>
        /// Добавляет участки стальной базы для паттерна 1
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        /// <param name="baseWidth">Ширина базы</param>
        /// <param name="baseLength">Длина базы</param>
        /// <param name="baseInternalLength">Внутренний размер</param>
        public static void GetPartsType1(SteelBase steelBase, double baseWidth, double baseLength, double baseInternalLength)
        {
            GetPartsType1(steelBase, baseWidth, baseLength, 0, baseInternalLength, false, true);
        }
        /// <summary>
        /// Добавляет участки стальной базы для паттерна 2
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        /// <param name="baseWidth">Ширина базы</param>
        /// <param name="baseLength">Длина базы</param>
        /// <param name="contilever">Консольный свес</param>
        /// <param name="baseInternalLength">Внутренний размер</param>
        /// <param name="edgeX">Флаг наличия ребра вдоль оси X</param>
        /// <param name="edgeY">Флаг наличия ребра вдоль оси Y</param>
        public static void GetPartsType1(SteelBase steelBase, double baseWidth, double baseLength, double contilever, double baseInternalLength, bool edgeX, bool edgeY)
        {
            int partCounter = 1;
            double widthOuterPart = baseWidth / 2;
            double lengthOuterPart = (baseLength - baseInternalLength) / 2;
            double addY = baseInternalLength / 2;
            steelBase.SteelBaseParts = new ObservableCollection<SteelBasePart>();
            SteelBasePart part;
            //Создаем угловые участки
            #region TopLeft
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = -widthOuterPart / 2;
            part.Center.Y = (addY + lengthOuterPart / 2);
            //Если необходимо устанавливать ребро вдоль оси Y
            if (edgeY) part.FixRight = true;
            part.FixBottom = true;
            part.RightOffset = 0.008;
            part.BottomOffset = 0.008;
            #endregion
            #region TopRight
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = widthOuterPart / 2;
            part.Center.Y = (addY + lengthOuterPart / 2);
            //Если необходимо устанавливать ребро вдоль оси Y
            if (edgeY) part.FixLeft = true;
            part.FixBottom = true;
            part.LeftOffset = 0.008;
            part.BottomOffset = 0.008;
            #endregion
            #region BottomLeft
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = -widthOuterPart / 2;
            part.Center.Y = -(addY + lengthOuterPart / 2);
            //Если необходимо устанавливать ребро вдоль оси Y
            if (edgeY) part.FixRight = true;
            part.FixTop = true;
            part.RightOffset = 0.008;
            part.TopOffset = 0.008;
            #endregion
            #region BottomRight
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = widthOuterPart / 2;
            part.Center.Y = -(addY + lengthOuterPart / 2);
            //Если необходимо устанавливать ребро вдоль оси Y
            if (edgeY) part.FixLeft = true;
            part.FixTop = true;
            part.LeftOffset = 0.008;
            part.TopOffset = 0.008;
            #endregion
            //Если вылет консольного участка ,больше 3см, то учитываем его
            if (contilever > 0.03 & edgeX)
            {
                //Создаем консольный участок
                #region Left
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = contilever;
                part.Length = baseInternalLength;
                part.Center.X = -widthOuterPart + contilever / 2;
                part.Center.Y = 0;
                part.FixRight = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.RightOffset = 0;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
                #region Right
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = contilever;
                part.Length = baseInternalLength;
                part.Center.X = widthOuterPart - contilever /2;
                part.Center.Y = 0;
                part.FixLeft = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.LeftOffset = 0.008;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
            }
            //Иначе величину консоли принимаем равной нулю
            else { contilever = 0;}
            //Если необходимо устанавливать ребро вдоль оси X
            //то создаем четыре участка опертые по трем сторонам
            if (edgeX)
            {
                //Создаем участки внутри базы
                #region TopLeft
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = widthOuterPart - contilever;
                part.Length = baseInternalLength / 2;
                part.Center.X = -widthOuterPart / 2 + contilever / 2;
                part.Center.Y = baseInternalLength / 4;
                part.FixRight = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.RightOffset = 0.008;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
                #region TopRight
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = widthOuterPart - contilever;
                part.Length = baseInternalLength / 2;
                part.Center.X = widthOuterPart / 2 - contilever / 2;
                part.Center.Y = baseInternalLength / 4;
                part.FixLeft = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.RightOffset = 0.008;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
                #region BottomLeft
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = widthOuterPart - contilever;
                part.Length = baseInternalLength / 2;
                part.Center.X = -widthOuterPart / 2 + contilever / 2;
                part.Center.Y = -baseInternalLength / 4;
                part.FixRight = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.RightOffset = 0.008;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
                #region BottomRight
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = widthOuterPart - contilever;
                part.Length = baseInternalLength / 2;
                part.Center.X = widthOuterPart / 2 - contilever / 2;
                part.Center.Y = -baseInternalLength / 4;
                part.FixLeft = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.RightOffset = 0.008;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
            }
            //Иначе создаем два участка
            else
            {
                #region MiddleLeft
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = widthOuterPart - contilever;
                part.Length = baseInternalLength;
                part.Center.X = -(widthOuterPart - contilever) / 2;
                part.Center.Y = 0;
                part.FixRight = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.RightOffset = 0.008;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
                #region MiddleRight
                part = new SteelBasePart(steelBase);
                part.Id = ProgrammSettings.CurrentId;
                part.Name = Convert.ToString(partCounter++);
                part.Width = widthOuterPart - contilever;
                part.Length = baseInternalLength;
                part.Center.X = (widthOuterPart - contilever) / 2;
                part.Center.Y = 0;
                part.FixLeft = true;
                part.FixTop = true;
                part.FixBottom = true;
                part.LeftOffset = 0.008;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
                #endregion
            }


        }
        /// <summary>
        /// Добавляет участки стальной базы для паттерна 3
        /// </summary>
        /// <param name="steelBase">База стальной колонны</param>
        /// <param name="baseWidth">Ширина базы</param>
        /// <param name="baseLength">Длина базы</param>
        /// <param name="baseInternalWidth">Внутренняя ширина</param>
        /// <param name="baseInternalLength">Внутрення длина</param>
        /// <param name="edgeX">Флаг наличия ребер вдоль оси X</param>
        public static void GetPartsType3(SteelBase steelBase, double baseWidth, double baseLength, double baseInternalWidth, double baseInternalLength, bool edgeX)
        {
            int partCounter = 1;
            double widthOuterPart = (baseWidth - baseInternalWidth) / 2;
            double lengthOuterPart = (baseLength - baseInternalLength) / 2;
            double addX = baseInternalWidth / 2;
            double addY = baseInternalLength / 2;
            double centerX = addX + widthOuterPart / 2;
            double centerY = addY + lengthOuterPart / 2;
            steelBase.SteelBaseParts = new ObservableCollection<SteelBasePart>();
            SteelBasePart part;
            //Создаем угловые участки
            #region TopLeft
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = - centerX;
            part.Center.Y = centerY;            
             part.FixRight = true;
            //Если необходимо устанавливать ребро вдоль оси X
            if (edgeX) part.FixBottom = true;
            part.RightOffset = 0.008;
            part.BottomOffset = 0.008;
            #endregion
            #region TopRight
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = centerX;
            part.Center.Y = centerY;
            part.FixLeft = true;
            //Если необходимо устанавливать ребро вдоль оси X
            if (edgeX) part.FixBottom = true;
            part.LeftOffset = 0.008;
            part.BottomOffset = 0.008;
            #endregion
            #region BottomLeft
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = -centerX;
            part.Center.Y = -centerY;
            part.FixRight = true;
            //Если необходимо устанавливать ребро вдоль оси X
            if (edgeX) part.FixTop = true;
            part.RightOffset = 0.008;
            part.TopOffset = 0.008;
            #endregion
            #region BottomRight
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = lengthOuterPart;
            part.Center.X = centerX;
            part.Center.Y = -centerY;
            part.FixLeft = true;
            //Если необходимо устанавливать ребро вдоль оси X
            if (edgeX) part.FixTop = true;
            part.LeftOffset = 0.008;
            part.TopOffset = 0.008;
            #endregion
            //Создаем средние участки
            #region TopCenter
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = baseInternalWidth;
            part.Length = lengthOuterPart;
            part.Center.X = 0;
            part.Center.Y = centerY;
            part.FixLeft = true;
            part.FixRight = true;
            part.FixBottom = true;
            part.LeftOffset = 0.008;
            part.RightOffset = 0.008;
            part.BottomOffset = 0.008;
            #endregion
            #region BottomCenter
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = baseInternalWidth;
            part.Length = lengthOuterPart;
            part.Center.X = 0;
            part.Center.Y = - centerY;
            part.FixLeft = true;
            part.FixRight = true;
            part.FixTop = true;
            part.LeftOffset = 0.008;
            part.RightOffset = 0.008;
            part.TopOffset = 0.008;
            #endregion
            #region LeftCenter
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = baseInternalLength;
            part.Center.X = -centerX;
            part.Center.Y = 0;
            part.FixRight = true;
            //Если необходимо устанавливать ребро вдоль оси X
            if (edgeX)
            {
                part.FixTop = true;
                part.FixBottom = true;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
            }
            part.RightOffset = 0.008;
            #endregion
            #region RightCenter
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = widthOuterPart;
            part.Length = baseInternalLength;
            part.Center.X = centerX;
            part.Center.Y = 0;
            part.FixLeft = true;
            //Если необходимо устанавливать ребро вдоль оси X
            if (edgeX)
            {
                part.FixTop = true;
                part.FixBottom = true;
                part.TopOffset = 0.008;
                part.BottomOffset = 0.008;
            }
            part.LeftOffset = 0.008;

            #endregion
            //Создаем центральный участок
            #region LeftCenter
            part = new SteelBasePart(steelBase);
            part.Id = ProgrammSettings.CurrentId;
            part.Name = Convert.ToString(partCounter++);
            part.Width = baseInternalWidth;
            part.Length = baseInternalLength;
            part.Center.X = 0;
            part.Center.Y = 0;
            part.FixLeft = true;
            part.FixRight = true;
            part.FixTop = true;
            part.FixBottom = true;
            part.LeftOffset = 0.008;
            part.RightOffset = 0.008;
            part.TopOffset = 0.008;
            part.BottomOffset = 0.008;
            #endregion
        }
    }
}
