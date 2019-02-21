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
using LcsSaveEditor.Models;
using System;

namespace LcsSaveEditor.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public event EventHandler<TabRefreshEventArgs> TabRefresh;
        public event EventHandler<SaveDataEventArgs> DataLoaded;
        public event EventHandler<SaveDataEventArgs> DataClosing;
        public event EventHandler<MessageBoxEventArgs> MessageBoxRequested;
        public event EventHandler<FileDialogEventArgs> FileDialogRequested;
        public event EventHandler LogWindowRequested;
        public event EventHandler AboutDialogRequested;

        private void OnTabRefresh(TabRefreshTrigger trigger, int desiredTabIndex = -1)
        {
            TabRefresh?.Invoke(this, new TabRefreshEventArgs(trigger));

            if (desiredTabIndex != -1 && desiredTabIndex == SelectedTabIndex) {
                SelectedTabIndex = -1;
            }
            SelectedTabIndex = desiredTabIndex;
        }

        private void OnDataLoaded(SaveData data)
        {
            DataLoaded?.Invoke(this, new SaveDataEventArgs(data));
        }

        private void OnDataClosing(SaveData data)
        {
            DataClosing?.Invoke(this, new SaveDataEventArgs(data));
        }

        private void OnMessageBoxRequested(MessageBoxEventArgs e)
        {
            MessageBoxRequested?.Invoke(this, e);
        }

        private void OnFileDialogRequested(FileDialogEventArgs e)
        {
            FileDialogRequested?.Invoke(this, e);
        }

        private void OnLogWindowRequested()
        {
            LogWindowRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnAboutDialogRequested()
        {
            AboutDialogRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
