using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1v3.Color
{
    public class ColorChangedEventArgs : EventArgs
    {
        public bool Overflow { get; set; }

        public ColorChangedEventArgs() : this(false)
        {
        }

        public ColorChangedEventArgs(bool overflow)
        {
            Overflow = overflow;
        }
    }
}
