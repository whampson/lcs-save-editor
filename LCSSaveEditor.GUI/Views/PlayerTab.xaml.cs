using System;
using System.Collections.Generic;
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
using LCSSaveEditor.Core;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for PlayerTab.xaml
    /// </summary>
    public partial class PlayerTab : TabPageBase<ViewModels.PlayerTab>
    {
        public static readonly DependencyProperty CurrentOutfitImageProperty = DependencyProperty.Register(
            nameof(CurrentOutfitImage), typeof(BitmapImage), typeof(PlayerTab));

        public BitmapImage CurrentOutfitImage
        {
            get { return (BitmapImage) GetValue(CurrentOutfitImageProperty); }
            set { SetValue(CurrentOutfitImageProperty, value); }
        }

        public PlayerTab()
        {
            InitializeComponent();
        }

        private void LoadOutfitImage(int id)
        {
            if (OutfitUris.Count > id)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.DecodePixelWidth = 420;
                img.UriSource = new Uri(OutfitUris[id]);
                img.EndInit();
                CurrentOutfitImage = img;
            }
            else
            {
                CurrentOutfitImage = null;
            }
        }

        private void CurrentOutfitImage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadOutfitImage(ViewModel.CurrentOutfit);
        }

        private static readonly IReadOnlyList<string> OutfitUris = new List<string>()
        {
            @"pack://application:,,,/Resources/costume00.png",
            @"pack://application:,,,/Resources/costume01.png",
            @"pack://application:,,,/Resources/costume02.png",
            @"pack://application:,,,/Resources/costume03.png",
            @"pack://application:,,,/Resources/costume04.png",
            @"pack://application:,,,/Resources/costume05.png",
            @"pack://application:,,,/Resources/costume06.png",
            @"pack://application:,,,/Resources/costume07.png",
            @"pack://application:,,,/Resources/costume08.png",
            @"pack://application:,,,/Resources/costume09.png",
            @"pack://application:,,,/Resources/costume10.png",
            @"pack://application:,,,/Resources/costume11.png",
            @"pack://application:,,,/Resources/costume12.png",
            @"pack://application:,,,/Resources/costume13.png",
            @"pack://application:,,,/Resources/costume14.png",
            @"pack://application:,,,/Resources/costume15.png",
        };
    }
}
