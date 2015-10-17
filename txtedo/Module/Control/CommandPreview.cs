using System;

namespace txtedo.Module.Control
{
    public class PreviewItem
    {
        private string commandTitle;
        private string commandTip;

        public PreviewItem()
        {
            this.commandTitle = "none set";
            this.commandTip = "";
        }

        public PreviewItem (Command c)
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
