using System.Windows.Input;

namespace PresentationViewModel
{
    internal class RelayCommand : ICommand
    {
        private Action _action;


        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action action)
        {
            this._action = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;

        }

        public void Execute(object? parameter)
        {
            _action();
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
