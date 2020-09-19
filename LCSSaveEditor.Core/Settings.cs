using GTASaveData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WpfEssentials;

namespace LCSSaveEditor.Core
{
    public class Settings : ObservableObject
    {
        public const int DefaultRecentFilesCapacity = 10;

        private string m_lastDirAccessed;
        private string m_lastFileAccessed;
        private bool m_setTimeStamp;
        private string m_welcomeDir;
        private bool m_welcomeRecurse;
        private ObservableCollection<string> m_welcomeList;
        private ObservableCollection<string> m_recentFiles;
        private int m_recentFilesCapacity;
        private UpdaterSettings m_updater;

        public string LastDirectoryAccessed
        {
            get { return m_lastDirAccessed; }
            set { m_lastDirAccessed = value; OnPropertyChanged(); }
        }

        public string LastFileAccessed
        {
            get { return m_lastFileAccessed; }
            set { m_lastFileAccessed = value; OnPropertyChanged(); }
        }

        public bool UpdateFileTimeStamp
        {
            get { return m_setTimeStamp; }
            set { m_setTimeStamp = value; OnPropertyChanged(); }
        }

        public string WelcomeDirectory
        {
            get { return m_welcomeDir; }
            set { m_welcomeDir = value; OnPropertyChanged(); }
        }

        public bool WelcomeRecursiveSearch
        {
            get { return m_welcomeRecurse; }
            set { m_welcomeRecurse = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> WelcomeList
        {
            get { return m_welcomeList; }
            set { m_welcomeList = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> RecentFiles
        {
            get { return m_recentFiles; }
            set { m_recentFiles = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public string MostRecentFile
        {
            get { return (m_recentFiles.Count > 0) ? m_recentFiles[0] : null; }
        }

        public int RecentFilesCapacity
        {
            get { return m_recentFilesCapacity; }
            set { m_recentFilesCapacity = value; OnPropertyChanged(); }
        }

        public UpdaterSettings Updater
        {
            get { return m_updater; }
            set { m_updater = value; OnPropertyChanged(); }
        }

        public static Settings TheSettings { get; }

        static Settings()
        {
            TheSettings = new Settings();
        }

        public Settings()
        {
            WelcomeList = new ObservableCollection<string>();
            RecentFiles = new ObservableCollection<string>();
            RecentFilesCapacity = DefaultRecentFilesCapacity;
            UpdateFileTimeStamp = true;
            Updater = new UpdaterSettings();
        }

        public void AddRecentFile(string path)
        {
            int index = RecentFiles.IndexOf(path);
            if (index == 0)
            {
                return;
            }
            if (index != -1)
            {
                RecentFiles.Move(index, 0);
                return;
            }

            RecentFiles.Insert(0, path);
            while (RecentFiles.Count > RecentFilesCapacity)
            {
                RecentFiles.RemoveAt(RecentFilesCapacity);
            }
        }

        public void ClearRecentFiles()
        {
            RecentFiles.Clear();
        }

        public void SetLastAccess(string path)
        {
            bool isDir = Directory.Exists(path);
            bool isFile = File.Exists(path);

            if (isDir)
            {
                LastDirectoryAccessed = Path.GetFullPath(path);
            }
            if (isFile)
            {
                LastFileAccessed = Path.GetFullPath(path);
                LastDirectoryAccessed = Path.GetDirectoryName(path);
            }
        }

        public void LoadSettings(string path)
        {
            string settingsJson = File.ReadAllText(path);
            JsonConvert.PopulateObject(settingsJson, this);
        }

        public void SaveSettings(string path)
        {
            string settingsJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, settingsJson);
        }
    }

    public class UpdaterSettings
    {
        public bool CheckForUpdatesAtStartup { get; set; }
        public bool PreReleaseRing { get; set; }
        public bool StandaloneRing { get; set; }
        public bool CleanupAfterUpdate { get; set; }
        public List<string> CleanupList { get; set; }

        public UpdaterSettings()
        {
            CheckForUpdatesAtStartup = true;
            CleanupList = new List<string>();
        }

        public bool ShouldSerializeStandaloneRing()
        {
            return StandaloneRing != false;
        }

        public bool ShouldSerializeCleanupAfterUpdate()
        {
            return CleanupAfterUpdate != false;
        }

        public bool ShouldSerializeCleanupList()
        {
            return CleanupList.Count > 0;
        }
    }
}
