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

using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Helpers;
using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Models;
using LcsSaveEditor.Resources;
using System.ComponentModel;

namespace LcsSaveEditor.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private SaveData m_currentSaveData;
        private bool m_isFileModified;
        private string m_mostRecentFilePath;
        private string m_windowTitle;
        private string m_statusText;

        public MainViewModel()
        {
            m_windowTitle = Strings.AppName;
            m_statusText = Strings.StatusTextWelcome;
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
                return Strings.ToolTipTextFileModified;
            }
        }

        public string FileTypeToolTipText
        {
            get {
                if (CurrentSaveData == null) {
                    return string.Empty;
                }
                GamePlatform plat = CurrentSaveData.FileType;
                string platName = EnumHelper.GetAttribute<DescriptionAttribute>(plat).Description;
                string fmt = (plat == GamePlatform.Android || plat == GamePlatform.IOS)
                    ? Strings.ToolTipTextFileType2
                    : Strings.ToolTipTextFileType1;

                return string.Format(fmt, platName);
            }
        }
    }
}
