using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace txtedo
{
    class Dictionary
    {
        public List<Command> commands = new List<Command>();

        public Dictionary ()
        {
            //Get all python modules in folder
            string[] fileNames = Directory.GetFiles("modules/", "*.py");
            Console.WriteLine("FILES:");
            Console.WriteLine(fileNames[0]);

            List<dynamic> modules = new List<dynamic>();

            //Start iron python
            ScriptRuntime ipy = Python.CreateRuntime();
            //TODO: Set variable scope when module is called=

            //Create a command for each module
            foreach (string file in fileNames)
            {
                //New instance
                dynamic module = ipy.UseFile(file);

                //Run module setup
                try
                {
                    //Every module must have start function
                    var info = module.Start();

                    if (info[0] != "")
                    {
                        if (info[1] == "")
                        {
                            info[1] = info[0];
                        }

                        Command newCommand = new Command(module, info[0], info[1]);
                        AddCommand(newCommand);

                        Console.WriteLine("File: {0}, {1} now imported!", file, info[0]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
            }

            //Get interface ALL modules inheirt from
            //var commandModule = typeof(IModule);
            //Gets all classes that inheirt from the interface
            //var modules = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(p => p.IsClass && commandModule.IsAssignableFrom(p)).ToArray();
            
            //for (int i = 0; i < modules.Length; i++)
            //{
             //   Console.WriteLine("Loading Module");
             //   //Load instance of module
             //   var module = Activator.CreateInstance(modules[i]);
              //  //Get registration method
              //  MethodInfo moduleInfo = commandModule.GetMethod("register");

                //Pass this dictionary to reg method
               // object[] parametersArray = new object[] {this, module};
                //Invoke reg method
               // moduleInfo.Invoke(module, parametersArray);
            //}
        }

        public void AddCommand (Command command)
        {
            commands.Add(command);
        }
    }
}
