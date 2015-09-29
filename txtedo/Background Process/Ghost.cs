using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Utilities;

namespace txtedo.Background
{
    class Ghost
    {
        private MainWindow txtedoBar;

        private bool isVisible;

        public Ghost(MainWindow txtBar)
        {
            this.txtedoBar = txtBar;
            this.isVisible = true;
        }

        public void Phase()
        {
            if (this.isVisible)
            {
                txtedoBar.Hide();

                this.isVisible = false;
            }
            else
            {
                txtedoBar.Show();

                this.isVisible = true;
            }
        }

        public void TypeFocus()
        {
            txtedoBar.CommandBox.Focus();
        }

        public void Bind()
        {
            this.CreateBinding();
        }

        private void CreateBinding()
        {
            globalKeyboardHook hook = new globalKeyboardHook();

            hook.HookedKeys.Add(Keys.Alt);
            hook.HookedKeys.Add(Keys.X);

            hook.KeyDown += new KeyEventHandler(this.OpenTxtedo);
        }

        private void OpenTxtedo(object sender, KeyEventArgs e)
        {
            Console.WriteLine("WELOP");

            this.Phase();
            this.TypeFocus();
        }
    }
}
