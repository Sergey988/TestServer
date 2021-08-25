using System;
using System.Net.Sockets;
using System.Text;

namespace ConsoleClient
{
    class Program
    {
        const string address = "127.0.0.1";
        static void Main(string[] args)
        {

            int clientPort = Int32.Parse(args[0]);
            Console.WriteLine($"Welcome: {clientPort}");

            TcpClient client = null;
            try
            {
                client = new TcpClient(address, clientPort);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    string message = Console.ReadLine();
                    message = String.Format(message);
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    message = builder.ToString();
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}