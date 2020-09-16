using GTASaveData;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Events;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfEssentials;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class MainWindow : WindowBase
    {
        public event EventHandler<TabUpdateEventArgs> TabUpdate;
        public event EventHandler SettingsWindowRequest;
        public event EventHandler AboutWindowRequest;
        public event EventHandler GlobalsWindowRequest;
        public event EventHandler MapWindowRequest;
        public event EventHandler StatsWindowRequest;
        public event EventHandler LogWindowRequest;

        private ObservableCollection<TabPageBase> m_tabs;
        private readonly DispatcherTimer m_timer;
        private bool m_isRevertingFile;
        private bool m_isDirty;
        private int m_selectedTabIndex;
        private int m_timerTick;
        private string m_currentStatusText;
        private string m_permanentStatusText;

        public ObservableCollection<TabPageBase> Tabs
        {
            get { return m_tabs; }
            set { m_tabs = value; OnPropertyChanged(); }
        }

        public int SelectedTabIndex
        {
            get { return m_selectedTabIndex; }
            set { m_selectedTabIndex = value; OnPropertyChanged(); }
        }

        public string StatusText
        {
            get { return m_currentStatusText; }
            private set { m_currentStatusText = value; OnPropertyChanged(); }
        }

        public bool IsDirty
        {
            get { return m_isDirty; }
            private set { m_isDirty = value; OnPropertyChanged(); }
        }

        public MainWindow()
        {
            m_timer = new DispatcherTimer();
            Tabs = new ObservableCollection<TabPageBase>
            {
                new WelcomeTab(this),
                new GeneralTab(this),
                new PlayerTab(this),
                new GaragesTab(this),
                new StatsTab(this)
            };
        }

        public override void Initialize()
        {
            base.Initialize();

            m_timer.Tick += StatusTimer_Tick;

            OpenFileRequest += OpenFileRequest_Handler;
            SaveFileRequest += SaveFileRequest_Handler;
            RevertFileRequest += RevertFileRequest_Handler;
            CloseFileRequest += CloseFileRequest_Handler;

            TheEditor.FileOpening += TheEditor_FileOpening;
            TheEditor.FileOpened += TheEditor_FileOpened;
            TheEditor.FileClosing += TheEditor_FileClosing;
            TheEditor.FileClosed += TheEditor_FileClosed;
            TheEditor.FileSaving += TheEditor_FileSaving;
            TheEditor.FileSaved += TheEditor_FileSaved;

            LoadSettings();
            LoadGxt();
            LoadCarcols();
            InitializeTabs();
            UpdateTitle();
            RefreshTabs(TabUpdateTrigger.WindowLoaded);
        }

        public override void Shutdown()
        {
            base.Shutdown();

            m_timer.Tick -= StatusTimer_Tick;

            OpenFileRequest -= OpenFileRequest_Handler;
            SaveFileRequest -= SaveFileRequest_Handler;
            RevertFileRequest -= RevertFileRequest_Handler;
            CloseFileRequest -= CloseFileRequest_Handler;

            TheEditor.FileOpening -= TheEditor_FileOpening;
            TheEditor.FileOpened -= TheEditor_FileOpened;
            TheEditor.FileClosing -= TheEditor_FileClosing;
            TheEditor.FileClosed -= TheEditor_FileClosed;
            TheEditor.FileSaving -= TheEditor_FileSaving;
            TheEditor.FileSaved -= TheEditor_FileSaved;

            RefreshTabs(TabUpdateTrigger.WindowClosing);
            ShutdownTabs();
            SaveSettings();
        }

        private void InitializeTabs()
        {
            foreach (var tab in Tabs)
            {
                tab.Initialize();
            }
        }

        private void ShutdownTabs()
        {
            foreach (var tab in Tabs)
            {
                tab.Shutdown();
            }
        }

        public void LoadSettings()
        {
            if (File.Exists(App.SettingsPath))
            {
                TheSettings.LoadSettings(App.SettingsPath);
            }
        }

        public void SaveSettings()
        {
            TheSettings.SaveSettings(App.SettingsPath);
        }

        public void LoadGxt()
        {
            MemoryStream m = new MemoryStream();
            var info = Application.GetResourceStream(App.GxtResourceUri);
            info.Stream.CopyTo(m);

            TheText.Load(m.ToArray());
        }

        public void LoadCarcols()
        {
            MemoryStream m = new MemoryStream();
            var info = Application.GetResourceStream(App.CarcolsResourceUri);
            info.Stream.CopyTo(m);

            TheCarcols.Load(m.ToArray());
        }

        public void RefreshTabs(TabUpdateTrigger trigger)
        {
            TabUpdate?.Invoke(this, new TabUpdateEventArgs(trigger));
            SelectedTabIndex = Tabs.IndexOf(Tabs.Where(x => x.IsVisible).FirstOrDefault());
        }

        #region File I/O Handlers
        public void OpenFileRequest_Handler(object sender, FileIOEventArgs e)
        {
            if (TheEditor.IsFileOpen)
            {
                if (IsDirty && !e.SuppressPrompting)
                {
                    PromptSaveChanges((r) =>
                    {
                        if (r != MessageBoxResult.Cancel)
                        {
                            if (r == MessageBoxResult.Yes)
                            {
                                SaveFile();
                            }

                            ClearDirty();
                            OpenFile(TheSettings.LastFileAccessed);
                        }
                    });
                    return;
                }
                CloseFile();
            }

            try
            {
                TheEditor.OpenFile(e.Path);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                if (ex is InvalidDataException)
                {
                    ShowError("The file is not a valid GTA:LCS save file.");
                    return;
                }

                ShowException(ex, "The file could not be opened.");

#if !DEBUG
                return;
#else
                throw;
#endif

            }
        }

        public void SaveFileRequest_Handler(object sender, FileIOEventArgs e)
        {
            try
            {
                TheEditor.SaveFile(e.Path);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                ShowException(ex, "The file could not be saved.");
                SetTimedStatusText("Error saving file.", 10, expiredStatus: "Ready.");

#if !DEBUG
                return;
#else
                throw;
#endif
            }
        }

        public void RevertFileRequest_Handler(object sender, FileIOEventArgs e)
        {
            if (IsDirty && !e.SuppressPrompting)
            {
                PromptConfirmRevert((r) =>
                {
                    if (r == MessageBoxResult.Yes)
                    {
                        ClearDirty();
                        RevertFile();
                    }
                });
                return;
            }

            m_isRevertingFile = true;
            Log.Info("Reverting file...");

            Tabs.Where(t => t.Visibility == TabPageVisibility.WhenFileIsOpen).ToList().ForEach(t => t.Unload());
            TheEditor.CloseFile();

            TheEditor.OpenFile(TheSettings.MostRecentFile);
            Tabs.Where(t => t.Visibility == TabPageVisibility.WhenFileIsOpen).ToList().ForEach(t => t.Load());
            Tabs.Where(t => t.Visibility == TabPageVisibility.WhenFileIsOpen).ToList().ForEach(t => t.Update());

            Log.Info("File reverted.");
            m_isRevertingFile = false;
        }

        public void CloseFileRequest_Handler(object sender, FileIOEventArgs e)
        {
            if (IsDirty && !e.SuppressPrompting)
            {
                PromptSaveChanges((r) =>
                {
                    if (r != MessageBoxResult.Cancel)
                    {
                        if (r == MessageBoxResult.Yes)
                        {
                            SaveFile();
                        }

                        ClearDirty();
                        CloseFile();
                    }
                });
                return;
            }

            TheEditor.CloseFile();
        }

        private void TheEditor_FileOpening(object sender, string e)
        {
            if (!m_isRevertingFile)
            {
                SetStatusText("Opening file...");
            }
        }

        private void TheEditor_FileOpened(object sender, EventArgs e)
        {
            SuppressExternalChangesCheck = false;

            UpdateTitle();
            RegisterDirtyHandlers(TheSave);
            OnPropertyChanged(nameof(TheSave));

            if (!m_isRevertingFile)
            {
                RefreshTabs(TabUpdateTrigger.FileOpened);
                SetTimedStatusText("File opened.", expiredStatus: "Ready.");
            }
            else
            {
                SetTimedStatusText("File reverted.", expiredStatus: "Ready.");
            }
        }

        private void TheEditor_FileClosing(object sender, EventArgs e)
        {
            if (!m_isRevertingFile)
            {
                SetStatusText("Closing file...");
                RefreshTabs(TabUpdateTrigger.FileClosing);
            }

            UnregisterDirtyHandlers(TheSave);
        }

        private void TheEditor_FileClosed(object sender, EventArgs e)
        {
            SuppressExternalChangesCheck = false;

            ClearDirty();
            UpdateTitle();
            OnPropertyChanged(nameof(TheSave));
            SetTimedStatusText("File closed.", expiredStatus: "Ready.");
        }

        private void TheEditor_FileSaving(object sender, string e)
        {
            SetStatusText("Saving file...");
            if (!TheSave.FileFormat.IsPS2 || TheSettings.UpdateTimeStampOnSave)
            {
                TheSave.TimeStamp = DateTime.Now;
            }
        }

        private void TheEditor_FileSaved(object sender, EventArgs e)
        {
            SuppressExternalChangesCheck = false;

            ClearDirty();
            SetTimedStatusText("File saved.", expiredStatus: "Ready.");
        }
        #endregion

        #region Status Text
        public void SetStatusText(string status)
        {
            if (m_timer.IsEnabled)
            {
                m_timer.Stop();
            }

            StatusText = status;
            m_permanentStatusText = status;
        }

        public void SetTimedStatusText(string status,
            int duration = 5,   // seconds
            string expiredStatus = null)
        {
            if (expiredStatus == null)
            {
                expiredStatus = m_permanentStatusText;
            }

            if (m_timer.IsEnabled)
            {
                m_timer.Stop();
                m_currentStatusText = expiredStatus;
            }

            m_permanentStatusText = expiredStatus;
            StatusText = status;
            m_timerTick = duration;
            m_timer.Interval = TimeSpan.FromSeconds(1);
            m_timer.Start();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (m_timerTick <= 0)
            {
                m_timer.Stop();
                StatusText = m_permanentStatusText;
            }

            m_timerTick--;
        }
        #endregion

        #region Dirty Bit
        public void SetDirty()
        {
            if (!IsDirty)
            {
                IsDirty = true;
                UpdateTitle();
            }
        }

        public void ClearDirty()
        {
            if (IsDirty)
            {
                IsDirty = false;
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            string title = App.Name;
            if (IsDirty) title = $"*{title}";
            if (TheEditor.IsFileOpen) title += $" - {TheSettings.MostRecentFile}";

            Title = title;
        }

        private void RegisterDirtyHandlers(INotifyPropertyChanged o)
        {
            if (o == null) return;

            foreach (var p in o.GetType().GetProperties().Where(x => x.GetIndexParameters().Length == 0))
            {
                object v = p.GetValue(o, null);
                if (p.PropertyType.GetInterface(nameof(INotifyItemStateChanged)) != null)
                {
                    (v as INotifyItemStateChanged).ItemStateChanged += TheSave_CollectionElementDirty;
                }
                if (p.PropertyType.GetInterface(nameof(INotifyCollectionChanged)) != null)
                {
                    (v as INotifyCollectionChanged).CollectionChanged += TheSave_CollectionDirty;
                }
                if (p.PropertyType.GetInterface(nameof(INotifyPropertyChanged)) != null)
                {
                    RegisterDirtyHandlers(v as INotifyPropertyChanged);
                }
            }
            o.PropertyChanged += TheSave_PropertyDirty;
        }

        private void UnregisterDirtyHandlers(INotifyPropertyChanged o)
        {
            if (o == null) return;

            foreach (var p in o.GetType().GetProperties().Where(x => x.GetIndexParameters().Length == 0))
            {
                object v = p.GetValue(o, null);
                if (p.PropertyType.GetInterface(nameof(INotifyItemStateChanged)) != null)
                {
                    (v as INotifyItemStateChanged).ItemStateChanged -= TheSave_CollectionElementDirty;
                }
                if (p.PropertyType.GetInterface(nameof(INotifyCollectionChanged)) != null)
                {
                    (v as INotifyCollectionChanged).CollectionChanged -= TheSave_CollectionDirty;
                }
                if (p.PropertyType.GetInterface(nameof(INotifyPropertyChanged)) != null)
                {
                    UnregisterDirtyHandlers(v as INotifyPropertyChanged);
                }
            }
            o.PropertyChanged -= TheSave_PropertyDirty;
        }

        private void TheSave_PropertyDirty(object sender, PropertyChangedEventArgs e)
        {
            SetDirty();

            var type = sender.GetType();
            var prop = type.GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop.GetIndexParameters().Length == 0)
            {
                // Only deal with non-indexer properties
                var data = prop.GetValue(sender);
                Log.Info($"PropertyChanged: {type.Name}.{e.PropertyName} = {data}");
            }
        }

        private void TheSave_CollectionDirty(object sender, NotifyCollectionChangedEventArgs e)
        {
            var type = sender.GetType().GetGenericArguments()[0];
            var name = type.Name;

            if (sender == Scripts.GlobalVariables)
            {
                name = "Scripts.Globals";
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var data = e.NewItems[i];
                        if (data is SaveDataObject o) data = o.ToJsonString(Formatting.None);
                        Log.Info($"PropertyChanged: {name}[{e.NewStartingIndex + i}] = {data}");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var data = e.OldItems[i];
                        Log.Info($"PropertyChanged: deleted {name}[{e.OldStartingIndex + i}]");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var data = e.NewItems[i];
                        if (data is SaveDataObject o) data = o.ToJsonString(Formatting.None);
                        Log.Info($"PropertyChanged: {name}[{e.NewStartingIndex + i}] = {data}");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Move:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        Log.Info($"PropertyChanged: {name}[{e.OldStartingIndex + i}] => {name}[{e.NewStartingIndex + i}]");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    Log.Info($"PropertyChanged: cleared {name}");
                    break;
                }
            }
            SetDirty();
        }

        private void TheSave_CollectionElementDirty(object sender, ItemStateChangedEventArgs e)
        {
            SetDirty();

            if (!(sender is IList list))
            {
                return;
            }

            var item = list[e.ItemIndex];
            var type = item.GetType();
            var prop = type.GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            var data = prop.GetValue(item);
            if (data is SaveDataObject o) data = o.ToJsonString(Formatting.None);
            Log.Info($"PropertyChanged: {type.Name}[{e.ItemIndex}].{e.PropertyName} = {data}");

        }
        #endregion

        #region Window Commands
        public ICommand FileOpenRecentCommand => new RelayCommand<string>
        (
            (x) => { TheSettings.SetLastAccess(x); OpenFile(x); },
            (_) => TheSettings.RecentFiles.Count > 0
        );

        public ICommand FileTransferDataCommand => new RelayCommand
        (
            () => ShowInfo("TODO: transfer data dialog"),
            () => TheEditor.IsFileOpen
        );

        public ICommand EditGlobalsCommand => new RelayCommand
        (
            () => GlobalsWindowRequest?.Invoke(this, EventArgs.Empty),
            () => TheEditor.IsFileOpen
        );

        public ICommand EditConvertAndroidCommand => new RelayCommand
        (
            () => TheEditor.ChangeFileFormat(LCSSave.FileFormats.Android),
            () => TheEditor.IsFileOpen
        );

        public ICommand EditConvertiOSCommand => new RelayCommand
        (
            () => TheEditor.ChangeFileFormat(LCSSave.FileFormats.iOS),
            () => TheEditor.IsFileOpen
        );

        public ICommand EditConvertPS2Command => new RelayCommand
        (
            () => TheEditor.ChangeFileFormat(LCSSave.FileFormats.PS2),
            () => TheEditor.IsFileOpen
        );

        public ICommand EditConvertPSPCommand => new RelayCommand
        (
            () => TheEditor.ChangeFileFormat(LCSSave.FileFormats.PSP),
            () => TheEditor.IsFileOpen
        );

        public ICommand EditSettingsCommand => new RelayCommand
        (
            () => SettingsWindowRequest?.Invoke(this, EventArgs.Empty)
        );

        public ICommand ViewMapCommand => new RelayCommand
        (
            () => MapWindowRequest?.Invoke(this, EventArgs.Empty),
            () => TheEditor.IsFileOpen
        );

        public ICommand ViewStatsCommand => new RelayCommand
        (
            () => StatsWindowRequest?.Invoke(this, EventArgs.Empty),
            () => TheEditor.IsFileOpen
        );

        public ICommand ViewLogCommand => new RelayCommand
        (
            () => LogWindowRequest?.Invoke(this, EventArgs.Empty)
        );

        public ICommand HelpUpdateCommand => new RelayCommand
        (
            () => ShowInfo("TODO: updater")
        );

        public ICommand HelpAboutCommand => new RelayCommand
        (
            () => AboutWindowRequest?.Invoke(this, EventArgs.Empty)
        );

#if DEBUG
        public ICommand DebugLoadGxtFile => new RelayCommand
        (
            () =>
            {
                ShowFileDialog(FileDialogType.OpenFileDialog, (x, y) =>
                {
                    if (x == true)
                    {
                        TheText.Load(y.FileName);
                    }
                });
            }
        );

        public ICommand DebugLoadCarcolsFile => new RelayCommand
        (
            () =>
            {
                ShowFileDialog(FileDialogType.OpenFileDialog, (x, y) =>
                {
                    if (x == true)
                    {
                        TheCarcols.Load(y.FileName);
                    }
                });
            }
        );

        public ICommand DebugRaiseUnhandledException => new RelayCommand
        (
            () =>
            {
                Random r = new Random();
                int func = r.Next(0, 3);

                switch (func)
                {
                    // Any prospecting programers out there? Find the issue with each function!
                    case 0:
                    {
                        int[] foo = { 0, 1 };
                        foo[2] = 3;
                        break;
                    }
                    case 1:
                    {
                        using StreamBuffer buf = new StreamBuffer(10);
                        buf.Write(r.Next());
                        buf.Write(r.Next());
                        buf.Write(r.Next());
                        break;
                    }
                    case 2:
                    {
                        int.Parse("one");
                        break;
                    }
                }
            }
        );

        public ICommand DebugShowGxtDialog => new RelayCommand
        (
            () => ShowGxtDialog((r, e) => { }, allowTableSelection: true, modal: false)
        );

        public event EventHandler DestroyAllWindowsRequest;
        public ICommand DebugDestroyAllWindows => new RelayCommand
        (
            () => DestroyAllWindowsRequest?.Invoke(this, EventArgs.Empty)
        );

#endif
        #endregion
    }
}
