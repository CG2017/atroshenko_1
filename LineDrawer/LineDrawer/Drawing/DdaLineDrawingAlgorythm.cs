using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LineDrawer.Drawing
{
    public class DdaLineDrawingAlgorythm : IAlgorythm
    {
        public IReadOnlyCollection<Point> GetPixels(float x0, float y0, float x1, float y1)
        {
            var result = new List<Point>();

            var xStart = (int) Math.Round(x0);
            var yStart = (int) Math.Round(y0);
            var xEnd = (int) Math.Round(x1);
            var yEnd = (int) Math.Round(y1);

            int l = Math.Max(Math.Abs(xEnd - xStart), Math.Abs(yEnd - yStart));

            float dX = (x1 - x0) / l;
            float dY = (y1 - y0) / l;

            result.Add(new Point(x0, y0));
            var i = 1;
            float xPrev = x0;
            float yPrev = y0;
            while (i < l)
            {

                result.Add(new Point(Math.Round(xPrev + dX), Math.Round(yPrev + dY)));
                xPrev += dX;
                yPrev += dY;
                i++;
            }
            result.Add(new Point(x1, y1));

            return result;
        }

        IReadOnlyCollection<Point> IAlgorythm.GetPixels(int x0, int y0, int x1, int y1)
        {
            return GetPixels(x0, y0, x1, y1);
        }
    }
}
