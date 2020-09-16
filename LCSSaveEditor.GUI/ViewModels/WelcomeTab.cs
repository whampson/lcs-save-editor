using GTASaveData;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;
using System.Windows.Input;
using WpfEssentials;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class WelcomeTab : TabPageBase
    {
        private BackgroundWorker m_lukeFileWalker;
        private ObservableCollection<SaveFileInfo> m_saveFiles;
        private SaveFileInfo m_selectedFile;
        private bool m_openedOnce;
        private bool m_isSearching;
        private bool m_searchPending;
        private bool m_cancelPending;

        private static string AppendTrailingSlash(string path)
        {
            if (path != null && !path.EndsWith('\\'))
            {
                path += "\\";
            }

            return path;
        }

        public string SelectedDirectory
        {
            get { return AppendTrailingSlash(TheSettings.WelcomeDirectory); }
            set { TheSettings.WelcomeDirectory = AppendTrailingSlash(value); OnPropertyChanged(); }
        }

        public bool RecursiveSearch
        {
            get { return TheSettings.WelcomeRecursiveSearch; }
            set { TheSettings.WelcomeRecursiveSearch = value; OnPropertyChanged(); }
        }

        public BackgroundWorker SearchWorker
        {
            get { return m_lukeFileWalker; }
            set { m_lukeFileWalker = value; OnPropertyChanged(); }
        }

        public ObservableCollection<SaveFileInfo> SaveFiles
        {
            get { return m_saveFiles; }
            private set { m_saveFiles = value; OnPropertyChanged(); }
        }

        public SaveFileInfo SelectedFile
        {
            get { return m_selectedFile; }
            set { m_selectedFile = value; OnPropertyChanged(); }
        }

        public bool SearchPending
        {
            get { return m_searchPending; }
            set { m_searchPending = value; OnPropertyChanged(); }
        }

        public bool CancelPending
        {
            get { return m_cancelPending; }
            set { m_cancelPending = value; OnPropertyChanged(); }
        }

        public bool IsSearching
        {
            get { return m_isSearching; }
            set { m_isSearching = value; OnPropertyChanged(); }
        }

        public WelcomeTab(MainWindow window)
            : base("Welcome", TabPageVisibility.WhenFileIsClosed, window)
        {
            SaveFiles = new ObservableCollection<SaveFileInfo>();
            SearchWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
        }

        public override void Initialize()
        {
            base.Initialize();
            SearchWorker.DoWork += FileWalker_DoWork;
            SearchWorker.ProgressChanged += FileWalker_ProgressChanged;
            SearchWorker.RunWorkerCompleted += FileWalker_RunWorkerCompleted;
        }

        public override void Shutdown()
        {
            base.Shutdown();
            SearchWorker.DoWork -= FileWalker_DoWork;
            SearchWorker.ProgressChanged -= FileWalker_ProgressChanged;
            SearchWorker.RunWorkerCompleted -= FileWalker_RunWorkerCompleted;
        }

        public override void Load()
        {
            base.Load();
            if (!m_openedOnce)
            {
                if (string.IsNullOrEmpty(SelectedDirectory))
                {
                    SelectedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                Search();
                m_openedOnce = true;
            }
            OnPropertyChanged(nameof(SelectedDirectory));
            OnPropertyChanged(nameof(RecursiveSearch));
        }

        public override void Unload()
        {
            base.Unload();
            CancelSearch();
        }

        public override void Update()
        {
            base.Update();
            Refresh();
        }

        public void Refresh()
        {
            for (int i = 0; i < SaveFiles.Count; i++)
            {
                SaveFileInfo.TryGetInfo(SaveFiles[i].Path, out SaveFileInfo newInfo);
                SaveFiles[i] = newInfo;
            }
        }

        public void Search()
        {
            if (IsSearching)
            {
                SearchPending = true;
                CancelSearch();
                return;
            }

            SearchPending = false;
            if (Directory.Exists(SelectedDirectory))
            {
                SaveFiles.Clear();
                SearchWorker.RunWorkerAsync();
                IsSearching = true;

                Log.Info($"Searching for GTA:LCS save files...{(RecursiveSearch ? " (recursive)" : "")}");
            }
        }

        public void CancelSearch()
        {
            if (!CancelPending)
            {
                SearchWorker.CancelAsync();
                CancelPending = true;
            }
        }

        public void OpenSelectedItem()
        {
            CancelSearch();

            if (SelectedFile != null)
            {
                TheEditor.OpenFile(SelectedFile.Path);
            }
        }

        #region Event Handlers
        public void FolderDialogRequested_Callback(bool? result, FileDialogEventArgs e)
        {
            if (result == true)
            {
                SelectedDirectory = e.FileName;
                Search();
            }
        }

        private void FileWalker_DoWork(object sender, DoWorkEventArgs e)
        {
            SearchOption o = (RecursiveSearch) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            IEnumerable<string> files = Directory.EnumerateFiles(SelectedDirectory, "*.*", o);

            string currDir;
            string lastDir = null;

            foreach (string path in files)
            {
                if (SearchWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                currDir = Path.GetDirectoryName(path);
                if (currDir != lastDir)
                {
                    lastDir = currDir;
                    TheWindow.SetStatusText(@$"Searching {currDir}\...");   // TODO: path shortening func
                }

                if (SaveFileInfo.TryGetInfo(path, out SaveFileInfo info))
                {
                    SearchWorker.ReportProgress(-1, info);
                }
            }
        }

        private void FileWalker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is SaveFileInfo info)
            {
                SaveFiles.Add(info);
            }
        }

        private void FileWalker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsSearching = false;
            CancelPending = false;
            
            if (e.Cancelled)
            {
                Log.Info("Search cancelled.");
                TheWindow.SetTimedStatusText("Search cancelled.", expiredStatus: "Ready.");
                return;
            }

            if (e.Error != null)
            {
                Log.Exception(e.Error);
                if (!(e.Error is UnauthorizedAccessException ex))
                {
#if DEBUG
                    // Re-throw and preserve stack trace
                    ExceptionDispatchInfo.Capture(e.Error).Throw();
#endif
                }

                Log.Info($"Search completed with errors. Found {SaveFiles.Count} save files.");
                TheWindow.SetTimedStatusText("Search completed with errors. See the log for details.", expiredStatus: "Ready.");
                return;
            }

            if (SearchPending)
            {
                Search();
                return;
            }

            Log.Info($"Search completed. Found {SaveFiles.Count} save files.");
            TheWindow.SetTimedStatusText("Search completed.", expiredStatus: "Ready.");
        }
        #endregion

        #region Commands
        public ICommand SearchToggleCommand => new RelayCommand
        (
            () => { if (!IsSearching) Search(); else CancelSearch(); }
        );

        public ICommand OpenCommand => new RelayCommand
        (
            () => OpenSelectedItem(),
            () => SelectedFile != null
        );

        public ICommand BrowseCommand => new RelayCommand
        (
            () => TheWindow.ShowFolderDialog(FileDialogType.OpenFileDialog, FolderDialogRequested_Callback)
        );

        public ICommand RefreshCommand => new RelayCommand
        (
            () => Refresh()
        );
        #endregion

        public class SaveFileInfo : ObservableObject
        {
            private LCSSave m_saveFile;
            private string m_path;
            private string m_title;
            private DateTime m_lastModified;
            private FileFormat m_fileType;

            public LCSSave SaveFile
            {
                get { return m_saveFile; }
                set { m_saveFile = value; OnPropertyChanged(); }
            }

            public string Path
            {
                get { return m_path; }
                set { m_path = value; OnPropertyChanged(); }
            }

            public string Title
            {
                get { return m_title; }
                set { m_title = value; OnPropertyChanged(); }
            }

            public DateTime LastModified
            {
                get { return m_lastModified; }
                set { m_lastModified = value; OnPropertyChanged(); }
            }

            public FileFormat FileType
            {
                get { return m_fileType; }
                set { m_fileType = value; OnPropertyChanged(); }
            }

            public static bool TryGetInfo(string path, out SaveFileInfo info)
            {
                if (Editor.TryOpenFile(path, out LCSSave saveFile))
                {
                    string lastMissionPassedKey = saveFile.Stats.LastMissionPassedName;
                    if (!Gxt.TheText.TryGetValue("MAIN", lastMissionPassedKey, out string title))
                    {
                        title = $"(invalid GXT key: {lastMissionPassedKey})";
                    }

                    saveFile.SimpleVars.TimeStamp.TryGetDateTime(out DateTime timeStamp);
                    if (!saveFile.FileFormat.IsPS2)
                    {
                        timeStamp = File.GetLastWriteTime(path);
                    }

                    info = new SaveFileInfo()
                    {
                        SaveFile = saveFile,
                        Path = path,
                        FileType = saveFile.FileFormat,
                        LastModified = timeStamp,
                        Title = title
                    };

                    return true;
                }

                info = new SaveFileInfo()
                {
                    SaveFile = null,
                    Path = path,
                    FileType = FileFormat.Default,
                    LastModified = DateTime.MinValue,
                    Title = "(invalid save file)"
                };

                return false;
            }
        }
    }
}
