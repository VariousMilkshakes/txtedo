using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtedo
{
    interface IModule
    {
        void register(Dictionary dictionary, Object rules);
        void tasker(string task);
    }
}
