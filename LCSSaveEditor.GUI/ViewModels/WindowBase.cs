using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Events;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class WindowBase : ViewModelBase
    {
        public event EventHandler<MessageBoxEventArgs> MessageBoxRequest;
        public event EventHandler<FileDialogEventArgs> FileDialogRequest;
        public event EventHandler<FileDialogEventArgs> FolderDialogRequest;
        public event EventHandler<GxtDialogEventArgs> GxtDialogRequest;
        public event EventHandler<FileIOEventArgs> OpenFileRequest;
        public event EventHandler<FileIOEventArgs> SaveFileRequest;
        public event EventHandler<FileIOEventArgs> CloseFileRequest;
        public event EventHandler<FileIOEventArgs> RevertFileRequest;

        private string m_title;
        private bool m_suppressExternalChangesCheck;

        public string Title
        {
            get { return m_title; }
            set { m_title = value; OnPropertyChanged(); }
        }

        public bool SuppressExternalChangesCheck
        {
            get { return m_suppressExternalChangesCheck; }
            set { m_suppressExternalChangesCheck = value; OnPropertyChanged(); }
        }

        public WindowBase()
        { }

        public virtual void Initialize()
        { }

        public virtual void Shutdown()
        { }

        #region Window-Global Functions
        public void OpenFile(string path)
        {
            OpenFileRequest?.Invoke(this, new FileIOEventArgs() { Path = path });
        }

        public void SaveFile()
        {
            SaveFile(TheSettings.MostRecentFile);
        }

        public void SaveFile(string path)
        {
            SaveFileRequest?.Invoke(this, new FileIOEventArgs() { Path = path });
        }

        public void RevertFile()
        {
            RevertFileRequest?.Invoke(this, new FileIOEventArgs());
        }

        public void CloseFile()
        {
            CloseFileRequest?.Invoke(this, new FileIOEventArgs());
        }
        
        public void CheckForExternalChanges()
        {
            if (!TheEditor.IsFileOpen || SuppressExternalChangesCheck)
            {
                return;
            }

            DateTime lastWriteTime = File.GetLastWriteTime(TheSettings.MostRecentFile);
            if (lastWriteTime != TheEditor.LastWriteTime)
            {
                Log.Info("External changes detected.");
                PromptExternalChangesDetected((r) =>
                {
                    if (r == MessageBoxResult.Yes)
                    {
                        RevertFileRequest?.Invoke(this, new FileIOEventArgs() { SuppressPrompting = true });
                    }
                    else if (r == MessageBoxResult.No)
                    {
                        SuppressExternalChangesCheck = true;
                    }
                });
            }
        }
        #endregion

        #region Dialog Functions
        public void ShowInfo(string text, string title = "Information")
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(
                text, title, icon: MessageBoxImage.Information));
        }

        public void ShowWarning(string text, string title = "Warning")
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(
                text, title, icon: MessageBoxImage.Warning));
        }

        public void ShowError(string text, string title = "Error")
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(
                text, title, icon: MessageBoxImage.Error));
        }

        public void ShowException(Exception e, string text = "An error has occurred.", string title = "Error")
        {
            text += $"\n\n{e.GetType().Name}: {e.Message}";
            ShowError(text, title);
        }

        public void PromptSaveChanges(Action<MessageBoxResult> callback)
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(
                "Do you want to save your changes?", "Save Changes?",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question, callback: callback));
        }

        public void PromptConfirmRevert(Action<MessageBoxResult> callback)
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(
                "Reverting the file will cause you to lose all unsaved changes.\n\n" +
                "Continue?", "Revert File",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, callback: callback));
        }

        public void PromptExternalChangesDetected(Action<MessageBoxResult> callback)
        {
            MessageBoxRequest?.Invoke(this, new MessageBoxEventArgs(
                "The file has been modified on disk.\n\n" +
                "Do you want to reload the file? You will lose all unsaved changes.",
                "External Changes Detected",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, callback: callback));
        }

        public void ShowFileDialog(FileDialogType type, Action<bool?, FileDialogEventArgs> callback)
        {
            FileDialogEventArgs e = new FileDialogEventArgs(type, callback)
            {
                InitialDirectory = Settings.TheSettings.LastDirectoryAccessed
            };
            FileDialogRequest?.Invoke(this, e);
        }

        public void ShowFolderDialog(FileDialogType type, Action<bool?, FileDialogEventArgs> callback)
        {
            FileDialogEventArgs e = new FileDialogEventArgs(type, callback)
            {
                InitialDirectory = Settings.TheSettings.LastDirectoryAccessed
            };
            FolderDialogRequest?.Invoke(this, e);
        }

        public void ShowGxtDialog(Action<bool?, GxtDialogEventArgs> callback,
            string table = "MAIN", bool allowTableSelection = false,
            bool modal = true)
        {
            GxtDialogRequest?.Invoke(this, new GxtDialogEventArgs()
            {
                TableName = table,
                AllowTableSelection = allowTableSelection,
                Modal = modal,
                Callback = callback
            });
        }
        #endregion

        #region Window-Global Commands
        public ICommand FileOpenCommand => new RelayCommand
        (
            () => ShowFileDialog(FileDialogType.OpenFileDialog, (r, e) =>
            {
                if (r != true) return;
                TheSettings.SetLastAccess(e.FileName);
                OpenFile(e.FileName);
            })
        );

        public ICommand FileSaveCommand => new RelayCommand
        (
            () => SaveFile(),
            () => TheEditor.IsFileOpen
        );

        public ICommand FileSaveAsCommand => new RelayCommand
        (
            () => ShowFileDialog(FileDialogType.SaveFileDialog, (r, e) =>
            {
                if (r != true) return;
                TheSettings.SetLastAccess(e.FileName);
                SaveFile(e.FileName);
            }),
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

        public ICommand FileExitCommand => new RelayCommand
        (
            () => Application.Current.MainWindow.Close()
        );
        #endregion
    }
}
