using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace txtedo.Module
{
    public class Command
    {
        //Command to type
        public string command;
        //Tip displayed for user
        public string commandTip;
        public List<Command> childCommands = new List<Command>();
        //Controller to invoke rules from
        public dynamic module;
        //Command triggers
        public List<string> triggers;

        public Command (dynamic script, string name, string tip = "")
        {
            this.command = name;

            //If no tip is set default to something (command name just placeholder)
            if (tip == "")
            {
                tip = name;
            }

            this.commandTip = tip;

            this.module = script;
        }

        //Add child command to command
        public void NewChild (Command child)
        {
            childCommands.Add(child);
        }
    }

    public class CommandMessenger
    {
        public string command = "";
        public string desc = "";
        public string parent = "";
        public List<string> triggers = new List<string>();
    }
}
