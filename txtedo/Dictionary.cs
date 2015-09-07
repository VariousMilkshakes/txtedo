using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace txtedo
{
    class Dictionary
    {
        private List<Command> commands = new List<Command>();

        public Dictionary ()
        {
            //Get interface ALL modules inheirt from
            var commandModule = typeof(IModule);
            //Gets all classes that inheirt from the interface
            var modules = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(p => p.IsClass && commandModule.IsAssignableFrom(p)).ToArray();
            
            for (int i = 0; i < modules.Length; i++)
            {
                Console.WriteLine("Loading Module");
                //Load instance of module
                var module = Activator.CreateInstance(modules[i]);
                //Get registration method
                MethodInfo moduleInfo = commandModule.GetMethod("register");

                //Pass this dictionary to reg method
                object[] parametersArray = new object[] {this};
                //Invoke reg method
                moduleInfo.Invoke(module, parametersArray);
            }
        }

        public void AddCommand (Command command)
        {
            commands.Add(command);
        }
    }
}
