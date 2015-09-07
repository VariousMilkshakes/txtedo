using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtedo
{
    class Command
    {
        //Command to type
        private string command;
        //Tip displayed for user
        private string commandTip;
        public List<Command> childCommands = new List<Command>();

        public Command (string name, string tip = "")
        {
            this.command = name;

            //If no tip is set default to something (command name just placeholder)
            if (tip == "")
            {
                tip = name;
            }

            this.commandTip = tip;
        }

        //Add child command to command
        public void NewChild (Command child)
        {
            childCommands.Add(child);
        }
    }
}
