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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media.Animation;

using Brush = System.Windows.Media.Brush;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;
using System.Globalization;
using System.Collections.ObjectModel;
using DragDropObjects;

namespace EvgRed01
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;

        private bool _isDown;
        private bool _isDragging;
        //private Canvas _myCanvas;
        private UIElement _originalElement;
        private double _originalLeft;
        private double _originalTop;
        private DragDropObjects.SimpleCircleAdorner _overlayElement;
        private Point _startPoint;

        public MainWindow()
        {
            InitializeComponent();
            sTitle = this.Title;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.MouseRightButtonDown += OnMouseRightButtonDown;
            // scrollViewer.MouseRightButtonUp += OnMouseRightButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;
            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.MouseLeave += OnMouseLeave;
            scrollViewer.MouseMove += OnMouseMove;
            slider.ValueChanged += OnSliderValueChanged;

            Canvas.SetTop(menuAdd, 10);
            Canvas.SetLeft(menuAdd, 10);

            _myCanvas.PreviewMouseRightButtonDown += MyCanvas_PreviewMouseRightButtonDown;
            _myCanvas.PreviewMouseMove += MyCanvas_PreviewMouseMove;
            _myCanvas.PreviewMouseRightButtonUp += MyCanvas_PreviewMouseRightButtonUp;
            PreviewKeyDown += _PreviewKeyDown;

        }
        int width_ = 0;
        int height_ = 0;
        string sTitle;
        public WriteableBitmap wbitmap;
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            this.Title = sTitle;
        }
        private void OnMouseRightButtonDown(object sender, MouseEventArgs e)
        {

            Point posNow = e.GetPosition(scrollViewer);
            this.Title = "X: " + Math.Round(posNow.X).ToString() + " Y: " + Math.Round(posNow.Y).ToString();

            // Горизонтальная линия
            this.lineH.X1 = 0;
            this.lineH.Y1 = posNow.Y;
            if (this.lineH.Y1 < 0) this.lineH.Y1 = 0;
            this.lineH.X2 = width_*slider.Value;
            if (this.lineH.X2 > scrollViewer.Width) this.lineH.X2 = scrollViewer.Width;
            this.lineH.Y2 = this.lineH.Y1;

            this.lineH.Stroke = Brushes.Cyan;
            this.lineH.StrokeThickness = 1;
            this.lineH.Visibility = Visibility.Visible;

            // Вертикальная линия
            this.lineV.X1 = posNow.X;
            this.lineV.Y1 = 0;
            if (this.lineV.X1 < 0) this.lineV.X1 = 0;
            this.lineV.X2 = posNow.X;
            this.lineV.Y2 = height_ * slider.Value;
            if (this.lineV.Y2 > scrollViewer.Height) this.lineV.Y2 = scrollViewer.Height;

            this.lineV.Stroke = Brushes.Cyan;
            this.lineV.StrokeThickness = 1;
            this.lineV.Visibility = Visibility.Visible;

            this.menuAdd.Visibility = Visibility.Visible;
        }
        private void exitAdd(object sender, RoutedEventArgs e)
        {
            this.lineH.Visibility = Visibility.Hidden;
            this.lineV.Visibility = Visibility.Hidden;
            this.menuAdd.Visibility = Visibility.Hidden;
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point posNow = e.GetPosition(scrollViewer);
            if (lastDragPoint.HasValue)
            {
                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }

            this.Title = mainPict.ActualHeight.ToString() + "  :  " + mainPict.ActualWidth.ToString();
        }
        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            this.Title = String.Format("X: {0}, Y: {1},  VX: {2}, VY: {3}", mousePos.X, mousePos.Y, scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);

            if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y < scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }

        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(grid);

            if (e.Delta > 0)
            {
                slider.Value += .1;
            }
            if (e.Delta < 0)
            {
                slider.Value -= .1;
            }
            if (this.lineH.IsVisible)
            {
                this.lineH.X2 = width_ * slider.Value;
                if (this.lineH.X2 > scrollViewer.Width) this.lineH.X2 = scrollViewer.Width;
                this.lineV.Y2 = height_ * slider.Value;
                if (this.lineV.Y2 > scrollViewer.Height) this.lineV.Y2 = scrollViewer.Height;
            }
            this.Title = "slider.Value: " + slider.Value.ToString();
            e.Handled = true;
        }
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Hand;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scaleTransform.ScaleX = e.NewValue;
            scaleTransform.ScaleY = e.NewValue;

            //this.Title = String.Format("X: {0}, Y: {1}", scaleTransform.ScaleX, scaleTransform.ScaleY);

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid);
        }

        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, grid);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(grid);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / grid.Width;
                    double multiplicatorY = e.ExtentHeight / grid.Height;

                    double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }
        // ---------------------------  Перетаскивание меню   ------------------------------
        private void _PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _isDragging)
            {
                DragFinished(true);
            }
        }

        private void MyCanvas_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDown)
            {
                DragFinished(false);
                e.Handled = true;
            }
        }

        private void DragFinished(bool cancelled)
        {
            Mouse.Capture(null);
            if (_isDragging)
            {
                AdornerLayer.GetAdornerLayer(_overlayElement.AdornedElement).Remove(_overlayElement);

                if (cancelled == false)
                {
                    Canvas.SetTop(_originalElement, _originalTop + _overlayElement.TopOffset);
                    Canvas.SetLeft(_originalElement, _originalLeft + _overlayElement.LeftOffset);
                }
                _overlayElement = null;
            }
            _isDragging = false;
            _isDown = false;
        }

        private void MyCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(_myCanvas).X - _startPoint.X) >
                      SystemParameters.MinimumHorizontalDragDistance) ||
                     (Math.Abs(e.GetPosition(_myCanvas).Y - _startPoint.Y) >
                      SystemParameters.MinimumVerticalDragDistance)))
                {
                    DragStarted();
                }
                if (_isDragging)
                {
                    DragMoved();
                }
            }
        }

        private void DragStarted()
        {
            _isDragging = true;
            _originalLeft = Canvas.GetLeft(_originalElement);
            _originalTop = Canvas.GetTop(_originalElement);

            _overlayElement = new SimpleCircleAdorner(_originalElement);
            var layer = AdornerLayer.GetAdornerLayer(_originalElement);
            layer.Add(_overlayElement);
        }

        private void DragMoved()
        {
            var currentPosition = Mouse.GetPosition(_myCanvas);

            _overlayElement.LeftOffset = currentPosition.X - _startPoint.X;
            _overlayElement.TopOffset = currentPosition.Y - _startPoint.Y;
        }

        private void MyCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == _myCanvas)
            {
            }
            else
            {
                _myCanvas.Cursor = Cursors.SizeAll;
                _isDown = true;
                _startPoint = e.GetPosition(_myCanvas);
                _originalElement = e.Source as UIElement;
                _myCanvas.CaptureMouse();
                e.Handled = true;
            }
        }

        //----------------------------------------------------------------------------------
        private void loadFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog loadF = new Microsoft.Win32.OpenFileDialog();
            loadF.DefaultExt = ".txt";
            loadF.Filter = "TXT Files (*.txt)|*.txt|PNG Files (*.png)|*.png|XML Files (*.xml)|*.xml";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = loadF.ShowDialog();

            // Get the selected file name
            if (result == true)
            {
                // Open document
                int iPoint = 0;
                string ext = " ";
                string filename = loadF.FileName;
                if (filename.Length > 0)
                {
                    iPoint = filename.LastIndexOf(".");
                    ext = filename.Substring(iPoint + 1);
                    if (ext == "txt")
                    {
                       //  int counter = 0;
                        string line;
                        height_ = 0;
                        width_ = 0;
                        List<decimal> dPlots = new List<decimal>();
                        List<upCode> up_Code = new List<upCode>();

                        ObservableCollection<PictFromTXT> fromTXT = new ObservableCollection<PictFromTXT>();
                        System.IO.StreamReader fileTxt = new System.IO.StreamReader(@filename);
                        while ((line = fileTxt.ReadLine()) != null)
                        {
                            if (line.IndexOf("X") < 0)
                            {
                                line = line.ToString().Trim();
                                int position = line.IndexOf("	"); // Тут не пробел, а какой-то спецзнак

                                int iNumb = Convert.ToInt32(line.Substring(0, position));
                                line = line.Substring(position).Trim();
                                position = line.IndexOf("	");
                                int iX = Convert.ToInt32(line.Substring(0, position));

                                iX--;

                                line = line.Substring(position).Trim();
                                position = line.IndexOf("	");
                                int iY = Convert.ToInt32(line.Substring(0, position));
                                line = line.Substring(position).Trim();
                                position = line.IndexOf("	");
                                decimal dZ;
                                if (position > 0)
                                {
                                    dZ = Convert.ToDecimal(line.Substring(0, position), CultureInfo.InvariantCulture);
                                } else {
                                    dZ = Convert.ToDecimal(line.ToString(), CultureInfo.InvariantCulture);
                                }
                                
                                if (iX > width_) width_ = iX;
                                if (iY > height_) height_ = iY;
                                if (dPlots.IndexOf(dZ) < 0 ) dPlots.Add(dZ);

                                PictFromTXT lineFromTXT = new PictFromTXT { Numb = iNumb, CoordX = iX, CoordY = iY, Plot = dZ, colorPix = 0 };
                                fromTXT.Add(lineFromTXT);
                            }
                        }
                        // MessageBox.Show(width_.ToString() + ":" + height_.ToString());
                        fileTxt.Close();

                        int operColors = Convert.ToInt32(txtNum.Text);
                        int kolColors = dPlots.Count;
                        dPlots.Sort();
                        // Определяем количество цветовых интервалов
                        // назначаем им цвета
                        if (kolColors > operColors) kolColors = operColors;
                        // Величины интервалов
                        long colorInterval = 16776600 / kolColors;
                        int dInterval = dPlots.Count / kolColors;
                        int k = 0;

                        this.Title = "kolColors: " + kolColors.ToString() + "; colorInterval" +
                            colorInterval.ToString() + "; dInterval: " + dInterval.ToString();
                       
                        for ( long i = 180; i <= 16776600; i+=colorInterval )
                        {
                            int iR = (int) i/65536;
                            int iG = (int)(i - iR * 65536) / 255;
                            int iB = (int) i - iR * 65536 - iG * 255;
                            int l = k + dInterval;
                            for (int j = k; j <= l; j++)
                            {
                                if (j < dPlots.Count)
                                {
                                    up_Code.Add( new upCode { dColor=(double)dPlots[j], bColorB=(byte)iB,
                                        bColorG=(byte)iG, bColorR=(byte)iR } );
                                    //escButton.Content = dPlots[j].ToString();
                                }
                                k = j;
                            }
                            // Добавление кнопки для управления цветом
                            Button bColor = new Button();
                            bColor.Background = new SolidColorBrush(Color.FromArgb((byte)255, (byte)iR, (byte)iG, (byte)iB));
                            bColor.Width = 45;
                            bColor.Height = 20;
                            listColors.Items.Add(bColor);

                        }

                        width_++; height_++;
                        byte[,,] pixels = new byte[width_, height_, 4]; // Подготовительный массив для картинки
                        fileTxt = new System.IO.StreamReader(@filename);
                        while ((line = fileTxt.ReadLine()) != null)
                        {
                            if (line.IndexOf("X") < 0)
                            {
                                line = line.ToString().Trim();
                                int position = line.IndexOf("	"); // Тут не пробел, а какой-то спецзнак

                                int iNumb = Convert.ToInt32(line.Substring(0, position));
                                line = line.Substring(position).Trim();
                                position = line.IndexOf("	");
                                int iX = Convert.ToInt32(line.Substring(0, position));
                                iX--;
                                line = line.Substring(position).Trim();
                                position = line.IndexOf("	");
                                int iY = Convert.ToInt32(line.Substring(0, position));
                                line = line.Substring(position).Trim();
                                position = line.IndexOf("	");
                                decimal dZ;
                                if (position > 0)
                                {
                                    dZ = Convert.ToDecimal(line.Substring(0, position), CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    dZ = Convert.ToDecimal(line.ToString(), CultureInfo.InvariantCulture);
                                }
                                var listItem = up_Code.Where(li => li.dColor == (double)dZ).ToList();
                                                            
                                foreach (var element in listItem)
                                {
                                    pixels[iX, iY, 0] = element.bColorB; // Blue
                                    pixels[iX, iY, 1] = element.bColorG; // Green
                                    pixels[iX, iY, 2] = element.bColorR; // Red
                                    pixels[iX, iY, 3] = 255;
                                }
                            }

                        }

                        byte[] pixels1d = new byte[width_ * height_ *  4];
                        int index = 0;
                        for (int row = 0; row < width_; row++)
                        {
                            for (int col = 0; col < height_; col++)
                            {
                                for (int i = 0; i < 4; i++)
                                    pixels1d[index++] = pixels[row, col, i];
                            }
                        }

                        wbitmap = new WriteableBitmap(height_, width_, 96, 96, PixelFormats.Bgra32, null);
                        // Update writeable bitmap with the colorArray to the image.
                        Int32Rect rect = new Int32Rect(0, 0, width_, height_);
                        int stride = 4 * width_;
                        wbitmap.WritePixels(rect, pixels1d, stride, 0);

                        //Set the Image source.
                        mainPict.Source = wbitmap;
                    }

                    if (ext == "png")   // Загрузка PNG-картинки
                    {
                        BitmapImage bi = new BitmapImage(new Uri(@filename));
                        wbitmap = new WriteableBitmap(bi);
                        height_ = wbitmap.PixelHeight;
                        width_ = wbitmap.PixelWidth;

                        mainPict.Width = width_;
                        mainPict.Height = height_;
                        mainPict.Source = wbitmap;
                    }

                    if (ext == "xml")       // Загрузка картинки из XML-файла
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(@filename);
                        XmlElement xRoot = xDoc.DocumentElement;
                        XmlNodeList childnodes = xRoot.SelectNodes("PXLS");

                        foreach (XmlNode n in childnodes) // Определяем габариты картинки
                        {
                            int coordX = int.Parse(n.SelectSingleNode("CoordX").InnerText);
                            int coordY = int.Parse(n.SelectSingleNode("CoordY").InnerText);
                            if (coordX > width_) width_ = coordX;
                            if (coordY > height_) height_ = coordY;
                        }
                        height_++; width_++; // Нормализуем
                        byte[,,] pixels = new byte[width_, height_, 4]; // Подготовительный массив для картинки
                        wbitmap = new WriteableBitmap(height_, width_, 96, 96, PixelFormats.Bgra32, null);
                        foreach (XmlNode n in childnodes)
                        {
                            int coordX = int.Parse(n.SelectSingleNode("CoordX").InnerText);
                            int coordY = int.Parse(n.SelectSingleNode("CoordY").InnerText);
                            byte colorB = byte.Parse(n.SelectSingleNode("ColorB").InnerText);
                            byte colorG = byte.Parse(n.SelectSingleNode("ColorG").InnerText);
                            byte colorR = byte.Parse(n.SelectSingleNode("ColorR").InnerText);
                            byte transp = byte.Parse(n.SelectSingleNode("Transparency").InnerText);
                            pixels[coordX, coordY, 0] = colorB;              // Blue
                            pixels[coordX, coordY, 1] = colorG;             // Green
                            pixels[coordX, coordY, 2] = colorR;               // Red
                            pixels[coordX, coordY, 3] = transp;                // Transparency Прозрачность
                        }
                        
                        byte[] pixels1d = new byte[height_ * width_ * 4];
                        int index = 0;
                        for (int row = 0; row < width_; row++)
                        {
                            for (int col = 0; col < height_; col++)
                            {
                                for (int i = 0; i < 4; i++)
                                    pixels1d[index++] = pixels[row, col, i];
                            }
                        }

                        // Update writeable bitmap with the colorArray to the image.
                        Int32Rect rect = new Int32Rect(0, 0, height_, width_);
                        int stride = 4 * height_;
                        wbitmap.WritePixels(rect, pixels1d, stride, 0);

                        //Set the Image source.
                        mainPict.Source = wbitmap;

                    }
                }
            }
        }
        private void exitApp(object sender, RoutedEventArgs e)
        {
            this.Close(); // закрытие программы
        }
        private void plotH(object sender, RoutedEventArgs e)
        {

            this.Title = this.lineH.Y1.ToString();
            Window1 winPlot = new Window1();
            winPlot.lineH.X1 = 0;
            winPlot.lineH.Y1 = 10;
            winPlot.lineH.X2 = width_;
            winPlot.lineH.Y2 = 10;
            winPlot.lineH.Stroke = Brushes.Black;
            winPlot.lineH.StrokeThickness = 3;
            winPlot.lineH.Visibility = Visibility.Visible;
            winPlot.Title = width_.ToString() + "   " + height_.ToString();

            winPlot.Show();

            int stride = 4 * (int)(mainPict.ActualWidth);
            int arrayLength = stride * (int)mainPict.ActualHeight;

            byte[] pixels1d1 = new byte[arrayLength];
            this.Title = stride.ToString();
            wbitmap.CopyPixels(pixels1d1, stride, 0);

            Polygon plot = new Polygon();
            PointCollection plot_Points = new PointCollection();
            Point point1 = new Point(0,10);
            plot_Points.Add(point1);
            for (int iX = 0; iX <= (int)mainPict.ActualWidth; iX++)
            {
                winPlot.Title = "";
                // iZ - Определяем начало описания цвета пикселя
                // Количество полностью заполненных строк iY * 4 * на ширину строки плюс смещение по X это iX*4
                // Вычисляем цвет
                int iZ = 4 * (int)mainPict.Source.Width  * (int)this.lineH.Y1 + iX * 4;
                int iY = ((int)pixels1d1[iZ]*4 + (int)pixels1d1[iZ + 1]*2 + (int)pixels1d1[iZ+2])/4;
                point1 = new Point(iX, iY);
                plot_Points.Add(point1);
            }
            point1 = new Point((int)mainPict.ActualWidth, 10);
            plot_Points.Add(point1);

            plot.Points = plot_Points;
            plot.Fill = Brushes.LightCoral;
            winPlot.gridPlot.Children.Add(plot);
            
        }
        private void plotV(object sender, RoutedEventArgs e)
        {

        }
        //-----------------------------------------------------------------------
        private int _numValue = 16;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue>0) NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!int.TryParse(txtNum.Text, out _numValue))
                txtNum.Text = _numValue.ToString();
        }
        //-----------------------------------------------------------------------
    }
    public class PXLS
    {
        public int CoordX { get; set; }     // Координата Х (на прямоугольнике)
        public int CoordY { get; set; }     // Координата Y (там же)
        public byte ColorB { get; set; }    // Цвет точки - синяя составляющая
        public byte ColorG { get; set; }    // Цвет точки - зеленая составляющая
        public byte ColorR { get; set; }    // Цвет точки - красная составляющая
        public byte Transp { get; set; }    // Прозрачность точки от 0 до 255 ****  255 - 100% непрозрачности

    }
    public class PictConcreteArea
    {
        public double CoordX { get; set; }     // Координата Х (на прямоугольнике)
        public double CoordY { get; set; }     // Координата Y (там же)
        public double colorPix { get; set; } // Цвет точки
    }
    public class PictFromTXT
    {
        public double Numb { get; set; }                   // номер строки в файле
        public double CoordX { get; set; }     // Координата Х (на прямоугольнике)
        public double CoordY { get; set; }     // Координата Y (там же)
        public decimal Plot { get; set; }     // величина из файла 
        public double colorPix { get; set; } // Цвет точки
    }
    public class plotPoints
    {
        public int pointX { get; set; }
        public int pointY { get; set; }
    }
    public class upCode
    {
        public double dColor { get; set; }
        public byte bColorB { get; set; }
        public byte bColorG { get; set; }
        public byte bColorR { get; set; }
    }
        class GridAdorner : Adorner
    {
        FrameworkElement elem;
        public System.Drawing.Brush Pen { get; set; }
        public GridAdorner(FrameworkElement elem) : base(elem)
        {
            this.elem = elem;
        }
        protected override void OnRender(DrawingContext dc)
        {
            double wid = elem.ActualWidth;
            double hig = elem.ActualHeight;
            int size = ExtAdorner.GetSize(elem);
            Brush penbrush = ExtAdorner.GetPenBrush(elem);
            Pen pen = new Pen(penbrush, 0.25);

            for (int i = 0; i < wid; i += size)
                dc.DrawLine(pen, new Point(i, 0), new Point(i, hig));
            for (int i = 0; i < hig; i += size)
                dc.DrawLine(pen, new Point(0, i), new Point(wid, i));
        }
    }

    class ExtAdorner : FrameworkElement
    {
        public static int GetSize(DependencyObject obj)
        {
            return (int)obj.GetValue(SizeProperty);
        }
        public static void SetSize(DependencyObject obj, int value)
        {
            obj.SetValue(SizeProperty, value);
        }
        public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached("Size", typeof(int), typeof(GridAdorner), new PropertyMetadata(0, onsizechanged));
        private static void onsizechanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement elem = d as FrameworkElement;
            elem.Loaded += (s, arg) =>
            {
                elem = s as FrameworkElement;
                GridAdorner ga = new GridAdorner(elem);
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(elem);
                layer.Add(ga);
            };
        }
        public static Brush GetPenBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PenBrushProperty);
        }
        public static void SetPenBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PenBrushProperty, value);
        }
        public static readonly DependencyProperty PenBrushProperty = DependencyProperty.RegisterAttached("PenBrush", typeof(Brush), typeof(ExtAdorner), new PropertyMetadata(Brushes.Red));
    }

    class JumperItem : ContentControl
    {
        TranslateTransform move;
        public int JSpeed { get; set; } // ms top-down
        public int JDelayStart { get; set; }// ms delay before start
        public string JText { get; set; }
        public JumperItem() : base()
        {
            move = new TranslateTransform();
            RenderTransform = move;

            Loaded += delegate
            {
                if (JText != null)
                    Content = new TextBlock() { Text = JText };
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!DesignerProperties.GetIsInDesignMode(this))
                        IsRunning = true;
                }), null);
            };
        }
        // амплитуда зависит только от высоты РОДИТЕЛЬСКОГО КОНТЕЙНЕРА
        // для смнхронизации коллекции элементов они должны иметь ОДИНАКОВУЮ высоту
        void run()
        {
            FrameworkElement parent = Parent as FrameworkElement;
            double from = 0, to = 0, dy = parent.ActualHeight - ActualHeight;

            if (VerticalAlignment == VerticalAlignment.Bottom)
                to = -dy;

            if (VerticalAlignment == VerticalAlignment.Top)
                to = dy;

            DoubleAnimation da = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(JSpeed));
            da.AutoReverse = true;
            da.RepeatBehavior = RepeatBehavior.Forever;
            da.BeginTime = TimeSpan.FromMilliseconds(JDelayStart);
            move.BeginAnimation(TranslateTransform.YProperty, da);
        }
        void stop()
        {
            move.BeginAnimation(TranslateTransform.YProperty, null);
        }
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }
        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning", typeof(bool), typeof(JumperItem), new PropertyMetadata(false, isrunningchanged));
        private static void isrunningchanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            JumperItem jt = d as JumperItem;
            if ((bool)e.NewValue)
                jt.run();
            else jt.stop();
        }

        //----------------------------------------------------------------------------------

    }


}
