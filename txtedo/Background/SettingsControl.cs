using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using txtedo.Properties;

namespace txtedo.Background
{
    class SettingsControl
    {
        public SettingsControl ()
        {
            //Persistent Settings
            Settings.Default.WinVersion = OSVerion();

            Settings.Default.Save();

            //Session Settings

        }

        private int OSVerion()
        {
            //string[] versionString = Environment.OSVersion.ToString().Split(' ');
            //string versionNumber =

            //string[] versionParts = versionNumber.Split('.');

            string totalVersion =  Environment.OSVersion.Version.Major.ToString() +
                                   Environment.OSVersion.Version.MajorRevision;

            int winVer = 0;
            
            switch (totalVersion)
            {
                //Win 7
                case "60":
                    winVer = 7;
                    break;
                //Win 8
                case "6.2":
                    winVer = 8;
                    break;
                //Win 8.1
                case "6.3":
                    winVer = 81;
                    break;
                //Win 10
                case "100":
                    winVer = 10;
                    break;
                default:
                    winVer = 7;
                    break;
            }

            return winVer;
        }
    }
}
