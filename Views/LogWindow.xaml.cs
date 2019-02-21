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

using LcsSaveEditor.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace LcsSaveEditor.Views
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();

            ViewModel.DialogCloseRequested += ViewModel_DialogCloseRequested;
            ViewModel.FileDialogRequested += ViewModel_FileDialogRequested;
        }

        public LogViewModel ViewModel
        {
            get { return (LogViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void ViewModel_DialogCloseRequested(object sender, DialogCloseEventArgs e)
        {
            Close();
        }

        private void ViewModel_FileDialogRequested(object sender, FileDialogEventArgs e)
        {
            e.ShowDialog(this);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Hide instead of close so we can open it again later
            e.Cancel = true;
            Hide();
        }
    }
}
