using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class Server
    {
        public static List<Client> Clients = new List<Client>();

        public static void NewClient(Socket socket)
        {
            try
            {
                Client newClient = new Client(socket);
                Clients.Add(newClient);
                Console.WriteLine("В комнату вошел: {0}", socket.RemoteEndPoint);
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0} Не удалось подключиться", ex.Message);
            }
        }

        public static void DisconnectClient(Client client)
        {
            try
            {
                client.EndConnection();
                Clients.Remove(client);
                Console.WriteLine("Пользователь {0} отключился", client.UserName);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка с отключением : {0}",ex.Message);
            }
        }

        public static void UpdateAllChats()
        {
            try
            {
                int countUsers = Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Clients[i].UpdateChat();
                }
            }
            catch (Exception exp) { Console.WriteLine("Error with updateAllChats: {0}.", exp.Message); }
        }
    }
}
