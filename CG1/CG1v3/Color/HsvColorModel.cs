using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1v3.Color
{
    class HsvColorModel : ColorModel
    {
//        private bool _hUndefined = true;
        private double _h;
        private double _s;
        private double _v;
        private const double Eps = 1e-6;
        protected override void OnColorPropertyChanged(PropertyChangedEventArgs e)
        {
            RecalculateHsvComponents();
            base.OnColorPropertyChanged(e);
        }

        private void RecalculateHsvComponents()
        {
            double r = Color.WinColor.R / 255.0;
            double g = Color.WinColor.G / 255.0;
            double b = Color.WinColor.B / 255.0;

            double cmax = Math.Max(Math.Max(r, g), b);
            double cmin = Math.Min(Math.Min(r, g), b);
            double delta = cmax - cmin;

            if (delta >= Eps)
            {
//                _hUndefined = false;
                // Here we just check whether particular component was assigned to cmax.
                // No floating point errors can occur here

                // ReSharper disable CompareOfFloatsByEqualityOperator

                if (cmax == r)
                {
                    if (b < g) _h = (g - b) / delta;
                    else _h = (g - b) / delta + 6;  //else _h = (b - g) / delta;
                }
                if (cmax == g)
                    _h = (b - r) / delta + 2;
                if (cmax == b)
                    _h = (r - g) / delta + 4;

//                var delR = (((cmax - r) / 6) + (delta / 2)) / delta;
//                var delG = ( ( ( cmax - g ) / 6 ) + ( delta / 2 ) ) / delta;
//                var delB = ( ( ( cmax - b ) / 6 ) + ( delta / 2 ) ) / delta;
//
//                if (r == cmax) _h = delB - delG;
//                else if (g == cmax) _h = (1 / 3.0) + delR - delB;
//                else if (b == cmax) _h = (2 / 3.0) + delG - delR;
//
//                if (_h < 0) _h += 1;
//                if ( _h > 1 ) _h -= 1;

                // ReSharper restore CompareOfFloatsByEqualityOperator
            }
            else
            {
                _h = 0;
//                _hUndefined = true;
            }

            _h /= 6;
            _v = cmax;
            if (_v < Eps) _s = 0;
            else _s = delta / _v;
        }

        public override double GetComponent(int i)
        {
            if (i == 0) return _h * 360;
            if (i == 1) return _s * 100;
            if (i == 2) return _v * 100;

            throw new IndexOutOfRangeException();
        }

        public override void SetComponent(int i, double v)
        {
            if (!IsValidValue(v, i))
                throw new ArgumentOutOfRangeException();

            if (i == 0) _h = v / 360;
            else if (i == 1) _s = v / 100;
            else if (i == 2) _v = v / 100;
            else throw new IndexOutOfRangeException();

            RecalculateRgbComponents();
        }

        private static bool IsValidValue(double v, int i)
        {
            if (i == 0)
                return v >= 0 && v <= 360;
            if (i == 1 || i == 2)
                return v >= 0 && v <= 100;
            throw new IndexOutOfRangeException();
        }

        private void RecalculateRgbComponents()
        {
            double c = _v * _s;
            double m = _v - c;
            double h = _h * 6;
            double x = c * (1 - Math.Abs(h % 2 - 1));

            double r, g, b;
            
//            if (_h == 0)
//            {
//                r = 0;
//                g = 0;
//                b = 0;
//            }
            if (h < 1)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (h < 2)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (h < 3)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (h < 4)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (h < 5)
            {
                r = x;
                g = 0;
                b = c;
            }
            else if (h <= 6)
            {
                r = c;
                g = 0;
                b = x;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            Color col = new Color
            {
                R = Convert.ToByte((r + m) * 255),
                G = Convert.ToByte((g + m) * 255),
                B = Convert.ToByte((b + m) * 255)
            };
            SilentlySetColor(col);
        }
    }
}
