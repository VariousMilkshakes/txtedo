using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtedo.modules
{
    class Explorer : IModule
    {
        public Command register ()
        {
            Command masterCommand = new Command("file-explore", "Navigate to a file.");

            return masterCommand;
        }
    }
}
