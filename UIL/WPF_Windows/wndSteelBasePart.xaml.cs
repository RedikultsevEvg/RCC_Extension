using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RDBLL.Entity.SC.Column;
using System.Collections.ObjectModel;
using RDBLL.Processors.SC;
using Winforms = System.Windows.Forms;

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для SteelBasePart.xaml
    /// </summary>
    public partial class WndSteelBasePart : Window
    {
        private SteelColumnBase _steelColumnBase;

        public WndSteelBasePart(SteelColumnBase steelColumnBase)
        {
            InitializeComponent();
            _steelColumnBase = steelColumnBase;
            DrawScetch(_steelColumnBase);
            this.DataContext = _steelColumnBase;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }
        //метод отрисовки участков базы
        private void DrawBasePart(SteelBasePart basePart, double[] columnBaseCenter, double scale_factor, int koeffX, int koeffY, double opacity, bool showName)
        {
            double[] basePartCenter = new double[2] { cvScetch.Width / 2 + basePart.Center[0] * scale_factor * koeffX, cvScetch.Height / 2 - basePart.Center[1] * scale_factor * koeffY };
            Rectangle basePartRect = new Rectangle();
            basePartRect.Width = basePart.Width * scale_factor;
            basePartRect.Height = basePart.Length * scale_factor;
            basePartRect.Fill = Brushes.LightBlue;
            basePartRect.Opacity = opacity;
            cvScetch.Children.Add(basePartRect);
            Canvas.SetLeft(basePartRect, basePartCenter[0] - basePartRect.Width / 2);
            Canvas.SetTop(basePartRect, basePartCenter[1] - basePartRect.Height / 2);
            Ellipse centerEllipse = new Ellipse();
            centerEllipse.Width = 5;
            centerEllipse.Height = 5;
            centerEllipse.Fill = Brushes.Red;
            centerEllipse.Opacity = opacity;
            cvScetch.Children.Add(centerEllipse);
            Canvas.SetLeft(centerEllipse, basePartCenter[0] - centerEllipse.Width / 2);
            Canvas.SetTop(centerEllipse, basePartCenter[1] - centerEllipse.Height / 2);
            #region Если требуются закрепления по сторонам, рисуем соответствующие линии
            if (basePart.FixLeft)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                cvScetch.Children.Add(borderLeft);
            }
            if (basePart.FixRight)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                cvScetch.Children.Add(borderLeft);
            }
            if (basePart.FixTop)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] - basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                cvScetch.Children.Add(borderLeft);
            }
            if (basePart.FixBottom)
            {
                Line borderLeft = new Line();
                borderLeft.X1 = basePartCenter[0] - basePartRect.Width / 2 * koeffX;
                borderLeft.Y1 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.X2 = basePartCenter[0] + basePartRect.Width / 2 * koeffX;
                borderLeft.Y2 = basePartCenter[1] + basePartRect.Height / 2 * koeffY;
                borderLeft.Stroke = Brushes.Blue;
                borderLeft.StrokeThickness = 2;
                borderLeft.StrokeDashArray = new DoubleCollection { 5, 2 };
                cvScetch.Children.Add(borderLeft);
            }
            #endregion
            //Если требуется указать имя участка, рисуем его
            if (showName)
            {
                TextBlock basePartNameText = new TextBlock();
                basePartNameText.Text = basePart.Name;
                basePartNameText.Foreground = Brushes.Black;
                cvScetch.Children.Add(basePartNameText);
                Canvas.SetLeft(basePartNameText, basePartCenter[0]- basePartNameText.FontSize);
                Canvas.SetTop(basePartNameText, basePartCenter[1] - basePartNameText.FontSize);
            }
        }

        private void DrawScetch(SteelColumnBase steelColumnBase)
        {
            double zoom_factor_X = cvScetch.Width / steelColumnBase.Width / 1.2;
            double zoom_factor_Y = cvScetch.Height / steelColumnBase.Length / 1.2;
            double scale_factor;
            double[] columnBaseCenter = new double[2] { cvScetch.Width / 2, cvScetch.Height / 2 };
            if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
            #region Рисуем прямоугольник для базы
            Rectangle columnBaseRect = new Rectangle();
            columnBaseRect.Width = steelColumnBase.Width * scale_factor;
            columnBaseRect.Height = steelColumnBase.Length * scale_factor;
            columnBaseRect.Fill = Brushes.Gray;
            columnBaseRect.Opacity = 0.3;
            columnBaseRect.Stroke = Brushes.Black;
            columnBaseRect.StrokeThickness = 3;
            cvScetch.Children.Add(columnBaseRect);
            Canvas.SetLeft(columnBaseRect, columnBaseCenter[0] - columnBaseRect.Width / 2);
            Canvas.SetTop(columnBaseRect, columnBaseCenter[1] - columnBaseRect.Height / 2);
            #endregion
            #region Рисуем оси координат
            //Рисуем ось X
            Line axisX = new Line();
            axisX.X1 = columnBaseCenter[0] - columnBaseRect.Width / 2 * 1.2;
            axisX.Y1 = columnBaseCenter[1];
            axisX.X2 = columnBaseCenter[0] + columnBaseRect.Width / 2 * 1.2;
            axisX.Y2 = columnBaseCenter[1];
            axisX.Stroke = Brushes.Red;
            axisX.StrokeThickness = 1;
            axisX.StrokeDashArray = new DoubleCollection { 10, 4 };
            cvScetch.Children.Add(axisX);
            //Рисуем ось Y
            Line axisY = new Line();
            axisY.X1 = columnBaseCenter[0];
            axisY.Y1 = columnBaseCenter[1] - columnBaseRect.Height / 2 * 1.2;
            axisY.X2 = columnBaseCenter[0];
            axisY.Y2 = columnBaseCenter[1] + columnBaseRect.Height / 2 * 1.2;
            axisY.Stroke = Brushes.Red;
            axisY.StrokeThickness = 1;
            axisY.StrokeDashArray = new DoubleCollection { 10, 4 };
            cvScetch.Children.Add(axisY);
            #endregion
            //Рисуем участки
            foreach (SteelBasePart basePart in steelColumnBase.SteelBaseParts)
            {
                foreach (SteelBasePart steelBasePart in steelColumnBase.SteelBaseParts)
                {
                    foreach (SteelBasePart steelBasePartEh in SteelColumnBasePartProcessor.GetSteelBasePartsFromPart(steelBasePart))
                    {
                        DrawBasePart(steelBasePartEh, columnBaseCenter, scale_factor, 1, 1, 0.8, true);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cvScetch.Children.Clear();
            DrawScetch(_steelColumnBase);
        }

        private void BtnAddPart_Click(object sender, RoutedEventArgs e)
        {
            _steelColumnBase.SteelBaseParts.Add(new SteelBasePart(_steelColumnBase));
        }

        private void BtnDeletePart_Click(object sender, RoutedEventArgs e)
        {
            if (lvPartsList.SelectedIndex >= 0)
            {
                Winforms.DialogResult result = Winforms.MessageBox.Show("Элемент будет удален", "Подтверждаете удаление элемента?",
                    Winforms.MessageBoxButtons.YesNo,
                    Winforms.MessageBoxIcon.Information,
                    Winforms.MessageBoxDefaultButton.Button1,
                    Winforms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == Winforms.DialogResult.Yes)
                {
                    int a = lvPartsList.SelectedIndex;
                    if (lvPartsList.Items.Count == 1) lvPartsList.UnselectAll();
                    else if (a < (lvPartsList.Items.Count - 1)) lvPartsList.SelectedIndex = a + 1;
                    else lvPartsList.SelectedIndex = a - 1;
                    _steelColumnBase.SteelBaseParts.RemoveAt(a);
                }
            }
            else
            {
                MessageBox.Show("Ничего не выбрано", "Выберите один из элементов");
            }
        }

        private void StpPartBtns_MouseMove(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 1;
        }

        private void StpPartBtns_MouseLeave(object sender, MouseEventArgs e)
        {
            ((StackPanel)sender).Opacity = 0.5;
        }
    }
}
