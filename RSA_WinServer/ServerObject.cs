using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSA_WinServer
{
    public class ServerObject
    {
        #region --VARS--
        static TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>();
        LogMethod Log = Console.WriteLine;
        Encoding byteConverter = new UTF8Encoding();
        #endregion

        #region --PROPS--

        #endregion

        #region --CTOR--

        #endregion

        #region --METHODS--
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        protected internal void RemoveConnection(string id)
        {
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
            }
        }

        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(getMyIp()??IPAddress.Any, 9999);
                tcpListener.Start();
                Log($"Server started! Address: {tcpListener.LocalEndpoint.ToString()}  Waiting for connection");


                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();

                }
            }
            catch (Exception exc)
            {
                Log($"ERROR: {exc.Message}");
                Disconnect();
            }
        }

       

        internal void BroadCastMessage(string message, string id)
        {
            byte[] data = byteConverter.GetBytes(message);
            foreach (var client in clients)
            {
                if (client.Id != id)
                {
                    client.Stream.Write(data, 0, data.Length);
                }
            }
        }

        protected internal void Disconnect()
        {
            tcpListener.Stop();
            clients.ForEach(p => p.Close());
            Environment.Exit(0);
        }

        private IPAddress getMyIp()
        {
            IPHostEntry entry = Dns.GetHostEntry(Environment.MachineName);
            return entry.AddressList.FirstOrDefault(s => s.AddressFamily == AddressFamily.InterNetwork);

        }

        #endregion

    }
}
