using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtedo.modules
{
    class Explorer : IModule
    {
        public Explorer()
        {
            //Runs when explorer is loaded
        }

        public Command register(Dictionary dictionary)
        {
            //Invoked by dictionary after loaded
            Console.WriteLine("Explorer Loaded");

            Command masterCommand = new Command("file-explore", "Navigate to a file.");

            dictionary.AddCommand(masterCommand);

            return masterCommand;
        }
    }
}
