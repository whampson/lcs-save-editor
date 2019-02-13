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

using LcsSaveEditor.Resources;
using Microsoft.Win32;
using System;
using System.Windows;

namespace LcsSaveEditor.Infrastructure
{
    /// <summary>
    /// Parameters for opening a <see cref="FileDialog"/> from an event.
    /// </summary>
    public class FileDialogEventArgs : EventArgs
    {
        public FileDialogEventArgs(FileDialogType dialogType,
            string title = null,
            string filter = null,
            string fileName = null,
            string initialDirectory = null,
            Action<bool?, FileDialogEventArgs> resultAction = null)
        {
            DialogType = dialogType;
            Title = title;
            Filter = filter;
            FileName = fileName;
            InitialDirectory = initialDirectory;
            ResultAction = resultAction;
        }

        public FileDialogType DialogType
        {
            get;
        }

        public string Title
        {
            get;
        }

        public string Filter
        {
            get;
        }

        public string FileName
        {
            get;
            private set;
        }

        public string InitialDirectory
        {
            get;
        }

        public Action<bool?, FileDialogEventArgs> ResultAction
        {
            get;
        }

        public void ShowDialog()
        {
            ShowDialog(null);
        }

        public void ShowDialog(Window owner)
        {
            FileDialog dialog;
            switch (DialogType) {
                case FileDialogType.OpenDialog:
                    dialog = new OpenFileDialog();
                    break;
                case FileDialogType.SaveDialog:
                    dialog = new SaveFileDialog();
                    break;
                default:
                    string msg = string.Format("{0} ({1})",
                        Strings.ExceptionOops, nameof(ShowDialog));
                    throw new InvalidOperationException(msg);
            }

            dialog.Title = Title;
            dialog.Filter = Filter;
            dialog.FileName = FileName;
            dialog.InitialDirectory = InitialDirectory;

            bool? result;
            if (owner == null) {
                result = dialog.ShowDialog();
            }
            else {
                result = dialog.ShowDialog(owner);
            }

            if (result == true) {
                FileName = dialog.FileName;
            }

            ResultAction?.Invoke(result, this);
        }
    }

    public enum FileDialogType
    {
        OpenDialog,
        SaveDialog
    }
}
