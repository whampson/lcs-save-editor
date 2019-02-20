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
using LcsSaveEditor.Infrastructure;
using System.Collections.Specialized;

namespace LcsSaveEditor.ViewModels
{
    public partial class WeaponsViewModel : PageViewModelBase
    {
        private void MainViewModel_DataLoaded(object sender, SaveDataEventArgs e)
        {
            m_globals = e.Data.Scripts.GlobalVariables;
            m_globals.CollectionChanged += GlobalVariables_CollectionChanged;

            switch (e.Data.FileType) {
                case GamePlatform.Android:
                case GamePlatform.IOS:
                    InitAndroidIOSWeaponVars();
                    break;
                case GamePlatform.PS2:
                case GamePlatform.PSP:
                    InitPS2PSPWeaponVars();
                    break;
            }

            ReadHandSlot();
            ReadMeleeSlot();
            ReadProjectileSlot();
            ReadPistolSlot();
            ReadShotgunSlot();
            ReadSmgSlot();
            ReadAssaultSlot();
            ReadHeavySlot();
            ReadSniperSlot();
            ReadSpecialSlot();
        }

        private void MainViewModel_DataClosing(object sender, SaveDataEventArgs e)
        {
            m_globals.CollectionChanged -= GlobalVariables_CollectionChanged;
            m_globals = null;
        }

        private void GlobalVariables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Detect external changes made to global variables list.

            // This event occurs when a global variable is modified from outside
            // the Weapons page.

            if (m_suppressRefresh) {
                return;
            }

            switch (e.Action) {
                //case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    for (int i = 0; i < e.NewItems.Count; i++) {
                        RefreshWeaponSlot(e.NewStartingIndex + i);
                    }
                    break;
                //case NotifyCollectionChangedAction.Remove:
                //    for (int i = 0; i < e.OldItems.Count; i++) {
                //        RefreshWeaponSlot(e.OldStartingIndex + i);
                //    }
                //    break;
            }
        }
    }
}
