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

namespace LcsSaveEditor.ViewModels
{
    /// <summary>
    /// Base class for Page view models.
    /// </summary>
    public abstract class PageViewModel : ObservableObject
    {
        private bool m_isVisible;

        /// <summary>
        /// Creates a new Page view model with the specified
        /// page title, page visibility, and reference to the main view model.
        /// </summary>
        /// <param name="title">The title of the page.</param>
        /// <param name="visibility">A value indicating when the page should be visible.</param>
        /// <param name="mainViewModel">A reference to the main view model.</param>
        public PageViewModel(string title, PageVisibility visibility, MainViewModel mainViewModel)
        {
            Title = title;
            Visibility = visibility;
            MainViewModel = mainViewModel;
            m_isVisible = visibility == PageVisibility.Always;

            MainViewModel.TabRefresh += MainViewModel_TabRefresh;
        }

        /// <summary>
        /// Gets the title of this page.
        /// </summary>
        public string Title
        {
            get;
        }

        /// <summary>
        /// Gets the page's visibility setting.
        /// </summary>
        public PageVisibility Visibility
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating wheter this page is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the main window's view model for accessing functions
        /// visible to the whole program.
        /// </summary>
        public MainViewModel MainViewModel
        {
            get;
        }

        private void MainViewModel_TabRefresh(object sender, TabRefreshEventArgs e)
        {
            if (Visibility == PageVisibility.Always) {
                IsVisible = true;
                return;
            }

            switch (e.Trigger) {
                case TabRefreshTrigger.WindowLoaded:
                case TabRefreshTrigger.FileClosed:
                    IsVisible = Visibility == PageVisibility.WhenFileClosed;
                    break;
                case TabRefreshTrigger.FileLoaded:
                    IsVisible = Visibility == PageVisibility.WhenFileLoaded;
                    break;
            }
        }
    }
}
