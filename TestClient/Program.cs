using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerConnector con1 = new ServerConnector("yourip.de", 3939); // Your Server IP and Port
            while (true)
            {
                Console.WriteLine("What do you want to send?");
                string str = Console.ReadLine();
                con1.SendData(Encoding.UTF8.GetBytes(str));
            }
            
        }

    }
}
