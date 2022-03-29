using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    internal class Server
    {
        private static TcpListener tcpListener;

        public static void StartServer()
        {
            Console.WriteLine("Starting Server...");

            tcpListener = new TcpListener(IPAddress.Any, 3939);
            tcpListener.Start();
            Console.WriteLine("Waiting for new Clients...");
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        }

        private static void TCPConnectCallback(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            Console.WriteLine("Accepted new Client");
            Clients client = new Clients(tcpClient);

            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        }

    }
}
