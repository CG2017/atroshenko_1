using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LineDrawer.Drawing
{
    class StepByStepLineDrawingAlgorythm : IAlgorythm
    {
        public IReadOnlyCollection<Point> GetPixels(int x0, int y0, int x1, int y1)
        {
            var points = new List<Point>();

            if (x0 > x1)
            {
                return GetPixels(x1, y1, x0, y0);
            }

            float y = y0;
            float m = (float)(y1 - y0) / (x1 - x0);

            if (Math.Abs(m) <= 1)
            {
                for (int xi = x0; xi <= x1; ++xi)
                {
                    points.Add(new Point(xi, (int)Math.Round(y)));
                    y += m;
                }
            }
            else
            {
                return RasterSteepLine(y0, x0, y1, x1);
            }

            return points;
        }

        private IReadOnlyCollection<Point> RasterSteepLine(int x0, int y0, int x1, int y1)
        {
            var points = new List<Point>();

            if (x0 > x1)
            {
                return RasterSteepLine(x1, y1, x0, y0);
            }

            float y = y0;
            float m = (float)(y1 - y0) / (x1 - x0);

            for (int xi = x0; xi <= x1; ++xi)
            {
                points.Add(new Point((int)Math.Round(y), xi));
                y += m;
            }

            return points;
        }
    }
}
