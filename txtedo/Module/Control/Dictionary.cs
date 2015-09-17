using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

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

        public Dictionary ()
        {
            //Get all python modules in folder
            string[] fileNames = Directory.GetFiles("modules/", "*.py");
            Console.WriteLine("FILES:");
            Console.WriteLine(fileNames[0]);

            List<dynamic> modules = new List<dynamic>();

            //Create python engine
            ScriptEngine python = Python.CreateEngine();
            //Python module paths
            ICollection<string> paths = python.GetSearchPaths();
            //Add stdlib
            paths.Add("modules/Lib");
            python.SetSearchPaths(paths);

            //Start iron python
            ScriptRuntime ipy = python.Runtime;
            

            //TODO: Set variable scope when module is called

            //Command[] commandHolder = new Command[fileNames.Length];
            List<Command> commandHolder = new List<Command>();
            List<YoungChild> lostChildren = new List<YoungChild>();

            //Create a command for each module
            int chIndex = 0;
            foreach (string file in fileNames)
            {
                //Run module setup
                try
                {
                    //New instance
                    dynamic module = ipy.UseFile(file);

                    //Every module must have start function
                    var info = module.Start(); 

                    //Command for module
                    Command newCommand;
                    
                    //Check if python had provided info
                    if (info[0] != "")
                    {
                        if (info[1] == "")
                        {
                            //Fallback to command name as tool tip
                            info[1] = info[0];
                        }

                        //Create command for module
                        newCommand = new Command(module, info[0], info[1]);

                        Console.WriteLine("File: {0}, {1} now imported!", file, info[0]);

                        //If module has a parent
                        if (info.Count == 3)
                        {
                            //Turn command into child, without parent
                            YoungChild lostChild = new YoungChild(info[2], newCommand, lostChildren.Count);
                            lostChildren.Add(lostChild);
                        }
                        else
                        {
                            //Add parents to holder
                            commandHolder.Add(newCommand);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Problem creating python module: {0}", ex);
                }

                chIndex++;
            }

            bool AllChildrenAccountedFor = false;

            while (!AllChildrenAccountedFor)
            {
                //Loop through every child
                if (lostChildren.Count > 0)
                {
                    for (int i = 0; i < lostChildren.Count; i++)
                    {
                        YoungChild lostChild = lostChildren[i];

                        //Parent command to pair with
                        string targetParent = lostChild.parentCommand;

                        //Check with each parent
                        for (int c = 0; c < commandHolder.Count; c++)
                        {
                            Command parent = commandHolder[c];

                            if (parent.command == targetParent)
                            {
                                //Add child command to parent
                                parent.NewChild(lostChild.me);
                                //Remove paired child from list
                                lostChildren.RemoveAt(lostChild.at);
                            }
                        }
                    }
                }
                else
                {
                    AllChildrenAccountedFor = true;
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

        public void AddCommand (Command command)
        {
            commands.Add(command);
        }
    }
}
