using System;
using System.Windows.Input;

namespace txtedo.ViewModel
{
    //User input passes through here then is redirected to the View Model Method
    public class InputListener : ICommand
    {
        //Function to execute
        private Action whatToExecute;
        //Check whether to execute function
        private Func<bool> whenToExecute;

        public event EventHandler CanExecuteChanged;

        public InputListener (Action What, Func<bool> When)
        {
            this.whatToExecute = What;
            this.whenToExecute = When;
        }

        public bool CanExecute(object parameter)
        {
            return this.whenToExecute();
        }

        public void Execute(object parameter)
        {
            this.whatToExecute();
        }
    }
}
