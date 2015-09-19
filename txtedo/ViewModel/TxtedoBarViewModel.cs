using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

using txtedo.Module.Control;

namespace txtedo.ViewModel
{
    public class TxtedoBarViewModel : INotifyPropertyChanged
    {
        private TxtedoBar bar = new TxtedoBar();

        private InputListener ilr_SendCommand;

        public TxtedoBarViewModel ()
        {
            //Pass txtedo bar commands to listener
            ilr_SendCommand = new InputListener(this.SubmitCommand, bar.IsValid);
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
                bar.currentCommand = value;
                bar.ChangeInput();

                this.RefreshAll();
            }
        }

        //Binds List of current availible commands to list
        public ObservableCollection<CommandPreview> Preview
        {
            get { return bar.CommandPreview; }
        }

        private void SubmitCommand()
        {
            Console.WriteLine("Heard");
            bar.SendCommand();

            bar.currentCommand = "";
            bar.ChangeInput();

            this.RefreshAll();
        }

        private void RefreshAll()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("TxtCommandInput"));
                PropertyChanged(this, new PropertyChangedEventArgs("LblPrompt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Preview"));
            }
        }

        //Notify UI of changed property
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
