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
using System.Collections.ObjectModel;

namespace LcsSaveEditor.ViewModels
{
    public class RecentFilesList : ObservableObject
    {
        private int m_capacity;
        private ObservableCollection<RecentlyAccessedFile> m_items;

        public RecentFilesList(int capacity)
        {
            m_capacity = capacity;
            m_items = new ObservableCollection<RecentlyAccessedFile>();
        }

        public int Capacity
        {
            get { return m_capacity; }
            set { m_capacity = value; OnPropertyChanged(); }
        }

        public ObservableCollection<RecentlyAccessedFile> Items
        {
            get { return m_items; }
            set { m_items = value; OnPropertyChanged(); }
        }

        public void Add(RecentlyAccessedFile f)
        {
            int idx = m_items.IndexOf(f);
            if (idx == 0) {
                // Already at the head of the list, nothing to do
                return;
            }
            else if (idx != -1) {
                // Move to head of the list
                m_items.Move(idx, 0);
                return;
            }

            // Add new item
            m_items.Insert(0, f);

            // Remove the oldest items if we are over capacity
            while (m_items.Count > Capacity) {
                m_items.RemoveAt(Capacity);
            }
        }

    }
}
