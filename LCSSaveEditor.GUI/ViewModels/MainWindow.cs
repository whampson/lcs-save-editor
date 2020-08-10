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
using WpfEssentials;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class MainWindow : WindowBase
    {
        public event EventHandler SettingsWindowRequest;
        public event EventHandler AboutWindowRequest;
        public event EventHandler LogWindowRequest;
        public event EventHandler<TabUpdateEventArgs> TabUpdate;

        private ObservableCollection<TabPageBase> m_tabs;
        private int m_selectedTabIndex;
        private bool m_isRevertingFile;
        private bool m_isDirty;

        public Editor TheEditor => Editor.TheEditor;
        public LCSSave TheSave => TheEditor.ActiveFile;
        public Settings TheSettings => Settings.TheSettings;
        public Gxt TheText => Gxt.TheText;

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

        public bool IsDirty
        {
            get { return m_isDirty; }
            set { m_isDirty = value; OnPropertyChanged(); }
        }

        public MainWindow()
        {
            Tabs = new ObservableCollection<TabPageBase>
            {
                new WelcomeTab(this),
                new GeneralTab(this),
            };

            UpdateTitle();
        }

        #region Window Actions
        public override void Initialize()
        {
            base.Initialize();

            TheEditor.FileOpening += TheEditor_FileOpening;
            TheEditor.FileOpened += TheEditor_FileOpened;
            TheEditor.FileClosing += TheEditor_FileClosing;
            TheEditor.FileClosed += TheEditor_FileClosed;
            TheEditor.FileSaving += TheEditor_FileSaving;
            TheEditor.FileSaved += TheEditor_FileSaved;

            LoadSettings();
            LoadGxt();
            InitializeTabs();
            RefreshTabs(TabUpdateTrigger.WindowLoaded);
        }

        public override void Shutdown()
        {
            base.Shutdown();

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
            // Load settings file
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

        public void OpenFile(string path)
        {
            if (TheEditor.IsFileOpen)
            {
                if (IsDirty)
                {
                    PromptSaveChanges(OpenFilePrompt_Callback);
                    return;
                }
                CloseFile();
            }

            try
            {
                TheEditor.OpenFile(path);
            }
            catch (Exception e)
            {
                Log.Exception(e);
                if (e is InvalidDataException)
                {
                    ShowError("The file is not a valid GTA:LCS save file.");
                    return;
                }

                ShowException(e, "The file could not be opened.");

            #if !DEBUG
                return;
            #else
                throw;
            #endif

            }
        }

        public void SaveFile()
        {
            SaveFile(TheSettings.MostRecentFile);
        }

        public void SaveFile(string path)
        {
            try
            {
                TheEditor.SaveFile(path);
            }
            catch (Exception e)
            {
                Log.Exception(e);
                ShowException(e, "The file could not be saved.");
                SetTimedStatusText("Error saving file.", 10, expiredStatus: "Ready.");

            #if !DEBUG
                return;
            #else
                throw;
            #endif
            }
        }

        public void RevertFile()
        {
            if (IsDirty)
            {
                PromptConfirmRevert(RevertFilePrompt_Callback);
                return;
            }

            m_isRevertingFile = true;
            Log.Info("Reverting file...");

            Tabs.Where(t => t.Visibility == TabPageVisibility.WhenFileIsOpen).ToList().ForEach(t => t.Unload());
            TheEditor.CloseFile();

            TheEditor.OpenFile(TheSettings.MostRecentFile);
            Tabs.Where(t => t.Visibility == TabPageVisibility.WhenFileIsOpen).ToList().ForEach(t => t.Load());

            Log.Info("File reverted.");
            m_isRevertingFile = false;
        }

        public void CloseFile()
        {
            if (IsDirty)
            {
                PromptSaveChanges(CloseFilePrompt_Callback);
                return;
            }

            TheEditor.CloseFile();
        }

        public void RefreshTabs(TabUpdateTrigger trigger)
        {
            TabUpdate?.Invoke(this, new TabUpdateEventArgs(trigger));
            SelectedTabIndex = Tabs.IndexOf(Tabs.Where(x => x.IsVisible).FirstOrDefault());
        }
        #endregion

        #region Window Event Handlers
        private void TheEditor_FileOpening(object sender, string e)
        {
            if (!m_isRevertingFile)
            {
                SetStatusText("Opening file...");
            }
        }

        private void TheEditor_FileOpened(object sender, EventArgs e)
        {
            UpdateTitle();

            RegisterDirtyHandlers(TheSave);
            OnPropertyChanged(nameof(TheSave));

            if (!m_isRevertingFile)
            {
                RefreshTabs(TabUpdateTrigger.FileOpened);
                SetTimedStatusText("File opened successfully.", expiredStatus: "Ready.");
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
        }

        private void TheEditor_FileClosed(object sender, EventArgs e)
        {
            ClearDirty();

            UnregisterDirtyHandlers(TheSave);
            OnPropertyChanged(nameof(TheSave));

            SetTimedStatusText("File closed.", expiredStatus: "Ready.");
        }

        private void TheEditor_FileSaving(object sender, string e)
        {
            SetStatusText("Saving file...");
            if (TheSettings.UpdateTimeStampOnSave)
            {
                TheSave.TimeStamp = DateTime.Now;
            }
        }

        private void TheEditor_FileSaved(object sender, EventArgs e)
        {
            ClearDirty();
            SetTimedStatusText("File saved successfully.", expiredStatus: "Ready.");
        }
        #endregion

        #region Dialog Callbacks
        private void RevertFilePrompt_Callback(MessageBoxResult r)
        {
            if (r == MessageBoxResult.Yes)
            {
                ClearDirty();
                RevertFile();
            }
        }

        private void OpenFilePrompt_Callback(MessageBoxResult r)
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
        }

        private void CloseFilePrompt_Callback(MessageBoxResult r)
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
        }

        private void ShowFileDialog_Callback(bool? result, FileDialogEventArgs e)
        {
            if (result != true) return;

            TheSettings.SetLastAccess(e.FileName);
            switch (e.DialogType)
            {
                case FileDialogType.OpenFileDialog:
                    OpenFile(e.FileName);
                    break;
                case FileDialogType.SaveFileDialog:
                    SaveFile(e.FileName);
                    break;
            }
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
                    RegisterDirtyHandlers(v as INotifyPropertyChanged);
                }
            }
            o.PropertyChanged -= TheSave_PropertyDirty;
        }

        private void TheSave_PropertyDirty(object sender, PropertyChangedEventArgs e)
        {
            SetDirty();

            if (sender is IEnumerable)
            {
                // Ignore collections
                return;
            }

            var type = sender.GetType();
            var prop = type.GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            var data = prop.GetValue(sender);
            Log.Info($"PropertyChanged: {type.FullName}.{e.PropertyName} = {data}");
        }

        private void TheSave_CollectionDirty(object sender, NotifyCollectionChangedEventArgs e)
        {
            var type = sender.GetType().GetGenericArguments()[0];
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var data = e.NewItems[i];
                        if (data is SaveDataObject o) data = o.ToJsonString(Formatting.None);
                        Log.Info($"CollectionChanged: Add: {type.FullName}[{e.NewStartingIndex + i}] = {data}");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var data = e.OldItems[i];
                        if (data is SaveDataObject o) data = o.ToJsonString(Formatting.None);
                        Log.Info($"CollectionChanged: Remove: {type.FullName}[{e.OldStartingIndex + i}] = {data}");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var data = e.NewItems[i];
                        if (data is SaveDataObject o) data = o.ToJsonString(Formatting.None);
                        Log.Info($"CollectionChanged: Replace: {type.FullName}[{e.NewStartingIndex + i}] = {data}");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Move:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        Log.Info($"CollectionChanged: Move: {type.FullName}[{e.OldStartingIndex + i}] => {type.FullName}[{e.NewStartingIndex + i}]");
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    Log.Info($"CollectionChanged: Reset: {type.FullName}");
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
            Log.Info($"PropertyChanged: {type.FullName}[{e.ItemIndex}].{e.PropertyName} = {data}");

        }
        #endregion

        #region Commands
        public ICommand FileOpenCommand => new RelayCommand
        (
            () => ShowFileDialog(FileDialogType.OpenFileDialog, ShowFileDialog_Callback)
        );

        public ICommand FileOpenRecentCommand => new RelayCommand<string>
        (
            (x) => { TheSettings.SetLastAccess(x); OpenFile(x);  },
            (_) => TheSettings.RecentFiles.Count > 0
        );

        public ICommand FileSaveCommand => new RelayCommand
        (
            () => SaveFile(),
            () => TheEditor.IsFileOpen
        );

        public ICommand FileSaveAsCommand => new RelayCommand
        (
            () => ShowFileDialog(FileDialogType.SaveFileDialog, ShowFileDialog_Callback),
            () => TheEditor.IsFileOpen
        );

        public ICommand FileCloseCommand => new RelayCommand
        (
            () => CloseFile(),
            () => TheEditor.IsFileOpen
        );

        public ICommand FileRevertCommand => new RelayCommand
        (
            () => RevertFile(),
            () => TheEditor.IsFileOpen
        );

        public ICommand FileTransferDataCommand => new RelayCommand
        (
            () => ShowInfo("TODO: transfer data dialog"),
            () => TheEditor.IsFileOpen
        );

        public ICommand FileExitCommand => new RelayCommand
        (
            () => Application.Current.MainWindow.Close()
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
                ShowFileDialog(FileDialogType.OpenFileDialog, (x,y) =>
                {
                    if (x == true)
                    {
                        TheText.Load(y.FileName);
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
                    // Any prospecting programers out there? Find the issues with each function!
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

#endif
        #endregion
    }
}
