using ModelsLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static Socket clientSocket;
        static string username = "vas_2.0";

        static void Main(string[] args)
        {
            Console.Title = "-= CLIENT =-";
            const int PORT = 55_555;
            const string HOST = "127.0.0.1";

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(HOST), PORT);

            try
            {
                clientSocket.Connect(endPoint);
                Console.WriteLine("Success connected");
                // запуск прослушки сообщений от сервера
                StartListening();

                while (true)
                {
                    Console.Write("Enter your message: ");
                    string msg = Console.ReadLine();

                    SendMessage(msg);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.WriteLine("Hello World!");
        }

        static void SendMessage(string message)
        {
            /*SimpleMessage msg = new SimpleMessage
            {
                Text = message,
                From = username,
                RemoteIP = clientSocket.RemoteEndPoint.ToString()
            };

            string json = JsonConvert.SerializeObject(msg);

            clientSocket.Send(ConvertStringToBytes(json));*/
            clientSocket.Send(ConvertStringToBytes(message));
        }

        static void StartListening()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte[] buff = ReceiveMessage();
                    string message = ConvertBytesToString(buff);

                    Console.WriteLine($"Receive message from the server - {message}");
                }
            });
        }

        static string ConvertBytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        static byte[] ReceiveMessage()
        {
            const int BUFF_SIZE = 100;
            byte[] buff = new byte[BUFF_SIZE];

            clientSocket.Receive(buff);

            return buff;
        }

        static byte[] ConvertStringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
