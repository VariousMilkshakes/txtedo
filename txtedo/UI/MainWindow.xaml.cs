using System;
using System.Windows;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;



namespace txtedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double pageHeight;
        public double pageWidth;
        private bool visibleFeedback = false;

        public MainWindow()
        {

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
            //CollectionViewSource feedbackViewSource = (CollectionViewSource)(FindResource("feedbackData"));
            //feedbackViewSource.Source = feedbackList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Test");
                //dynamic test = masterList[0].childCommands[0].module;
                //test.Run("new ");
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
    }
}
