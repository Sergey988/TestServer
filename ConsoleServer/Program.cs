using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    class Program
    {
        static TcpListener listener;

        static void Main(string[] args)
        {
            UsersList usersList = new UsersList();

            int serverPort = Int32.Parse(args[0]);
            Console.WriteLine($"You are using server port: {serverPort}");

            try
            {
                listener = new TcpListener(IPAddress.Parse("0.0.0.0"), serverPort);
                listener.Start();
                Console.WriteLine("Waiting for connections...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine(client.Client.RemoteEndPoint);
                    ClientObject clientObject = new ClientObject(client);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));

                    UsersList.users.Add(clientObject);


                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
    }




}