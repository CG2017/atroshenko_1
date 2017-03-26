using System;

namespace CG1v3.Color
{
    class RgbColorModel : ColorModel
    {
        public override double GetComponent(int i)
        {
            if (i == 0) return Color.R / 255.0;
            if (i == 1) return Color.G / 255.0;
            if (i == 2) return Color.B / 255.0;

            throw new IndexOutOfRangeException();
        }

        public override void SetComponent(int i, double v)
        {
            if (v < 0 || v > 1)
                throw new ArgumentOutOfRangeException();

            Color c = new Color()
            {
                R = Color.R,
                G = Color.G,
                B = Color.B
            };

            if (i == 0) c.R = Convert.ToByte(v * 255);
            else if (i == 1) c.G = Convert.ToByte(v * 255);
            else if (i == 2) c.B = Convert.ToByte(v * 255);
            else throw new IndexOutOfRangeException();

            SilentlySetColor(c);
        }
    }
}
