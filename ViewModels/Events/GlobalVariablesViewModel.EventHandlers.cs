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

using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Models;
using System.Collections.Specialized;
using System.IO;

namespace LcsSaveEditor.ViewModels
{
    public partial class GlobalVariablesViewModel : PageViewModelBase
    {
        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_suppressGlobalVariablesChanged) {
                return;
            }

            m_suppressNamedGlobalVariablesChanged = true;
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++) {
                        m_namedGlobalVariables.Insert(e.NewStartingIndex + i, new NamedScriptVariable((ScriptVariable) e.NewItems[i]));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++) {
                        m_namedGlobalVariables.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    for (int i = 0; i < e.NewItems.Count; i++) {
                        m_namedGlobalVariables[e.NewStartingIndex + i].Value = (ScriptVariable) e.NewItems[i];
                    }
                    break;
            }
            m_suppressNamedGlobalVariablesChanged = false;
        }

        private void NamedGlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_suppressNamedGlobalVariablesChanged) {
                return;
            }

            m_suppressGlobalVariablesChanged = true;
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++) {
                        m_globalVariables.Insert(e.NewStartingIndex + i, ((NamedScriptVariable) e.NewItems[i]).Value);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++) {
                        m_globalVariables.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    for (int i = 0; i < e.NewItems.Count; i++) {
                        m_globalVariables[e.NewStartingIndex + i] = ((NamedScriptVariable) e.NewItems[i]).Value;
                    }
                    break;
            }
            m_suppressGlobalVariablesChanged = false;
        }

        private void FileDialog_ResultAction(bool? dialogResult, FileDialogEventArgs e)
        {
            if (dialogResult != true) {
                return;
            }

            Settings.Current.CustomVariablesFileDialogDirectory = Path.GetDirectoryName(e.FileName);

            switch (e.DialogType) {
                case FileDialogType.OpenDialog:
                    LoadCustomVariables(e.FileName);
                    break;
                case FileDialogType.SaveDialog:
                    SaveCustomVariables(e.FileName);
                    break;
            }
        }
    }
}
