using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LCSSaveEditor.GUI.Controls
{
    public abstract class LocationPickerBase : UserControl
    {
        public static readonly DependencyProperty HorizontalGapProperty = DependencyProperty.Register(
            nameof(HorizontalGap), typeof(Thickness), typeof(LocationPicker),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0)));

        public static readonly DependencyProperty VerticalGapProperty = DependencyProperty.Register(
            nameof(VerticalGap), typeof(Thickness), typeof(LocationPicker),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 5, 0)));

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register(
            nameof(FormatString), typeof(string), typeof(LocationPicker),
            new FrameworkPropertyMetadata(""));

        public Thickness HorizontalGap
        {
            get { return (Thickness) GetValue(HorizontalGapProperty); }
            set { SetValue(HorizontalGapProperty, value); }
        }

        public Thickness VerticalGap
        {
            get { return (Thickness) GetValue(VerticalGapProperty); }
            set { SetValue(VerticalGapProperty, value); }
        }

        public string FormatString
        {
            get { return (string) GetValue(FormatStringProperty); }
            set { SetValue(FormatStringProperty, value); }
        }
    }
}
