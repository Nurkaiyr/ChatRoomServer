using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Client
    {
        private string _userName;
        private Socket _handler;
        private Thread _userThread;
        public Client(Socket socket)
        {
            _handler = socket;
            _userThread = new Thread(Listen);
            _userThread.IsBackground = true;
            _userThread.Start();
        }
        public string UserName
        {
            get { return _userName; }
        }

        public void Listen()
        {
            while (true)
            {
                try
                {
                    int bytes;
                    byte[] data = new byte[1024];
                    bytes = _handler.Receive(data);
                    string newData = Encoding.UTF8.GetString(data, 0, bytes);
                    HandleCommand(newData);
   
                }
                catch
                {
                    Server.DisconnectClient(this); return;
                }
            }
        }

        public void HandleCommand(string data)
        {
            if (data.Contains("#setname"))
            {
                _userName = data.Split('&')[1];
                UpdateChat();
                return;
            }
            if (data.Contains("#newmsg"))
            {
                string message = data.Split('&')[1];
                ChatController.AddMessage(_userName, message);
                return;
            }
        }

        public void EndConnection()
        {
            try
            {
                _handler.Close();
                _userThread.Abort();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateChat()
        {
            Send(ChatController.GetChat());
        }

        public void Send(string message)
        {
            try
            {
                int bytesSend = _handler.Send(Encoding.UTF8.GetBytes(message));
                if (bytesSend > 0) Console.WriteLine("Успешно!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
