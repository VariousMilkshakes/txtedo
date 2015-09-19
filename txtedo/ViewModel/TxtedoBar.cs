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

                
            }
            else
            {
                complete = this.tran.Run(c.module);
            }

            if (complete)
            {
                currentCommand = "";
                ChangeInput();
            }
        }

        public void ChangeInput ()
        {
            if (currentCommand != "")
            {
                this.hiddenPrompt = visibleInput;
                visibleInput = "";

                preview = new ObservableCollection<CommandPreview>(tran.QueryTop(currentCommand));
            }
            else
            {
                visibleInput = this.hiddenPrompt;

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
