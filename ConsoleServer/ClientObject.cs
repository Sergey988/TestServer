using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer
{

    public class UsersList
    {
        public static List<ClientObject> users = new List<ClientObject>();
    }

    public class ClientObject
    {
        public TcpClient client;
        int sumNumbers = 0;
        string commandList = "list";
        string ipUser = "";

        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
            ipUser = tcpClient.Client.RemoteEndPoint.ToString();
        }

        public void Process()
        {
            int number;
            bool isNumerical;
            string message = "";
            NetworkStream stream = null;

            try
            {
                byte[] data = new byte[64];
                stream = client.GetStream();

                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }

                    while (stream.DataAvailable);
                    
                        isNumerical = int.TryParse(builder.ToString(), out number);
                        if (isNumerical)
                        {
                            number = int.Parse(builder.ToString());
                            sumNumbers += number;
                            message = "The sum of all numbers entered during the session: " + sumNumbers.ToString() + "\n";
                        }
                        else if ((String.Compare(builder.ToString(), commandList)) == 0)
                        {
                            message = "";
                            foreach (ClientObject item in UsersList.users)
                            {
                                message += "User IP: " + item.ipUser + " sum: " + item.sumNumbers + "\n";
                            }
                        }
                        else
                        {
                            message = "Error. Enter a number or one of the commands.";
                        }

                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();

                int indexUser = 0;
                foreach (ClientObject item in UsersList.users)
                {
                    indexUser = UsersList.users.IndexOf(item);
                }
                UsersList.users.RemoveAt(indexUser);
            }
        }
    }



}