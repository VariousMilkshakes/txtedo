using System;
using System.Collections.Generic;

namespace txtedo.Module
{
    public abstract class ModuleBase
    {
        public string Name; // Name of the Module e.g. "web"
        public string Desc; // Desc of the Module e.g. "Used to search interwebz"
        public bool hasQuery;
        public bool isParent = true;
        public List<KeyValuePair<string, ModuleBase>> Commands = new List<KeyValuePair<string, ModuleBase>>(); // Look below
                                                                            // Stores all related module commands, List of KeyValuePairs
                                                                            // Format: string (key), Function (Value)
                                                                            //                       Function takes string as an argument
                                                                            //                       Function Returns an array of strings


        public abstract string[] Init(); // Ran when the module is compiled
        public abstract string[] OnRun(); // Ran when the module is called by the user

        public void AddCommand(ModuleBase childModule) // Universal method to add commands to module base
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

        public void Child()
        {
            isParent = false;
        }
    }
}
