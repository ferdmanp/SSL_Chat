using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace RSA_WinServer
{
    class Program
    {
        public  delegate void LogMethod(string message);

        const int MAX_CONNECTION_QUEUE_LENGTH = 10;
        const int CONNECTION_PORT = 9999;
        const int BUFFER_SIZE = 1024;

        static Encoding ByteConverter = new UTF8Encoding();
        

        static void Main(string[] args)
        {
            LogMethod log = Console.WriteLine;
            List<ClientConnection> _connections = new List<ClientConnection>();
            

            IPHostEntry host = Dns.GetHostEntry(Environment.MachineName);
            IPAddress serverIp = host.AddressList[0];

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    serverIp = ip;
            }

            IPEndPoint serverEndPoint = new IPEndPoint(serverIp, CONNECTION_PORT);

            Socket sListener = new Socket(serverIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                log("Init socket");
                sListener.Bind(serverEndPoint);
                sListener.Listen(MAX_CONNECTION_QUEUE_LENGTH);

                while (true)
                {
                    log($"Waiting for connection on port {serverEndPoint.ToString()}");
                    Socket handler = sListener.Accept();

                    log($"New connection from {((IPEndPoint)handler.RemoteEndPoint).Address}!");

                    _connections.Add(new ClientConnection
                            {
                                clientAddress = ((IPEndPoint)handler.RemoteEndPoint).Address
                            }
                        );

                    byte[] data = new byte[BUFFER_SIZE];
                    string strData = String.Empty;

                    int dataCount = handler.Receive(data, BUFFER_SIZE, SocketFlags.None);
                    strData += ByteConverter.GetString(data, 0, dataCount);

                    log($"Recieved data: {strData}{Environment.NewLine}");

                    string strReply = $"Ответ: ваше сообщение: {strData}<END>";
                    byte[] reply = ByteConverter.GetBytes(strReply);
                    handler.Send(reply);

                    if (strData.IndexOf("<END>") > -1)
                    {
                        log("Server closed connection to client");
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        break;
                    }


                    

                }

            }
            catch (Exception exc)
            {
                log(exc.Message);
            }
            finally
            {
                Console.ReadKey();
            }

        }
    }
}
