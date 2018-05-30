using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ChatController
    {
        public static List<Message> Chat = new List<Message>();

        public struct Message
        {
            public string username;
            public string data;
            public Message(string name, string message)
            {
                username = name;
                data = message;
            }
        }

        public static void AddMessage(string username, string message)
        {
            try
            {
                Message newMessage = new Message(username, message);
                Chat.Add(newMessage);
                Console.WriteLine("Новое сообщение от :", username);
                Server.UpdateAllChats();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error with addMessage :{0}",ex.Message);
            }
        }
        public static void ClearChat()
        {
            Chat.Clear();
        }

        public static string GetChat()
        {
            try
            {
                string data = "#updatechat&";
                int countMessages = Chat.Count;
                if (countMessages <= 0) return string.Empty;
                for (int i = 0; i < countMessages; i++)
                {
                    data += String.Format("{0}~{1}|", Chat[i].username, Chat[i].data);
                }
                return data;
            }
            catch (Exception exp) { Console.WriteLine("Error with getChat: {0}", exp.Message); return string.Empty; }
        }
    }
}
