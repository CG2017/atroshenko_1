using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LineDrawer.Drawing;

namespace LineDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class DefaultAlgo : IAlgorythm
        {
            public IReadOnlyCollection<Point> GetPixels(int x0, int y0, int x1, int y1)
            {
                return new List<Point>();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            PixelDrawer.Algorythm = new DefaultAlgo();
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IAlgorythm algo = new StepByStepLineDrawingAlgorythm();

            switch ((AlgoSelect.SelectedItem as ComboBoxItem).Content.ToString())
            {
            case "Step by Step algorythm":
                algo = new StepByStepLineDrawingAlgorythm();
                break;
            case "Bresenham Circle":
                algo = new BresenhamCircleAlgorythm();
                break;
            case "DDA line":
                algo = new DdaLineDrawingAlgorythm();
                break;
            case "Bresenham line":
                algo = new BresenhamLineDrawingAlgorythm();
                break;
            }

            PixelDrawer.Algorythm = algo;
        }
    }
}
