using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerApp
{
    class Program
    {
        private static int defaultConnectionCount = 3;
        private const string _serverHost = "127.0.0.1";
        private const int _serverPort = 3535;
        private static Thread _serverThread;
        static void Main(string[] args)
        {
            _serverThread = new Thread(StartServer);
            _serverThread.IsBackground = true;
            _serverThread.Start();
            while (true)
                HandlerCommands(Console.ReadLine());
        }
        private static void HandlerCommands(string cmd)
        {
            cmd = cmd.ToLower();
            if (cmd.Contains("/getusers"))
            {
                int countUsers = Server.Server.Clients.Count();
                for (int i = 0; i < countUsers; i++)
                {
                    Console.WriteLine("[{0}]: {1}", i, Server.Server.Clients[i].UserName);
                }
            }
        }
        public static void StartServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(_serverHost);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, _serverPort);

            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndPoint);
            socket.Listen(defaultConnectionCount);
            Console.WriteLine("Сервер работает на ip: {0}", ipEndPoint);
            while (true)
            {
                try
                {
                    Socket incomeConnection = socket.Accept();
                    Server.Server.NewClient(incomeConnection);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error {0}", ex.Message);
                }
            }
        }
    }
}