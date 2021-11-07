using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        static List<Socket> connectedClients = new List<Socket>();
        static void Main(string[] args)
        {
            Console.Title = "-= SERVER =-";
            const int PORT = 55_555;
            const string HOST = "127.0.0.1";

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(HOST), PORT);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(endPoint);
                serverSocket.Listen(5);

                Console.WriteLine("Success started!");
                Console.WriteLine("Wait clients...");

                while(true)
                {
                    // ожидание нового клиента
                    Socket socket = serverSocket.Accept();
                    Console.WriteLine($"Client has been connected - {socket.RemoteEndPoint}");
                    connectedClients.Add(socket);

                    // запуск прослушки в отдельном потоке для нового клиента
                    ListenConnectedSocket(socket);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.WriteLine("Hello World!");
        }

        static void ListenConnectedSocket(Socket socket)
        {
            Task.Run(() =>
            {
                Console.WriteLine($"Start listening for the socket - {socket.RemoteEndPoint}");
                while(true)
                {
                    byte[] msgBuff = ReceiveMessage(socket);
                    string message = ConvertBytesToString(msgBuff);

                    Console.WriteLine($"Server, receive message - {message}");

                    // Retrasmission
                    RetransmissionMessage(msgBuff);
                }
            });
        }

        static void RetransmissionMessage(byte[] data)
        {
            connectedClients.ForEach(s =>
            {
                if (s.Connected)
                {
                    s.Send(data);
                }
            });
        }

        static byte[] ReceiveMessage(Socket socket)
        {
            const int BUFF_SIZE = 100;
            byte[] buff = new byte[BUFF_SIZE];

            socket.Receive(buff);

            return buff;
        }

        static string ConvertBytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
    }
}
