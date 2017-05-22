using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1v3.Color
{
    class LuvColorModel : ColorModel
    {

        private const double Tolerance = 1e-6;
        private const double Epsilon = .008856;
        private const double Kappa = 903.3;

        private double _l;
        private double _u;
        private double _v;

        private double _x;
        private double _y;
        private double _z;

        private int _r;
        private int _g;
        private int _b;

        private const double Xr = 95.047;
        private const double Yr = 100;
        private const double Zr = 108.883;

        protected override void OnColorPropertyChanged(PropertyChangedEventArgs e)
        {
            _r = Color.R;
            _g = Color.G;
            _b = Color.B;

            RecalculateRgbToXyz();
            RecalculateXyzToLuv();
            CheckAndFixOverflow();

            base.OnColorPropertyChanged(e);
        }

        private void RecalculateRgbToXyz()
        {
            double varR = (_r / 255d); //R from 0 to 255
            double varG = (_g / 255d); //G from 0 to 255
            double varB = (_b / 255d); //B from 0 to 255

            if (varR > 0.04045) varR = Math.Pow((varR + 0.055) / 1.055, 2.4);
            else varR = varR / 12.92;
            if (varG > 0.04045) varG = Math.Pow((varG + 0.055) / 1.055, 2.4);
            else varG = varG / 12.92;
            if (varB > 0.04045) varB = Math.Pow((varB + 0.055) / 1.055, 2.4);
            else varB = varB / 12.92;

            varR = varR * 100;
            varG = varG * 100;
            varB = varB * 100;

            _x = varR * 0.4124 + varG * 0.3576 + varB * 0.1805;
            _y = varR * 0.2126 + varG * 0.7152 + varB * 0.0722;
            _z = varR * 0.0193 + varG * 0.1192 + varB * 0.9505;
        }

        private void RecalculateXyzToRgb()
        {
            double varX = _x / 100;
            double varY = _y / 100;
            double varZ = _z / 100;

            double varR = varX * 3.2406 + varY * -1.5372 + varZ * -0.4986;
            double varG = varX * -0.9689 + varY * 1.8758 + varZ * 0.0415;
            double varB = varX * 0.0557 + varY * -0.2040 + varZ * 1.0570;

            if (varR > 0.0031308) varR = 1.055 * Math.Pow(varR, (1 / 2.4)) - 0.055;
            else varR = 12.92 * varR;
            if (varG > 0.0031308) varG = 1.055 * (Math.Pow(varG, (1 / 2.4))) - 0.055;
            else varG = 12.92 * varG;
            if (varB > 0.0031308) varB = 1.055 * (Math.Pow(varB, (1 / 2.4))) - 0.055;
            else varB = 12.92 * varB;

            _r = (int) (varR * 255);
            _g = (int) (varG * 255);
            _b = (int) (varB * 255);
        }

        private void RecalculateXyzToLuv()
        {
            if (Math.Abs(_x) < Tolerance)
                _x = Tolerance;
            if (Math.Abs(_y) < Tolerance)
                _y = Tolerance;
            if (Math.Abs(_z) < Tolerance)
                _z = Tolerance;
            double varU = (4 * _x) / (_x + (15 * _y) + (3 * _z));
            double varV = (9 * _y) / (_x + (15 * _y) + (3 * _z));

            double varY = _y / 100;
            if (varY > Epsilon) varY = Math.Pow(varY, (1.0 / 3));
            else varY = (7.787 * varY) + (16.0 / 116);

            const double ur = (4 * Xr) / (Xr + (15 * Yr) + (3 * Zr));
            const double vr = (9 * Yr) / (Xr + (15 * Yr) + (3 * Zr));

            _l = (116 * varY) - 16;
            if (_l < Tolerance)
                _l = 0;
            _u = 13 * _l * (varU - ur);
            _v = 13 * _l * (varV - vr);
        }

        private void RecalculateLuvToXyz()
        {
            if (_l == 0)
            {
                _l = _x = _y = _z = 0;
                return;
            }

            double varY = (_l + 16) / 116;
            if (Math.Pow(varY, 3) > 0.008856)
                varY = Math.Pow(varY, 3);
            else
                varY = (varY - 16.0 / 116) / 7.787;

            const double ur = 4 * Xr / (Xr + 15 * Yr + 3 * Zr);
            const double vr = 9 * Yr / (Xr + 15 * Yr + 3 * Zr);

            double varU = _u / (13 * _l) + ur;
            double varV = _v / (13 * _l) + vr;

            _y = varY * 100;
            _x = -(9 * _y * varU) / ((varU - 4) * varV - varU * varV);
            _z = (9 * _y - 15 * varV * _y - varV * _x) / (3 * varV);
        }

        private void CheckAndFixOverflow()
        {
            string errorMessage = null;

            if (_r > 255)
            {
                errorMessage = String.Format("R is {0} more than 255", _r);
                _r = 255;
            }
            if (_g > 255)
            {
                errorMessage = String.Format("G is {0} more than 255", _g);
                _g = 255;
            }
            if (_b > 255)
            {
                errorMessage = String.Format("B is {0} more than 255", _b);
                _b = 255;
            }
            if (_r < 0)
            {
                errorMessage = String.Format("R is {0} less than 0", _r);
                _r = 0;
            }
            if (_g < 0)
            {
                errorMessage = String.Format("G is {0} less than 0", _g);
                _g = 0;
            }
            if (_b < 0)
            {
                errorMessage = String.Format("B is {0} less than 0", _b);
                _b = 0;
            }

            if (errorMessage != null)
            {
                RecalculateRgbToXyz();
                RecalculateXyzToLuv();
                OnOverflowOccurred(new OverflowEventArgs(errorMessage, true));
            }
            else
            {
                OnOverflowOccurred(new OverflowEventArgs(null, false));
            }
        }

        public override double GetComponent(int i)
        {
            if (i == 0) return _l;
            if (i == 1) return _u;
            if (i == 2) return _v;

            throw new IndexOutOfRangeException();
        }

        private bool IsValidComponent(int i, double v)
        {
            if (i == 0)
                return v >= 0 && v <= 100;
            if (i == 1 || i == 2)
                return v >= -200 && v <= 200;
            throw new IndexOutOfRangeException("i");
        }

        public override void SetComponent(int i, double v)
        {
            if (!IsValidComponent(i, v))
                throw new ArgumentOutOfRangeException();

            if (i == 0) _l = v;
            else if (i == 1) _u = v;
            else if (i == 2) _v = v;
            else 
                throw new IndexOutOfRangeException();

            RecalculateLuvToXyz();
            RecalculateXyzToRgb();

            CheckAndFixOverflow();

            Color c = new Color
            {
                R = Convert.ToByte(_r),
                G = Convert.ToByte(_g),
                B = Convert.ToByte(_b)
            };

            SilentlySetColor(c);
        }
    }
}
