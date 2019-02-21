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
using System.Collections.Specialized;
using System.IO;

namespace LcsSaveEditor.ViewModels
{
    public partial class ScriptsViewModel : PageViewModel
    {
        private bool m_suppressGlobalVariablesCollectionChanged;
        private bool m_suppressNamedGlobalVariablesCollectionChanged;
        private bool m_suppressNamedGlobalVariablesItemPropertyChanged;

        private void MainViewModel_DataLoaded(object sender, SaveDataEventArgs e)
        {
            m_globalVariables = e.Data.Scripts.GlobalVariables;
            foreach (ScriptVariable v in m_globalVariables) {
                m_namedGlobalVariables.Add(new NamedScriptVariable(v));
            }

            AutoLoadCustomVariables();
            ActiveThreads = e.Data.Scripts.RunningScripts;

            m_globalVariables.CollectionChanged += GlobalVariables_CollectionChanged;
            m_namedGlobalVariables.CollectionChanged += NamedGlobalVariables_CollectionChanged;
            m_namedGlobalVariables.ItemPropertyChanged += NamedGlobalVariables_ItemPropertyChanged;
        }

        private void MainViewModel_DataClosing(object sender, SaveDataEventArgs e)
        {
            m_globalVariables.CollectionChanged -= GlobalVariables_CollectionChanged;
            m_namedGlobalVariables.CollectionChanged -= NamedGlobalVariables_CollectionChanged;
            m_namedGlobalVariables.ItemPropertyChanged -= NamedGlobalVariables_ItemPropertyChanged;

            m_namedGlobalVariables.Clear();
            m_globalVariables = null;

            ActiveThreads = null;
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Detect external changes made to the global variables list (e.g. from other Pages)
            // and reflect those changes in the NamedScriptVariable list that the user sees.

            // This event occurs when a global variable is modified from outside of the Scripts page.
            // This happens often, as a lot of features involve modifying this list.

            if (m_suppressGlobalVariablesCollectionChanged) {
                return;
            }

            m_suppressNamedGlobalVariablesCollectionChanged = true;
            m_suppressNamedGlobalVariablesItemPropertyChanged = true;
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
            m_suppressNamedGlobalVariablesCollectionChanged = false;
            m_suppressNamedGlobalVariablesItemPropertyChanged = false;
        }

        private void NamedGlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Detect changes made to the named global variables list itself (e.g. adding/removing item)
            // and push those changes to the ScriptVariable list that's stored in the save file.

            // This event occurs when the user adds or deletes a global variable from
            // within the Scripts page. There should be no way to add/remove items from
            // that list elsewhere in the program.

            if (m_suppressNamedGlobalVariablesCollectionChanged) {
                return;
            }

            m_suppressGlobalVariablesCollectionChanged = true;
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
            m_suppressGlobalVariablesCollectionChanged = false;
        }

        private void NamedGlobalVariables_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            // Detect changes made to the 'Value' field of a NamedScriptVariable and
            // push those changes to the corresponding element in save data's global variables list.
            // The 'Name' field is not stored in the save file, so we ignore those changes.

            // This event occurrs when the user modifies the value of one of the global variables
            // from within the Scripts page.

            if (m_suppressNamedGlobalVariablesItemPropertyChanged) {
                return;
            }

            FullyObservableCollection<NamedScriptVariable> vars = sender as FullyObservableCollection<NamedScriptVariable>;
            NamedScriptVariable var = vars[e.CollectionIndex];

            if (e.PropertyName != nameof(var.Value)) {
                return;
            }

            m_suppressGlobalVariablesCollectionChanged = true;
            m_globalVariables[e.CollectionIndex] = var.Value;
            m_suppressGlobalVariablesCollectionChanged = false;
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
