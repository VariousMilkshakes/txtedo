using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;

using txtedo.Module.Control;

namespace txtedo.ViewModel
{
    public class TxtedoBarViewModel : INotifyPropertyChanged
    {
        private TxtedoBar bar = new TxtedoBar();

        //Event handlers
        private InputListener ilr_SendCommand;
        private InputListener ilr_BackUpCommand;

        public TxtedoBarViewModel ()
        {
            //Pass txtedo bar commands to listener
            ilr_SendCommand = new InputListener(this.SubmitCommand, bar.IsValid);
            ilr_BackUpCommand = new InputListener(this.BackUp, bar.IsValid);
        }

        //Give XML access to ICommand
        public ICommand submitInput
        {
            get { return ilr_SendCommand; }
        }

        public ICommand backUpInput
        {
            get { return ilr_BackUpCommand; }
        }

        //Quote Alert
        public string LblQuote
        {
            get
            {
                if (bar.inQuotes)
                {
                    return "\"";
                }
                else
                {
                    return "";
                }
            }
        }

        //User input prompt
        public string LblPrompt
        {
            get { return bar.visiblePrompt; }
            set
            {
                bar.visiblePrompt = value;

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

        public int PreviewHeight
        {
            get
            {
                if (bar.CommandPreview == null)
                {
                    return 0;

                }
                return (bar.CommandPreview.Count * 20) + 35; 
            }

            set { return; }
        }

        public int WindowHeight
        {
            get
            {
                bar.height = PreviewHeight + 50;
                return bar.height;
            }
            //For some reason it will only update if the binding is twoway
            set { return; }
        }

        public double LeftLock
        {
            get
            {
                double pageWidth = (double)SystemParameters.PrimaryScreenWidth;
                pageWidth -= bar.width;
                return pageWidth;
            }

            set { return; }
        }

        public double TopLock
        {
            get
            {
                double pageHeight = (double)SystemParameters.PrimaryScreenHeight;

                if (bar.height == 0)
                {
                    bar.height = WindowHeight;
                }

                pageHeight -= bar.height;
                pageHeight -= taskbarHeight;
                return pageHeight;
            }

            set { return; }
        }

        private int taskbarHeight
        {
            get { return (Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height) / 2; }
        }

        //UI events
        private void SubmitCommand()
        {
            Console.WriteLine("Heard");
            bar.SendCommand();

            bar.currentCommand = "";
            bar.ChangeInput();

            this.RefreshAll();
        }

        private void BackUp()
        {
            Console.WriteLine("Back");

            //Default back functionality
            if (bar.currentCommand.Length > 0)
            {
                bar.currentCommand = bar.currentCommand.Remove(bar.currentCommand.Length - 1, 1);
                bar.ChangeInput();
            }
            else
            {
                bar.BackUpCommand();
            }

            this.RefreshAll();
        }

        //Refresh every UI element
        private void RefreshAll()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("TxtCommandInput"));
                PropertyChanged(this, new PropertyChangedEventArgs("LblPrompt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Preview"));
                PropertyChanged(this, new PropertyChangedEventArgs("LblQuote"));
                PropertyChanged(this, new PropertyChangedEventArgs("WindowHeight"));
                PropertyChanged(this, new PropertyChangedEventArgs("PreviewHeight"));
                PropertyChanged(this, new PropertyChangedEventArgs("TopLock"));
            }
        }

        //Notify UI of changed property
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
