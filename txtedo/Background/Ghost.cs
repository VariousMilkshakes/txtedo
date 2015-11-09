using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

using txtedo.Properties;
using txtedo.ViewModel;

using Hardcodet.Wpf.TaskbarNotification;

using Utilities;

namespace txtedo.Background
{
    public class Ghost
    {
        private bool isVisible;

        private Action phaseEvent;

        private GhostCommands commands;

        public Ghost()
        {
            this.isVisible = true;

            this.CreateTux();

            commands = new GhostCommands();
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

        public void ReBind()
        {
            this.CreateBinding();
        }

        private void CreateBinding()
        {
            globalKeyboardHook hook = new globalKeyboardHook();

            hook.HookedKeys.Add(Keys.X);

            hook.KeyDown += new KeyEventHandler(this.ToggleTxtedo);
        }

        private void CreateTux()
        {
            TaskbarIcon tux = new TaskbarIcon();
            tux.Icon = Resources.tuxicon;
            tux.ToolTipText = "Txtedo";

            //Controls.ContextMenu tuxMenu = new ContextMenu();
            System.Windows.Controls.ContextMenu tuxMenu = new System.Windows.Controls.ContextMenu();

            System.Windows.Controls.MenuItem settingsItem = new System.Windows.Controls.MenuItem();
            settingsItem.Header = "Settings";
            settingsItem.Click += delegate { commands.openSettings(); };
            System.Windows.Controls.MenuItem openItem = new System.Windows.Controls.MenuItem();
            openItem.Header = "Open";
            openItem.Click += delegate { this.phaseEvent(); };
            System.Windows.Controls.MenuItem closeItem = new System.Windows.Controls.MenuItem();
            closeItem.Header = "Close";
            closeItem.Click += delegate { commands.exitTxtedo(); };

            tuxMenu.Items.Add(settingsItem);
            tuxMenu.Items.Add(openItem);
            tuxMenu.Items.Add(closeItem);

            tux.ContextMenu = tuxMenu;
        }

        private void ToggleTxtedo(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Alt)
            {
                this.phaseEvent();
            }
        }
    }
}
