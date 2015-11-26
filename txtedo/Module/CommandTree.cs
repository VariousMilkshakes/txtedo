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
        //Pass initial query
        public bool hasQuery;
        //Language of module
        public string language;
        //Async Module?
        public bool isAsync;

        /// <summary>
        /// A trigger will pause the module and wait for user feedback to continue returning void
        /// </summary>
        private Dictionary<string, Action<string>> Triggers = new Dictionary<string, Action<string>>();
        /// <summary>
        /// An event is run without user input and return void e.g. a callback for the translator
        /// </summary>
        private Dictionary<string, Action> Events = new Dictionary<string, Action>();

        public Command (ModuleBase mb)
        {
            this.command = mb.Name;
            
            if (mb.Desc == "")
            {
                this.commandTip = this.command;
            }
            else
            {
                this.commandTip = mb.Desc;
            }

            this.hasQuery = mb.hasQuery;

            this.Triggers = mb.Triggers;

            this.Events = mb.Events;

            this.language = "c#";
        }

        public Command (dynamic script, string name, string tip, string lang, bool query, bool _async)
        {
            this.command = name;

            //If no tip is set default to something (command name just placeholder)
            if (tip == "")
            {
                tip = name;
            }

            this.commandTip = tip;

            this.language = lang;

            this.hasQuery = query;

            this.isAsync = _async;
        }

        //Add child command to command
        public void NewChild (Command child)
        {
            childCommands.Add(child);
        }

        public Action findEvent(string key)
        {
            if (!this.Events.ContainsKey(key) && !this.Triggers.ContainsKey(key))
            {
                Action target = this.Events[key];

                return target;
            }

            return null;
        }

        public Action<string> findTrigger(string key)
        {
            if (!this.Events.ContainsKey(key) && !this.Triggers.ContainsKey(key))
            {
                Action<string> target = this.Triggers[key];

                return target;
            }

            return null;
        }
    }

    public class CommandMessenger
    {
        public string command = "";
        public string desc = "";
        public string parent = "";
        public List<string> triggers = new List<string>();
        public bool hasInitialQuery = true;
        public bool isAsync = false;
    }
}
