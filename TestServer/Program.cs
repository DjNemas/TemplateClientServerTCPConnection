using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server.StartServer();
            while (true)
            {
                Console.WriteLine("What do you want to send?");
                string str = Console.ReadLine();
                if (Clients.clientsList[0] != null)
                {
                    Clients.clientsList[0].SendData(Encoding.UTF8.GetBytes(str));
                }
            }
        }
    }
}
