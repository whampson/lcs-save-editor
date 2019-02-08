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
using System.Windows;

namespace LcsSaveEditor.ViewModels
{
    public partial class MainViewModel
    {
        private void FileClosePrompt_ResultAction(MessageBoxResult result)
        {
            // Does the user want to save before closing?
            switch (result) {
                case MessageBoxResult.Yes:
                    SaveFile(MostRecentFilePath);
                    DoCloseFile();
                    break;
                case MessageBoxResult.No:
                    DoCloseFile();
                    break;
            }
        }

        public void FileDialog_ResultAction(bool? dialogResult, FileDialogEventArgs e)
        {
            // Ignore if anything other than "OK" pressed
            if (dialogResult != true) {
                return;
            }

            // Open or Save based on dialog type
            switch (e.DialogType) {
                case FileDialogType.OpenDialog:
                    OpenFile(e.FileName);
                    break;
                case FileDialogType.SaveDialog:
                    SaveFile(e.FileName);
                    break;
            }
        }
    }
}
