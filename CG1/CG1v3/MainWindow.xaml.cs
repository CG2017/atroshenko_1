using System;
using System.ComponentModel;
using System.Windows.Media;
using CG1v3.Color;

namespace CG1v3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Color.Color Color { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            Color = new Color.Color();

            ColorModel rgbColorModel = ColorModel.CreateColorModel("rgb");
            ColorModel cmyColorModel = ColorModel.CreateColorModel("cmy");
            ColorModel hsvColorModel = ColorModel.CreateColorModel("hsv");
            ColorModel luvColorModel = ColorModel.CreateColorModel("luv");

            rgbColorModel.Color = Color;
            cmyColorModel.Color = Color;
            hsvColorModel.Color = Color;
            luvColorModel.Color = Color;

            RgbSliders.Model = rgbColorModel;
            CmySliders.Model = cmyColorModel;
            HsvSliders.Model = hsvColorModel;
            LuvSliders.Model = luvColorModel;

            Color.PropertyChanged += ColorOnPropertyChanged;
            luvColorModel.OverflowOccurred += LuvColorModelOnOverflowOccurred;
        }

        private void LuvColorModelOnOverflowOccurred(object sender, OverflowEventArgs args)
        {
            ErrorView.Text = args.ErrorMessage;
            if (args.OverflowBit)
            {
                ErrorView.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
            }
            else
            {
                ErrorView.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
        }

        private void ColorOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            ColorPreview.Fill = new SolidColorBrush(Color.WinColor);
        }
    }
}
