using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using txtedo.Module;
using txtedo.Module.Control;
using txtedo.Background;

namespace txtedo.ViewModel
{
    public class TxtedoBar
    {
        public string visiblePrompt;
        public string command;
        public string options;
        public string currentCommand;

        public bool inQuotes = false;

        public int width = 400;

        private string hiddenPrompt;
        private string visibleState;
        private int windowHeight;

        private Dictionary dict;
        private Translator tran;
        private ObservableCollection<CommandPreview> preview;

        private Command runningCommand;
        //Commands used by user
        private List<Command> commandStack;

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
            
            commandStack = new List<Command>();
        }

        public int height
        {
            get { return this.windowHeight; }
            set { this.windowHeight = value; }
        }

        public string barVisibility
        {
            get
            {
                return this.visibleState;
            }

            set
            {
                this.visibleState = value;
            }
        }

        public void SendCommand()
        {
            bool complete = false;

            if (preview.Count != 0 || runningCommand != null)
            {
                complete = this.tran.Run(this.runningCommand.module, currentCommand);

                if (complete)
                {
                    Console.WriteLine("Command finished");

                    preview = new ObservableCollection<CommandPreview>(this.tran.GetAll());
                    //Reset after command is run
                    this.runningCommand = null;
                    commandStack = new List<Command>();
                }
                else
                {
                    Console.WriteLine("Command failed");
                }
            } 
        }

        public void ChangeInput()
        {
            QuoteFormatter();

            if (currentCommand == "")
            {
                //Display hidden prompt
                visiblePrompt = this.hiddenPrompt;

                //Display full list
                if (this.runningCommand == null)
                {
                    preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(""));
                }
                else
                {
                    preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", this.runningCommand.childCommands));
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
                    //Check if input isn't in quotes or is confirmed option
                    if (this.inQuotes || this.runningCommand != null && this.runningCommand.childCommands.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        //Remove space from command
                        string thisCommand = currentCommand.Remove(currentCommand.Length - 1, 1);

                        Command tempCommand;
                        currentCommand = "";

                        //If no commands have been inputted yet
                        if (this.runningCommand == null)
                        {
                            //Search from masterlist
                            //Convert preview back into normal List
                            tempCommand = this.tran.GetFrom(new List<CommandPreview>(preview), thisCommand);

                            try
                            {
                                //Show command as prompt
                                visiblePrompt = tempCommand.command;
                            }
                            catch
                            {
                                //If user trys to use no existent command
                                //Reset list
                                preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(""));
                                return;
                            }
                        }
                        //Show list of child commands
                        else
                        {
                            tempCommand = this.tran.GetFrom(new List<CommandPreview>(preview), thisCommand, this.runningCommand.childCommands);
                        }

                        //If no matches can be found stop
                        if (tempCommand == null)
                        {
                            //None existent child command is used
                            //Reset preview
                            preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", this.runningCommand.childCommands));
                            return;
                        }
                        //If the found command has children
                        else if (tempCommand.childCommands.Count > 0)
                        {
                            //Show children
                            preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", tempCommand.childCommands));
                            //Store current command
                            this.runningCommand = tempCommand;
                            commandStack.Add(this.runningCommand);
                        }
                        //Next input must be option
                        else
                        {
                            //Clear prompts
                            preview = null;

                            visiblePrompt = tempCommand.command;

                            this.runningCommand = tempCommand;
                            commandStack.Add(this.runningCommand);
                        }
                    }
                }
                else
                {
                    if (this.runningCommand == null)
                    {
                        preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(currentCommand));
                    }
                    else
                    {
                        preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(currentCommand, this.runningCommand.childCommands));
                    }
                }
            }
        }

        private void QuoteFormatter()
        {
            //Toggle quotes on and off
            if (currentCommand != "" && currentCommand[currentCommand.Length - 1] == '"')
            {
                //Allow use of escaped quotes
                if (currentCommand.Length < 2 || currentCommand[currentCommand.Length - 2] != '\\')
                {
                    this.inQuotes = !this.inQuotes;

                    //Remove open and close quote from input
                    if (this.inQuotes)
                    {
                        currentCommand = currentCommand.Remove(currentCommand.Length - 1, 1);
                    }
                    else
                    {
                        currentCommand = currentCommand.Remove(currentCommand.Length - 1, 1);
                    }
                }
                //Hide escaped character
                else
                {
                    currentCommand = currentCommand.Remove(currentCommand.Length - 2, 1);
                }
            }
        }

        //Move back up the command tree by one
        public void BackUpCommand()
        {
            if (this.runningCommand == null || currentCommand.Length != 0)
            {
                //No parent
                ChangeInput();
            }
            else
            {
                if (commandStack.Count == 1)
                {
                    //Return to master list
                    preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(""));
                    visiblePrompt = "Your Command";
                    runningCommand = null;
                }
                else if (commandStack.Count > 1)
                {
                    preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", commandStack[commandStack.Count - 2].childCommands));

                    Command oldCommand = commandStack[commandStack.Count - 2];
                    visiblePrompt = oldCommand.command;
                    this.runningCommand = oldCommand;
                }

                commandStack.RemoveAt(commandStack.Count - 1);
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
