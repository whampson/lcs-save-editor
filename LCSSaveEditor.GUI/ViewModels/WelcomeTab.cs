﻿using GTASaveData;
using GTASaveData.LCS;
using GTASaveData.Types;
using LCSSaveEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class WelcomeTab : TabPageBase
    {
        private BackgroundWorker m_lukeFileWalker;
        private ObservableCollection<ListItem> m_listItems;
        private ListItem m_selectedItem;
        private bool m_openedOnce;
        private bool m_isSearching;
        private bool m_searchPending;
        private bool m_cancelPending;

        public string SelectedDirectory
        {
            get { return MainViewModel.TheSettings.SaveDirectory; }
            set { MainViewModel.TheSettings.SaveDirectory = value; OnPropertyChanged(); }
        }

        public bool SearchSubDirectories
        {
            get { return MainViewModel.TheSettings.SaveDirectoryRecursiveSearch; }
            set { MainViewModel.TheSettings.SaveDirectoryRecursiveSearch = value; OnPropertyChanged(); }
        }

        public BackgroundWorker SearchWorker
        {
            get { return m_lukeFileWalker; }
            set { m_lukeFileWalker = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ListItem> ListItems
        {
            get { return m_listItems; }
            private set { m_listItems = value; OnPropertyChanged(); }
        }

        public ListItem SelectedItem
        {
            get { return m_selectedItem; }
            set { m_selectedItem = value; OnPropertyChanged(); }
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

        public WelcomeTab(MainWindow mainViewModel)
            : base("Welcome", TabPageVisibility.WhenFileIsClosed, mainViewModel)
        {
            ListItems = new ObservableCollection<ListItem>();
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
            OnPropertyChanged(nameof(SearchSubDirectories));
        }

        public override void Unload()
        {
            base.Unload();
            CancelSearch();
        }

        public override void Update()
        {
            base.Update();
            //Search();
            // TODO: refresh files in list
        }

        public void Search()
        {
            // TODO: detect external changes

            if (IsSearching)
            {
                SearchPending = true;
                CancelSearch();
                return;
            }

            SearchPending = false;
            if (Directory.Exists(SelectedDirectory))
            {
                ListItems.Clear();
                SearchWorker.RunWorkerAsync();
                IsSearching = true;

                Log.Info($"Searching for GTA:LCS save files in {SelectedDirectory}...{(SearchSubDirectories ? " (recursive)" : "")}");
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

            if (SelectedItem != null)
            {
                MainViewModel.TheSettings.AddRecentFile(SelectedItem.Path);
                MainViewModel.TheEditor.ActiveFile = SelectedItem.SaveFile;
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
            SearchOption o = (SearchSubDirectories) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
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
                    MainViewModel.SetStatusText($"Searching {currDir}...");   // TODO: path shortening func
                }
                if (Editor.TryOpenFile(path, out LCSSave saveFile))
                {
                    string lastMissionPassedKey = saveFile.Stats.LastMissionPassedName;
                    saveFile.SimpleVars.TimeStamp.TryGetDateTime(out DateTime timeStamp);

                    if (!saveFile.FileFormat.IsPS2)
                    {
                        timeStamp = File.GetLastWriteTime(path);
                    }

                    ListItem item = new ListItem()
                    {
                        SaveFile = saveFile,
                        Path = path,
                        FileType = saveFile.FileFormat,
                        LastModified = timeStamp,
                        Title = lastMissionPassedKey
                    };

                    //if (item.Title.StartsWith('\uFFFF'))
                    //{
                    //    string key = item.Title.Substring(1);
                    //    if (MainViewModel.TheText.TryGetValue(key, out string title))
                    //    {
                    //        item.Title = title;
                    //    }
                    //}

                    SearchWorker.ReportProgress(-1, item);
                }
            }
        }

        private void FileWalker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is ListItem item)
            {
                ListItems.Add(item);
            }
        }

        private void FileWalker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsSearching = false;
            CancelPending = false;
            
            if (e.Cancelled)
            {
                Log.Info("Search cancelled.");
                MainViewModel.SetTimedStatusText("Search cancelled.", expiredStatus: "Ready.");
                return;
            }

            if (e.Error != null)
            {
                if (e.Error is UnauthorizedAccessException)
                {
                    Log.Exception(e.Error);
                }
                else
                {
                    // Re-throw and preserve stack trace
                    ExceptionDispatchInfo.Capture(e.Error).Throw();
                }

                Log.Info("Search completed with errors.");
                MainViewModel.SetTimedStatusText("Search completed with errors. See the log for details.", expiredStatus: "Ready.");
                return;
            }

            if (SearchPending)
            {
                Search();
                return;
            }

            Log.Info("Search completed.");
            MainViewModel.SetTimedStatusText("Search completed.", expiredStatus: "Ready.");
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
            () => SelectedItem != null
        );

        public ICommand BrowseCommand => new RelayCommand
        (
            () => MainViewModel.ShowFolderDialog(FileDialogType.OpenFileDialog, FolderDialogRequested_Callback)
        );
        #endregion

        public class ListItem
        {
            public LCSSave SaveFile { get; set; }
            public string Path { get; set; }
            public string Title { get; set; }
            public DateTime LastModified { get; set; }
            public FileFormat FileType { get; set; }
        }
    }
}
