using System;
using System.Windows;
using System.Windows.Controls;

using txtedo.Properties;

using IronPython.Hosting;

using Microsoft.Scripting.Hosting;

using txtedo.ViewModel;
using txtedo.Background;
using txtedo.Module.Control;
using txtedo.Network;

namespace txtedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public double pageHeight;
        public double pageWidth;
        private bool visibleFeedback = false;

        public MainWindow()
        {
            InitializeComponent();

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.PhaseChange);

            //Interface tcp = new Interface();
        }

        private void PhaseChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            //If bar becomes visible
            if ((bool)e.NewValue == true)
            {
                CommandBox.Focus();
            }
        }
    }
}
