using System;
using System.Collections.Generic;

namespace txtedo.Module
{
    public abstract class ModuleBase
    {
        public string Name; // Name of the Module e.g. "web"
        public string Desc; // Desc of the Module e.g. "Used to search interwebz"
        public bool hasQuery = false; // Requires user input to run module
        public bool isParent = true; // It is the start of a module tree
        public List<KeyValuePair<string, ModuleBase>> Commands = new List<KeyValuePair<string, ModuleBase>>(); // Look below
                                                                            // Stores all related module commands, List of KeyValuePairs
                                                                            // Format: string (key), Function (Value)
                                                                            //                       Function takes string as an argument
                                                                            //                       Function Returns an array of strings
        /// <summary>
        /// A trigger will pause the module and wait for user feedback to continue returning void
        /// </summary>
        public Dictionary<string, Action<string>> Triggers = new Dictionary<string, Action<string>>();
        /// <summary>
        /// An event is run without user input and return void e.g. a callback for the translator
        /// </summary>
        public Dictionary<string, Action> Events = new Dictionary<string, Action>();


        public abstract string[] Init(); // Ran when the module is compiled
        /// <summary>
        /// Run when module is choosen by the user
        /// </summary>
        public abstract void OnRun(string input);

        /// <summary>
        /// Allows universal control of module construction
        /// </summary>
        public ModuleBase()
        {
            Init();

            Action<string> run = OnRun;
            Triggers.Add("run", run);
        }

        /// <summary>
        /// Universal function for adding child modules to module
        /// </summary>
        /// <param name="childModule">Child module to add</param>
        public void AddCommand(ModuleBase childModule)
        {
            try
            {
                Commands.Add(new KeyValuePair<string, ModuleBase>(childModule.Name, childModule));
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Add triggers to module
        /// </summary>
        /// <param name="triggerName">Unique name of trigger</param>
        /// <param name="trigger">Void function with string argument to act as trigger</param>
        public void AddTrigger(string triggerName, Action<string> trigger)
        {
            try
            {
                Triggers.Add(triggerName, trigger);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Add event to module
        /// </summary>
        /// <param name="eventName">Unique name of event</param>
        /// <param name="_event">V function with no arguments to act as event</param>
        public void AddEvent(string eventName, Action _event)
        {
            try
            {
                Events.Add(eventName, _event);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Register module as a child
        /// </summary>
        public void Child()
        {
            isParent = false;
        }
    }
}
