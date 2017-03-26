using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1v3.Color
{
    public abstract class ColorModel
    {
        private Color _color;
        private bool _overflowBit;

        protected bool OverflowBit
        {
            get { return _overflowBit; }
            set
            {
                _overflowBit = value;
                OnOverflowOccurred(new OverflowEventArgs());
            }
        }

        public delegate void OverflowEventHandler(object sender, OverflowEventArgs args);

        public event OverflowEventHandler OverflowOccurred;

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _color.PropertyChanged += _colorOnPropertyChanged;
            }
        }

        private void _colorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnColorPropertyChanged(e);
        }

        protected virtual void OnColorPropertyChanged(PropertyChangedEventArgs e)
        {
            OnColorChanged();
        }

        public event EventHandler<ColorChangedEventArgs> ColorChanged;

        protected void SilentlySetColor(Color c)
        {
            _color.PropertyChanged -= _colorOnPropertyChanged;
            
            _color.R = c.R;
            _color.G = c.G;
            _color.B = c.B;

            _color.PropertyChanged += _colorOnPropertyChanged;
        }

        public static ColorModel CreateColorModel(string modelName)
        {
            if ("rgb".Equals(modelName, StringComparison.OrdinalIgnoreCase))
                return new RgbColorModel();
            if ("cmy".Equals(modelName, StringComparison.OrdinalIgnoreCase))
                return new CmyColorModel();
            if ("hsv".Equals(modelName, StringComparison.OrdinalIgnoreCase))
                return new HsvColorModel();
            if ("luv".Equals(modelName, StringComparison.OrdinalIgnoreCase))
                return new LuvColorModel();

            throw new ArgumentException();
        }

        public abstract double GetComponent(int i);

        public abstract void SetComponent(int i, double v);

        protected virtual void OnColorChanged()
        {
            var handler = ColorChanged;
            if (handler != null) handler(this, new ColorChangedEventArgs(OverflowBit));
        }

        protected virtual void OnOverflowOccurred(OverflowEventArgs args)
        {
            var handler = OverflowOccurred;
            if (handler != null) handler(this, args);
        }
    }

    public class OverflowEventArgs : EventArgs
    {
        public OverflowEventArgs() { }

        public OverflowEventArgs(string errorMessage, bool overflowBit)
        {
            ErrorMessage = errorMessage;
            OverflowBit = overflowBit;
        }

        public string ErrorMessage { get; set; }

        public bool OverflowBit { get; set; }
    }
}
