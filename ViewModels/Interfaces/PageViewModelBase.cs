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

namespace LcsSaveEditor.ViewModels
{
    /// <summary>
    /// Base class for Page view models.
    /// </summary>
    public abstract class PageViewModelBase : ObservableObject
    {
        private MainViewModel m_mainViewModel;
        private string m_header;

        /// <summary>
        /// Creates a new Page view model with the specified page title.
        /// </summary>
        /// <param name="header">The title of the Page.</param>
        public PageViewModelBase(MainViewModel mainViewModel, string header)
        {
            m_mainViewModel = mainViewModel;
            m_header = header;
        }

        public MainViewModel MainViewModel
        {
            get { return m_mainViewModel; }
            set { m_mainViewModel = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the title of this page.
        /// </summary>
        public string Header
        {
            get { return m_header; }
            set { m_header = value; OnPropertyChanged(); }
        }
    }
}
