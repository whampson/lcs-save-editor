using GTASaveData;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using WpfEssentials;

namespace LCSSaveEditor.Core
{
    public class Settings : ObservableObject
    {
        public const int DefaultRecentFilesCapacity = 10;

        private ObservableCollection<string> m_recentFiles;
        private int m_recentFilesCapacity;
        private string m_lastDirAccessed;
        private string m_lastFileAccessed;
        private string m_welcomeDir;
        private bool m_welcomeRecurse;
        private bool m_updateTimeStamp;

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

        public bool UpdateTimeStampOnSave
        {
            get { return m_updateTimeStamp; }
            set { m_updateTimeStamp = value; OnPropertyChanged(); }
        }

        public static Settings TheSettings { get; private set; }

        static Settings()
        {
            TheSettings = new Settings();
        }

        public Settings()
        {
            m_recentFiles = new ObservableCollection<string>();
            m_recentFilesCapacity = DefaultRecentFilesCapacity;
            UpdateTimeStampOnSave = true;
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
}
