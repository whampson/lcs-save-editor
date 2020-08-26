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
        public const int NumPackages = 100;
        public const int NumRampages = 20;
        public const int NumStuntJumps = 26;

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
                for (int i = 0; i < NumPackages; i++)
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
                for (int i = 0; i < NumRampages; i++)
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
                for (int i = 0; i < NumStuntJumps; i++)
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

            for (int i = 0; i < NumPackages; i++) CreateAndRegisterBlip(BlipType.Package, i, Packages[i]);
            for (int i = 0; i < NumRampages; i++) CreateAndRegisterBlip(BlipType.Rampage, i, Rampages[i]);
            for (int i = 0; i < NumStuntJumps; i++) CreateAndRegisterBlip(BlipType.StuntJump, i, StuntJumps[i]);

            TheEditor.FileOpened += TheEditor_FileOpened;
            TheEditor.FileClosing += TheEditor_FileClosing;

            PlotCollectibles();
        }

        public override void Shutdown()
        {
            base.Shutdown();

            foreach (var b in Blips) b.PropertyChanged -= Blip_PropertyChanged;
            Blips.Clear();

            TheEditor.FileOpened -= TheEditor_FileOpened;
            TheEditor.FileClosing -= TheEditor_FileClosing;
        }

        public void RegisterChangeHandlers()
        {
            if (!m_handlersRegistered)
            {
                TheSave.Scripts.GlobalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
                m_handlersRegistered = true;
            }
        }

        public void UnregisterChangeHandlers()
        {
            if (m_handlersRegistered)
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


        public static IList<Point> Packages => new List<Point>
        {
            new Point(1287.02, 292.76),
            new Point(722.17, 249.19),
            new Point(971.04, 197.93),
            new Point(1142.48, 68.04),
            new Point(908.86, 10.88),
            new Point(1512.27, 21.02),
            new Point(1159.89, -75.87),
            new Point(1256.88, -138.59),
            new Point(922.52, -167.11),
            new Point(1148.03, -255.0),
            new Point(944.54, -274.63),
            new Point(1627.24, -272.5),
            new Point(889.5, -300.21),
            new Point(1012.17, -345.3),
            new Point(1254.67, -315.84),
            new Point(1444.56, -282.86),
            new Point(1646.57, -350.35),
            new Point(1081.95, -402.48),
            new Point(1149.7, -379.59),
            new Point(825.93, -399.22),
            new Point(1321.55, -465.75),
            new Point(1319.2, -535.67),
            new Point(773.21, -586.65),
            new Point(1099.09, -572.32),
            new Point(1178.73, -566.11),
            new Point(1576.76, -644.86),
            new Point(874.65, -659.55),
            new Point(1595.06, -700.58),
            new Point(1203.06, -706.73),
            new Point(1214.2, -816.75),
            new Point(1010.57, -867.5),
            new Point(1402.59, -862.7),
            new Point(1246.94, -913.47),
            new Point(792.05, -927.81),
            new Point(841.65, -932.92),
            new Point(1534.23, -934.79),
            new Point(1300.37, -954.67),
            new Point(1391.32, -1024.32),
            new Point(1204.77, -1129.27),
            new Point(1115.52, -1248.15),
            new Point(178.98, 175.83),
            new Point(-117.42, 93.59),
            new Point(295.27, 14.94),
            new Point(539.91, -23.96),
            new Point(365.31, 7.32),
            new Point(369.69, -209.08),
            new Point(244.19, -135.22),
            new Point(396.16, -173.41),
            new Point(536.09, -206.42),
            new Point(-180.01, -342.82),
            new Point(-56.26, -340.3),
            new Point(375.4, -605.92),
            new Point(480.96, -603.74),
            new Point(-24.04, -658.52),
            new Point(342.16, -854.23),
            new Point(166.77, -931.74),
            new Point(534.57, -940.26),
            new Point(297.2, -1320.45),
            new Point(97.36, -1430.04),
            new Point(253.71, -1554.09),
            new Point(-120.8, -1175.08),
            new Point(174.92, -1252.93),
            new Point(438.17, -870.74),
            new Point(490.16, -1426.32),
            new Point(93.13, -723.72),
            new Point(425.18, -374.08),
            new Point(302.0, -319.88),
            new Point(398.71, -7.39),
            new Point(709.67, -282.09),
            new Point(265.51, -835.62),
            new Point(-756.44, -569.69),
            new Point(-1096.98, -842.63),
            new Point(-413.85, -139.61),
            new Point(-1102.79, -68.79),
            new Point(-658.09, 649.08),
            new Point(-1396.65, 279.89),
            new Point(-1073.62, 36.2),
            new Point(-1042.93, -690.71),
            new Point(-801.46, 122.61),
            new Point(-1089.33, -605.08),
            new Point(-519.55, -67.8),
            new Point(-956.36, -149.19),
            new Point(-1047.26, -142.35),
            new Point(-1142.87, 332.98),
            new Point(-1183.59, -613.89),
            new Point(-1130.59, -194.5),
            new Point(-632.4, -584.18),
            new Point(-524.23, 301.29),
            new Point(-412.6, 155.45),
            new Point(-1182.68, -305.1),
            new Point(-1087.1, 445.89),
            new Point(-1237.9, 67.76),
            new Point(-977.48, -1292.52),
            new Point(-730.17, -271.01),
            new Point(-193.52, 216.95),
            new Point(-848.46, -698.1),
            new Point(-1076.0, 208.19),
            new Point(-800.24, -94.97),
            new Point(-1183.42, -45.02),
            new Point(-1412.76, -612.61),
        };

        public static IList<Point> Rampages => new List<Point>
        {
            new Point(861.748, -663.76),
            new Point(849.0, -476.0),
            new Point(967.45, -65.0),
            new Point(1344.0, -383.0),
            new Point(1184.0, -146.0),
            new Point(1253.78, -530.23),
            new Point(189.84, -642.3),
            new Point(104.0, -962.75),
            new Point(244.32, -1188.05),
            new Point(130.25, -41.0),
            new Point(65.01, -1589.96),
            new Point(47.0, -640.6),
            new Point(-163.5, -1406.5),
            new Point(-1082.5, -24.5),
            new Point(-1205.5, 551.0),
            new Point(-848.75, -184.5),
            new Point(-1040.82, -271.38),
            new Point(-501.0, -44.5),
            new Point(-458.21, 287.37),
            new Point(-544.1, 34.3),
        };

        public static IList<Point> StuntJumps => new List<Point>
        {
            new Point(1402.772, -841.776),
            new Point(846.64, 215.21),
            new Point(1484.0, -614.1),
            new Point(1284.4, -814.2),
            new Point(892.22, -1133.5),
            new Point(797.3, -873.9),
            new Point(1177.6, -639.4),
            new Point(1408.7, -925.5),
            new Point(921.3, -1001.0),
            new Point(777.84, 141.82),
            new Point(1370.43, -506.7),
            new Point(1015.24, -329.3),
            new Point(929.22, 20.93),
            new Point(-1251.19, 22.03),
            new Point(-1075.17, -205.36),
            new Point(-999.2, -45.1),
            new Point(-954.7, -145.9),
            new Point(-728.8, 50.3),
            new Point(393.7, -1564.22),
            new Point(472.59, -122.01),
            new Point(-109.87, 95.62),
            new Point(-95.12, -864.81),
            new Point(441.17, -1127.87),
            new Point(-221.9, -1358.77),
            new Point(-780.3, -127.0),
            new Point(-1159.3, 166.46),
        };
    }
}
