using GTASaveData;
using GTASaveData.LCS;
using System;
using System.IO;
using WpfEssentials;

namespace LCSSaveEditor.Core
{
    public class Editor : ObservableObject
    {
        public event EventHandler<string> FileOpening;
        public event EventHandler FileOpened;
        public event EventHandler FileClosing;
        public event EventHandler FileClosed;
        public event EventHandler<string> FileSaving;
        public event EventHandler FileSaved;
        

        private LCSSave m_activeFile;

        public LCSSave ActiveFile
        {
            get { return m_activeFile; }
            set
            {
                CloseFile();
                if (value != null) OnFileOpening(null);
                m_activeFile = value;
                if (value != null) OnFileOpened();
                OnPropertyChanged();
            }
        }

        public bool IsFileOpen => m_activeFile != null;

        public static Settings TheSettings => Settings.TheSettings;

        public static Editor TheEditor { get; private set; }

        static Editor()
        {
            TheEditor = new Editor();
        }

        public Editor()
        { }

        public void ChangeFileFormat(FileFormat newFormat)
        {
            if (!IsFileOpen) throw NoFileOpen();

            if (newFormat != ActiveFile.FileFormat)
            {
                // TODO: actually implement this
                ActiveFile.FileFormat = newFormat;
            }
        }

        public void OpenFile(string path)
        {
            if (IsFileOpen) throw FileAlreadyOpened();

            OnFileOpening(path);
            m_activeFile = ((TheSettings.AutoDetectFileType)
                ? SaveData.Load<LCSSave>(path)
                : SaveData.Load<LCSSave>(path, TheSettings.ForcedFileType))
                ?? throw BadSaveData();
            TheSettings.AddRecentFile(path);
            OnFileOpened();
        }

        public void SaveFile(string path)
        {
            if (!IsFileOpen) throw NoFileOpen();
            
            OnFileSaving(path);
            ActiveFile.Save(path);
            TheSettings.AddRecentFile(path);
            OnFileSaved();
        }

        public void CloseFile()
        {
            if (!IsFileOpen) return;

            OnFileClosing();
            m_activeFile = null;
            OnFileClosed();
            OnPropertyChanged(nameof(ActiveFile));
        }

        private void OnFileOpening(string path)
        {
            Log.InfoF("Opening file{0}...", (path != null) ? $" at {path}" : "");
            FileOpening?.Invoke(this, path);
        }

        private void OnFileOpened()
        {
            if (!ActiveFile.FileFormat.IsMobile)
            {
                ActiveFile.Name = Path.GetFileNameWithoutExtension(TheSettings.MostRecentFile);
            }
            if (!ActiveFile.FileFormat.IsPS2)
            {
                ActiveFile.TimeStamp = File.GetLastWriteTime(TheSettings.MostRecentFile);
            }

            OnPropertyChanged(nameof(IsFileOpen));
            Log.Info("File opened successfully.");
            Log.Info("File Info:");
            Log.Info($"        Type = {ActiveFile.FileFormat}");
            Log.Info($"        Name = {ActiveFile.Name}");
            Log.Info($"  Time Stamp = {ActiveFile.TimeStamp}");
            Log.Info($"    Progress = {(ActiveFile.Stats.ProgressMade / ActiveFile.Stats.TotalProgressInGame):P2}");
            Log.Info($"Last Mission = {ActiveFile.Stats.LastMissionPassedName}");

            FileOpened?.Invoke(this, EventArgs.Empty);
        }

        private void OnFileClosing()
        {
            Log.Info("Closing file...");
            FileClosing?.Invoke(this, EventArgs.Empty);
        }

        private void OnFileClosed()
        {
            OnPropertyChanged(nameof(IsFileOpen));
            Log.Info("File closed.");

            FileClosed?.Invoke(this, EventArgs.Empty);
        }

        private void OnFileSaving(string path)
        {
            Log.InfoF($"Saving file to {path}...");
            FileSaving?.Invoke(this, path);
        }

        private void OnFileSaved()
        {
            Log.Info("File saved successfully.");
            FileSaved?.Invoke(this, EventArgs.Empty);
        }

        private InvalidOperationException FileAlreadyOpened()
        {
            return new InvalidOperationException("A file is already open!");
        }

        private InvalidOperationException NoFileOpen()
        {
            return new InvalidOperationException("No file is open!");
        }

        private InvalidDataException BadSaveData()
        {
            throw new InvalidDataException("The file is not a valid GTA:LCS save file!");
        }
    }
}
