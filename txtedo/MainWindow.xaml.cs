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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace txtedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double pageHeight;
        public double pageWidth;

        public MainWindow()
        {
            Dictionary commandList = new Dictionary();

            InitializeComponent();

            pageWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Console.WriteLine(pageWidth);
            pageWidth -= Width;
            pageHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            Console.WriteLine(pageHeight);
            pageHeight -= Height + 40;

            Top = pageHeight;
            Left = pageWidth;

            Console.WriteLine("{0} : {1}", Top, Left);
        }
    }
}
