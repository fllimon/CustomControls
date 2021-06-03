using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorPicker
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ColorPicker"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ColorPicker;assembly=ColorPicker"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ColorPicker/>
    ///
    /// </summary>

    [TemplatePart(Name = "PART_RedSlider", Type = typeof(RangeBase))]
    [TemplatePart(Name = "PART_GreenSlider", Type = typeof(RangeBase))]
    [TemplatePart(Name = "PART_BlueSlider", Type = typeof(RangeBase))]
    public class ColorPicker : Control
    {
        public static readonly DependencyProperty RedProperty;
        public static readonly DependencyProperty GreenProperty;
        public static readonly DependencyProperty BlueProperty;
        public static readonly DependencyProperty ColorProperty;

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));

            ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker),
                                                        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorChanged)));

            RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(ColorPicker),
                                            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRGBColorChanged)));

            GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(ColorPicker),
                                            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRGBColorChanged)));

            BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(ColorPicker),
                                            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRGBColorChanged)));

        }



        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        private static void OnRGBColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)d;
            Color color = picker.Color;

            if (e.Property == RedProperty)
            {
                color.R = (byte)e.NewValue;
            }
            else
            {
                if (e.Property == GreenProperty)
                {
                    color.G = (byte)e.NewValue;
                }
                else
                {
                    if (e.Property == BlueProperty)
                    {
                        color.B = (byte)e.NewValue;
                    }
                }
            }

            picker.Color = color;
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Color newColor = (Color)e.NewValue;
            Color oldColor = (Color)e.OldValue;

            ColorPicker picker = (ColorPicker)d;

            picker.Red = newColor.R;
            picker.Green = newColor.G;
            picker.Blue = newColor.B;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ApplyTemplate("PART_RedSlider", "Red");
            ApplyTemplate("PART_GreenSlider", "Green");
            ApplyTemplate("PART_BlueSlider", "Blue");

            SolidColorBrush brush = GetTemplateChild("PART_PreviewBrush") as SolidColorBrush;
            
            if (brush != null)
            {
                Binding bind = new Binding("Color");
                bind.Source = brush;
                bind.Mode = BindingMode.OneWayToSource;
                SetBinding(ColorProperty, bind);
            }
        }

        private void ApplyTemplate(string templatePartName, string propName)
        {
            RangeBase slider = GetTemplateChild(templatePartName) as RangeBase;

            if (slider != null)
            {
                Binding bind = new Binding(propName);
                bind.Source = this;
                bind.Mode = BindingMode.TwoWay;
                slider.SetBinding(RangeBase.ValueProperty, bind);
            }
        }
    }
}
