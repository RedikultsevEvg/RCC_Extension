using System;
using System.Collections.Generic;
using System.Drawing;
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
using Point = System.Windows.Point;

namespace EvgRed01
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;
        public MainWindow()
        {
            InitializeComponent();
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;
            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;

            scrollViewer.MouseMove += OnMouseMove;
            slider.ValueChanged += OnSliderValueChanged;
        }
        int width_ = 0;
        int height_ = 0;
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //this.Title = lastDragPoint.HasValue.ToString();
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }
        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            //this.Title = String.Format("X: {0}, Y: {1},  VX: {2}, VY: {3}", mousePos.X, mousePos.Y, scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);
            
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
                slider.Value += 1;
            }
            if (e.Delta < 0)
            {
                slider.Value -= 1;
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

            this.Title = String.Format("X: {0}, Y: {1}", scaleTransform.ScaleX, scaleTransform.ScaleY);

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
        private void loadFile(object sender, RoutedEventArgs e)
        {
            //readonly byte[,,] pixels = new byte[height, width, 4];
            Microsoft.Win32.OpenFileDialog loadF = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            loadF.DefaultExt = ".xml";
            loadF.Filter = "PNG Files (*.png)|*.png|XML Files (*.xml)|*.xml";
            
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
                    ext = filename.Substring(iPoint+1);
                    if (ext == "png")   // Загрузка PNG-картинки
                    {
                        BitmapImage bi = new BitmapImage(new Uri(@filename));
                        WriteableBitmap eb = new WriteableBitmap(bi);
                        mainPict.Source = eb;
                    }

                    if (ext == "xml")       // Загрузка картинки из XML-файла
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(@filename);
                        XmlElement xRoot = xDoc.DocumentElement;
                        XmlNodeList childnodes = xRoot.SelectNodes("PXLS");

                        foreach (XmlNode n in childnodes) // Определяем габариты картинки
                        {
                            int coordX = int.Parse( n.SelectSingleNode("CoordX").InnerText );
                            int coordY = int.Parse( n.SelectSingleNode("CoordY").InnerText );
                            if (coordX > width_)  width_  = coordX;
                            if (coordY > height_) height_ = coordY;
                        }
                        //this.Title = width_.ToString() + "   " + height_.ToString();  // Смотрим ширину и высоту
                        height_++; width_++; // Нормализуем
                        byte[,,] pixels = new byte[width_, height_, 4]; // Подготовительный массив для картинки
                        WriteableBitmap wbitmap = new WriteableBitmap(height_, width_, 96, 96, PixelFormats.Bgra32, null);
                        this.Title = "Begin";
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
                        this.Title = "Pass 1";
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
                        this.Title = "Pass 2";
                        // Update writeable bitmap with the colorArray to the image.
                        Int32Rect rect = new Int32Rect(0, 0, height_, width_);
                        int stride = 4 * height_;
                        wbitmap.WritePixels(rect, pixels1d, stride, 0);
      
                        //Set the Image source.
                        mainPict.Source = wbitmap;
                        this.Title = "mainPict";
                        
                    }
                } 
            }
        }
        private void exitApp(object sender, RoutedEventArgs e)
        {
            this.Close(); // закрытие программы
        }
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
}
