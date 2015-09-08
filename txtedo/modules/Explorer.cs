using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace txtedo.modules
{
    class Explorer : IModule
    {
        public Explorer()
        {
            //Runs when explorer is loaded
        }

        public void register(Dictionary dictionary, Object rules)
        {
            //Invoked by dictionary after loaded
            Console.WriteLine("Explorer Loaded");

            Command masterCommand = new Command(rules, "file-explore", "Navigate to a file.");

            dictionary.AddCommand(masterCommand);
        }

        //Manage all rules related to command
        public void tasker(string task)
        {
            Console.WriteLine(task);
        }
    }
}
