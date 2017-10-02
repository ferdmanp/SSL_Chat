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

        static void Main(string[] args)
        {
            LogMethod log = Console.WriteLine;
            List<ClientConnection> _connections = new List<ClientConnection>();
            

            IPHostEntry host = Dns.GetHostEntry(Environment.MachineName);
            IPAddress serverIp = host.AddressList[1];
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

                    

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

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
