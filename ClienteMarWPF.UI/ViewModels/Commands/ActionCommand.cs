using System;
using System.Windows.Input; 

namespace ClienteMarWPF.UI.ViewModels.Commands
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Predicate<object> _canExecute;

        public ActionCommand(Action<object> action): this(action, null) { }

        public ActionCommand(Action<object> action, Predicate<object> canExecute) 
        {
            _action = action;
            _canExecute = canExecute;        
        }

        public event  EventHandler CanExecuteChanged 
        {
          add { CommandManager.RequerySuggested += value; }
          remove { CommandManager.RequerySuggested -= value; }
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


    }// fin de clase 

}// fin de namespace
