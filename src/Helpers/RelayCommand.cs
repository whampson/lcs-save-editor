using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

/*
 * Derived from Josh Smith's RelayCommand class.
 * https://msdn.microsoft.com/en-us/magazine/dd419663.aspx
 */

namespace WHampson.LcsSaveEditor.Helpers
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
