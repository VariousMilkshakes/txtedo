using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using txtedo.Properties;
using txtedo.Network;

namespace txtedo.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            NetAPI na = new NetAPI();

            if (Settings.Default.autoLogin)
            {
                if (na.autoLogin())
                {
                    Settings.Default.networkOn = true;

                    MainWindow txtedoBar = new MainWindow();

                    
                }
                else
                {
                    InitializeComponent();
                }
            }
        }
    }
}
