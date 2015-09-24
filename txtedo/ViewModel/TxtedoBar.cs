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
        public bool inQuotes = false;

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

        public void SendCommand()
        {
            bool complete = false;

            complete = this.tran.Run(runningCommand.module, currentCommand);

            if (complete)
            {
                Console.WriteLine("Command finished");

                preview = new ObservableCollection<CommandPreview>(this.tran.GetAll());
                runningCommand = null;
            }
            else
            {
                Console.WriteLine("Command failed");
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
                    //Check if input isn't in quotes or is confirmed option
                    if (this.inQuotes || runningCommand != null && runningCommand.childCommands.Count == 0)
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
                        if (runningCommand == null)
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
                            tempCommand = this.tran.GetFrom(new List<CommandPreview>(preview), thisCommand, runningCommand.childCommands);
                        }

                        //If no matches can be found stop
                        if (tempCommand == null)
                        {
                            //None existent child command is used
                            //Reset preview
                            preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn("", runningCommand.childCommands));
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
            if (runningCommand == null || currentCommand.Length != 0)
            {
                //No parent
                ChangeInput();
            }
            else
            {
                preview = new ObservableCollection<CommandPreview>(this.tran.QueryAllIn(runningCommand.parent));
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
