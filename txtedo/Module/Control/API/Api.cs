using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using txtedo.ViewModel;
using txtedo.Properties;

namespace txtedo.Module.Control.API
{
    public class Api
    {
        private TxtedoBarViewModel uiControl;

        public PreviewItem ListItem;

        public Api (TxtedoBarViewModel viewModel)
        {
            ListItem = new PreviewItem();

            this.uiControl = viewModel;
        }
    }
}
