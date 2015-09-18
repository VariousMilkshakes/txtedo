using System;
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

        public TxtedoBar()
        {
            //Prompt on start up
            visibleInput = "Your Command";
            currentCommand = "";

            //Collect commands
            this.dict = new Dictionary();
            this.tran = new Translator(dict);
        }

        public void SendCommand ()
        {
            List<string> splitInput = currentCommand.Split(' ').ToList();
            command = splitInput[0];

            if (splitInput.Count > 1)
            {
                splitInput.RemoveAt(0);
                options = string.Join(" ", splitInput.ToArray());
            }

            this.tran.Run(this.tran.Get(command), options);
        }

        public void ChangeInput ()
        {
            if (currentCommand != "")
            {
                this.hiddenPrompt = visibleInput;
                visibleInput = "";
            }
            else
            {
                visibleInput = this.hiddenPrompt;
            }
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
