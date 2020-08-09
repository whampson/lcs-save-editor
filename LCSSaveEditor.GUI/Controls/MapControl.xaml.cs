using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.Controls
{
    /// <summary>
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        #region Defaults
        public const double DefaultZoom = 1.0;
        public const double DefaultMinimumZoom = 0.1;
        public const double DefaultMaximumZoom = 5.0;
        public const double DefaultZoomDelta = 0.1;
        public const double DefaultPanDelta = 10;
        public const bool DefaultPanWithKeyboard = true;
        public const bool DefaultPanWithMouseDrag = true;
        public const bool DefaultZoomWithKeyboard = true;
        public const bool DefaultZoomWithMouseWheel = true;
        public const Key DefaultPanUpKey = Key.Up;
        public const Key DefaultPanDownKey = Key.Down;
        public const Key DefaultPanLeftKey = Key.Left;
        public const Key DefaultPanRightKey = Key.Right;
        public const Key DefaultZoomInKey = Key.OemPlus;
        public const Key DefaultZoomOutKey = Key.OemMinus;
        public static readonly Vector DefaultScale = new Vector(1, 1);
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the render transform matrix.
        /// </summary>
        public Matrix RenderMatrix
        {
            get { return (Matrix) GetValue(RenderMatrixProperty); }
            set { SetValue(RenderMatrixProperty, value); }
        }

        /// <summary>
        /// Gets or sets the map image.
        /// </summary>
        public BitmapImage Image
        {
            get { return (BitmapImage) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ratio between distance on the map
        /// and distance in the world.
        /// </summary>
        /// <remarks>
        /// The map distance is measured in pixels.
        /// </remarks>
        public Vector Scale
        {
            get { return (Vector) GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the map/world coodinate origin.
        /// </summary>
        /// <remarks>
        /// The coordinate origin is measured as a pixel offset from
        /// the top left corner of the map iamge.
        /// </remarks>
        public Point Origin
        {
            get { return (Point) GetValue(OriginProperty); }
            set { SetValue(OriginProperty, value); }
        }

        /// <summary>
        /// Gets or sets centerpoint of the viewport.
        /// </summary>
        /// <remarks>
        /// The center is measured as a map coordinate
        /// (measured in pixels) centered about the
        /// <see cref="Origin"/>.
        /// </remarks>
        public Point Center
        {
            get { return (Point) GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the initial value of <see cref="Center"/>.
        /// </summary>
        public Point InitialCenter
        {
            get { return (Point) GetValue(InitialCenterProperty); }
            set { SetValue(InitialCenterProperty, value); }
        }

        /// <summary>
        /// Gets or sets effective panning speed when using the
        /// keyboard to pan.
        /// </summary>
        public double PanDelta
        {
            get { return (double) GetValue(PanDeltaProperty); }
            set { SetValue(PanDeltaProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether to allow panning with the keyboard.
        /// </summary>
        public bool PanWithKeyboard
        {
            get { return (bool) GetValue(PanWithKeyboardProperty); }
            set { SetValue(PanWithKeyboardProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether to allow panning by dragging the mouse.
        /// </summary>
        public bool PanWithMouseDrag
        {
            get { return (bool) GetValue(PanWithMouseDragProperty); }
            set { SetValue(PanWithMouseDragProperty, value); }
        }

        /// <summary>
        /// Gets or sets the key used to pan up.
        /// </summary>
        public Key PanUpKey
        {
            get { return (Key) GetValue(PanUpKeyProperty); }
            set { SetValue(PanUpKeyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the key used to pan down.
        /// </summary>
        public Key PanDownKey
        {
            get { return (Key) GetValue(PanDownKeyProperty); }
            set { SetValue(PanDownKeyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the key used to pan left.
        /// </summary>
        public Key PanLeftKey
        {
            get { return (Key) GetValue(PanLeftKeyProperty); }
            set { SetValue(PanLeftKeyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the key used to pan right.
        /// </summary>
        public Key PanRightKey
        {
            get { return (Key) GetValue(PanRightKeyProperty); }
            set { SetValue(PanRightKeyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mouse button to use for panning.
        /// </summary>
        public MouseButton PanWithMouseDragButton
        {
            get { return (MouseButton) GetValue(PanWithMouseDragButtonProperty); }
            set { SetValue(PanWithMouseDragButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets the magnification scale.
        /// </summary>
        public double Zoom
        {
            get { return (double) GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the initial value of <see cref="Zoom"/>.
        /// </summary>
        public double InitialZoom
        {
            get { return (double) GetValue(InitialZoomProperty); }
            set { SetValue(InitialZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum zoom value.
        /// </summary>
        public double MinimumZoom
        {
            get { return (double) GetValue(MinimumZoomProperty); }
            set { SetValue(MinimumZoomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum zoom value.
        /// </summary>
        public double MaximumZoom
        {
            get { return (double) GetValue(MaximumZoomProperty); }
            set { SetValue(MaximumZoomProperty, value); }
        }

        /// <summary>
        /// The amount to zoom when using the mouse wheel or keyboard.
        /// </summary>
        public double ZoomDelta
        {
            get { return (double) GetValue(ZoomDeltaProperty); }
            set { SetValue(ZoomDeltaProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether to enable zooming with the keyboard.
        /// </summary>
        public bool ZoomWithKeyboard
        {
            get { return (bool) GetValue(ZoomWithKeyboardProperty); }
            set { SetValue(ZoomWithKeyboardProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether to enable zooming with the mouse wheel.
        /// </summary>
        public bool ZoomWithMouseWheel
        {
            get { return (bool) GetValue(ZoomWithMouseWheelProperty); }
            set { SetValue(ZoomWithMouseWheelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the key used to zoom in.
        /// </summary>
        public Key ZoomInKey
        {
            get { return (Key) GetValue(ZoomInKeyProperty); }
            set { SetValue(ZoomInKeyProperty, value); }
        }

        /// <summary>
        /// Gets or set the key used to zoom out.
        /// </summary>
        public Key ZoomOutKey
        {
            get { return (Key) GetValue(ZoomOutKeyProperty); }
            set { SetValue(ZoomOutKeyProperty, value); }
        }

        /// <summary>
        /// Gets the point in the view the mouse was last clicked.
        /// </summary>
        public Point MouseClickPoint
        {
            get { return (Point) GetValue(MouseClickPointProperty); }
            set { SetValue(MouseClickPointProperty, value); }
        }

        /// <summary>
        /// Gets the pixel offset from the <see cref="Origin"/> where
        /// the mouse was last clicked.
        /// </summary>
        public Point MouseClickOffset
        {
            get { return (Point) GetValue(MouseClickOffsetProperty); }
            set { SetValue(MouseClickOffsetProperty, value); }
        }

        /// <summary>
        /// Gets the world coordinates where the mouse was last clicked.
        /// </summary>
        public Point MouseClickCoords
        {
            get { return (Point) GetValue(MouseClickCoordsProperty); }
            set { SetValue(MouseClickCoordsProperty, value); }
        }

        /// <summary>
        /// Gets the point in the view where the mouse was last moved.
        /// </summary>
        public Point MouseOverPoint
        {
            get { return (Point) GetValue(MouseOverPointProperty); }
            set { SetValue(MouseOverPointProperty, value); }
        }

        /// <summary>
        /// Gets the pixel offset from the <see cref="Origin"/> where
        /// the mouse was last moved.
        /// </summary>
        public Point MouseOverOffset
        {
            get { return (Point) GetValue(MouseOverOffsetProperty); }
            set { SetValue(MouseOverOffsetProperty, value); }
        }

        /// <summary>
        /// Gets the world coordinates where the mouse was last moved.
        /// </summary>
        public Point MouseOverCoords
        {
            get { return (Point) GetValue(MouseOverCoordsProperty); }
            set { SetValue(MouseOverCoordsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection containing map overlays, such as labels or blips.
        /// </summary>
        public ObservableCollection<UIElement> Overlays
        {
            get { return (ObservableCollection<UIElement>) GetValue(OverlaysProperty); }
            set { SetValue(OverlaysProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty RenderMatrixProperty = DependencyProperty.Register(
            nameof(RenderMatrix), typeof(Matrix), typeof(MapControl),
            new PropertyMetadata(default(Matrix)));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(BitmapImage), typeof(MapControl),
            new PropertyMetadata(default(BitmapImage)));

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            nameof(Scale), typeof(Vector), typeof(MapControl),
            new PropertyMetadata(DefaultScale));

        public static readonly DependencyProperty OriginProperty = DependencyProperty.Register(
            nameof(Origin), typeof(Point), typeof(MapControl),
            new PropertyMetadata(default(Point)));

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            nameof(Center), typeof(Point), typeof(MapControl),
            new FrameworkPropertyMetadata(
                default(Point),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OffsetPropertyChanged));

        public static readonly DependencyProperty InitialCenterProperty = DependencyProperty.Register(
            nameof(InitialCenter), typeof(Point), typeof(MapControl),
            new PropertyMetadata(default(Point)));

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            nameof(Zoom), typeof(double), typeof(MapControl),
            new FrameworkPropertyMetadata(
                DefaultZoom,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(ZoomPropertyChanged),
                new CoerceValueCallback(ZoomCoerceValue)));

        public static readonly DependencyProperty InitialZoomProperty = DependencyProperty.Register(
            nameof(InitialZoom), typeof(double), typeof(MapControl),
            new PropertyMetadata(DefaultZoom));

        public static readonly DependencyProperty MinimumZoomProperty = DependencyProperty.Register(
            nameof(MinimumZoom), typeof(double), typeof(MapControl),
            new PropertyMetadata(DefaultMinimumZoom));

        public static readonly DependencyProperty MaximumZoomProperty = DependencyProperty.Register(
            nameof(MaximumZoom), typeof(double), typeof(MapControl),
            new PropertyMetadata(DefaultMaximumZoom));

        public static readonly DependencyProperty ZoomDeltaProperty = DependencyProperty.Register(
            nameof(ZoomDelta), typeof(double), typeof(MapControl),
            new PropertyMetadata(DefaultZoomDelta));

        public static readonly DependencyProperty ZoomWithKeyboardProperty = DependencyProperty.Register(
            nameof(ZoomWithKeyboard), typeof(bool), typeof(MapControl),
            new PropertyMetadata(DefaultZoomWithKeyboard));

        public static readonly DependencyProperty ZoomWithMouseWheelProperty = DependencyProperty.Register(
            nameof(ZoomWithMouseWheel), typeof(bool), typeof(MapControl),
            new PropertyMetadata(DefaultZoomWithMouseWheel));

        public static readonly DependencyProperty PanDeltaProperty = DependencyProperty.Register(
            nameof(PanDelta), typeof(double), typeof(MapControl),
            new PropertyMetadata(DefaultPanDelta));

        public static readonly DependencyProperty PanWithKeyboardProperty = DependencyProperty.Register(
            nameof(PanWithKeyboard), typeof(bool), typeof(MapControl),
            new PropertyMetadata(DefaultPanWithKeyboard));

        public static readonly DependencyProperty PanWithMouseDragProperty = DependencyProperty.Register(
            nameof(PanWithMouseDrag), typeof(bool), typeof(MapControl),
            new PropertyMetadata(DefaultPanWithMouseDrag));

        public static readonly DependencyProperty PanUpKeyProperty = DependencyProperty.Register(
            nameof(PanUpKey), typeof(Key), typeof(MapControl),
            new PropertyMetadata(DefaultPanUpKey));

        public static readonly DependencyProperty PanDownKeyProperty = DependencyProperty.Register(
            nameof(PanDownKey), typeof(Key), typeof(MapControl),
            new PropertyMetadata(DefaultPanDownKey));

        public static readonly DependencyProperty PanLeftKeyProperty = DependencyProperty.Register(
            nameof(PanLeftKey), typeof(Key), typeof(MapControl),
            new PropertyMetadata(DefaultPanLeftKey));

        public static readonly DependencyProperty PanRightKeyProperty = DependencyProperty.Register(
            nameof(PanRightKey), typeof(Key), typeof(MapControl),
            new PropertyMetadata(DefaultPanRightKey));

        public static readonly DependencyProperty ZoomInKeyProperty = DependencyProperty.Register(
            nameof(ZoomInKey), typeof(Key), typeof(MapControl),
            new PropertyMetadata(DefaultZoomInKey));

        public static readonly DependencyProperty ZoomOutKeyProperty = DependencyProperty.Register(
            nameof(ZoomOutKey), typeof(Key), typeof(MapControl),
            new PropertyMetadata(DefaultZoomOutKey));

        public static readonly DependencyProperty PanWithMouseDragButtonProperty = DependencyProperty.Register(
            nameof(PanWithMouseDragButton), typeof(MouseButton), typeof(MapControl),
            new PropertyMetadata(default(MouseButton)));

        private static readonly DependencyProperty MouseClickPointProperty = DependencyProperty.Register(
            nameof(MouseClickPoint), typeof(Point), typeof(MapControl),
            new FrameworkPropertyMetadata(
                default(Point),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static readonly DependencyProperty MouseClickOffsetProperty = DependencyProperty.Register(
            nameof(MouseClickOffset), typeof(Point), typeof(MapControl),
            new FrameworkPropertyMetadata(
                default(Point),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static readonly DependencyProperty MouseClickCoordsProperty = DependencyProperty.Register(
            nameof(MouseClickCoords), typeof(Point), typeof(MapControl),
            new FrameworkPropertyMetadata(
                default(Point),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static readonly DependencyProperty MouseOverPointProperty = DependencyProperty.Register(
            nameof(MouseOverPoint), typeof(Point), typeof(MapControl),
            new FrameworkPropertyMetadata(
                default(Point),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static readonly DependencyProperty MouseOverOffsetProperty = DependencyProperty.Register(
            nameof(MouseOverOffset), typeof(Point), typeof(MapControl),
            new FrameworkPropertyMetadata(
                default(Point),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static readonly DependencyProperty MouseOverCoordsProperty = DependencyProperty.Register(
            nameof(MouseOverCoords), typeof(Point), typeof(MapControl),
            new FrameworkPropertyMetadata(
                default(Point),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty OverlaysProperty = DependencyProperty.Register(
            nameof(Overlays), typeof(ObservableCollection<UIElement>), typeof(MapControl),
            new FrameworkPropertyMetadata(
                new ObservableCollection<UIElement>(),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OverlaysPropertyChanged)));
        #endregion

        #region Private Fields
        private Point m_prePanRenderOffset;
        private bool m_suppressZoomChangedHandler;
        private bool m_suppressOffsetChangedHandler;
        private bool m_initialResetDone;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="MapControl"/> instance.
        /// </summary>
        public MapControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Resets the pan offset and zoom to their initial values.
        /// </summary>
        public void Reset()
        {
            RenderMatrix = Matrix.Identity;
            Center = InitialCenter;
            Zoom = InitialZoom;

            ApplyZoom();
        }

        public void ZoomIn()
        {
            ZoomCenter(ZoomDelta);
        }

        public void ZoomOut()
        {
            ZoomCenter(-ZoomDelta);
        }

        /// <summary>
        /// Translates a set of pixel coordinates into a set of world coordinates.
        /// </summary>
        /// <remarks>
        /// Pixel coordinates are measured from the top-left of the map image.
        /// World coordinates are determined based on the <see cref="Origin"/>
        /// and <see cref="Scale"/> properties.
        /// </remarks>
        public Point PixelToWorld(Point pixel)
        {
            return new Point()
            {
                X = (pixel.X - Origin.X) / Scale.X,
                Y = (pixel.Y - Origin.Y) / Scale.Y,
            };
        }

        /// <summary>
        /// Translates a set of world coordinates into a set of pixel coordinates.
        /// </summary>
        /// <remarks>
        /// Pixel coordinates are measured from the top-left of the map image.
        /// World coordinates are determined based on the <see cref="Origin"/>
        /// and <see cref="Scale"/> properties.
        /// </remarks>
        public Point WorldToPixel(Point world)
        {
            return new Point()
            {
                X = (world.X * Scale.X) + Origin.X,
                Y = (world.Y * Scale.Y) + Origin.Y,
            };
        }

        /// <summary>
        /// Translates a set of pixel coordinates into a set of map coordinates.
        /// </summary>
        /// <remarks>
        /// Pixel coordinates are measured from the top-left of the map image.
        /// Map coordinates are pixel coordinates centered about the <see cref="Origin"/>.
        /// </remarks>
        public Point PixelToMap(Point pixel)
        {
            return new Point()
            {
                X = pixel.X - Origin.X,
                Y = pixel.Y - Origin.Y
            };
        }

        /// <summary>
        /// Translates a set of map coordinates into a set of pixel coordinates.
        /// </summary>
        /// <remarks>
        /// Pixel coordinates are measured from the top-left of the map image.
        /// Map coordinates are pixel coordinates centered about the <see cref="Origin"/>.
        /// </remarks>
        public Point MapToPixel(Point map)
        {
            return new Point()
            {
                X = map.X + Origin.X,
                Y = map.Y + Origin.Y,
            };
        }

        public Point WorldToMap(Point world)
        {
            return new Point()
            {
                X = world.X * Scale.X,
                Y = world.Y * Scale.Y,
            };
        }

        public Point MapToWorld(Point map)
        {
            return new Point()
            {
                X = map.X / Scale.X,
                Y = map.Y / Scale.Y,
            };
        }
        #endregion

        #region Private Functions
        private void AddOverlay(UIElement overlay)
        {
            m_canvas.Children.Add(overlay);
        }

        private void AddAllOverlays()
        {
            foreach (var item in Overlays) AddOverlay(item);
        }

        private void RemoveOverlay(UIElement overlay)
        {
            m_canvas.Children.Remove(overlay);
        }

        private void RemoveAllOverlays()
        {
            // Map lives at index 0, keep that!
            m_canvas.Children.RemoveRange(1, m_canvas.Children.Count - 1);
        }

        private double GetZoomScale(double delta)
        {
            double scale = 1;
            if (delta > 0) scale = (delta + 1);
            if (delta < 0) scale = (1 / (-delta + 1));

            return scale;
        }

        /// <summary>
        /// Magnifies the map to the current Zoom value
        /// then pans to Center.
        /// </summary>
        private void ApplyZoom()
        {
            Matrix m = Matrix.Identity;
            m.ScalePrepend(Zoom, Zoom);
            RenderMatrix = m;

            PanToCenter();
        }

        /// <summary>
        /// Applies an incremental zoom over a point.
        /// </summary>
        private void ZoomToPoint(Point p, double delta)
        {
            double scale = GetZoomScale(delta);
            if (Zoom * scale < MinimumZoom) return;
            if (Zoom * scale > MaximumZoom) return;

            m_suppressZoomChangedHandler = true;
            Zoom *= scale;
            m_suppressZoomChangedHandler = false;

            Matrix m = RenderMatrix;
            m.ScaleAtPrepend(scale, scale, p.X, p.Y);
            RenderMatrix = m;

            UpdateCenter();
        }

        private void ZoomCenter(double delta)
        {
            double scale = GetZoomScale(delta);
            double newZoom = Zoom * scale;
            if (newZoom >= MinimumZoom && newZoom <= MaximumZoom)
            {
                Zoom = newZoom;
            }
        }

        /// <summary>
        /// Pans the map by an X and Y amount.
        /// </summary>
        private void Pan(double xDelta, double yDelta)
        {
            Point p = Center;
            p.X += xDelta;
            p.Y += yDelta;
            Center = p;
        }

        /// <summary>
        /// Pans the map to the Center corrdinate.
        /// </summary>
        private void PanToCenter()
        {
            double offsetX = (m_border.ActualWidth / 2) - ((Origin.X - Center.X) * Zoom);
            double offsetY = (m_border.ActualHeight / 2) - ((Origin.Y - Center.Y) * Zoom);
            SetRenderMatrixOffset(offsetX, offsetY);
        }


        /// <summary>
        /// Ensures the Center coordinate is in-sync with the render matrix offset.
        /// </summary>
        private void UpdateCenter()
        {
            Point offset = GetRenderMatrixOffset();
            double x = Origin.X + ((offset.X - (m_border.ActualWidth / 2)) / Zoom);
            double y = Origin.Y + ((offset.Y - (m_border.ActualHeight / 2)) / Zoom);

            m_suppressOffsetChangedHandler = true;      // We don't need to pan the map again
            Center = new Point(x, y);
            m_suppressOffsetChangedHandler = false;
        }

        /// <summary>
        /// Gets the render matrix translation offset.
        /// </summary>
        private Point GetRenderMatrixOffset()
        {
            return new Point(RenderMatrix.OffsetX, RenderMatrix.OffsetY);
        }

        /// <summary>
        /// Sets the render matrix translation offset.
        /// </summary>
        private void SetRenderMatrixOffset(double x, double y)
        {
            Matrix m = RenderMatrix;
            m.OffsetX = x;
            m.OffsetY = y;
            RenderMatrix = m;
        }
        #endregion

        #region Event Handlers
        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (PanWithKeyboard || ZoomWithKeyboard)
            {
                Keyboard.Focus(m_canvas);
            }
        }

        private void MapControl_LayoutUpdated(object sender, EventArgs e)
        {
            if (ActualHeight > 0 || ActualWidth > 0)
            {
                if (!m_initialResetDone)
                {
                    Reset();
                    m_initialResetDone = true;
                }
            }
        }
        private void MapControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCenter();
        }

        private void MapControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PanWithKeyboard || ZoomWithKeyboard)
            {
                Keyboard.Focus(m_canvas);
            }

            if (!m_canvas.IsMouseCaptured)
            {
                Point borderPos = e.MouseDevice.GetPosition(m_border);
                Point imagePos = e.MouseDevice.GetPosition(m_image);

                MouseClickPoint = borderPos;
                MouseClickOffset = PixelToMap(imagePos);
                MouseClickCoords = PixelToWorld(imagePos);

                if (PanWithMouseDrag && PanWithMouseDragButton == e.ChangedButton)
                {
                    m_prePanRenderOffset = GetRenderMatrixOffset();
                    m_canvas.CaptureMouse();
                }
            }
        }

        private void MapControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (m_canvas.IsMouseCaptured)
            {
                m_canvas.ReleaseMouseCapture();
            }
        }

        private void MapControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point borderPos = e.MouseDevice.GetPosition(m_border);
            Point imagePos = e.MouseDevice.GetPosition(m_image);

            MouseOverPoint = borderPos;
            
            if (m_canvas.IsMouseCaptured)
            {
                Point p = e.MouseDevice.GetPosition(m_border);
                double x = m_prePanRenderOffset.X + (p.X - MouseClickPoint.X);
                double y = m_prePanRenderOffset.Y + (p.Y - MouseClickPoint.Y);
                SetRenderMatrixOffset(x, y);
                UpdateCenter();
            }
            else
            {
                MouseOverOffset = PixelToMap(imagePos);
                MouseOverCoords = PixelToWorld(imagePos);
            }
        }

        private void MapControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!m_canvas.IsMouseCaptured && ZoomWithMouseWheel)
            {
                Point p = e.MouseDevice.GetPosition(m_canvas);
                double increment = (e.Delta > 0) ? +ZoomDelta : -ZoomDelta;
                ZoomToPoint(p, increment);
            }
        }
        #endregion

        #region Dependency Property Callbacks
        private static void OffsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MapControl mapControl)
            {
                if (!mapControl.m_suppressOffsetChangedHandler)
                {
                    mapControl.PanToCenter();
                }
            }
        }

        private static void ZoomPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MapControl mapControl)
            {
                if (!mapControl.m_suppressZoomChangedHandler)
                {
                    mapControl.ApplyZoom();
                }
            }
        }

        private static object ZoomCoerceValue(DependencyObject sender, object o)
        {
            if (sender is MapControl mapControl && o is double value)
            {
                if (value < mapControl.MinimumZoom) value = mapControl.MinimumZoom;
                if (value > mapControl.MaximumZoom) value = mapControl.MaximumZoom;

                return value;
            }

            throw new ArgumentException();
        }

        private static void OverlaysPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MapControl view)
            {
                var collectionChangedAction = new NotifyCollectionChangedEventHandler((sender, e) =>
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (var item in e.NewItems) view.AddOverlay(item as UIElement);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            foreach (var item in e.OldItems) view.RemoveOverlay(item as UIElement);
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            foreach (var item in e.OldItems) view.RemoveOverlay(item as UIElement);
                            foreach (var item in e.NewItems) view.AddOverlay(item as UIElement);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            view.RemoveAllOverlays();
                            if (e.NewItems != null)
                                foreach (var item in e.NewItems) view.AddOverlay(item as UIElement);
                            break;
                    }
                });

                if (e.OldValue != null)
                {
                    var oldCollection = (INotifyCollectionChanged) e.OldValue;
                    oldCollection.CollectionChanged -= collectionChangedAction;
                    view.RemoveAllOverlays();
                }
                if (e.NewValue != null)
                {
                    var newCollection = (INotifyCollectionChanged) e.NewValue;
                    newCollection.CollectionChanged += collectionChangedAction;
                    view.AddAllOverlays();
                }
            }
        }
        #endregion

        #region Commands
        public ICommand PanUpCommand => new RelayCommand
        (
            () => Pan(0, PanDelta),
            () => PanWithKeyboard
        );

        public ICommand PanDownCommand => new RelayCommand
        (
            () => Pan(0, -PanDelta),
            () => PanWithKeyboard
        );

        public ICommand PanLeftCommand => new RelayCommand
        (
            () => Pan(PanDelta, 0),
            () => PanWithKeyboard
        );

        public ICommand PanRightCommand => new RelayCommand
        (
            () => Pan(-PanDelta, 0),
            () => PanWithKeyboard
        );

        public ICommand ZoomInCommand => new RelayCommand
        (
            () => ZoomIn(),
            () => ZoomWithKeyboard
        );

        public ICommand ZoomOutCommand => new RelayCommand
        (
            () => ZoomOut(),
            () => ZoomWithKeyboard
        );
        #endregion
    }
}