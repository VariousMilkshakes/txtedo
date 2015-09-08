using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace txtedo
{
    class Command
    {
        //Command to type
        private string command;
        //Tip displayed for user
        private string commandTip;
        public List<Command> childCommands = new List<Command>();
        //Controller to invoke rules from
        public Object commandRules;

        public Command (Object rules, string name, string tip = "")
        {
            this.command = name;

            //If no tip is set default to something (command name just placeholder)
            if (tip == "")
            {
                tip = name;
            }

            this.commandTip = tip;

            this.commandRules = rules;
        }

        //Add child command to command
        public void NewChild (Command child)
        {
            childCommands.Add(child);
        }
    }

    public class InitiateModuleCommand
    {
        //Use to run rules associated with command
        public void Start(Object command)
        {
            //Functions in Module interface
            var commandModule = typeof(IModule);
            //Default function to start module rules with
            MethodInfo tasker = commandModule.GetMethod("tasker");

            //Options for task; not sure what to do with this yet
            object[] commandOptions = new object[] { "WOOP" };

            //Invoke tasker function
            tasker.Invoke(command, commandOptions);
        }
    }
}
