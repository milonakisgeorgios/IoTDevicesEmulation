namespace Test01
{
    internal class Program
    {

        static void PrintMenu(IoTClient client)
        {
            Console.WriteLine();
            Console.WriteLine("\t\t*******************************************");
            Console.WriteLine("\t\t*        Q -> Quit");
            if(client.IsConnected)
            {
                Console.WriteLine("\t\t*        D -> Drop");
            }
            else
            {
                Console.WriteLine("\t\t*        C -> Connect");
            }

            if(client.IsConnected)
            {
                Console.WriteLine("\t\t*        1 -> Send1");
                Console.WriteLine("\t\t*        2 -> Send2");
                Console.WriteLine("\t\t*        3 -> Send3a");
                Console.WriteLine("\t\t*        e -> Send3b");
                Console.WriteLine("\t\t*        4 -> Send4");
            }
            Console.WriteLine("\t\t*******************************************");
        }
        static void Main(string[] args)
        {
            var quitEvent = new ManualResetEvent(false);
            var client = new IoTClient("192.168.1.6", 5122, quitEvent);

            Task.Factory.StartNew(() => {

                while (true)
                {
                    PrintMenu(client);
                    var input = Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant();

                    if (input == "Q")
                    {
                        Console.WriteLine("\nQuit");
                        if(client.IsConnected)
                        {
                            client.Stop();
                        }
                        quitEvent.Set();
                        Thread.Sleep(500);
                    }
                    else if (input == "C")
                    {
                        Console.WriteLine("\tConnect");
                        if(client.IsConnected == false)
                        {
                            client.Connect();
                        }
                    }
                    else if (input == "D")
                    {
                        Console.WriteLine("Disconnect");
                        if(client.IsConnected == true)
                        {
                            client.Stop();
                        }
                    }

                    else if (input == "1")
                    {
                        if(client.IsConnected)
                        {
                            Console.WriteLine("\tSend1");
                            client.Send1();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "2")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend2");
                            client.Send2();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "3")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend3a");
                            client.Send3a();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "e")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend3b");
                            client.Send3b();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "4")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend4");
                            client.Send4();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                }

            });


            quitEvent.WaitOne(-1);
        }
    }
}
