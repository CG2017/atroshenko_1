using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LineDrawer.Drawing
{
    class BresenhamLineDrawingAlgorythm : IAlgorythm
    {
        public IReadOnlyCollection<Point> GetPixels(int x0, int y0, int x1, int y1)
        {
            var result = new List<Point>();
            var deltaX = Math.Abs(x1 - x0);
            var deltaY = Math.Abs(y1 - y0);
            var s1 = Math.Sign(x1 - x0);
            var s2 = Math.Sign(y1 - y0);
            var y = y0;
            var x = x0;
            bool changeFlag;

            if (deltaY > deltaX)
            {
                int temp = deltaX;
                deltaX = deltaY;
                deltaY = temp;
                changeFlag = true;
            }
            else
            {
                changeFlag = false;
            }

            int t = 2 * deltaY - deltaX;

            for (int i = 0; i <= deltaX; i++)
            {
                result.Add(new Point(x, y));
                while (t >= 0)
                {
                    if (changeFlag)
                        x += s1;
                    else
                        y += s2;
                    t -= 2 * deltaX;
                }
                if (changeFlag)
                {
                    y += s2;
                }
                else
                {
                    x += s1;
                }
                t += 2 * deltaY;
            }

            return result;
        }
    }
}
