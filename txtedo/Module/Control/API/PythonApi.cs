using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Configuration;

using txtedo.ViewModel;
using txtedo.Properties;

namespace txtedo.Module.Control.API
{
    public class PythonApi : baseAPI
    {
        public string fileExtention;

        private TxtedoBarViewModel uiControl;

        public PreviewItem ListItem;

        private ModuleHandler tgh;

        public PythonApi (TxtedoBarViewModel viewModel, string extention)
        {
            ListItem = new PreviewItem();

            fileExtention = extention;

            this.uiControl = viewModel;
        }

        public void setTriggerHandler(ModuleHandler mh)
        {
            this.tgh = mh;
        }

        public void stop()
        {
            this.tgh.stop();
        }

        public void newTriggers(string[] triggerIDs)
        {

        }

        public void trigger(string triggerID)
        {

        }

        //Loop through all settings and return list of viewable settings
        public List<readSetting> getSettings ()
        {
            List<readSetting> settings = new List<readSetting>();

            foreach (SettingsProperty setting in  Settings.Default.Properties)
            {
                readSetting viewableSetting = new readSetting(setting.Name);
                settings.Add(viewableSetting);
            }

            return settings;
        }

        public readSetting getSetting (string settingName)
        {
            return new readSetting(settingName);
        }

        //Display custom list in preview list
        public void previewCustomList (List<PreviewItem> newList)
        {
            ObservableCollection<PreviewItem> newPreview = new ObservableCollection<PreviewItem>(newList);
            uiControl.Preview = newPreview;
        }

        //Create a new item for a preview list
        public PreviewItem newPreviewItem (string header, string body)
        {
            PreviewItem item = new PreviewItem();
            item.name = header;
            item.tip = body;

            return item;
        }

        //Create a viewable settings object from setting name
        public class readSetting
        {
            private string title;
            private string setValue;

            public readSetting (string settingTitle)
            {
                SettingsPropertyCollection settingList = Settings.Default.Properties;
                
                foreach (SettingsProperty setting in settingList)
                {
                    if (setting.Name == settingTitle)
                    {
                        this.title = setting.Name;
                        this.setValue = setting.DefaultValue.ToString();
                    }
                }
            }

            public string key
            {
                get { return this.title; }
            }

            public string value
            {
                get { return this.setValue; }
            }
        }
    }
}
