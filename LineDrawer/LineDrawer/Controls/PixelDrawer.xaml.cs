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
using LineDrawer.Drawing;

namespace LineDrawer.Controls
{
    /// <summary>
    /// Interaction logic for PixelDrawer.xaml
    /// </summary>
    public partial class PixelDrawer : UserControl
    {
        private int _pixelSize;

        public IAlgorythm Algorythm { get; set; }

        public int PixelSize
        {
            get
            {
                return _pixelSize;
            }
            set
            {
                _pixelSize = value;
                DrawGrid();
            }
        }

        public PixelDrawer()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                DrawGrid();
            };
        }

        public void PutPixel(double x, double y)
        {
            x = Math.Floor(x);
            y = Math.Floor(y);

            var actualX = x * PixelSize;
            var actualY = y * PixelSize;
            var rect = new Rectangle
            {
                Fill = Brushes.Black,
                Width = PixelSize,
                Height = PixelSize
            };

            Canvas.SetLeft(rect, actualX);
            Canvas.SetTop(rect, actualY);

            Canvas.Children.Add(rect);
        }

        private void DrawGrid()
        {
            for (int i = 0; i < Width; i += PixelSize)
            {
                var line = new Line
                {
                    Stroke = Brushes.LightSteelBlue,
                    X1 = i,
                    X2 = i,
                    Y1 = 0,
                    Y2 = Double.IsNaN(Height) ? 0 : Height,
                    StrokeThickness = .1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Canvas.Children.Add(line);
            }
            for (int i = 0; i < Height; i += PixelSize)
            {
                var line = new Line
                {
                    Stroke = Brushes.LightSteelBlue,
                    X1 = 0,
                    X2 = Double.IsNaN(Width) ? 0 : Width,
                    Y1 = i,
                    Y2 = i,
                    StrokeThickness = .1,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };

                Canvas.Children.Add(line);
            }
        }

        private Point? StartPoint { get; set; }
        private Point? EndPoint { get; set; }

        private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StartPoint == null)
            {
                StartPoint = e.GetPosition(Canvas);
                var x = (int)Math.Floor(StartPoint.Value.X / PixelSize);
                var y = (int)Math.Floor(StartPoint.Value.Y / PixelSize);

                Canvas.Children.Clear();
                DrawGrid();

                PutPixel(x, y);
            }
            else
            {
                EndPoint = e.GetPosition(Canvas);
            }

            if (StartPoint == null || EndPoint == null)
            {
                return;
            }

            var line = new Line
            {
                X1 = StartPoint.Value.X,
                X2 = EndPoint.Value.X,
                Y1 = StartPoint.Value.Y,
                Y2 = EndPoint.Value.Y,
                Stroke = Brushes.Red
            };
            var x0 = (int) Math.Floor(StartPoint.Value.X / PixelSize);
            var y0 = (int) Math.Floor(StartPoint.Value.Y / PixelSize);
            var x1 = (int) Math.Floor(EndPoint.Value.X / PixelSize);
            var y1 = (int) Math.Floor(EndPoint.Value.Y / PixelSize);
            var pixels = Algorythm.GetPixels(x0, y0, x1, y1);

            foreach (var pixel in pixels)
            {
                PutPixel(pixel.X, pixel.Y);
            }

            Canvas.Children.Add(line);

            this.Label.Content = string.Format("x0: {0}; y0: {1}; x1: {2}; y1:{3}",
                x0, y0, x1, y1);

            Canvas.Children.Add(this.Label);

            StartPoint = null;
            EndPoint = null;
        }
    }
}
