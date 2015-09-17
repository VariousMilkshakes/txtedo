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

        private InputListener ilr;

        public TxtedoBarViewModel ()
        {
            //Pass txtedo bar commands to listener
            ilr = new InputListener(bar.SendCommand, bar.IsValid);
        }

        //Give XML access to ICommand
        public ICommand submitInput
        {
            get { return ilr; }
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
            get { return bar.command; }
            set
            {
                bar.ChangeInput(value);

                if (PropertyChanged != null)
                {
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
