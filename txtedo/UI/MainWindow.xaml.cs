using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Scripting.Hosting;
using txtedo.ViewModel;
using txtedo.Background;


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

        private void PhaseChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            //If bar becomes visible
            if ((bool)e.NewValue == true)
            {
                CommandBox.Focus();
            }
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
                    /*ScriptRuntime pyRuntime = Python.CreateRuntime();
                    //Set Python variables
                    ScriptScope scope = pyRuntime.CreateScope();
                    scope.SetVariable("userInput", searchQuery);

                    //Use Lib
                    //pyRuntime.ImportModule(@"modules/Lib");

                    dynamic script = pyRuntime.UseFile("modules/Explorer.py");
                    script.Run(searchQuery);*/
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void UnselectCells(DataGrid dg)
        {
            dg.UnselectAll();
            dg.UnselectAllCells();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            UnselectCells(sender as DataGrid);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as DataGrid;
            UnselectCells(obj);
           
        }
    }
}
