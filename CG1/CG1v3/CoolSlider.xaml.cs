using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CG1v3
{
    /// <summary>
    /// Interaction logic for CoolSlider.xaml
    /// </summary>
    public partial class CoolSlider : UserControl
    {
        public CoolSlider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Specifies the content of label in this control
        /// </summary>
        public static readonly DependencyProperty LabelContentProperty = DependencyProperty.Register(
            "LabelContent", typeof(object), typeof(CoolSlider), new PropertyMetadata(default(object)));

        /// <summary>
        /// Gets or sets the value of label content
        /// </summary>
        public object LabelContent
        {
            get { return (object) GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }

        /// <summary>
        /// Specifies the value that is stored in this control
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(CoolSlider), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the double value stored in this control
        /// </summary>
        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
