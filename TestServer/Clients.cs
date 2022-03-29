using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    internal class Clients
    {
        public static List<Clients> clientsList = new List<Clients>();

        private NetworkStream stream;

        private byte[] buffer;

        private byte[] dataRecived;

        private readonly int bufferSize = 1048576;

        public Clients (TcpClient tcpClient)
        {
            stream = tcpClient.GetStream();
            clientsList.Add(this);
            ReadData();
        }

        private void ReadData()
        {
            buffer = new byte[bufferSize];
            stream.BeginRead(buffer, 0, buffer.Length, ReadDataCallback, null);
        }

        private void ReadDataCallback(IAsyncResult result)
        {
            int dataSize = stream.EndRead(result);
            Console.WriteLine("New Data Recived");
            dataRecived = new byte[dataSize];
            Array.Copy(buffer, dataRecived, dataSize);
            DoSomeStuffWithData();

            stream.BeginRead(buffer, 0, buffer.Length, ReadDataCallback, null);
        }

        private void DoSomeStuffWithData()
        {
            Console.WriteLine(Encoding.UTF8.GetString(dataRecived));
        }

        public void SendData(byte[] data)
        {
            stream.BeginWrite(data, 0, data.Length, SendDataCallback, null);
        }

        private void SendDataCallback(IAsyncResult result)
        {
            stream.EndWrite(result);
        }
    }
}
