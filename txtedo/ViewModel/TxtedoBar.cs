using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using txtedo.Module;
using txtedo.Module.Control;

namespace txtedo.ViewModel
{
    public class TxtedoBar
    {
        public string visibleInput;
        public string command;
        public string options;
        public string currentCommand;

        private string hiddenPrompt;

        private Dictionary dict;
        private Translator tran;
        private ObservableCollection<CommandPreview> preview;

        //BACK END CONTROL FOR UI OPERATIONS
        public TxtedoBar()
        {
            //Prompt on start up
            visibleInput = "Your Command";
            currentCommand = "";

            //Collect commands
            this.dict = new Dictionary();
            this.tran = new Translator(dict);
            preview = new ObservableCollection<CommandPreview>(this.tran.GetAll());
        }

        public void SendCommand ()
        {
            List<string> splitInput = currentCommand.Split(' ').ToList();
            command = splitInput[0];

            //Relevant command
            Command c = this.tran.GetFrom(new List<CommandPreview>(this.preview), command);

            bool complete = false;

            //If user has input more options
            if (splitInput.Count > 1)
            {
                splitInput.RemoveAt(0);
                options = string.Join(" ", splitInput.ToArray());

                //TODO: route commands to children or send with options
            }
            else
            {
                //Run python without parameters
                complete = this.tran.Run(c.module);
            }

            //If script ran successfully clear command and start again
            //Maybe hide bar on completion
            if (complete)
            {
                currentCommand = "";
                ChangeInput();
            }
        }

        //Runs every time user input changes
        public void ChangeInput ()
        {
            if (currentCommand != "")
            {
                //Keep track of prompt while it is not displayed
                this.hiddenPrompt = visibleInput;
                visibleInput = "";

                //Data tables user ObservableCollections instead of lists
                //Convert between the two
                preview = new ObservableCollection<CommandPreview>(tran.QueryTop(currentCommand));
            }
            else
            {
                //Display prompt again
                visibleInput = this.hiddenPrompt;

                //Display all parent commands
                preview = new ObservableCollection<CommandPreview>(this.tran.GetAll());
            }
        }

        public ObservableCollection<CommandPreview> CommandPreview
        {
            get { return this.preview; }
        }

        //UI Validation
        public bool IsValid ()
        {
            //Prompt should disappear once user starts typing
            if (currentCommand != "" && this.visibleInput != "")
            {
                Console.WriteLine("Flase");
                return false;
            }

            Console.WriteLine("true");
            return true;
        }
    }
}
