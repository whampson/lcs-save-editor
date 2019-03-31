#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using LcsSaveEditor.Core;
using LcsSaveEditor.Core.Helpers;
using LcsSaveEditor.Models;
using LcsSaveEditor.Resources;
using System.Collections.ObjectModel;

namespace LcsSaveEditor.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private RecentFilesList m_recentFiles;
        private ObservableCollection<PageViewModel> m_tabs;
        private int m_selectedTabIndex;
        private SaveData m_currentSaveData;
        private bool m_isFileModified;
        private string m_mostRecentFilePath;
        private string m_windowTitle;
        private string m_statusText;

        public MainViewModel()
        {
            m_recentFiles = new RecentFilesList(10);
            m_tabs = new ObservableCollection<PageViewModel>();
            m_selectedTabIndex = -1;
            m_statusText = FrontendResources.Main_StatusText_Welcome;

            UpdateWindowTitle();
        }

        public ObservableCollection<PageViewModel> Tabs
        {
            get { return m_tabs; }
            set { m_tabs = value; OnPropertyChanged(); }
        }

        public int SelectedTabIndex
        {
            get { return m_selectedTabIndex; }
            set { m_selectedTabIndex = value; OnPropertyChanged(); }
        }

        public SaveData CurrentSaveData
        {
            get { return m_currentSaveData; }
            set {
                m_currentSaveData = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFileOpen));
                OnPropertyChanged(nameof(FileTypeToolTipText));
            }
        }

        public bool IsFileOpen
        {
            get { return m_currentSaveData != null; }
        }

        public bool IsFileModified
        {
            get { return m_isFileModified; }
            set { m_isFileModified = value; OnPropertyChanged(); }
        }

        public string MostRecentFilePath
        {
            get { return m_mostRecentFilePath; }
            set { m_mostRecentFilePath = value; OnPropertyChanged(); }
        }

        public RecentFilesList RecentFiles
        {
            get { return m_recentFiles; }
            set { m_recentFiles = value; OnPropertyChanged(); }
        }

        public string WindowTitle
        {
            get { return m_windowTitle; }
            set { m_windowTitle = value; OnPropertyChanged(); }
        }

        public string StatusText
        {
            get { return m_statusText; }
            set { m_statusText = value; OnPropertyChanged(); }
        }

        public string FileModifiedToolTipText
        {
            get {
                if (CurrentSaveData == null) {
                    return string.Empty;
                }
                return FrontendResources.Main_ToolTip_UnsavedChanges;
            }
        }

        public string FileTypeToolTipText
        {
            get {
                if (CurrentSaveData == null) {
                    return string.Empty;
                }

                string platName = GamePlatformHelper.GetPlatformName(CurrentSaveData.FileType);
                return string.Format(FrontendResources.Main_ToolTip_Compatibility, platName);
            }
        }
    }
}
