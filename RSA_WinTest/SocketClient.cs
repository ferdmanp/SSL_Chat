using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace RSA_WinTest
{
    public class SocketClient
    {
        #region --CONST--        
        const int DEFAULT_CONNECTION_PORT = 9999;
        const int DEFAULT_MESSAGE_BUFFER_SIZE=1024;

        #endregion

        #region --VARS--
        IPAddress localIp;
        IPAddress serverIp;
        int port;
        Socket sHandler;
        private Encoding defaultByteConverter = new UTF8Encoding();

        private LogMethod Logger;
        
        #endregion

        #region --PROPS--
        public bool IsConnected {
            get {
                if (sHandler == null) return false;
                return sHandler.Connected;
            }
        }
        #endregion

        #region --CTOR--
        public SocketClient():this(String.Empty,DEFAULT_CONNECTION_PORT){}

        public SocketClient(int port):this(String.Empty,port){}

        public SocketClient(string ipAddress) : this(ipAddress, DEFAULT_CONNECTION_PORT) { }
       
        public SocketClient(string serverIpAddress, int port)
        {

            IPHostEntry ipHost = Dns.GetHostEntry(Environment.MachineName);
            
            foreach (var ip in ipHost.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.localIp = ip;
                }
                //else
                //{
                //    throw new ArgumentException("Cannot define local IP address!");
                //}
            }

            if (String.IsNullOrEmpty(serverIpAddress))
            {
                this.serverIp = this.localIp;
            }
            else if(!IPAddress.TryParse(serverIpAddress, out this.serverIp))
            {
                throw new ArgumentException("Bad IPv4 Address!");
            }

            if (port > 0 && port < 65535)
            {
                this.port = port;
            }
            else
            {
                throw new ArgumentException("Bad port number!");
            }

        }
        #endregion

        #region --METHODS--

        private void connect()
        {
            try
            {
                Log("Starting connection");
                sHandler = new Socket(serverIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(serverIp, port);
                sHandler.Connect(endPoint);
                Log($"Connected to {endPoint.ToString()}");
            }
            catch (Exception exc)
            {
                LogError(exc.Message);
                //throw;
            }
            
        }

        private void close()
        {
            if (sHandler.Connected)
            {
                sHandler.Close();
            }
            Log("Connection closed");
        }

        private void checkConnection()
        {
            if (!IsConnected) this.connect();
        }

        public void SendMessage(string message, Encoding encoding)
        {
            try
            {
                if (String.IsNullOrEmpty(message)) return;
                checkConnection();
                byte[] _byteMessage = encoding.GetBytes(message);
                sHandler.Send(_byteMessage);
                Log($"Message sent {message}");
            }
            catch (Exception exc)
            {
                LogError(exc.Message);
                //throw;
            }

        }

        

        public void SendMessage(string message)
        {
            SendMessage(message, defaultByteConverter);
        }


        public string RecieveMessage(Encoding encoding, int bufferSize= DEFAULT_MESSAGE_BUFFER_SIZE)
        {
            checkConnection();
            string result = String.Empty;

            byte[] msg_buffer = new byte[bufferSize];
            int bytesRecieved= sHandler.Receive(msg_buffer);
            result = encoding.GetString(msg_buffer, 0, bytesRecieved);
            Log($"Message from server: {result}");
            return result;
        }

        public string RecieveMessage(int bufferSize = DEFAULT_MESSAGE_BUFFER_SIZE)
        {
            return RecieveMessage(defaultByteConverter,bufferSize);
        }

        public string ProcessMessages(Encoding encoding)
        {
            string message = String.Empty;

            do
            {
                message += RecieveMessage(encoding);
            } while (message.IndexOf(ProtocolDescription.EndOfMessage) == -1);

            return message;
            
        }

        public string ProcessMessages()
        {
            return ProcessMessages(defaultByteConverter);
        }


        #endregion

        #region --LOGGING--

        public void RegisterLogger(LogMethod logger)
        {
            this.Logger += logger;
        }

        public void Log(string Message)
        {
            this.Logger(Message);
        }

        public void LogError(string Message)
        {
            this.Logger($"ERROR: {Message}");
        }

        #endregion
    }
}
