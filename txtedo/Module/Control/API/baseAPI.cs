using System;

namespace txtedo.Module.Control.API
{
    public abstract class baseApi : coreAPI
    {
        private Command command;

        public baseApi (Command c)
        {
            this.command = c;
        }

        public string userInput()
        {
            string input = "";

            return input;
        }

        public void processCallback(string callback)
        {
            Action eve = this.command.findEvent(callback);

            if (eve != null)
            {
                resumeEvent(eve);
                return;
            }

            Action<string> trig = this.command.findTrigger(callback);

            if (eve != null)
            {
                resumeTrigger(trig);
            }
        }

        public void resumeEvent(Action eve)
        {
            eve.DynamicInvoke();
        }

        public void resumeTrigger(Action<string> trig)
        {
            string triggerOptions = userInput();

            trig.DynamicInvoke(triggerOptions);
        }

        public string userInput(string input)
        {

            return input;
        }
    }
}
