using System.Windows.Input;

namespace SRTEditor_MVVM.Infrastructure
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }


        /// <summary>
        /// All class's Fields and Properties define here
        /// </summary>
        #region Fields and Properties
        readonly Action _TargetExecuteMethod;
        readonly Func<bool>? _TargetCanExecuteMethod;
        #endregion


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #region ICommand Members

        bool ICommand.CanExecute(object? parameter)
        {
            if (_TargetCanExecuteMethod != null)
                return _TargetCanExecuteMethod();

            return _TargetExecuteMethod != null;
        }

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler? CanExecuteChanged = delegate { };

        void ICommand.Execute(object? parameter)
        {
            _TargetExecuteMethod?.Invoke();
        }
        #endregion
    }
}
