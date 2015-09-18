using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace txtedo.ViewModel
{
    public class TxtedoBarViewModel : INotifyPropertyChanged
    {
        private TxtedoBar bar = new TxtedoBar();

        private InputListener ilr_SendCommand;
        private InputListener ilr_InputChanged;

        public TxtedoBarViewModel ()
        {
            //Pass txtedo bar commands to listener
            ilr_SendCommand = new InputListener(bar.SendCommand, bar.IsValid);
            ilr_InputChanged = new InputListener(bar.ChangeInput, bar.IsValid);
        }

        //Give XML access to ICommand
        public ICommand submitInput
        {
            get { return ilr_SendCommand; }
        }

        //User input prompt
        public string LblPrompt
        {
            get { return bar.visibleInput; }
            set
            {
                bar.visibleInput = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LblPrompt"));
                }
            }
        }

        public string TxtCommandInput
        {
            get { return bar.currentCommand; }
            set
            {
                Console.WriteLine("Changed");
                bar.currentCommand = value;
                bar.ChangeInput();

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TxtCommandInput"));
                    PropertyChanged(this, new PropertyChangedEventArgs("LblPrompt"));
                }
            }
        }

        public void SubmitCommand()
        {
            Console.WriteLine("Heard");
            bar.SendCommand();
        }

        //Notify UI of changed property
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
