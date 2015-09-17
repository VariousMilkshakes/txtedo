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
    }
}
