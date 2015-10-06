using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtedo.Background
{
    class SettingsControl
    {
        public int OSVerion()
        {
            string[] versionString = Environment.OSVersion.ToString().Split(' ');
            string versionNumber = versionString[versionString.Length - 1];

            string[] versionParts = versionNumber.Split('.');

            string totalVersion = versionParts[0] + "." + versionParts[1];

            int winVer = 0;
            
            switch (totalVersion)
            {
                //Win 7
                case "6.1":
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
                case "10.0":
                    winVer = 10;
                    break;
            }

            return winVer;
        }
    }
}
