using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LineDrawer.Drawing
{
    public interface IAlgorythm
    {
        IReadOnlyCollection<Point> GetPixels(int x0, int y0, int x1, int y1);
    }
}
