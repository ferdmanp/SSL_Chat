using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace RSA_WinTest
{
    class ChatClient
    {
        #region --VARS--
        string userName;
        IPAddress ipHost;
        int port=9999;
        TcpClient client;
        NetworkStream stream;
        Encoding ByteConverter = new UTF8Encoding();
        List<string> messages = new List<string>();
        
        LogMethod Log;
        #endregion

        #region --PROPS--
        public Queue<string> msgQueue { get; private set; }
        public bool IsConnected
        {
            get {
                return (client != null) ? client.Connected : false;
            }
        }
        #endregion

        #region --CTOR--
        public ChatClient(string host, int port, string username)
        {
            msgQueue = new Queue<string>();
            client = new TcpClient();
            this.userName = username;
            this.ipHost = IPAddress.Parse(host);
            this.port = port;
            Connect();
        }


        #endregion

        #region --METHODS--
        public void Connect()
        {
            client.Connect(ipHost, port);
            stream = client.GetStream();

            SendMessage(userName);
        }

        void SendMessage(string message)
        {
            while (true)
            {
                byte[] data = ByteConverter.GetBytes(message);
                stream.Write(data,0,data.Length);
            }
        }

        void RecieveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024];
                    string message = String.Empty;
                    int bytesCount = 0;
                    do
                    {
                        bytesCount = stream.Read(data, 0, data.Length);
                        message += ByteConverter.GetString(data, 0, bytesCount);
                    } while (stream.DataAvailable);

                    //messages.Add(message);
                    msgQueue.Enqueue(message);
                }
                catch (Exception exc)
                {
                    Log($"Connection interrupted! ERROR: {exc.Message}");
                    Disconnect();
                }
            }
        }

        private void Disconnect()
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
        }
        #endregion
    }
}
