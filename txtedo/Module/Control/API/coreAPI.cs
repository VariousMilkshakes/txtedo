using System;

namespace txtedo.Module.Control.API
{
    interface coreAPI
    {
        string userInput(string input);
        void processCallback(string callback);
        void resumeEvent(Action eve);
        void resumeTrigger(Action<string> trig);
    }
}
