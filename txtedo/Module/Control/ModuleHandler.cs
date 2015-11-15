using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using txtedo.Module.Control.API;

namespace txtedo.Module.Control
{
    public class ModuleHandler
    {
        private bool asyncModule;
        private bool needsQuery;
        private bool moduleRunning = false;

        private baseAPI selectedAPI;

        private dynamic targetModule;

        private Command module;
        private List<baseAPI> APIlist;

        public ModuleHandler(Command module, List<baseAPI> list)
        {
            // TODO: Complete member initialization
            this.module = module;
            this.APIlist = list;

            this.extractModule(module, this.APIlist);

            this.targetModule.SetVariable("PyAPI", this.selectedAPI);
        }

        public bool running
        {
            get { 
                if (targetModule == null)
                {
                    return false;
                }
                return true;
            }
        }

        public void runModule(string options)
        {
            this.moduleRunning = true;

            if (needsQuery && options != "")
            {
                this.targetModule.Run(options);
            }
            else
            {
                this.targetModule.Run();
            }

            while(this.moduleRunning)
            {
                Thread.Sleep(500);
                Console.WriteLine("WOW : " + moduleRunning);
            }
        }

        public void stop()
        {
            this.moduleRunning = false;
            this.targetModule = null;
        }

        public void pause()
        {
            Thread.Sleep(Timeout.Infinite);
        }

        //Grab relevant data from module
        private void extractModule(Command module, List<Control.API.baseAPI> apiStack)
        {
            this.asyncModule = module.isAsync;
            this.needsQuery = module.hasQuery;
            this.targetModule = module.module;

            switch (module.language)
            {
                case "python":
                    this.selectedAPI = apiStack[0];
                    break;
                case "c#":
                    this.selectedAPI = apiStack[0];  
                    break;
            }

            this.selectedAPI.setTriggerHandler(this);
        }
    }
}
