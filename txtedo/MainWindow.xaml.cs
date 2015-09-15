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

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.IO;

namespace txtedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double pageHeight;
        public double pageWidth;
        public List<Command> masterList;
        private bool visibleFeedback = false;

        public MainWindow()
        {
            Dictionary commandList = new Dictionary();
            masterList = commandList.commands;

            InitializeComponent();

            LockPosition();

            //ToggleFeedback();
            UpdateFeedback();
        }

        //Lock to bottom left
        //TODO: Change in settings where to lock to
        public void LockPosition()
        {
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

        //Toggle feedback box
        public void ToggleFeedback()
        {
            const double compactHeight = 50.0;
            const double expandedHeight = 300.0;

            if (visibleFeedback)
            {
                Height = compactHeight;
                visibleFeedback = false;
            }
            else
            {
                Height = expandedHeight;
                visibleFeedback = true;
            }
        }

        //Update feedback box
        public void UpdateFeedback()
        {
            var feedbackList = new List<FeedbackElement>();
            
            foreach (Command command in masterList) {
                feedbackList.Add(new FeedbackElement {
                    Header = command.command,
                    Descriptor = command.commandTip
                });
            }

            //CollectionViewSource feedbackViewSource = (CollectionViewSource)(FindResource("feedbackData"));
            //feedbackViewSource.Source = feedbackList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Test");
                dynamic test = masterList[0].childCommands[0].module;
                test.Run("new ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void RunSearch(string searchQuery)
        {
            if (searchQuery != "")
            {
                try
                {
                    ScriptRuntime pyRuntime = Python.CreateRuntime();
                    //Set Python variables
                    ScriptScope scope = pyRuntime.CreateScope();
                    scope.SetVariable("userInput", searchQuery);

                    //Use Lib
                    //pyRuntime.ImportModule(@"modules/Lib");

                    dynamic script = pyRuntime.UseFile("modules/Explorer.py");
                    script.Run(searchQuery);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void UpdatePrompt(string prompt, bool solid = false)
        {
            string typingString = CommandBox.Text.ToString();

            if (typingString != "")
            {
                Application.Current.Resources["CurrentPrompt"] = "";
            }
            else
            {
                Application.Current.Resources["CurrentPrompt"] = prompt;
            }

            PromptLabel.Text = Application.Current.Resources["CurrentPrompt"].ToString();
        }

        private void CommandBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string oldPrompt = (string)PromptLabel.Text;
            UpdatePrompt(oldPrompt);
        }
    }
}
