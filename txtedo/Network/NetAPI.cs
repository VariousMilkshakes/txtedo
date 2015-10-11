using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using txtedo.Properties;

namespace txtedo.Network
{
    class NetAPI
    {
        private Interface netInt;

        public NetAPI ()
        {
            netInt = new Interface();
        }

        public bool autoLogin ()
        {
            if (Settings.Default.savedPassword != "" && Settings.Default.savedUsername != "")
            {

            }

            return true;
        }
    }
}
