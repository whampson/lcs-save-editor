using LCSSaveEditor.Core;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfEssentials;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class WindowBase : ObservableObject
    {
        public event EventHandler WindowHideRequest;
        public event EventHandler WindowCloseRequest;
        public event EventHandler<MessageBoxEventArgs> MessageBoxRequest;
        public event EventHandler<FileDialogEventArgs> FileDialogRequest;
        public event EventHandler<FileDialogEventArgs> FolderDialogRequest;

        public const int DefaultTimerDuration = 5;   // seconds

        private readonly DispatcherTimer m_timer;
        private string m_title;
        private int m_timerDuration;
        private int m_timerTick;
        private string m_statusText;
        private string m_permanentStatusText;
        

        public string Title
        {
            get { return m_title; }
            set { m_title = value; OnPropertyChanged(); }
        }

        public int TimerDuration
        {
            get { return m_timerDuration; }
            set { m_timerDuration = value; OnPropertyChanged(); }
        }

        public int TimerTick
        {
            get { return m_timerTick; }
            set { m_timerTick = value; OnPropertyChanged(); }
        }

        public string StatusText
        {
            get { return m_statusText; }
            set
            {
                if (m_timer.IsEnabled)
                {
                    m_timer.Stop();
                }
                m_statusText = value;
                OnPropertyChanged();
            }
        }

        public WindowBase()
        {
            m_timer = new DispatcherTimer();
            m_timerDuration = DefaultTimerDuration;
        }

        public virtual void Initialize()
        {
            m_timer.Tick += StatusTimer_Tick;
        }

        public virtual void Shutdown()
        {
            m_timer.Tick -= StatusTimer_Tick;
        }

        public void SetStatusText(string status)
        {
            StatusText = status;
            m_permanentStatusText = status;
        }

        public void SetTimedStatusText(string status)
        {
            if (m_timer.IsEnabled)
            {
                m_timer.Stop();
                m_statusText = m_permanentStatusText;
            }

            m_permanentStatusText = StatusText;
            StatusText = status;
            m_timerTick = m_timerDuration;
            m_timer.Interval = TimeSpan.FromSeconds(1);
            m_timer.Start();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (m_timerTick <= 0)
            {
                m_timer.Stop();
                StatusText = m_permanentStatusText;
            }

            m_timerTick--;
        }

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
                "You will lose all unsaved changes. Continue?", "Revert File?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, callback: callback));
        }

        public void ShowFileDialog(FileDialogType type, Action<bool?, FileDialogEventArgs> callback)
        {
            FileDialogEventArgs e = new FileDialogEventArgs(type, callback)
            {
                InitialDirectory = Settings.TheSettings.LastFileAccessed
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

        public ICommand WindowHideCommand => new RelayCommand
        (
            () => WindowHideRequest?.Invoke(this, EventArgs.Empty)
        );

        public ICommand WindowCloseCommand => new RelayCommand
        (
            () => WindowCloseRequest?.Invoke(this, EventArgs.Empty)
        );
    }
}
