using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Types;
using WpfEssentials;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public enum BlipType
    {
        Package,
        Rampage,
        StuntJump
    }

    public class CollectibleBlip : ObservableObject
    {
        private BlipType m_type;
        private int m_index;
        private Point m_coords;
        private bool m_isCollected;
        private bool isEnabled;
        private UIElement m_sprite;

        public BlipType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public int Index
        {
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public Point Coords
        {
            get { return m_coords; }
            set { m_coords = value; OnPropertyChanged(); }
        }

        public bool IsCollected
        {
            get { return m_isCollected; }
            set { m_isCollected = value; OnPropertyChanged(); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; OnPropertyChanged(); }
        }

        public UIElement Sprite
        {
            get { return m_sprite; }
            set { m_sprite = value; OnPropertyChanged(); }
        }
    }

    public class MapWindow : WindowBase
    {
        public static readonly Point Origin = new Point(1024, 1024);
        public static readonly Point Scale = new Point(0.512, -0.512);

        private ObservableCollection<UIElement> m_overlays;
        private ObservableCollection<CollectibleBlip> m_blips;
        private Point m_centerOffset;
        private Point m_mouseOffset;
        private Point m_mouseCoords;
        private double m_zoomLevel;
        private Cursor m_cursor;
        private bool m_isShowingLegend;
        private bool m_isShowingPackages;
        private bool m_isShowingRampages;
        private bool m_isShowingStuntJumps;
        private bool m_isShowingCollected;
        private bool m_isShowingSpawnPoint;
        private Color m_spawnPointColor;
        private Color m_packageColor;
        private Color m_packageColorCollected;
        private Color m_rampageColor;
        private Color m_rampageColorCollected;
        private Color m_stuntJumpColor;
        private Color m_stuntJumpColorCollected;
        private bool m_handlersRegistered;

        public ObservableCollection<UIElement> MapOverlays
        {
            get { return m_overlays; }
            set { m_overlays = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CollectibleBlip> Blips
        {
            get { return m_blips; }
            set { m_blips = value; OnPropertyChanged(); }
        }

        public Point CenterOffset
        {
            get { return m_centerOffset; }
            set { m_centerOffset = value; OnPropertyChanged(); }
        }

        public Point MouseOffset
        {
            get { return m_mouseOffset; }
            set { m_mouseOffset = value; OnPropertyChanged(); }
        }

        public Point MouseCoords
        {
            get { return m_mouseCoords; }
            set { m_mouseCoords = value; OnPropertyChanged(); }
        }

        public double ZoomLevel
        {
            get { return m_zoomLevel; }
            set { m_zoomLevel = value; OnPropertyChanged(); }
        }

        public Cursor Cursor
        {
            get { return m_cursor; }
            set { m_cursor = value; OnPropertyChanged(); }
        }

        public bool IsShowingLegend
        {
            get { return m_isShowingLegend; }
            set { m_isShowingLegend = value; OnPropertyChanged(); }
        }

        public bool IsShowingPackages
        {
            get { return m_isShowingPackages; }
            set { m_isShowingPackages = value; OnPropertyChanged(); }
        }

        public bool IsShowingRampages
        {
            get { return m_isShowingRampages; }
            set { m_isShowingRampages = value; OnPropertyChanged(); }
        }

        public bool IsShowingStuntJumps
        {
            get { return m_isShowingStuntJumps; }
            set { m_isShowingStuntJumps = value; OnPropertyChanged(); }
        }

        public bool IsShowingCollected
        {
            get { return m_isShowingCollected; }
            set { m_isShowingCollected = value; OnPropertyChanged(); }
        }

        public bool IsShowingSpawnPoint
        {
            get { return m_isShowingSpawnPoint; }
            set { m_isShowingSpawnPoint = value; OnPropertyChanged(); }
        }

        public Color SpawnPointColor
        {
            get { return m_spawnPointColor; }
            set { m_spawnPointColor = value; OnPropertyChanged(); }
        }

        public Color PackageColor
        {
            get { return m_packageColor; }
            set { m_packageColor = value; OnPropertyChanged(); }
        }

        public Color PackageColorCollected
        {
            get { return m_packageColorCollected; }
            set { m_packageColorCollected = value; OnPropertyChanged(); }
        }

        public Color RampageColor
        {
            get { return m_rampageColor; }
            set { m_rampageColor = value; OnPropertyChanged(); }
        }

        public Color RampageColorCollected
        {
            get { return m_rampageColorCollected; }
            set { m_rampageColorCollected = value; OnPropertyChanged(); }
        }

        public Color StuntJumpColor
        {
            get { return m_stuntJumpColor; }
            set { m_stuntJumpColor = value; OnPropertyChanged(); }
        }

        public Color StuntJumpColorCollected
        {
            get { return m_stuntJumpColorCollected; }
            set { m_stuntJumpColorCollected = value; OnPropertyChanged(); }
        }

        public Editor TheEditor => Editor.TheEditor;
        public LCSSave TheSave => Editor.TheEditor.ActiveFile;

        public int NumPackagesCollected
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Collectibles.NumPackages; i++)
                {
                    if (TheEditor.GetGlobal(GlobalVariable.Package1Collected + i) != 0) count++;
                }

                return count;
            }
        }

        public int NumRampagesCollected
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Collectibles.NumRampages; i++)
                {
                    if (TheEditor.GetGlobal(GlobalVariable.Rampage1Passed + i) != 0) count++;
                }

                return count;
            }
        }

        public int NumStuntJumpsCollected
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Collectibles.NumStuntJumps; i++)
                {
                    if (TheEditor.GetGlobal(GlobalVariable.StuntJump1Completed + i) != 0) count++;
                }

                return count;
            }
        }

        public MapWindow()
        {
            MapOverlays = new ObservableCollection<UIElement>();
            Blips = new ObservableCollection<CollectibleBlip>();
        }

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < Collectibles.NumPackages; i++) CreateAndRegisterBlip(BlipType.Package, i, Collectibles.Packages[i]);
            for (int i = 0; i < Collectibles.NumRampages; i++) CreateAndRegisterBlip(BlipType.Rampage, i, Collectibles.Rampages[i]);
            for (int i = 0; i < Collectibles.NumStuntJumps; i++) CreateAndRegisterBlip(BlipType.StuntJump, i, Collectibles.StuntJumps[i]);

            TheEditor.FileOpened += TheEditor_FileOpened;
            TheEditor.FileClosing += TheEditor_FileClosing;

            RegisterChangeHandlers();
            PlotCollectibles();
        }

        public override void Shutdown()
        {
            base.Shutdown();

            foreach (var b in Blips) b.PropertyChanged -= Blip_PropertyChanged;
            Blips.Clear();

            TheEditor.FileOpened -= TheEditor_FileOpened;
            TheEditor.FileClosing -= TheEditor_FileClosing;

            UnregisterChangeHandlers();
        }

        public void RegisterChangeHandlers()
        {
            if (!m_handlersRegistered && TheSave != null)
            {
                TheSave.Scripts.GlobalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
                m_handlersRegistered = true;
            }
        }

        public void UnregisterChangeHandlers()
        {
            if (m_handlersRegistered && TheSave != null)
            {
                TheSave.Scripts.GlobalVariables.CollectionChanged -= GlobalVariables_CollectionChanged;
                m_handlersRegistered = false;
            }
        }

        public void CreateAndRegisterBlip(BlipType type, int index, Point coords)
        {
            CollectibleBlip blip = new CollectibleBlip() { Type = type, Index = index, Coords = coords };
            blip.PropertyChanged += Blip_PropertyChanged;
            Blips.Add(blip);
        }

        public void PlotCollectibles()
        {
            if (IsShowingPackages) PlotAll(BlipType.Package);
            if (IsShowingRampages) PlotAll(BlipType.Rampage);
            if (IsShowingStuntJumps) PlotAll(BlipType.StuntJump);
        }

        public void PlotAll(BlipType type)
        {
            int stateArrayBase = -1;
            switch (type)
            {
                case BlipType.Package: stateArrayBase = (int) GlobalVariable.Package1Collected; break;
                case BlipType.Rampage: stateArrayBase = (int) GlobalVariable.Rampage1Passed; break;
                case BlipType.StuntJump: stateArrayBase = (int) GlobalVariable.StuntJump1Completed; break;
            }

            var b = Blips.Where(x => x.Type == type).ToArray();
            for (int i = 0; i < b.Length; i++)
            {
                int state = TheEditor.GetGlobal(stateArrayBase + i);
                b[i].IsCollected = (state != 0);
                b[i].IsEnabled = true;
            }
        }

        public void HideAll(BlipType type)
        {
            foreach (var b in Blips.Where(x => x.Type == type))
            {
                b.IsEnabled = false;
            }
        }

        public void DrawBlip(CollectibleBlip blip)
        {
            int size = 5;
            double thickness = 2;
            string toolTip =
                (blip.Type == BlipType.Package) ? $"Hidden Package #{blip.Index + 1}" :
                (blip.Type == BlipType.Rampage) ? $"Rampage #{blip.Index + 1}" :
                (blip.Type == BlipType.StuntJump) ? $"Unique Stunt Jump #{blip.Index + 1}" : "";

            if (blip.IsCollected)
            {
                size = 3;
                thickness = 1.5;
                toolTip += " (collected)";
            }

            blip.Sprite = MakeSprite(blip.Coords,
                scale: size,
                thickness: thickness,
                color: (int) blip.Type,
                isBright: blip.IsCollected,
                toolTip: toolTip
                    
            );
            MapOverlays.Add(blip.Sprite);
        }

        public void ClearBlip(CollectibleBlip blip)
        {
            MapOverlays.Remove(blip.Sprite);
        }

        private UIElement MakeSprite(Point loc,
            int scale = 4, double thickness = 1, int angle = 0,
            int color = 0, bool isBright = true,
            string toolTip = null)
        {
            const double Size = 4;
            double actualSize = Size * scale;

            SolidColorBrush brush = new SolidColorBrush
            {
                Color = GetBlipColor(color, isBright)
            };

            Rectangle rect = new Rectangle
            {
                Fill = brush,
                StrokeThickness = thickness,
                Stroke = Brushes.Black,
                Width = actualSize,
                Height = actualSize,
                ToolTip = toolTip
            };

            ApplyBlipTransform(rect, loc, angle, actualSize);
            return rect;
        }

        private static void ApplyBlipTransform(FrameworkElement e, Point loc,
            double angle = 0, double size = 1)
        {
            const double Root2Over2 = 0.707107;
            double hypAngleRad = (Math.PI * (angle + 45)) / 180.0;

            double centerX = (size / 2) * (Math.Cos(hypAngleRad) / Root2Over2);
            double centerY = (size / 2) * (Math.Sin(hypAngleRad) / Root2Over2);
            double scaleX = size / e.Width;
            double scaleY = size / e.Width;

            Point p = GetPixelCoords(loc);
            Matrix m = Matrix.Identity;
            m.Rotate(angle);
            m.OffsetX = p.X - centerX;
            m.OffsetY = p.Y - centerY;
            m.ScalePrepend(scaleX, scaleY);

            e.RenderTransform = new MatrixTransform(m);
        }

        private static Point GetPixelCoords(Point loc)
        {
            return new Point()
            {
                X = (loc.X * Scale.X) + Origin.X,
                Y = (loc.Y * Scale.Y) + Origin.Y,
            };
        }

        private Color GetBlipColor(int colorId, bool isBright)
        {
            if (colorId >= 0 && colorId < 7)
            {
                int colorIndex = colorId * 2;
                if (isBright) colorIndex++;
                return BlipColors[colorIndex];
            }

            byte r = (byte) (colorId >> 24);
            byte g = (byte) (colorId >> 16);
            byte b = (byte) (colorId >> 8);
            return Color.FromRgb(r, g, b);
        }

        public ObservableCollection<Color> BlipColors => new ObservableCollection<Color>()
        {
            PackageColor,
            PackageColorCollected,
            RampageColor,
            RampageColorCollected,
            StuntJumpColor,
            StuntJumpColorCollected,
            SpawnPointColor
        };

        private void TheEditor_FileOpened(object sender, EventArgs e)
        {
            RegisterChangeHandlers();
            PlotCollectibles();
        }

        private void TheEditor_FileClosing(object sender, EventArgs e)
        {
            UnregisterChangeHandlers();
            HideAll(BlipType.Package);
            HideAll(BlipType.Rampage);
            HideAll(BlipType.StuntJump);
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Replace)
            {
                return;
            }
            
            int packageMin = TheEditor.GetIndexOfGlobal(GlobalVariable.Package1Collected);
            int packageMax = TheEditor.GetIndexOfGlobal(GlobalVariable.Package100Collected);
            int rampageMin = TheEditor.GetIndexOfGlobal(GlobalVariable.Rampage1Passed);
            int rampageMax = TheEditor.GetIndexOfGlobal(GlobalVariable.Rampage20Passed);
            int stuntJumpMin = TheEditor.GetIndexOfGlobal(GlobalVariable.StuntJump1Completed);
            int stuntJumpMax = TheEditor.GetIndexOfGlobal(GlobalVariable.StuntJump26Completed);
            int index = e.NewStartingIndex;
            bool isCollected = ((int) e.NewItems[0]) != 0;

            CollectibleBlip blip = null;
            if (index >= packageMin && index <= packageMax)
            {
                blip = Blips.Where(x => x.Type == BlipType.Package && x.Index == (index - packageMin)).FirstOrDefault();
            }
            if (index >= rampageMin && index <= rampageMax)
            {
                blip = Blips.Where(x => x.Type == BlipType.Rampage && x.Index == (index - rampageMin)).FirstOrDefault();
            }
            if (index >= stuntJumpMin && index <= stuntJumpMax)
            {
                blip = Blips.Where(x => x.Type == BlipType.StuntJump && x.Index == (index - stuntJumpMin)).FirstOrDefault();
            }

            if (blip != null)
            {
                blip.IsCollected = isCollected;
                if (!isCollected || (isCollected && IsShowingCollected))
                {
                    blip.IsEnabled = true;
                }
            }
        }

        private void Blip_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is CollectibleBlip b) || e.PropertyName == nameof(CollectibleBlip.Sprite))
            {
                return;
            }

            if (e.PropertyName == nameof(CollectibleBlip.IsCollected))
            {
                switch (b.Type)
                {
                    case BlipType.Package: OnPropertyChanged(nameof(NumPackagesCollected)); break;
                    case BlipType.Rampage: OnPropertyChanged(nameof(NumRampagesCollected)); break;
                    case BlipType.StuntJump: OnPropertyChanged(nameof(NumStuntJumpsCollected)); break;
                }
            }

            ClearBlip(b);
            if (b.IsEnabled && (!b.IsCollected || (b.IsCollected && IsShowingCollected)))
            {
                DrawBlip(b);
            }
        }

        public ICommand TogglePackagesCommand => new RelayCommand(() => IsShowingPackages = !IsShowingPackages);
        public ICommand ToggleRampagesCommand => new RelayCommand(() => IsShowingRampages = !IsShowingRampages);
        public ICommand ToggleStuntJumpsCommand => new RelayCommand(() => IsShowingStuntJumps = !IsShowingStuntJumps);
        public ICommand ToggleLegendCommand => new RelayCommand(() => IsShowingLegend = !IsShowingLegend);
        public ICommand ToggleCollectedCommand => new RelayCommand(() => IsShowingCollected = !IsShowingCollected);
    }
}
