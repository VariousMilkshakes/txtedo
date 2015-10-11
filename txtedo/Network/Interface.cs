using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace txtedo.Network
{
    class Interface
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;

        public Interface()
        {
            tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect("127.0.0.1", 8765);
                Console.WriteLine("Connected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Could not connect to server");
                return;
            }

            networkStream = tcpClient.GetStream();

            byte[] incoming = new byte[4096];
            int byteIndex = 0;

            try
            {
                byteIndex = networkStream.Read(incoming, 0, 4096);
            }
            catch
            {
                Console.WriteLine("Could not read socket");
            }

            ASCIIEncoding encoder = new ASCIIEncoding();
            string message = encoder.GetString(incoming, 0, byteIndex);

            Console.WriteLine(message);

            JsonObject package = new JsonObject();
            package.Add("username", "coolkid");
            package.Add("password", "wowpass");

            NetCodes netcode = new NetCodes();

            Packet packet = new Packet("none", "none", netcode.login, package);

            string tcpData = packet.getJsonString();

            //byte[] outgoing = new byte[tcpData.Length * sizeof(char)];

            byte[] outgoing = encoder.GetBytes(tcpData);

            try
            {
                networkStream.Write(outgoing, 0, outgoing.Length);
            }
            catch
            {
                Console.WriteLine("Cannot send");
            }

            tcpClient.Close();
        }
    }
}
