using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using txtedo.UI;
using txtedo.ViewModel;

namespace txtedo.Background
{
    class GhostCommands
    {
        private SettingsWindow settings;

        public GhostCommands()
        {
            settings = new SettingsWindow();
        }

        public void openSettings()
        {
            settings.Show();
        }

        public void exitTxtedo()
        {
            Environment.Exit(0);
        }
    }
}
