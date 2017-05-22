using System;
using System.Collections.Generic;
using System.IO;
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
using Drawing = System.Drawing;

namespace PictureWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        BitmapSource inputImage;
        Drawing.Bitmap sourceBitmap;
        Drawing.Bitmap targetBitmap;

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";

            if (result == true)
            {
                filename = dlg.FileName;
                fileNameLabel.Content = filename;
            }
            var url = new Uri(filename);
            inputImage = new BitmapImage(url);
            this.sourceImage.Source = inputImage;
            BitmapImage2Bitmap(inputImage);
        }

        private void BitmapImage2Bitmap(BitmapSource bitmapImage)
        {
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                sourceBitmap = new Drawing.Bitmap(outStream);
            }
            width = sourceBitmap.Width;
            height = sourceBitmap.Height;
            targetBitmap = new Drawing.Bitmap(sourceBitmap);
            this.targetImage.Source = GetImage(targetBitmap);
        }

        private BitmapImage GetImage(Drawing.Bitmap image)
        {
            BitmapImage bmp = new BitmapImage();
            MemoryStream mem = new MemoryStream();
            image.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
            mem.Position = 0;
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.StreamSource = mem;
            bmp.DecodePixelWidth = width;
            bmp.DecodePixelHeight = height;
            bmp.EndInit();
            return bmp;
        }

        double L1, u1, v1;

        double radius = 50;

        double D = 0;

        double L0 = 0, u0 = 0, v0 = 0;
        int R0, G0, B0;

        Drawing.Color c;

        int width = 0;
        int height = 0;


        void ConvertColor()
        {
            targetBitmap = new Drawing.Bitmap(sourceBitmap);

            fromRGBtoLuv(colorR_old, colorG_old, colorB_old);

            L1 = colorL;
            u1 = coloru;
            v1 = colorv;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    c = sourceBitmap.GetPixel(x, y);

                    R0 = (int)c.R;
                    G0 = (int)c.G;
                    B0 = (int)c.B;

                    fromRGBtoLuv(R0, G0, B0);
                    L0 = colorL;
                    u0 = coloru;
                    v0 = colorv;

                    D = Math.Sqrt(Math.Pow((L1 - L0), 2) + Math.Pow((u1 - u0), 2) + Math.Pow((v1 - v0), 2));

                    if (D < radius)
                    {
                        targetBitmap.SetPixel(x, y, Drawing.Color.FromArgb(c.A, check(colorR_new, (colorR_old - R0)), check(colorG_new, (colorG_old - G0)), check(colorB_new, (colorB_old - B0))));
                    }
                }
            }
        }

        int tmp = 0;

        public int check(int val1, int val2)
        {
            if (val1 < val2)
            {
                return 0;
            }
            else
            {
                tmp = val1 - val2;
                if (tmp > 255)
                {
                    return 255;
                }
                else
                {
                    return tmp;
                }
            }
        }

        public bool isRightRGB(int colorRed, int colorGreen, int colorBlue)
        {
            if (!colorRed.Equals("") && colorRed <= 255 && colorRed >= 0 &&
                !colorGreen.Equals("") && colorGreen <= 255 && colorGreen >= 0 &&
                !colorBlue.Equals("") && colorBlue <= 255 && colorBlue >= 0)
            {
                return true;
            }
            return false;
        }


        int colorR_old = 0;
        int colorG_old = 0;
        int colorB_old = 0;

        int colorR_new = 0;
        int colorG_new = 0;
        int colorB_new = 0;

        public double F(double s)
        {
            if (s >= 0 && s < 0.008856)
            {
                return 7.787 * s + 16.0 / 116.0;
            }
            else
            {
                return Math.Pow(s, 1.0 / 3.0);
            }
        }
        
        double color_X = 0.0;
        double color_Y = 0.0;
        double color_Z = 0.0;

        const double Xn = 0.4124564 + 0.3575761 + 0.1804375;
        const double Yn = 0.2126729 + 0.7151522 + 0.0721750;
        const double Zn = 0.0193339 + 0.1191920 + 0.9503041;


        double un = 4.0 * (0.4124564 + 0.3575761 + 0.1804375) /
                  (double)((0.4124564 + 0.3575761 + 0.1804375) + 
                   15.0 * (0.2126729 + 0.7151522 + 0.0721750) +
                    3.0 * (0.0193339 + 0.1191920 + 0.9503041));
        
        double vn = 9.0 * (0.2126729 + 0.7151522 + 0.0721750) / 
                  (double)((0.4124564 + 0.3575761 + 0.1804375) +
                   15.0 * (0.2126729 + 0.7151522 + 0.0721750) +
                    3.0 * (0.0193339 + 0.1191920 + 0.9503041));
        

        public void fromRGBtoXYZ(int colorRed, int colorGreen, int colorBlue)
        {
            color_X = 0.4124564 * (colorRed / 255.0) + 0.3575761 * (colorGreen / 255.0) + 0.1804375 * (colorBlue / 255.0);
            color_Y = 0.2126729 * (colorRed / 255.0) + 0.7151522 * (colorGreen / 255.0) + 0.0721750 * (colorBlue / 255.0);
            color_Z = 0.0193339 * (colorRed / 255.0) + 0.1191920 * (colorGreen / 255.0) + 0.9503041 * (colorBlue / 255.0);
        }

        double colorL = 0;
        double coloru = 0;
        double colorv = 0;

        public void fromXYZtoLuv()
        {
            double whiteP = color_Y / Yn;

            colorL = 116.0 * F(whiteP) - 16.0;

            double uk = 0, vk = 0;

            if (color_X != 0)
            {
                uk = 4.0 * color_X / (double)(color_X + 15.0 * color_Y + 3.0 * color_Z);
            }
            if (color_Y != 0)
            {
                vk = 9.0 * color_Y / (double)(color_X + 15.0 * color_Y + 3.0 * color_Z);
            }

            coloru = 13 * colorL * (uk - un);
            colorv = 13 * colorL * (vk - vn);
        }

        public void fromRGBtoLuv(int colorRed, int colorGreen, int colorBlue)
        {
            fromRGBtoXYZ(colorRed, colorGreen, colorBlue);

            fromXYZtoLuv();
        }

        SolidColorBrush oldBrush = new SolidColorBrush();

        private void TextBox_TextChanged_R_old(object sender, TextChangedEventArgs e)
        {
            sliderR_old.Value = colorR_old;
            if (textBoxR_old.IsFocused || sliderR_old.IsFocused)
            {
                var textBox = sender as TextBox;

                if (isRightRGB(Int32.Parse(textBox.Text), colorG_old, colorB_old))
                {
                    colorR_old = Int32.Parse(textBox.Text);
                    oldBrush.Color = Color.FromRgb((Byte)colorR_old, (Byte)colorG_old, (Byte)colorB_old);
                    colorOld.Background = oldBrush;
                }
            }
        }

        private void Slider_ValueChanged_R_old(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorR_old = (int)e.NewValue;
            textBoxR_old.Text = colorR_old.ToString();
        }

        private void TextBox_TextChanged_G_old(object sender, TextChangedEventArgs e)
        {
            sliderG_old.Value = colorG_old;
            if (textBoxG_old.IsFocused || sliderG_old.IsFocused)
            {
                var textBox = sender as TextBox;

                if (isRightRGB(colorR_old, Int32.Parse(textBox.Text), colorB_old))
                {
                    colorG_old = Int32.Parse(textBox.Text);
                    oldBrush.Color = Color.FromRgb((Byte)colorR_old, (Byte)colorG_old, (Byte)colorB_old);
                    colorOld.Background = oldBrush;
                }
            }
        }

        private void Slider_ValueChanged_G_old(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorG_old = (int)e.NewValue;
            textBoxG_old.Text = colorG_old.ToString();
        }

        private void TextBox_TextChanged_B_old(object sender, TextChangedEventArgs e)
        {
            sliderB_old.Value = colorB_old;
            if (textBoxB_old.IsFocused || sliderB_old.IsFocused)
            {
                var textBox = sender as TextBox;

                if (isRightRGB(colorR_old, colorG_old, Int32.Parse(textBox.Text)))
                {
                    colorB_old = Int32.Parse(textBox.Text);
                    oldBrush.Color = Color.FromRgb((Byte)colorR_old, (Byte)colorG_old, (Byte)colorB_old);
                    colorOld.Background = oldBrush;
                }
            }
        }

        private void Slider_ValueChanged_B_old(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorB_old = (int)e.NewValue;
            textBoxB_old.Text = colorB_old.ToString();
        }

        SolidColorBrush newBrush = new SolidColorBrush();
        private void TextBox_TextChanged_R_new(object sender, TextChangedEventArgs e)
        {
            sliderR_new.Value = colorR_new;
            if (textBoxR_new.IsFocused || sliderR_new.IsFocused)
            {
                var textBox = sender as TextBox;

                if (isRightRGB(Int32.Parse(textBox.Text), colorG_new, colorB_new))
                {
                    newBrush.Color = Color.FromRgb((Byte)colorR_new, (Byte)colorG_new, (Byte)colorB_new);
                    colorNew.Background = newBrush;
                }
            }
        }

        private void Slider_ValueChanged_R_new(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorR_new = (int)e.NewValue;
            textBoxR_new.Text = colorR_new.ToString();
        }

        private void TextBox_TextChanged_G_new(object sender, TextChangedEventArgs e)
        {
            sliderG_new.Value = colorG_new;
            if (textBoxG_new.IsFocused || sliderG_new.IsFocused)
            {
                var textBox = sender as TextBox;

                if (isRightRGB(colorR_new, Int32.Parse(textBox.Text), colorB_new))
                {
                    
                    colorG_new = Int32.Parse(textBox.Text);
                    newBrush.Color = Color.FromRgb((Byte)colorR_new, (Byte)colorG_new, (Byte)colorB_new);
                    colorNew.Background = newBrush;
                }
            }
        }

        private void Slider_ValueChanged_G_new(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorG_new = (int)e.NewValue;
            textBoxG_new.Text = colorG_new.ToString();
        }

        private void TextBox_TextChanged_B_new(object sender, TextChangedEventArgs e)
        {
            sliderB_new.Value = colorB_new;
            if (textBoxB_new.IsFocused || sliderB_new.IsFocused)
            {
                var textBox = sender as TextBox;

                if (isRightRGB(colorR_new, colorG_new, Int32.Parse(textBox.Text)))
                {
                    colorB_new = Int32.Parse(textBox.Text);
                    newBrush.Color = Color.FromRgb((Byte)colorR_new, (Byte)colorG_new, (Byte)colorB_new);
                    colorNew.Background = newBrush;
                }
            }
        }

        private void Slider_ValueChanged_B_new(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            colorB_new = (int)e.NewValue;
            textBoxB_new.Text = colorB_new.ToString();
        }
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (radiusTextBox.IsFocused)
            {
                radius = Double.Parse(radiusTextBox.Text);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConvertColor();
            this.targetImage.Source = GetImage(targetBitmap);
        }
    }
}
