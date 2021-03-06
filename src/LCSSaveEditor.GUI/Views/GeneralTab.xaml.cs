﻿using System;
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
    }
}
