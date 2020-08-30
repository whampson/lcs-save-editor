using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GTASaveData.LCS;
using GTASaveData.Types;
using LCSSaveEditor.GUI.Controls;
using Xceed.Wpf.Toolkit.Core;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for GeneralTab.xaml
    /// </summary>
    public partial class GeneralTab : TabPageBase<ViewModels.GeneralTab>
    {
        public GeneralTab()
        {
            InitializeComponent();
        }

        private void TargetPosition_Changed(object sender, RoutedEventArgs e)
        {
            //if (ViewModel.SimpleVars.TargetIsOn && e is PropertyChangedEventArgs<float> args)
            //{
            //    Vector2D oldValue = ViewModel.SimpleVars.TargetPosition;
            //    Vector2D newValue = new Vector2D();

            //    switch (e.RoutedEvent.Name)
            //    {
            //        case nameof(LocationPicker2D.XChanged): newValue = new Vector2D(args.NewValue, oldValue.Y); break;
            //        case nameof(LocationPicker2D.YChanged): newValue = new Vector2D(oldValue.X, args.NewValue); break;
            //    }

            //    if (oldValue != newValue) ViewModel.SimpleVars.TargetPosition = newValue;
            //}
        }
    }
}
