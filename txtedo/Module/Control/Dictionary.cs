using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using txtedo.modules;

namespace txtedo.Module.Control
{
    class Dictionary
    {
        public List<Command> commands = new List<Command>();
        public string[] commandRef;

        public class YoungChild
        {
            public Command me;
            public string parentCommand;
            public int at;

            public YoungChild (string parent, Command child, int index)
            {
                this.me = child;
                this.parentCommand = parent;
                this.at = index;
            }
        }

        private string[] FindModules(string path="modules/")
        {
            return Directory.GetDirectories(path);
        }

        public Dictionary ()
        {
            //Very rough loop through different apis
            //Won't work just best just to do one after another or assign threads
            {
                //Get all python modules in folder
                //string[] fileNames = Directory.GetFiles("modules/", "*.py");
                //string[] moduleNames = FindModules();
                /*List<string> fileNames = new List<string>();
                foreach (string module in moduleNames)
                {
                    if (module == "modules/Lib") { continue; }
                    string[] tmp = Directory.GetFiles(module + "/", "*.py");
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        fileNames.Add(tmp[i]);
                    }
                }
                //Directory.GetFiles(moduleNames + "/", "*.py");
                Console.WriteLine("FILES:");
                //Console.WriteLine(fileNames[0]);

                List<dynamic> modules = new List<dynamic>();

                //Create python engine
                //ScriptEngine python = Python.CreateEngine();
                //Python module paths
                //ICollection<string> paths = python.GetSearchPaths();
                //Add stdlib
                //paths.Add("modules/Lib");
                //python.SetSearchPaths(paths);

                //Start iron python
                //ScriptRuntime ipy = python.Runtime;


                //TODO: Set variable scope when module is called
                */
                //Command[] commandHolder = new Command[fileNames.Length];
                
                List<Command> commandHolder = new List<Command>();
                List<YoungChild> lostChildren = new List<YoungChild>();

                IEnumerable<System.Type> modules = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.IsSubclassOf(typeof(ModuleBase)));

                foreach (System.Type type in modules)
                {
                    ModuleBase module = (ModuleBase)Activator.CreateInstance(type);

                    if (module.isParent && module.Commands.Count > 0)
                    {
                        Command command = new Command(module);

                        commandHolder.Add(command);

                        foreach (KeyValuePair<string, ModuleBase> cmd in module.Commands)
                        {
                            command.NewChild(new Command(cmd.Value));
                        }
                    }
                }

                //Assign finished list to command dictionary
                this.commands = commandHolder;
                this.commandRef = new string[commandHolder.Count];

                int index = 0;
                foreach (Command c in commandHolder)
                {
                    this.commandRef[index] = c.command;
                    index++;
                }
            }
        }

        public void AddCommand (Command command)
        {
            commands.Add(command);
        }
    }
}
