using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Forms;

using txtedo.Module.Control;
using txtedo.Module.Control.API;
using txtedo.Background;

namespace txtedo.ViewModel
{
    /// <summary>
    /// View Model for main Txtedo Bar UI
    /// </summary>
    class MainViewModel : INotifyPropertyChanged
    {

        #region private fields
        /// <summary>
        /// Backend control
        /// </summary>
        private TxtedoBar tBar;

        #region event handlers
        private InputListener ilr_SendCommand;
        private InputListener ilr_BackUpCommand;
        private InputListener ilr_HideBarCommand;
        #endregion

        /// <summary>
        /// Background and binding handler
        /// </summary>
        private Ghost background;

        /// <summary>
        /// Retrieve and Change Settings
        /// </summary>
        private SettingsControl settings;
        #endregion

        /// <summary>
        /// Notify UI of changed property
        /// </summary>
        public event PropertyChangedEventHandler propChanged;

        /// <summary>
        /// Create api objects and setup for use
        /// </summary>
        public MainViewModel()
        {
            List<baseAPI> apiStack = new List<baseAPI>();

            //Construct objects
            this.tBar = new TxtedoBar(apiStack);
            this.background = new Ghost();
            this.settings = new SettingsControl();
        }

        #region public events

        public ICommand submitInput
        {
            get { return this.ilr_SendCommand; }
        }

        public ICommand backUpInput
        {
            get { return this.ilr_BackUpCommand; }
        }

        public ICommand phaseOutBar
        {
            get { return this.ilr_HideBarCommand; }
        }

        #endregion

        #region public get/set variables

        /// <summary>
        /// Handles list of possible commands above Txtedo Bar
        /// </summary>
        public ObservableCollection<PreviewItem> preview
        {
            get { return this.tBar.CommandPreview; }
            set { this.tBar.CommandPreview = value; }
        }

        /// <summary>
        /// Notifys user if they have enabled quotes
        /// </summary>
        public string lblQuote
        {
            get
            {
                if (this.tBar.inQuotes)
                {
                    return "\"";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Handles prompt behind textbox
        /// </summary>
        public string lblPrompt
        {
            get { return this.tBar.visiblePrompt; }
            set
            {
                this.tBar.visiblePrompt = value;

                this.Refresh("lblPrompt");
            }
        }

        /// <summary>
        /// Tracks user input
        /// </summary>
        public string txtCommandInput
        {
            get { return this.tBar.currentCommand; }
            set
            {
                //Track user input
                this.tBar.currentCommand = value;

                //Tell txtedo that input has changed
                this.tBar.ChangeInput();

                //Refresh UI
                this.RefreshAll();
            }
        }

        /// <summary>
        /// Changes visibility of Txtedo Window
        /// </summary>
        public string visibility
        {
            get { return this.tBar.barVisibility; }
            set
            {
                this.tBar.barVisibility = value;

                this.Refresh("visibility");
            }
        }

        /// <summary>
        /// Height of item list above Txtedo Bar
        /// </summary>
        public int previewHeight
        {
            get
            {
                //Check if there are any items to display
                if (this.tBar.CommandPreview == null)
                {

                    //Collapse List Box
                    return 0;

                }

                int height = this.tBar.CommandPreview.Count * 25;

                return height + 5;
            }
            //Needs to be 2 way in order to update
            set { return; }
        }

        /// <summary>
        /// Height of Txtedo Window
        /// </summary>
        public int windowHeight
        {
            get
            {
                //Slightly taller than contents
                this.tBar.height = previewHeight + 30;

                return this.tBar.height;
            }
            //Needs to be 2 way in order to update
            set { return; }
        }

        public double leftLock
        {
            get
            {
                //Get width of monitor
                double pageWidth = SystemParameters.PrimaryScreenWidth;
                pageWidth -= this.tBar.width;
                return pageWidth;
            }
            //Needs to be 2 way in order to update
            set { return; }
        }

        public double topLock
        {
            get
            {
                //Get height of monitor
                double pageHeight = SystemParameters.PrimaryScreenHeight;

                //Check if txtedo has a size yet
                if (this.tBar.height == 0)
                {
                    //Fallback to window height
                    this.tBar.height = windowHeight;
                }

                pageHeight -= this.tBar.height;
                pageHeight -= this.taskbarHeight;

                return pageHeight;
            }
        }

        #endregion

        #region private get/set

        private int taskbarHeight
        {
            get
            {
                double height = Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height;

                //Check windows version
                if (Properties.Settings.Default.WinVersion > 7)
                {
                    height /= 2;
                }

                return height;
            }
        }

        #endregion

        #region ui events

        /// <summary>
        /// Sends user input to Txtedo Bar
        /// </summary>
        private void SubmitCommand()
        {
            this.tBar.SendCommand();

            this.RefreshAll();
        }

        /// <summary>
        /// Default backspace functionality, if empty input move backup command tree
        /// </summary>
        private void BackUp()
        {
            this.tBar.BackUpCommand();

            this.RefreshAll();
        }

        /// <summary>
        /// Toggle window between being locked in corner and minimised to system tray
        /// </summary>
        private void ChangeVisibility()
        {
            visibility = this.background.Phase();

            if (visibility == "Hidden")
            {

                this.tBar.ResetBar();

                this.RefreshAll();

            }
        }

        #endregion

        #region event triggers
        /// <summary>
        /// Refresh all elements
        /// </summary>
        private void RefreshAll()
        {
            if (this.propChanged != null)
            {
                this.propChanged(this, new PropertyChangedEventArgs("TxtCommandInput"));
                this.propChanged(this, new PropertyChangedEventArgs("lblPrompt"));
                this.propChanged(this, new PropertyChangedEventArgs("Preview"));
                this.propChanged(this, new PropertyChangedEventArgs("lblQuote"));
                this.propChanged(this, new PropertyChangedEventArgs("WindowHeight"));
                this.propChanged(this, new PropertyChangedEventArgs("PreviewHeight"));
                this.propChanged(this, new PropertyChangedEventArgs("TopLock"));
            }
        }

        /// <summary>
        /// Refresh target element
        /// </summary>
        /// <param name="element">Target Element</param>
        private void Refresh(string element)
        {
            if (this.propChanged != null)
            {
                this.propChanged(this, new PropertyChangedEventArgs(element));
            }
        }

        #endregion
    }
}
