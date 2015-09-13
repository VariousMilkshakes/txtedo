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

        public Dictionary ()
        {
            //Get all python modules in folder
            string[] fileNames = Directory.GetFiles("modules/", "*.py");
            Console.WriteLine("FILES:");
            Console.WriteLine(fileNames[0]);

            List<dynamic> modules = new List<dynamic>();

            //Start iron python
            ScriptRuntime ipy = Python.CreateRuntime();
            //TODO: Set variable scope when module is called

            //Command[] commandHolder = new Command[fileNames.Length];
            List<Command> commandHolder = new List<Command>();
            List<YoungChild> lostChildren = new List<YoungChild>();

            //Create a command for each module
            int chIndex = 0;
            foreach (string file in fileNames)
            {
                //New instance
                dynamic module = ipy.UseFile(file);

                //Run module setup
                try
                {
                    //Every module must have start function
                    var info = module.Start(); 

                    //Command for module
                    Command newCommand;
                    
                    if (info[0] != "")
                    {
                        if (info[1] == "")
                        {
                            info[1] = info[0];
                        }

                        newCommand = new Command(module, info[0], info[1]);

                        Console.WriteLine("File: {0}, {1} now imported!", file, info[0]);

                        if (info.Count == 3)
                        {
                            YoungChild lostChild = new YoungChild(info[2], newCommand, lostChildren.Count);
                            lostChildren.Add(lostChild);
                        }
                        else
                        {
                            commandHolder.Add(newCommand);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                chIndex++;
            }

            bool AllChildrenAccountedFor = false;

            while (!AllChildrenAccountedFor)
            {
                if (lostChildren.Count > 0)
                {
                    for (int i = 0; i < lostChildren.Count; i++)
                    {
                        YoungChild lostChild = lostChildren[i];

                        string targetParent = lostChild.parentCommand;

                        for (int c = 0; c < commandHolder.Count; c++)
                        {
                            Command parent = commandHolder[c];

                            if (parent.command == targetParent)
                            {
                                parent.NewChild(lostChild.me);
                                lostChildren.RemoveAt(lostChild.at);
                                Console.WriteLine(parent.command);
                            }
                        }
                    }
                }
                else
                {
                    AllChildrenAccountedFor = true;
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
