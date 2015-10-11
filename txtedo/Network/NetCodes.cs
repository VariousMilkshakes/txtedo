using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace txtedo.Network
{
    class NetCodes
    {
        //Even = expects return
        //Odd  = send information

        //Login Codes
        private int login_in = 100;
        private int login_out = 101;
        private int login_delete = 103;
        private int login_newDevice = 104;
        
        //Module Codes
        private int module_getAll = 200;
        private int module_getThis = 202;
        private int module_publish = 203;
        private int module_remove = 204;

        public int login
        {
            get { return login_in; }
        }

        public int logout
        {
            get { return login_out; }
        }

        public int deleteAccount
        {
            get { return login_delete; }
        }

        public int newDevice
        {
            get { return login_newDevice; }
        }

        public int allModules
        {
            get { return module_getAll; }
        }

        public int thisModule
        {
            get { return module_getThis; }
        }

        public int publish
        {
            get { return module_publish; }
        }

        public int removeModule
        {
            get { return module_remove; }
        }
    }
}
