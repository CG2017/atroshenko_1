using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CG1v3.Color
{
    public class Color : INotifyPropertyChanged
    {
        private System.Windows.Media.Color _winColor = System.Windows.Media.Color.FromRgb(0, 0, 0);

        public System.Windows.Media.Color WinColor
        {
            get { return _winColor; }
        }

        public byte R
        {
            get { return _winColor.R; }
            set
            {
                _winColor.R = value;
                OnPropertyChanged();
            }
        }

        public byte G
        {
            get { return _winColor.G; }
            set
            {
                _winColor.G = value; 
                OnPropertyChanged();
            }
        }

        public byte B
        {
            get { return _winColor.B; }
            set
            {
                _winColor.B = value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
