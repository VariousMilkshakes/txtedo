using System;

namespace txtedo.Module.Control
{
    public class CommandPreview
    {
        private string commandTitle;
        private string commandTip;

        public CommandPreview (Command c)
        {
            this.commandTitle = c.command;
            this.commandTip = c.commandTip;
        }

        public string name
        {
            get { return this.commandTitle; }
            set
            {
                this.commandTitle = value;
            }
        }

        public string tip
        {
            get { return this.commandTip; }
            set
            {
                this.commandTip = value;
            }
        }
    }
}
