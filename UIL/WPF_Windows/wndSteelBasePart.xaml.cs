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

namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для SteelBasePart.xaml
    /// </summary>
    public partial class WndSteelBasePart : Window
    {
        private SteelBasePart _steelBasePart;
        private SteelBasePart _tmpSteelBasePart;

        public WndSteelBasePart(SteelBasePart steelBasePart)
        {
            InitializeComponent();
            _steelBasePart = steelBasePart;
            #region Перенос значений из класса в контролы
            tbxName.Text = _steelBasePart.Name;
            cbFixLeft.IsChecked = _steelBasePart.FixLeft;
            cbFixRight.IsChecked = _steelBasePart.FixRight;
            cbFixTop.IsChecked = _steelBasePart.FixTop;
            cbFixBottom.IsChecked = _steelBasePart.FixBottom;
            tbxWidth.Text = Convert.ToString(_steelBasePart.Width * 1000);
            tbxLength.Text = Convert.ToString(_steelBasePart.Length * 1000);
            tbxCenterX.Text = Convert.ToString(_steelBasePart.Center[0] * 1000);
            tbxCenterY.Text = Convert.ToString(_steelBasePart.Center[1] * 1000);
            cbAddSymmetricX.IsChecked = _steelBasePart.AddSymmetricX; 
            cbAddSymmetricY.IsChecked = _steelBasePart.AddSymmetricY;
            _tmpSteelBasePart = (SteelBasePart)_steelBasePart.Clone();
            #endregion
            DrawScetch(_tmpSteelBasePart);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _steelBasePart.Name = tbxName.Text;
                _steelBasePart.FixLeft = Convert.ToBoolean(cbFixLeft.IsChecked);
                _steelBasePart.FixRight = Convert.ToBoolean(cbFixRight.IsChecked);
                _steelBasePart.FixTop = Convert.ToBoolean(cbFixTop.IsChecked);
                _steelBasePart.FixBottom = Convert.ToBoolean(cbFixBottom.IsChecked);
                _steelBasePart.Width = Convert.ToDouble(tbxWidth.Text)/1000;
                _steelBasePart.Length = Convert.ToDouble(tbxLength.Text)/1000;
                _steelBasePart.Center[0] = Convert.ToDouble(tbxCenterX.Text) / 1000;
                _steelBasePart.Center[1] = Convert.ToDouble(tbxCenterY.Text) / 1000;
                _steelBasePart.AddSymmetricX = Convert.ToBoolean(cbAddSymmetricX.IsChecked);
                _steelBasePart.AddSymmetricY = Convert.ToBoolean(cbAddSymmetricY.IsChecked);
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

        private void DrawScetch(SteelBasePart steelBasePart)
        {
            double zoom_factor_X = cvScetch.Width / steelBasePart.ColumnBase.Width / 1.2;
            double zoom_factor_Y = cvScetch.Height / steelBasePart.ColumnBase.Length / 1.2;
            double scale_factor;
            double[] columnBaseCenter = new double[2] { cvScetch.Width / 2, cvScetch.Height / 2 };
            if (zoom_factor_X < zoom_factor_Y) { scale_factor = zoom_factor_X; } else { scale_factor = zoom_factor_Y; }
            #region Рисуем прямоугольник для базы
            Rectangle columnBaseRect = new Rectangle();
            columnBaseRect.Width = steelBasePart.ColumnBase.Width * scale_factor;
            columnBaseRect.Height = steelBasePart.ColumnBase.Length * scale_factor;
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
            foreach (SteelBasePart basePart in steelBasePart.ColumnBase.SteelBaseParts)
            {
                SteelBasePart locBasePart = basePart;
                if (basePart.Equals(_steelBasePart)) { locBasePart = _tmpSteelBasePart; }
                DrawBasePart(locBasePart, columnBaseCenter, scale_factor, 1, 1, 0.8, true);
                //Если требуется отражение
                if (locBasePart.AddSymmetricX) { DrawBasePart(locBasePart, columnBaseCenter, scale_factor, 1, -1, 0.6, false); }
                if (locBasePart.AddSymmetricY) { DrawBasePart(locBasePart, columnBaseCenter, scale_factor, -1, 1, 0.6, false); }
                if (locBasePart.AddSymmetricX && basePart.AddSymmetricY) { DrawBasePart(locBasePart, columnBaseCenter, scale_factor, -1, -1, 0.6, false); }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cvScetch.Children.Clear();
            try
            {
                _tmpSteelBasePart.Name = tbxName.Text;
                _tmpSteelBasePart.FixLeft = Convert.ToBoolean(cbFixLeft.IsChecked);
                _tmpSteelBasePart.FixRight = Convert.ToBoolean(cbFixRight.IsChecked);
                _tmpSteelBasePart.FixTop = Convert.ToBoolean(cbFixTop.IsChecked);
                _tmpSteelBasePart.FixBottom = Convert.ToBoolean(cbFixBottom.IsChecked);
                _tmpSteelBasePart.Width = Convert.ToDouble(tbxWidth.Text) / 1000;
                _tmpSteelBasePart.Length = Convert.ToDouble(tbxLength.Text) / 1000;
                _tmpSteelBasePart.Center[0] = Convert.ToDouble(tbxCenterX.Text) / 1000;
                _tmpSteelBasePart.Center[1] = Convert.ToDouble(tbxCenterY.Text) / 1000;
                _tmpSteelBasePart.AddSymmetricX = Convert.ToBoolean(cbAddSymmetricX.IsChecked);
                _tmpSteelBasePart.AddSymmetricY = Convert.ToBoolean(cbAddSymmetricY.IsChecked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
            DrawScetch(_steelBasePart);
        }
    }
}
