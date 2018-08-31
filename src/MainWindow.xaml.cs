﻿#region License
/* Copyright(c) 2016-2018 Wes Hampson
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

using System;
using System.Windows;
using WHampson.LcsSaveEditor.Events;
using WHampson.LcsSaveEditor.ViewModels;

namespace WHampson.LcsSaveEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel vm)
            : this()
        {
            DataContext = vm;
            vm.MessageBoxRequested += View_MessageBoxRequested;
        }

        private void View_DataChanged(object sender, RoutedEventArgs e)
        {
            ((MainViewModel) DataContext).IsFileModified = true;
        }

        private void View_MessageBoxRequested(object sender, MessageBoxEventArgs e)
        {
            e.Show(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel vm = (MainViewModel) DataContext;
            if (vm.IsEditingFile) {
                vm.CloseFile.Execute(null);
            }

            // Only close if user didn't cancel
            if (vm.IsEditingFile) {
                e.Cancel = true;
            }
        }
    }
}
