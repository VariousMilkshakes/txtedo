using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

using txtedo.Module;
using txtedo.Module.Control;
using txtedo.Module.Control.API;
using txtedo.Background;

namespace txtedo.ViewModel
{
    /// <summary>
    /// Class handles events and controls for the Txtedo View Model
    /// </summary>
    public class TxtedoBar
    {

        #region public fields
        /// <summary>
        /// Current prompt visible in bar
        /// </summary>
        public string visiblePrompt;

        /// <summary>
        /// Current command input by user
        /// </summary>
        public string currentCommand;


        /// <summary>
        /// Is current user input in quotes, this will disable submit command
        /// </summary>
        public bool inQuotes = false;


        /// <summary>
        /// Width of Txtedo Bar
        /// </summary>
        public int width = 400;
        #endregion

        #region private variables
        /// <summary>
        /// Prompt which is hidden by user input
        /// </summary>
        private string hiddenPrompt;

        /// <summary>
        /// Check whether Txtedo Bar is hidden or not
        /// </summary>
        private string visibleState;

        /// <summary>
        /// Height of Txtedo Window as whole
        /// </summary>
        private int windowHeight;
        #endregion

        #region private objects

        private Dictionary dict;
        private Translator tran;
        private ObservableCollection<PreviewItem> preview;

        private Command runningCommand;
        //Commands used by user
        private List<Command> commandStack;

        private Ghost bgManager;
        #endregion

        /// <summary>
        /// Prompt at startup
        /// </summary>
        private const string PROMPT_DEFAULT = "Your Command";

        #region constructor
        /// <summary>
        /// Backend for UI operations
        /// </summary>
        /// <param name="apis">Stack of API for scripting languages</param>
        public TxtedoBar()
        {

            //Prompt on start up
            visiblePrompt = PROMPT_DEFAULT;
            
            //Start with no command
            currentCommand = "";

            //Collect modules into dictionary
            
            this.dict = new Dictionary();

            this.tran = new Translator(dict);

            //Display full list of commands
            preview = new ObservableCollection<PreviewItem>(this.tran.GetAll());
            
            //Empty command history
            commandStack = new List<Command>();

        }
        #endregion

        #region public get/set
        /// <summary>
        /// Txtedo Window Height
        /// </summary>
        public int height
        {
            get { return this.windowHeight; }
            set { this.windowHeight = value; }
        }

        /// <summary>
        /// Is Txtedo Visible or Hidden in Tray?
        /// </summary>
        public string barVisibility
        {
            get { return this.visibleState; }
            set { this.visibleState = value; }
        }

        /// <summary>
        /// Get / Set the Background and Hidden Worker
        /// </summary>
        public Ghost ghost
        {
            get { return this.bgManager; }
            set { this.bgManager = value; }
        }
        #endregion

        /// <summary>
        /// Reset to start up state
        /// </summary>
        public void ResetBar()
        {
            //Clean up txtedo bar
            visiblePrompt = "Your Command";
            currentCommand = "";

            //Display full list of commands
            preview = new ObservableCollection<PreviewItem>(this.tran.GetAll());

            //Get rid of current command
            this.runningCommand = null;

            //Empty previous command history
            commandStack = new List<Command>();
        }

        /// <summary>
        /// Checks then sends current command to translator
        /// </summary>
        public void SendCommand(string options)
        {

            //Is there any commands to choose from? OR Is the user sending options
            if (preview.Count != 0 || runningCommand != null)
            {

                //Check if module has finished successfully
                bool finished = true;

                //Send module to translator to run
                this.tran.Run(this.runningCommand, options);

                //Hide Txtedo Bar then rebind key binding
                bgManager.Phase();
                bgManager.ReBind();

                //Did the module complete fully? AND Module is inline
                if (finished && !this.runningCommand.isAsync)
                {
                    Console.WriteLine("Command finished");

                    ResetBar();
                }
                else
                {
                    Console.WriteLine("async");
                }
            } 
        }

        /// <summary>
        /// Big ol' conditional chain. Validates and increments user input.
        /// </summary>
        public void ChangeInput()
        {
            //Handle quote inputs
            QuoteFormatter();

            //Is the input empty
            if (currentCommand == "")
            {

                //Display hidden prompt
                visiblePrompt = this.hiddenPrompt;

                //Has a parent command been selected yet?
                if (this.runningCommand == null)
                {

                    preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn(""));

                }
                else
                {

                    //Display parent command's children
                    preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn("", this.runningCommand.childCommands));

                }

            }
            else
            {

                //Track prompt
                this.hiddenPrompt = visiblePrompt;
                //Hide prompt
                visiblePrompt = "";

                //Has a space been used?
                if (currentCommand[currentCommand.Length - 1] == ' ')
                {
                    //Is input in quotes OR (is a command selected AND there is not children) = has to be option
                    if (this.inQuotes || this.runningCommand != null && this.runningCommand.childCommands.Count == 0)
                    {

                        //Continue input
                        return;

                    }
                    else
                    {

                        //Remove space from the end of command
                        string thisCommand = currentCommand.Remove(currentCommand.Length - 1, 1);
                        
                        //Breifly hold selected command
                        Command tempCommand;

                        //Clear input
                        currentCommand = "";

                        //If no commands have been inputted yet
                        if (this.runningCommand == null)
                        {
                            //Search from masterlist
                            //Convert preview back into normal List
                            tempCommand = this.tran.GetFrom(new List<PreviewItem>(preview), thisCommand);

                            try
                            {

                                //Show command as prompt
                                visiblePrompt = tempCommand.commandTip;

                            }
                            catch
                            {

                                //If user trys to use no existent command
                                //Reset list
                                preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn(""));
                                return;

                            }
                        }
                        //Show list of child commands
                        else
                        {
                            tempCommand = this.tran.GetFrom(new List<PreviewItem>(preview), thisCommand, this.runningCommand.childCommands);
                        }

                        //If no matches can be found stop is user inputting option?
                        if (tempCommand == null && !this.runningCommand.hasQuery)
                        {
                            //None existent child command is used
                            //Reset preview
                            preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn("", this.runningCommand.childCommands));
                            return;
                        }
                        //If the found command has children
                        else if (tempCommand != null)
                        {
                            //Show children
                            preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn("", tempCommand.childCommands));
                            //Store current command
                            this.runningCommand = tempCommand;
                            commandStack.Add(this.runningCommand);
                        }
                        //Next input must be option
                        else
                        {
                            //this.runningCommand = tempCommand;

                            SendCommand(thisCommand);

                            //if (this.runningCommand.hasQuery)
                            //{
                            //    //Clear prompts
                            //    preview = null;

                            //    visiblePrompt = this.runningCommand.command;

                                
                            //    //commandStack.Add(this.runningCommand);
                            //}
                            //else
                            //{
                            //    //Run module with no query
                            //    SendCommand();
                            //}
                        }
                    }
                }
                else
                {
                    if (this.runningCommand == null)
                    {
                        preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn(currentCommand));
                    }
                    else
                    {
                        preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn(currentCommand, this.runningCommand.childCommands));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                    preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn(""));
                    visiblePrompt = "Your Command";
                    runningCommand = null;
                }
                else if (commandStack.Count > 1)
                {
                    preview = new ObservableCollection<PreviewItem>(this.tran.QueryAllIn("", commandStack[commandStack.Count - 2].childCommands));

                    Command oldCommand = commandStack[commandStack.Count - 2];
                    visiblePrompt = oldCommand.command;
                    this.runningCommand = oldCommand;
                }

                commandStack.RemoveAt(commandStack.Count - 1);
            }
        }

        public ObservableCollection<PreviewItem> CommandPreview
        {
            get { return this.preview; }
            set { this.preview = value; }
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
