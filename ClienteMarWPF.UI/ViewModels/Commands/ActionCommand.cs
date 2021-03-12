using System;
using System.Windows.Input;
using ClienteMarWPF.UI.ViewModels.Helpers;

namespace ClienteMarWPF.UI.ViewModels.Commands
{
    public class ActionCommand : ICommand
    {
        public bool No { get => Booleano.No; }
        public bool Si { get => Booleano.Si; }


        private Action<object> _action;
        private Predicate<object> _canExecute;


        public ActionCommand() { }

        public ActionCommand(Action<object> action) : this(action, null) { }

        public ActionCommand(Action<object> action, Predicate<object> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        protected virtual void SetAction(Action<object> action)
        {
            SetAction(action, null);
        }

        protected virtual void SetAction(Action<object> action, Predicate<object> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _action(parameter);
            }
        }


        private event EventHandler CanExecuteChangedInternal;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChangedInternal.Raise(this);
        }




    }// fin de clase 




    public class ActionCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public ActionCommand(Action<T> execute)
           : this(execute, null)
        {
            _execute = execute;
        }

        public ActionCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }// fin de clase, implementacion generica 








}// fin de namespace
