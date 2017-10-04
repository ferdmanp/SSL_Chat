using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSA_WinServer
{
    //https://metanit.com/sharp/net/4.4.php
    public delegate void LogMethod(string message);

    public class ClientObject
    {
        #region --VARS--
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server;
        LogMethod Log= Console.WriteLine;
        Encoding byteConverter = new UTF8Encoding();
        private readonly int DEFAULT_BUFFER_SIZE=64;
        #endregion

        #region --PROPS--

        #endregion

        #region --CTOR--
        public ClientObject(TcpClient tcpClient,ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
            
        }
        #endregion

        #region --METHODS--
        public void Process()
        {
            try
            {
                Stream = client.GetStream();

                //username
                string message = GetMessage();
                userName = message;
                message = $"{userName} вошел в чат";
                Log(message);
                server.BroadCastMessage(message, this.Id);

                //message recieving cycle
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = $"<{userName}>: {message}";
                        Log(message);
                        server.BroadCastMessage(message, this.Id);
                        
                    }
                    catch (Exception exc)
                    {
                        message = $"{userName} покинул чат";
                        Log(message);
                        Log($"ERROR: {exc.Message}");
                        server.BroadCastMessage(message, this.Id);
                        break;
                    }
                }


            }
            catch (Exception exc)
            {
                Log($"ERROR: {exc.Message}");
            }
            finally
            {
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        protected internal void Close()
        {
            if (Stream != null) Stream.Close();
            if (client != null) client.Close();
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[DEFAULT_BUFFER_SIZE];
            //string message = String.Empty;
            StringBuilder message = new StringBuilder();
            int bytesCount = 0;
            do
            {
                bytesCount = Stream.Read(data, 0, data.Length);
                //message += byteConverter.GetString(data, 0, bytesCount);
                message.Append(byteConverter.GetString(data, 0, bytesCount));
            } while (Stream.DataAvailable);

            return message.ToString();
            
        }
        #endregion
    }
}
