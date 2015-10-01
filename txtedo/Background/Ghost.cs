using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

using txtedo.Properties;

using Hardcodet.Wpf.TaskbarNotification;

using Utilities;

namespace txtedo.Background
{
    class Ghost
    {
        private bool isVisible;

        private Action phaseEvent;

        public Ghost()
        {
            this.isVisible = true;
        }

        public string Phase()
        {
            string state = "";

            if (this.isVisible)
            {
                this.isVisible = false;

                state = "Hidden";
            }
            else
            {
                this.isVisible = true;

                state = "Visible";
            }

            return state;
        }

        public void TypeFocus()
        {
            //txtedoBar.CommandBox.Focus();
        }

        public void Bind(Action bindEvent)
        {
            this.phaseEvent = bindEvent;

            this.CreateBinding();
        }

        private void CreateBinding()
        {
            globalKeyboardHook hook = new globalKeyboardHook();

            hook.HookedKeys.Add(Keys.Alt);
            hook.HookedKeys.Add(Keys.X);

            hook.KeyDown += new KeyEventHandler(this.ToggleTxtedo);
        }

        private void ToggleTxtedo(object sender, KeyEventArgs e)
        {
            Console.WriteLine("WELOP");

            this.phaseEvent();
        }
    }
}
