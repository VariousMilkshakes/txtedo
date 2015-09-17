using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace txtedo.ViewModel
{
    //User input passes through here then is redirected to the View Model Method
    public class InputListener : ICommand
    {
        private TxtedoBarViewModel vm;

        private Action WhatToExecute;
        private Func<bool> WhenToExecute;

        public InputListener (Action What, Func<bool> When)
        {
            WhatToExecute = What;
            WhenToExecute = When;
        }

        public bool CanExecute(object parameter)
        {
            return WhenToExecute();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            WhatToExecute();
        }
    }
}
