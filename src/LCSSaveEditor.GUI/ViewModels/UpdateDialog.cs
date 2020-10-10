using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Utils;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.ViewModels
{
    public class UpdateDialog : DialogBase
    {
        public static UpdaterSettings UpdaterSettings => Settings.TheSettings.Updater;

        private GitHubRelease m_info;
        private int m_progress;
        private string m_progressText;
        private bool m_isDownloading;
        private bool m_downloadStarted;
        private string m_downloadPath;
        CancellationTokenSource m_cancelSource;

        public GitHubRelease UpdateInfo
        {
            get { return m_info; }
            set { m_info = value; OnPropertyChanged(); }
        }

        public int Progress
        {
            get { return m_progress; }
            set { m_progress = value; OnPropertyChanged(); }
        }

        public string ProgressText
        {
            get { return m_progressText; }
            set { m_progressText = value; OnPropertyChanged(); }
        }

        public bool IsDownloading
        {
            get { return m_isDownloading; }
            set { m_isDownloading = value; OnPropertyChanged(); }
        }

        public bool DownloadStarted
        {
            get { return m_downloadStarted; }
            set { m_downloadStarted = value; OnPropertyChanged(); }
        }

        public string DownloadPath
        {
            get { return m_downloadPath; }
            set { m_downloadPath = value; OnPropertyChanged(); }
        }

        public async void StartDownload()
        {
            if (IsDownloading) return;

            var pkg = Updater.GetUpdatePackageInfo(UpdateInfo);
            if (pkg == null)
            {
                ShowError("Update package not found in this release!", "Updater Error");
                return;
            }

            string downloadDir = Path.Combine(Directory.GetCurrentDirectory(), "update");
            Directory.CreateDirectory(downloadDir);

            DownloadPath = Path.Combine(downloadDir, pkg.Name);
            string downloadName = DownloadPath + ".part";

            double totalSizeMB = pkg.Size / (1024.0 * 1024.0);
            Progress = 0;
            DownloadStarted = true;

            m_cancelSource = new CancellationTokenSource();
            Task downloadTask = Updater.DownloadUpdatePackage(pkg, downloadName, m_cancelSource.Token, new Progress<double>((p) =>
            {
                int progress = (int) Math.Round(p * 100);
                double downloadedMB = totalSizeMB * p;
                
                if (progress > Progress)
                {
                    if (progress % 10 == 0)
                    {
                        Log.Info($"Download progress: {progress}%");
                    }
                    Progress = progress;
                }
                ProgressText = $"{downloadedMB:F2} / {totalSizeMB:F2} MB";
                IsDownloading = true;
            }));

            try
            {
                await downloadTask;
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    Log.Info("Download cancelled.");
                    File.Delete(downloadName);
                    DownloadStarted = false;
                    return;
                }
                ShowException(e, "An error occurred while downloading the update.", "Updater Error");
                return;
            }
            finally
            {
                IsDownloading = false;
                m_cancelSource.Dispose();
            }

            File.Move(downloadName, DownloadPath, true);  // remove .part
            ShowInfo($"Download complete!", "Complete");
            CloseDialog(true);

        }

        public void StopAllTheDownloading()
        {
            if (!IsDownloading) return;
            m_cancelSource.Cancel();
        }

        public ICommand ToggleDownload => new RelayCommand
        (
            () => { if (!IsDownloading) StartDownload(); else StopAllTheDownloading(); }
        );

        public ICommand ViewOnGitHub => new RelayCommand
        (
            () => Process.Start(new ProcessStartInfo(UpdateInfo.Url) { UseShellExecute = true })
        );
    }
}
