﻿#region License
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

using LcsSaveEditor.Core.Controls;
using System;
using System.Windows;

namespace LcsSaveEditor.ViewModels
{
    /// <summary>
    /// Parameters for opening a <see cref="MessageBoxEx"/> from an event.
    /// </summary>
    public class MessageBoxEventArgs : EventArgs
    {
        public MessageBoxEventArgs(string text,
            string title = "",
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.None,
            MessageBoxOptions options = MessageBoxOptions.None,
            Action<MessageBoxResult> resultAction = null)
        {
            Text = text;
            Title = title;
            Buttons = buttons;
            Icon = icon;
            DefaultResult = defaultResult;
            Options = options;
            ResultAction = resultAction;
        }

        public string Text
        {
            get;
        }

        public string Title
        {
            get;
        }

        public MessageBoxButton Buttons
        {
            get;
        }

        public MessageBoxImage Icon
        {
            get;
        }

        public MessageBoxResult DefaultResult
        {
            get;
        }

        public MessageBoxOptions Options
        {
            get;
        }

        public Action<MessageBoxResult> ResultAction
        {
            get;
        }

        public void Show()
        {
            Show(null);
        }

        public void Show(Window w)
        {
            MessageBoxResult result;
            if (w == null) {
                result = MessageBoxEx.Show(Text, Title, Buttons, Icon, DefaultResult, Options);
            }
            else {
                result = MessageBoxEx.Show(w, Text, Title, Buttons, Icon, DefaultResult, Options);
            }

            ResultAction?.Invoke(result);
        }
    }
}
