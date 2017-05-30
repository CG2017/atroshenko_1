using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LineDrawer.Drawing
{
    class BresenhamCircleAlgorythm : IAlgorythm
    {
        public IReadOnlyCollection<Point> GetPixels(int x0, int y0, int x1, int y1)
        {
            var result = new List<Point>();
            var r = (int) Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
            var x = r;
            var y = 0;

            var delta = 1 - x;

            while (y <= x)
            {
                result.Add(new Point(x + x0, y + y0));
                result.Add(new Point(-x + x0, y + y0));
                result.Add(new Point(-x + x0, -y + y0));
                result.Add(new Point(x + x0, -y + y0));
                result.Add(new Point(y + x0, x + y0));
                result.Add(new Point(-y + x0, x + y0));
                result.Add(new Point(-y + x0, -x + y0));
                result.Add(new Point(y + x0, -x + y0));
                y++;
                if (delta <= 0)
                {
                    delta += 2 * y + 1;
                }
                else
                {
                    x--;
                    delta += 2 * (y - x) + 1;
                }
            }

            return result;
        }
    }
}
