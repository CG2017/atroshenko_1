using System;

namespace CG1v3.Color
{
    class CmyColorModel : ColorModel
    {
        public override double GetComponent(int i)
        {
            if (i == 0) return (255 - Color.R) / 255.0;
            if (i == 1) return (255 - Color.G) / 255.0;
            if (i == 2) return (255 - Color.B) / 255.0;

            throw new IndexOutOfRangeException();
        }

        public override void SetComponent(int i, double v)
        {
            if (v < 0 || v > 1)
                throw new ArgumentOutOfRangeException();

            Color c = new Color
            {
                R = Color.R,
                G = Color.G,
                B = Color.B
            };

            byte b = Convert.ToByte((1 - v) * 255);

            if (i == 0) c.R = b;
            else if (i == 1) c.G = b;
            else if (i == 2) c.B = b;
            else throw new IndexOutOfRangeException();

            SilentlySetColor(c);
        }
    }
}
