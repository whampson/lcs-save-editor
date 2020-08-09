using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class MainWindow : WindowBase
    {
        public event EventHandler LogWindowRequest;
        public event EventHandler<TabUpdateEventArgs> TabUpdate;

        private ObservableCollection<TabPageBase> m_tabs;
        private int m_selectedTabIndex;
        private bool m_isRevertingFile;
        private bool m_isDirty;

        public Editor TheEditor => Editor.TheEditor;
        public LCSSave TheSave => TheEditor.ActiveFile;
        public Settings TheSettings => Settings.TheSettings;

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
            Tabs = new ObservableCollection<TabPageBase>();

            UpdateTitle();
        }

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

        public void OpenFile(string path)
        {
            if (TheEditor.IsFileOpen)
            {
                ShowError("Please close the current file before opening a new one.");
                return;
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
#if !DEBUG
                else
                {
                    ShowException(e, "The file could not be loaded.");
                    return;
                }
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
            }
        }

        public void RevertFile()
        {
            if (IsDirty) PromptConfirmRevert(RevertFileDialog_Callback);
            else DoRevert();
        }

        public void DoRevert()
        {
            m_isRevertingFile = true;
            Log.Info("Reverting file...");

            Tabs.Where(t => t.Visibility == TabPageVisibility.WhenFileIsOpen).ToList()
                .ForEach(t => t.Unload());

            TheEditor.CloseFile();
            TheEditor.OpenFile(TheSettings.MostRecentFile);

            Tabs.Where(t => t.Visibility == TabPageVisibility.WhenFileIsOpen).ToList()
                .ForEach(t => t.Load());

            Log.Info("File reverted.");
            m_isRevertingFile = false;
        }

        public void CloseFile()
        {
            if (IsDirty) PromptSaveChanges(CloseFileDialog_Callback);
            else DoClose();
        }

        private void DoClose()
        {
            TheEditor.CloseFile();
        }

        private void DoExit()
        {
            Application.Current.Shutdown();
        }

        private void UpdateTitle()
        {
            string title = App.Name;
            if (IsDirty) title = $"*{title}";
            if (TheSave != null) title += $" - {TheSettings.MostRecentFile}";

            Title = title;
        }

        private void SetDirty()
        {
            if (!IsDirty)
            {
                IsDirty = true;
                UpdateTitle();
            }
        }

        #region Window Actions
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
                SetStatusText("Ready.");
                SetTimedStatusText("File opened successfully.");
            }
            else
            {
                SetStatusText("Ready.");
                SetTimedStatusText("File reverted.");
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
            IsDirty = false;
            UpdateTitle();

            UnregisterDirtyHandlers(TheSave);
            OnPropertyChanged(nameof(TheSave));

            SetStatusText("Ready.");
            SetTimedStatusText("File closed.");
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
            IsDirty = false;
            UpdateTitle();
            SetStatusText("Ready.");
            SetTimedStatusText("File saved successfully.");
        }

        void RevertFileDialog_Callback(MessageBoxResult r)
        {
            if (r == MessageBoxResult.Yes)
            {
                DoRevert();
            }
        }

        void CloseFileDialog_Callback(MessageBoxResult r)
        {
            if (r != MessageBoxResult.Cancel)
            {
                if (r == MessageBoxResult.Yes) SaveFile();
                DoClose();
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
            Log.Info($"PropertyChanged: {e.PropertyName}");
            SetDirty();
        }

        private void TheSave_CollectionDirty(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TOOD: log
            //Log.Info($"CollectionChanged: {e.}");
            SetDirty();
        }

        private void TheSave_CollectionElementDirty(object sender, ItemStateChangedEventArgs e)
        {
            Log.Info($"PropertyChanged: {e.PropertyName} at index {e.ItemIndex}");
            SetDirty();
        }
        #endregion

        #region Commands
        public ICommand FileOpenCommand => new RelayCommand
        (
            () => ShowFileDialog(FileDialogType.OpenFileDialog, ShowFileDialog_Callback)
        );

        public ICommand FileOpenRecentCommand => new RelayCommand<string>
        (
            (x) => OpenFile(x),
            (_) => TheSettings.RecentFiles.Count > 0
        );

        public ICommand FileCloseCommand => new RelayCommand
        (
            () => CloseFile(),
            () => TheEditor.IsFileOpen
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

        public ICommand FileRevertCommand => new RelayCommand
        (
            () => RevertFile(),
            () => TheEditor.IsFileOpen
        );

        public ICommand FileExitCommand => new RelayCommand
        (
            () => Application.Current.MainWindow.Close()
        );

        public ICommand ViewShowLogCommand => new RelayCommand
        (
            () => LogWindowRequest?.Invoke(this, EventArgs.Empty)
        );

        public ICommand HelpAboutCommand => new RelayCommand
        (
            () =>
            {
                ShowInfo(
                    $"{App.Name}\n" +
                    "(C) 2016-2020 Wes Hampson\n" +
                    "\n" +
                   $"Version: {App.InformationalVersion}\n",
                    title: "About");
            }
        );
    }
    #endregion
}
