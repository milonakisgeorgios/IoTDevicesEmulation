using System.Diagnostics;

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
                Console.WriteLine("\t\t*        1 -> Send1a (One packet)");
                Console.WriteLine("\t\t*        q -> Send1b (Two packets together)");
                Console.WriteLine("\t\t*        a -> Send1c (Three packets together)");
                Console.WriteLine("\t\t*        z -> Send1d (Four packets together)");
                Console.WriteLine("\t\t*        2 -> Send2a (First)");
                Console.WriteLine("\t\t*        w -> Send2b (Second Half)");
                Console.WriteLine("\t\t*        3 -> Send3a (First)");
                Console.WriteLine("\t\t*        e -> Send3b (Second)");
                Console.WriteLine("\t\t*        4 -> Send4a (in parts)");
                Console.WriteLine("\t\t*        5 -> Send5a (BAD CRC)");
                Console.WriteLine("\t\t*        t -> Send5b (BAD CONTENT)");
                Console.WriteLine("\t\t*        g -> Send5c (BAD DELIMITER)");
                Console.WriteLine("\t\t*        b -> Send5d (BUFFER OVERFLOW)");

                Console.WriteLine("\t\t*        6 -> del1");
                Console.WriteLine("\t\t*        7 -> del2");
                Console.WriteLine("\t\t*        8 -> del4");
                Console.WriteLine("\t\t*        9 -> del5");
                Console.WriteLine("\t\t*        0 -> del7");
                Console.WriteLine("\t\t*        y -> del8");
                Console.WriteLine("\t\t*        u -> del9");
                Console.WriteLine("\t\t*        i -> del10a");
                Console.WriteLine("\t\t*        o -> del10b");
                Console.WriteLine("\t\t*        p -> del10c");
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
                            Console.WriteLine("\nSend1a");
                            client.Send1a();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "Q")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\nSend1b");
                            client.Send1b();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "A")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\nSend1c");
                            client.Send1c();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "Z")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\nSend1d");
                            client.Send1d();
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
                            Console.WriteLine("\tSend2a");
                            client.Send2a();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "W")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend2b");
                            client.Send2b();
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
                    else if (input == "e" || input == "E")
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
                            Console.WriteLine("\tSend4a");
                            client.Send4a();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }


                    else if (input == "5")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend5a");
                            client.Send5a();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "T")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend5b");
                            client.Send5b();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "G")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\tSend5c");
                            client.Send5c();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "B")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\nSend5d");
                            client.Send5d();
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }


                    else if (input == "6")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel1");
                            client.del(1);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "7")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel2");
                            client.del(2);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "8")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel4");
                            client.del(4);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "9")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel5");
                            client.del(5);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "0")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel7");
                            client.del(7);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "Y")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel8");
                            client.del(8);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "U")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel9");
                            client.del(9);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }


                    else if (input == "I")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel10a");
                            client.del(30);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "O")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel10b");
                            client.del(31);
                        }
                        else
                        {
                            Console.WriteLine("NOT_CONNECTED");
                        }
                    }
                    else if (input == "P")
                    {
                        if (client.IsConnected)
                        {
                            Console.WriteLine("\ndel10c");
                            client.del(32);
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

        static void wait()
        {
            var stopwatch = Stopwatch.StartNew();
            var spinner = new SpinWait();

            while (stopwatch.ElapsedMilliseconds < 5)
            {
                spinner.SpinOnce();
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


            for (int i = 1; i <= 400; i++)
            {
                try
                {
                    client1.Connect();
                    Thread.Sleep(100);
                    client2.Connect();
                    Thread.Sleep(100);

                    client1.Stop();
                    client3.Connect();
                    Thread.Sleep(100);

                    client2.Stop();
                    client4.Connect();
                    Thread.Sleep(100);

                    client3.Stop();
                    client5.Connect();
                    Thread.Sleep(100);


                    client4.Stop();
                    client6.Connect();
                    Thread.Sleep(100);

                    client5.Stop();
                    client7.Connect();
                    Thread.Sleep(100);


                    client6.Stop();
                    client8.Connect();
                    Thread.Sleep(100);


                    client7.Stop();
                    client9.Connect();
                    Thread.Sleep(100);

                    client8.Stop();
                    client10.Connect();
                    Thread.Sleep(100);


                    client9.Stop();
                    Thread.Sleep(100);
                    client10.Stop();
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
