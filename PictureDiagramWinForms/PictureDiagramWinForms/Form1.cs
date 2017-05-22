using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PictureDiagramWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //CreateChart();
        }

        public void CreateChart()
        {
            int n = 3;
            double[] redValues = { 0.1, 0.8, 0.5 };
            //this.chart.Series["serias"].Points.AddXY("Max", redValues);
            for (int i = 0; i < n; i++)
            {
                this.chartRed.Series["Red"].Points.Add(redValues[i]);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }


        System.Drawing.Image inputImage;
        Bitmap sourceBitmap;
        private void button1_Click(object sender, EventArgs e)
        {
            
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                    inputImage = pictureBox1.Image;
                    fileNameLabel.Text = dlg.FileName;
                }
            }
            if (inputImage != null)
            {
                sourceBitmap = new Bitmap(inputImage);

                this.chartRed.Series["Red"].Points.Clear();
                this.chartGreen.Series["Green"].Points.Clear();
                this.chartBlue.Series["Blue"].Points.Clear();


                //createDiagramAutomatic();
                createDiagramManual();
            }
            
        }

        private void createDiagramManual()
        {
            int[] redValues = new int[256];
            int[] greenValues = new int[256];
            int[] blueValues = new int[256];

            for (int i = 0; i < 256; i++)
            {
                redValues[i] = 0;
                greenValues[i] = 0;
                blueValues[i] = 0;
            }

            double averageRed = 0.0;
            double averageGreen = 0.0;
            double averageBlue = 0.0;

            for (int i = 0; i < sourceBitmap.Width; i++)
            {
                for (int j = 0; j < sourceBitmap.Height; j++)
                {
                    System.Drawing.Color c = sourceBitmap.GetPixel(i, j);
                    redValues[c.R]++;
                    greenValues[c.G]++;
                    blueValues[c.B]++;
                    averageRed += c.R;
                    averageGreen += c.G;
                    averageBlue += c.B;
                }
            }

            averageRed = averageRed / (double)(sourceBitmap.Width * sourceBitmap.Height);
            averageGreen = averageGreen / (double)(sourceBitmap.Width * sourceBitmap.Height);
            averageBlue = averageBlue / (double)(sourceBitmap.Width * sourceBitmap.Height);

            averageRedLabel.Text = ((int)averageRed).ToString();
            averageGreenLabel.Text = ((int)averageGreen).ToString();
            averageBlueLabel.Text = ((int)averageBlue).ToString();

            for (int k = 0; k < 256; k++)
            {
                this.chartRed.Series["Red"].Points.Add(redValues[k]);
                this.chartGreen.Series["Green"].Points.Add(greenValues[k]);
                this.chartBlue.Series["Blue"].Points.Add(blueValues[k]);
            }
        }

        private void createDiagramAutomatic()
        {
            ImageStatistics rgbStatistics = new ImageStatistics(sourceBitmap);
            int[] redValues = rgbStatistics.Red.Values;
            int[] greenValues = rgbStatistics.Green.Values;
            int[] blueValues = rgbStatistics.Blue.Values;

            for (int k = 0; k < 256; k++)
            {
                this.chartRed.Series["Red"].Points.Add(redValues[k]);
                this.chartGreen.Series["Green"].Points.Add(greenValues[k]);
                this.chartBlue.Series["Blue"].Points.Add(blueValues[k]);
            }
        }

    }
}
