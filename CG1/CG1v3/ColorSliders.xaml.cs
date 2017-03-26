using System;
using System.Windows;
using System.Windows.Controls;
using CG1v3.Color;

namespace CG1v3
{
    /// <summary>
    /// Interaction logic for ColorSliders.xaml
    /// </summary>
    public partial class ColorSliders
    {
        private ColorModel _model;

        public ColorModel Model
        {
            get { return _model; }
            set
            {
                _model = value;

                ModelOnColorChanged(this, EventArgs.Empty);
                _model.ColorChanged += ModelOnColorChanged;
            }
        }

        public ColorSliders()
        {
            InitializeComponent();

            SubscribeToSliderValueChanged();
        }

        private void SubscribeToSliderValueChanged()
        {
            FirstSlider.Slider.ValueChanged += SliderOnValueChanged;
            SecondSlider.Slider.ValueChanged += SliderOnValueChanged;
            ThirdSlider.Slider.ValueChanged += SliderOnValueChanged;
        }

        private void UnsubscribeToSliderValueChanged()
        {
            FirstSlider.Slider.ValueChanged -= SliderOnValueChanged;
            SecondSlider.Slider.ValueChanged -= SliderOnValueChanged;
            ThirdSlider.Slider.ValueChanged -= SliderOnValueChanged;
        }

        private void SliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            Model.SetComponent(0, FirstSlider.Value);
            Model.SetComponent(1, SecondSlider.Value);
            Model.SetComponent(2, ThirdSlider.Value);
        }

        private void ModelOnColorChanged(object sender, EventArgs e)
        {
            UnsubscribeToSliderValueChanged();

            FirstSlider.Value = Model.GetComponent(0);
            SecondSlider.Value = Model.GetComponent(1);
            ThirdSlider.Value = Model.GetComponent(2);

            SubscribeToSliderValueChanged();
        }

        public string LabelsText
        {
            get
            {
                return String.Format("{0},{1},{2}",
                    FirstSlider.LabelContent,
                    SecondSlider.LabelContent,
                    ThirdSlider.LabelContent
                );
            }
            set
            {
                string[] texts = value.Split(',');

                FirstSlider.LabelContent = texts[0];
                SecondSlider.LabelContent = texts[1];
                ThirdSlider.LabelContent = texts[2];
            }
        }
    }
}
