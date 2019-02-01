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

using System;
using System.Windows.Input;

/*
 * Derived from Josh Smith's RelayCommand class.
 * https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
 */

namespace LcsSaveEditor.Infrastructure
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> canExecute;
        private readonly Action<T> execute;

        public RelayCommand(Action<T> what)
            : this(what, null)
        { }

        public RelayCommand(Action<T> what, Predicate<T> when)
        {
            execute = what;
            canExecute = when;
        }

        public event EventHandler CanExecuteChanged
        {
            add {
                if (canExecute != null) {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove {
                if (canExecute != null) {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return (canExecute == null) ? true : canExecute((T) parameter);
        }

        public void Execute(object parameter)
        {
            execute((T) parameter);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public RelayCommand(Action what)
            : this(what, null)
        { }

        public RelayCommand(Action what, Func<bool> when)
        {
            execute = what;
            canExecute = when;
        }

        public event EventHandler CanExecuteChanged
        {
            add {
                if (canExecute != null) {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove {
                if (canExecute != null) {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return (canExecute == null) ? true : canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}
