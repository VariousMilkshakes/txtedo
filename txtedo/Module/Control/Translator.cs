using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;

namespace txtedo.Module.Control
{
    class Translator
    {
        //List of all commands
        private List<Command> masterList;
        private List<PreviewItem> previewList;

        public Translator(Dictionary dictionary)
        {
            masterList = dictionary.commands;
        }

        //Returns all parent commands in CommandPreview Format
        public List<PreviewItem> GetAll()
        {
            List<PreviewItem> preview = new List<PreviewItem>();

            foreach(Command command in masterList)
            {
                //Convert command to CommandPreview
                PreviewItem cp = new PreviewItem(command);
                preview.Add(cp);
            }

            this.previewList = preview;
            return preview;
        }

        //Return best match CommandPreview from displayed commands for user input
        private PreviewItem BestMatch(List<PreviewItem> commands, string input)
        {
            //Similar to QueryTop process
            foreach (PreviewItem command in commands)
            {
                int index = 0;
                int roof = input.Length;

                foreach (char c in command.name)
                {
                    if (index < roof)
                    {
                        if (c != input[index])
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                    index++;
                }

                return command;
            }

            //No matching commands
            return null;
        }

        //Turn user input into command
        public Command GetFrom(List<PreviewItem> currentOptions, string choice, List<Command> from = null)
        {
            this.previewList = currentOptions;
            PreviewItem visualRef = BestMatch(currentOptions, choice);

            if (visualRef != null)
            {
                List<Command> thisList;

                if (from == null)
                {
                    thisList = masterList;
                }
                else
                {
                    thisList = from;
                }

                //Convert CommandPreview back into full Command
                foreach (Command command in thisList)
                {
                    if (command.command == visualRef.name)
                    {
                        return command;
                    }
                }
            }

            //No command found
            return null;
        }

        //Run dynamic module from command
        public bool Run(dynamic module, bool isAsync, string options = "")
        {
            //Check if the python was successfull
            bool success = false;
            object temp;

            if (options == "")
            {
                //Run python without parameters
                temp = module.Run();
            }
            else
            {
                //Run python with user parameters
                temp = module.Run(options);
            }

            if (isAsync)
            {
                success = false;
            }
            else
            {
                success = (bool)temp;
            }

            return success;
        }

        public List<PreviewItem> QueryAllIn(string command, List<Command> collection = null)
        {
            if (collection == null)
            {
                collection = masterList;
            }

            //Narrowed list of commands from query
            List<PreviewItem> smallList = new List<PreviewItem>();

            string query = command.Split(' ')[0];

            //Loop through each command
            foreach (Command com in collection)
            {
                bool match = true;

                //Index of user input
                int index = 0;
                //Max value of 'index'
                int roof = query.Length;

                //Each character in current command
                foreach (char c in com.command)
                {
                    //User input ends before command
                    if (index < roof)
                    {
                        if (c != query[index])
                        {
                            match = false;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                    index++;
                }

                if (match)
                {
                    smallList.Add(new PreviewItem(com));
                }

            }

            this.previewList = smallList;
            return smallList;
        }
    }
}
