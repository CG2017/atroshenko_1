using System;

namespace CG1v3.Color
{
    class RgbColorModel : ColorModel
    {
        public override double GetComponent(int i)
        {
            if (i == 0) return Color.R;
            if (i == 1) return Color.G;
            if (i == 2) return Color.B;

            throw new IndexOutOfRangeException();
        }

        public override void SetComponent(int i, double v)
        {
            if (v < 0 || v > 255)
                throw new ArgumentOutOfRangeException();

            Color c = new Color()
            {
                R = Color.R,
                G = Color.G,
                B = Color.B
            };

            if (i == 0) c.R = Convert.ToByte(v);
            else if (i == 1) c.G = Convert.ToByte(v);
            else if (i == 2) c.B = Convert.ToByte(v);
            else throw new IndexOutOfRangeException();

            SilentlySetColor(c);
        }
    }
}
