using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    internal class ServerConnector
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private readonly int bufferSize = 1048576;
        private byte[] buffer;
        private byte[] dataRecived;
        private string domain;
        private int port;

        public ServerConnector(string domain, int port)
        {
            this.domain = domain;
            this.port = port;
            Connect();
        }

        private async void Connect()
        {
            Console.WriteLine("Connecting to Server...");
            tcpClient = new TcpClient();
            int attempts = 1;
            while (!tcpClient.Connected)
            {
                await Task.Delay(1000);
                try
                {
                    await tcpClient.ConnectAsync(domain, port);
                    Console.WriteLine("Connected!");
                }
                catch (Exception)
                {
                    Console.WriteLine("Connection Attempt " + attempts);
                    attempts++;
                }
            }
            stream = tcpClient.GetStream();
            ReadData();
        }

        private void ReadData()
        {
            buffer = new byte[bufferSize];
            Console.WriteLine("Waiting for new Data");
            try
            {
                stream.BeginRead(buffer, 0, buffer.Length, ReadDataCallback, null);
            }
            catch (Exception)
            {
                Console.WriteLine("Lost Connection to Server. Reconnecting...");
                tcpClient.Close();
                Connect();
            }
        }

        private void ReadDataCallback(IAsyncResult result)
        {
            int dataSize = 1;
            try
            {
                dataSize = stream.EndRead(result);
            }
            catch (Exception)
            {
                // Blank
            }
            dataRecived = new byte[dataSize];
            Array.Copy(buffer, dataRecived, dataSize);
            Console.WriteLine("Got new Data");
            DoSomeStuffWithData();
            ReadData();
        }

        private void DoSomeStuffWithData()
        {
            Console.WriteLine(Encoding.UTF8.GetString(dataRecived));
        }

        public void SendData(byte[] data)
        {
            if (tcpClient.Connected)
                stream.BeginWrite(data, 0, data.Length, SendDataCallback, null);
            else
                Console.WriteLine("Not Connected to Server. Pls try again when connected.");
        }

        private void SendDataCallback(IAsyncResult result)
        {
            stream.EndWrite(result);
        }
    }
}
