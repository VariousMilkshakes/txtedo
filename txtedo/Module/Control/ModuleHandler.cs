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

        private dynamic targetModule;

        private Command module;

        public ModuleHandler(Command module)
        {
            // TODO: Complete member initialization
            this.module = module;

            this.targetModule.SetVariable("PyAPI");
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
    }
}
