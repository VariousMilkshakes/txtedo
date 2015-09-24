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
        public string visiblePrompt;
        public string command;
        public string options;
        public string currentCommand;

        private string hiddenPrompt;

        private Dictionary dict;
        private Translator tran;
        private ObservableCollection<CommandPreview> preview;

        private Command runningCommand;

        //BACK END CONTROL FOR UI OPERATIONS
        public TxtedoBar()
        {
            //Prompt on start up
            visiblePrompt = "Your Command";
            currentCommand = "";

            //Collect commands
            this.dict = new Dictionary();
            this.tran = new Translator(dict);
            preview = new ObservableCollection<CommandPreview>(this.tran.GetAll());
        }

        public void SendCommand ()
        {
            List<string> splitInput = currentCommand.Split(' ').ToList();
            Command c;

            //Relevant command
            if (runningCommand != null)
            {
                c = this.tran.GetFrom(new List<CommandPreview>(this.preview), runningCommand.command, runningCommand.childCommands);
            }
            else
            {
                c = this.tran.GetFrom(new List<CommandPreview>(this.preview), runningCommand.command);
            }
            
            //If c = null reached end of child tree and run command
            if (c == null)
            {
                bool complete = false;

                //If user has input more options
                if (splitInput.Count > 1)
                {
                    //TODO: find level of child to remove all but options
                    splitInput.RemoveAt(0);
                    options = string.Join(" ", splitInput.ToArray());

                    //complete = this.tran.Run(c.module, options);
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
        }

        public void ChangeInput()
        {
            if (currentCommand == "")
            {
                //Display hidden prompt
                visiblePrompt = this.hiddenPrompt;

                //Display full list
                if (runningCommand == null)
                {
                    preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(""));
                }
                else
                {
                    preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", runningCommand.childCommands));
                }
            }
            else
            {
                //Track prompt
                this.hiddenPrompt = visiblePrompt;
                //Hide prompt
                visiblePrompt = "";

                if (currentCommand[currentCommand.Length - 1] == ' ')
                {
                    //Remove space from command
                    string thisCommand = currentCommand.Remove(currentCommand.Length - 1, 1);

                    Command tempCommand;
                    currentCommand = "";

                    //If no commands have been inputted yet
                    if (runningCommand == null)
                    {
                        //Search from masterlist
                        //Convert preview back into normal List
                        tempCommand = this.tran.GetFrom(new List<CommandPreview>(preview), thisCommand);
                        //Show command as prompt
                        visiblePrompt = tempCommand.command;
                    }
                    //Show list of child commands
                    else
                    {
                        tempCommand = this.tran.GetFrom(new List<CommandPreview>(preview), thisCommand, runningCommand.childCommands);
                    }

                    //If no matches can be found stop
                    if (tempCommand == null)
                    {
                        return;
                    }
                    //If the found command has children
                    else if (tempCommand.childCommands.Count > 0)
                    {
                        //Show children
                        preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", tempCommand.childCommands));
                        //Store current command
                        runningCommand = tempCommand;
                    }
                    //Next input must be option
                    else
                    {
                        //Clear prompts
                        preview = null;

                        visiblePrompt = tempCommand.command;

                        runningCommand = tempCommand;
                    }
                }
                else
                {
                    if (runningCommand == null)
                    {
                        preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(currentCommand));
                    }
                    else
                    {
                        preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(currentCommand, runningCommand.childCommands));
                    }
                }
            }
        }

        //Runs every time user input changes
        public void ChangeInput1()
        {
            if (currentCommand != "")
            {
                //Keep track of prompt while it is not displayed
                this.hiddenPrompt = visiblePrompt;
                visiblePrompt = "";

                List<string> input = currentCommand.Split(' ').ToList();

                if (currentCommand[currentCommand.Length - 1] == ' ')
                {
                    string thisCommand = input[input.Count - 1];
                    Command tempCommand;

                    currentCommand = "";

                    if (runningCommand == null)
                    {
                        tempCommand = this.tran.GetFrom(new List<CommandPreview>(preview), thisCommand);
                        visiblePrompt = tempCommand.command;
                    }
                    else
                    {
                        tempCommand = this.tran.GetFrom(new List<CommandPreview>(preview), thisCommand, runningCommand.childCommands);
                        visiblePrompt = runningCommand.command;
                    }

                    if (tempCommand != null && tempCommand.childCommands.Count > 0)
                    {
                        preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", tempCommand.childCommands));
                        runningCommand = tempCommand;
                    }
                    else if (tempCommand != null)
                    {
                        runningCommand = tempCommand;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (input.Count > 1 && runningCommand != null)
                {
                    if (runningCommand.childCommands.Count != 0)
                    {
                        preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(input[1], runningCommand.childCommands));
                    }
                }
                else
                {
                    //Data tables user ObservableCollections instead of lists
                    //Convert between the two
                    preview = new ObservableCollection<CommandPreview>(tran.QueryAllIn(currentCommand));
                }
            }
            else
            {
                //Display prompt again
                visiblePrompt = this.hiddenPrompt;
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
            if (currentCommand != "" && this.visiblePrompt != "")
            {
                Console.WriteLine("Flase");
                return false;
            }

            Console.WriteLine("true");
            return true;
        }
    }
}
