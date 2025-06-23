namespace Test01
{
    internal class Program
    {

        static void PrintMenu(IoTClient client)
        {
            Console.WriteLine();
            Console.WriteLine("\t\t*******************************************");
            Console.WriteLine("\t\t*        Q -> Quit");
            Console.WriteLine("\t\t*        L -> Scenario1");
            Console.WriteLine("\t\t*        K -> Scenario2");
            if (client.IsConnected)
            {
                Console.WriteLine("\t\t*        R -> Reconnect");
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
            string _IP = "192.168.1.6";
            int _Port = 5122;

            var quitEvent = new ManualResetEvent(false);
            var client = new IoTClient(_IP, _Port, quitEvent);

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
                    else if (input == "R")
                    {
                        Console.WriteLine("Reconnect");
                        var client2 = new IoTClient(_IP, _Port, quitEvent);
                        client2.Connect();

                        client = client2;
                    }
                    else if (input == "L")
                    {
                        Console.WriteLine("Scenario1");

                        scenario1(_IP, _Port);
                    }
                    else if (input == "K")
                    {
                        Console.WriteLine("Scenario2");

                        scenario2(_IP, _Port);
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



        static void scenario1(string _IP, int _Port)
        {
            for(int i=1; i<=80; i++)
            {
                try
                {
                    var quitEvent = new ManualResetEvent(false);


                    var client1 = new IoTClient(_IP, _Port, quitEvent);
                    client1.Connect();

                    Thread.Sleep(60);
                    var client2 = new IoTClient(_IP, _Port, quitEvent);
                    client2.Connect();


                    Thread.Sleep(80);
                    var client3 = new IoTClient(_IP, _Port, quitEvent);
                    client3.Connect();

                    Thread.Sleep(80);
                    var client4 = new IoTClient(_IP, _Port, quitEvent);
                    client4.Connect();

                    Thread.Sleep(100);
                    var client5 = new IoTClient(_IP, _Port, quitEvent);
                    client5.Connect();

                    Thread.Sleep(200);
                    client1.Stop();
                    client2.Stop();
                    client3.Stop();
                    client4.Stop();
                    client5.Stop();
                }
                catch(Exception ex)
                {

                }
            }
        }



        static void scenario2(string _IP, int _Port)
        {
            var quitEvent = new ManualResetEvent(false);
            var client1 = new IoTClient(_IP, _Port, quitEvent);
            var client2 = new IoTClient(_IP, _Port, quitEvent);
            var client3 = new IoTClient(_IP, _Port, quitEvent);
            var client4 = new IoTClient(_IP, _Port, quitEvent);
            var client5 = new IoTClient(_IP, _Port, quitEvent);
            var client6 = new IoTClient(_IP, _Port, quitEvent);
            var client7 = new IoTClient(_IP, _Port, quitEvent);
            var client8 = new IoTClient(_IP, _Port, quitEvent);
            var client9 = new IoTClient(_IP, _Port, quitEvent);
            var client10 = new IoTClient(_IP, _Port, quitEvent);


            for (int i = 1; i <= 260; i++)
            {
                try
                {
                    client1.Connect();
                    Thread.Sleep(50);
                    client2.Connect();
                    Thread.Sleep(50);

                    client1.Stop();
                    client3.Connect();
                    Thread.Sleep(50);

                    client2.Stop();
                    client4.Connect();
                    Thread.Sleep(50);

                    client3.Stop();
                    client5.Connect();
                    Thread.Sleep(50);


                    client4.Stop();
                    client6.Connect();
                    Thread.Sleep(50);

                    client5.Stop();
                    client7.Connect();
                    Thread.Sleep(50);


                    client6.Stop();
                    client8.Connect();
                    Thread.Sleep(50);


                    client7.Stop();
                    client9.Connect();
                    Thread.Sleep(50);

                    client8.Stop();
                    client10.Connect();
                    Thread.Sleep(50);


                    client9.Stop();
                    Thread.Sleep(50);
                    client10.Stop();
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
