﻿using System;
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

                while(true)
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
            clientSocket.Send(ConvertStringToBytes(message));
        }

        static void StartListening()
        {
            Task.Run(() =>
            {
                while(true)
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

            int countReceiveBuffs = 0;
            int countReceiveBytes = 0;
            int totalReceiveBytes = 0;
            List<byte> totalBytes = new List<byte>();
            do
            {
                byte[] buff = new byte[BUFF_SIZE];
                countReceiveBytes = clientSocket.Receive(buff);
                totalReceiveBytes += countReceiveBytes;
                totalBytes.AddRange(buff);

                countReceiveBuffs++;
            } while (countReceiveBytes > 0);

            totalBytes.RemoveRange(totalReceiveBytes, BUFF_SIZE * countReceiveBuffs - totalReceiveBytes);
            return totalBytes.ToArray();
        }

        static byte[] ConvertStringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}