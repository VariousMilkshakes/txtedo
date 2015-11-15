using System;
using System.Collections.Generic;

namespace txtedo.Module
{
    public abstract class ModuleBase
    {
        public string Name; // Name of the Module e.g. "web"
        public string Desc; // Desc of the Module e.g. "Used to search interwebz"
        public List<KeyValuePair<string, Func<string, string[]>>> Commands = new List<KeyValuePair<string, Func<string, string[]>>>(); // Look below
                                                                            // Stores all related module commands, List of KeyValuePairs
                                                                            // Format: string (key), Function (Value)
                                                                            //                       Function takes string as an argument
                                                                            //                       Function Returns an array of strings

        public abstract string[] Init(); // Ran when the module is compiled
        public abstract string[] OnRun(); // Ran when the module is called by the user

        public void AddCommand(string CmdName, Func<string, string[]> Callback ) // Universal method to add commands to module base
        {
            try
            {
                Commands.Add(new KeyValuePair<string, Func<string, string[]>>(CmdName, Callback));
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
