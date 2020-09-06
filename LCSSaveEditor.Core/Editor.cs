using GTASaveData;
using GTASaveData.LCS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
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
        private DateTime m_lastWriteTime;
        private ScriptVersion m_scriptVersion;

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

        public DateTime LastWriteTime
        {
            get { return m_lastWriteTime; }
            set { m_lastWriteTime = value; OnPropertyChanged(); }
        }

        public ScriptVersion ScriptVersion
        {
            get { return m_scriptVersion; }
            set { m_scriptVersion = value; OnPropertyChanged(); }
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

        public static IEnumerable<GlobalVariable> GlobalVariableIDs =>
            Enum.GetValues(typeof(GlobalVariable)) as IEnumerable<GlobalVariable>;

        public ScriptVersion GetScriptVersion()
        {
            switch (ActiveFile.Scripts.MainScriptSize)
            {
                case 274282: return ScriptVersion.Android;
                case 274312: return ScriptVersion.iOS;
                case 272710: return ScriptVersion.PS2;
                case 270081: return ScriptVersion.PSP;
                default: return ScriptVersion.None;
            }
        }

        public bool IsMobileScript()
        {
            return ScriptVersion == ScriptVersion.Android || ScriptVersion == ScriptVersion.iOS;
        }

        public int GetNumGlobals()
        {
            if (!IsFileOpen) throw NoFileOpen();
            return ActiveFile.Scripts.GlobalVariables.Count;
        }

        public int GetIndexOfGlobal(GlobalVariable g)
        {
            if (g == GlobalVariable.Null)
            {
                return -1;
            }

            int index = (int) g;
            if (!IsMobileScript())
            {
                if (g == GlobalVariable._UnknownFlag)
                {
                    return -1;
                }
                if (g > GlobalVariable._UnknownFlag)
                {
                    index--;
                }
            }

            return index;
        }

        public GlobalVariable GetGlobalId(int index)
        {
            if (!IsMobileScript() && index >= 7)
            {
                index++;
            }

            GlobalVariable g = (GlobalVariable) index;
            if (GlobalVariableIDs.Contains(g))
            {
                return g;
            }

            return GlobalVariable.Null;
        }

        public int GetGlobal(GlobalVariable g)
        {
            return GetGlobal(GetIndexOfGlobal(g));
        }
        public float GetGlobalAsFloat(GlobalVariable g)
        {
            return GetGlobalAsFloat(GetIndexOfGlobal(g));
        }

        public int GetGlobal(int index)
        {
            if (!IsFileOpen) throw NoFileOpen();
            if (index < 0) throw BadGlobalVariable(index);

            return ActiveFile.Scripts.GetGlobal(index);
        }

        public float GetGlobalAsFloat(int index)
        {
            if (!IsFileOpen) throw NoFileOpen();
            if (index < 0) throw BadGlobalVariable(index);

            return ActiveFile.Scripts.GetGlobalAsFloat(index);
        }

        public void SetGlobal(GlobalVariable g, int value)
        {
            SetGlobal(GetIndexOfGlobal(g), value);
        }

        public void SetGlobal(GlobalVariable g, float value)
        {
            SetGlobal(GetIndexOfGlobal(g), value);
        }

        public void SetGlobal(int index, int value)
        {
            if (!IsFileOpen) throw NoFileOpen();
            if (index < 0) throw BadGlobalVariable(index);

            ActiveFile.Scripts.SetGlobal(index, value);
        }

        public void SetGlobal(int index, float value)
        {
            if (!IsFileOpen) throw NoFileOpen();
            if (index < 0) throw BadGlobalVariable(index);

            ActiveFile.Scripts.SetGlobal(index, value);
        }


        #region File I/O
        public void ChangeFileFormat(FileFormat newFormat)
        {
            if (!IsFileOpen) throw NoFileOpen();

            if (newFormat != ActiveFile.FileFormat)
            {
                // TODO: actually implement this
                ActiveFile.FileFormat = newFormat;
            }
        }

        public static bool TryOpenFile(string path, out LCSSave saveFile)
        {
            try
            {
                if (SaveData.GetFileFormat<LCSSave>(path, out FileFormat fmt))
                {
                    saveFile = SaveData.Load<LCSSave>(path, fmt);
                    return saveFile != null;
                }

                saveFile = null;
                return false;
            }
            catch (Exception e)
            {
                if (e is IOException ||
                    e is SecurityException ||
                    e is UnauthorizedAccessException ||
                    e is SerializationException ||
                    e is InvalidDataException)
                {
                    Log.Error(e);
                    saveFile = null;
                    return false;
                }
                throw;
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
            LastWriteTime = File.GetLastWriteTime(path);
            ScriptVersion = GetScriptVersion();
            OnFileOpened();
        }

        public void SaveFile(string path)
        {
            if (!IsFileOpen) throw NoFileOpen();
            
            OnFileSaving(path);
            ActiveFile.Save(path);
            TheSettings.AddRecentFile(path);
            LastWriteTime = File.GetLastWriteTime(path);
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
            Log.InfoF("Opening file{0}...", (path != null) ? $": {path}" : "");
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
            Log.Info($" Script Size = {ActiveFile.Scripts.MainScriptSize}");

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
            Log.InfoF($"Saving file: {path}...");
            FileSaving?.Invoke(this, path);
        }

        private void OnFileSaved()
        {
            Log.Info("File saved successfully.");
            FileSaved?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Exceptions
        private InvalidDataException BadSaveData()
        {
            throw new InvalidDataException("The file is not a valid GTA:LCS save file!");
        }

        private IndexOutOfRangeException BadGlobalVariable(int index)
        {
            throw new IndexOutOfRangeException($"Invalid global variable: {index}");
        }

        private InvalidOperationException FileAlreadyOpened()
        {
            return new InvalidOperationException("A file is already open!");
        }

        private InvalidOperationException NoFileOpen()
        {
            return new InvalidOperationException("No file is open!");
        }
        #endregion
    }
}
