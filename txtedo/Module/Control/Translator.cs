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
        private List<Command> masterList;

        public Translator(Dictionary dictionary)
        {
            masterList = dictionary.commands;
        }

        public dynamic Get(string command)
        {
            return masterList[0].childCommands[0].module;
        }

        public void Run(dynamic module, string options)
        {
            module.Run(options);
        }

        public List<Command> QueryTop(string command)
        {
            //Narrowed list of commands from query
            List<Command> smallList = new List<Command>();

            //Loop through each command
            foreach (Command com in masterList)
            {
                bool match = true;

                //Index of user input
                int index = 0;
                //Max value of 'index'
                int roof = com.command.Length;

                //Each character in current command
                foreach (char c in com.command)
                {
                    //User input ends before command
                    if (index < roof)
                    {
                        if (c != command[index])
                        {
                            match = false;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (match)
                {
                    smallList.Add(com);
                }
            }

            return smallList;
        }
    }
}
