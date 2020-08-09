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
        private string m_lastDirectoryAccessed;
        private string m_lastFileAccessed;
        private bool m_autoDetectFileType;
        private FileFormat m_forcedFileType;
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
            get { return m_lastDirectoryAccessed; }
            set { m_lastDirectoryAccessed = value; OnPropertyChanged(); }
        }
        public string LastFileAccessed
        {
            get { return m_lastFileAccessed; }
            set { m_lastFileAccessed = value; OnPropertyChanged(); }
        }

        public bool AutoDetectFileType
        {
            get { return m_autoDetectFileType; }
            set { m_autoDetectFileType = value; OnPropertyChanged(); }
        }

        public FileFormat ForcedFileType
        {
            get { return m_forcedFileType; }
            set { m_forcedFileType = value; OnPropertyChanged(); }
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
            AutoDetectFileType = true;
        }

        public void AddRecentFile(string path)
        {
            int index = m_recentFiles.IndexOf(path);
            if (index == 0)
            {
                return;
            }
            if (index != -1)
            {
                m_recentFiles.Move(index, 0);
                return;
            }

            m_recentFiles.Insert(0, path);
            while (m_recentFiles.Count > m_recentFilesCapacity)
            {
                m_recentFiles.RemoveAt(m_recentFilesCapacity);
            }
        }

        public void ClearRecentFiles()
        {
            m_recentFiles.Clear();
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
