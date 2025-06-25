using System.Diagnostics;

namespace Test01
{
    internal class Program
    {

        static void PrintMenu(IoTClient client)
        {
            Console.WriteLine();
            Console.WriteLine("\t\t*******************************************");
            Console.WriteLine("\t\t*        . -> Quit");
            Console.WriteLine("\t\t*        s1 -> Scenario1");
            Console.WriteLine("\t\t*        s2 -> Scenario2");
            if (client.IsConnected)
            {
                Console.WriteLine("\t\t*        R -> Reconnect");
                Console.WriteLine("\t\t*        / -> Drop");
            }
            else
            {
                Console.WriteLine("\t\t*        C -> Connect");
            }

            if(client.IsConnected)
            {
                Console.WriteLine("\t\t*        11 -> Send1a (One packet)");
                Console.WriteLine("\t\t*        12 -> Send1b (Two packets together)");
                Console.WriteLine("\t\t*        13 -> Send1c (Three packets together)");
                Console.WriteLine("\t\t*        14 -> Send1d (Four packets together)");
                Console.WriteLine("\t\t*        15 -> Send1e (Four packets together)");
                Console.WriteLine("\t\t*        21 -> Send2a (First)");
                Console.WriteLine("\t\t*        22 -> Send2b (Second Half)");
                Console.WriteLine("\t\t*        31 -> Send3a (First)");
                Console.WriteLine("\t\t*        32 -> Send3b (Second)");
                Console.WriteLine("\t\t*        41 -> Send4a (in parts)");
                Console.WriteLine("\t\t*        42 -> Send4b (0x0A with 1 repeated blocks)");
                Console.WriteLine("\t\t*        43 -> Send4c (0x0A with 2 repeated blocks)");
                Console.WriteLine("\t\t*        44 -> Send4d (0x0A with 3 repeated blocks)");
                Console.WriteLine("\t\t*        51 -> Send5a (BAD CRC)");
                Console.WriteLine("\t\t*        52 -> Send5b (BAD CONTENT)");
                Console.WriteLine("\t\t*        53 -> Send5c (BAD DELIMITER)");
                Console.WriteLine("\t\t*        54 -> Send5d (BUFFER OVERFLOW)");
                Console.WriteLine("\t\t*        55 -> Send5e (BUFFER OVERFLOW)");

                Console.WriteLine("\t\t*        d1 -> del1");
                Console.WriteLine("\t\t*        d2 -> del2");
                Console.WriteLine("\t\t*        d3 -> del3");
                Console.WriteLine("\t\t*        d4 -> del4");
                Console.WriteLine("\t\t*        d5 -> del5");
                Console.WriteLine("\t\t*        d6 -> del6");
                Console.WriteLine("\t\t*        d7 -> del7");
                Console.WriteLine("\t\t*        d8 -> del8");
                Console.WriteLine("\t\t*        d9 -> del9");
                Console.WriteLine("\t\t*        f1 -> del10a");
                Console.WriteLine("\t\t*        f2 -> del10b");
                Console.WriteLine("\t\t*        f3 -> del10c");
                Console.WriteLine("\t\t*        f4 -> del11");
                Console.WriteLine("\t\t*        f5 -> del12");
                Console.WriteLine("\t\t*        f6 -> del13");
                Console.WriteLine("\t\t*        f7 -> del14");
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
                    var input1 = Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant();
                    Console.Write(input1);
                    var input2 = input1;

                    if(input1 != "." && input1 != "C" && input1 != "/" && input1 != "R")
                    {
                        input2 = Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant();
                        Console.Write(input2);
                    }

                    if (input1 == ".")
                    {
                        Console.WriteLine("\tQuit");
                        if(client.IsConnected)
                        {
                            client.Stop();
                        }
                        quitEvent.Set();
                        Thread.Sleep(500);
                    }
                    else if (input1 == "C")
                    {
                        Console.WriteLine("\tConnect");
                        if(client.IsConnected == false)
                        {
                            client.Connect();
                        }
                    }
                    else if (input1 == "/")
                    {
                        Console.WriteLine("\tDisconnect");
                        if(client.IsConnected == true)
                        {
                            client.Stop();
                        }
                    }
                    else if (input1 == "R")
                    {
                        Console.WriteLine("\t Reconnect");
                        var client2 = new IoTClient(_IP, _Port, quitEvent);
                        client2.Connect();

                        client = client2;
                    }
                    else if (input1 == "S")
                    {
                        if (input2 == "1")
                        {
                            Console.WriteLine("\tScenario1");
                            scenario1(_IP, _Port);
                        }
                        else if (input2 == "2")
                        {
                            Console.WriteLine("\tScenario2");
                            scenario2(_IP, _Port);
                        }       
                    }
                    else if (input1 == "1")
                    {
                        if(client.IsConnected == false)
                        {
                            Console.WriteLine("\tNOT_CONNECTED");
                        }
                        else
                        {
                            if(input2=="1")
                            {
                                Console.WriteLine("\tSend1a");
                                client.Send1a();
                            }
                            else if (input2 == "2")
                            {
                                Console.WriteLine("\tSend1b");
                                client.Send1b();
                            }
                            else if (input2 == "3")
                            {
                                Console.WriteLine("\tSend1c");
                                client.Send1c();
                            }
                            else if (input2 == "4")
                            {
                                Console.WriteLine("\tSend1d");
                                client.Send1d();
                            }
                            else if (input2 == "5")
                            {
                                Console.WriteLine("\tSend1e");
                                client.Send1e();
                            }
                        }
                    }
                    else if (input1 == "2")
                    {
                        if (client.IsConnected == false)
                        {
                            Console.WriteLine("\tNOT_CONNECTED");
                        }
                        else
                        {
                            if (input2 == "1")
                            {
                                Console.WriteLine("\tSend2a");
                                client.Send2a();
                            }
                            else if (input2 == "2")
                            {
                                Console.WriteLine("\tSend2b");
                                client.Send2b();
                            }
                        }
                    }
                    else if (input1 == "3")
                    {
                        if (client.IsConnected == false)
                        {
                            Console.WriteLine("\tNOT_CONNECTED");
                        }
                        else
                        {
                            if (input2 == "1")
                            {
                                Console.WriteLine("\tSend3a");
                                client.Send3a();
                            }
                            else if (input2 == "2")
                            {
                                Console.WriteLine("\tSend3b");
                                client.Send3b();
                            }
                        }
                    }
                    else if (input1 == "4")
                    {
                        if (client.IsConnected == false)
                        {
                            Console.WriteLine("\tNOT_CONNECTED");
                        }
                        else
                        {
                            if (input2 == "1")
                            {
                                Console.WriteLine("\tSend4a");
                                client.Send4a();
                            }
                            else if (input2 == "2")
                            {
                                Console.WriteLine("\tSend4b");
                                client.Send4b();
                            }
                            else if (input2 == "3")
                            {
                                Console.WriteLine("\tSend4c");
                                client.Send4c();
                            }
                            else if (input2 == "4")
                            {
                                Console.WriteLine("\tSend4d");
                                client.Send4d();
                            }
                        }
                    }


                    else if (input1 == "5")
                    {
                        if (client.IsConnected == false)
                        {
                            Console.WriteLine("\tNOT_CONNECTED");
                        }
                        else
                        {
                            if (input2 == "1")
                            {
                                Console.WriteLine("\tSend5a");
                                client.Send5a();
                            }
                            else if (input2 == "2")
                            {
                                Console.WriteLine("\tSend5b");
                                client.Send5b();
                            }
                            else if (input2 == "3")
                            {
                                Console.WriteLine("\tSend5c");
                                client.Send5c();
                            }
                            else if (input2 == "4")
                            {
                                Console.WriteLine("\tSend5d");
                                client.Send5d();
                            }
                            else if (input2 == "5")
                            {
                                Console.WriteLine("\tSend5e");
                                client.Send5e();
                            }
                        }
                    }
                    else if (input1 == "D")
                    {
                        if (client.IsConnected == false)
                        {
                            Console.WriteLine("\tNOT_CONNECTED");
                        }
                        else
                        {
                            if (input2 == "1")
                            {
                                Console.WriteLine("\tDelim 1");
                                client.del(1);
                            }
                            else if (input2 == "2")
                            {
                                Console.WriteLine("\tDelim 2");
                                client.del(2);
                            }
                            else if (input2 == "3")
                            {
                                Console.WriteLine("\tDelim 3");
                                client.del(3);
                            }
                            else if (input2 == "4")
                            {
                                Console.WriteLine("\tDelim 4");
                                client.del(4);
                            }
                            else if (input2 == "5")
                            {
                                Console.WriteLine("\tDelim 5");
                                client.del(5);
                            }
                            else if (input2 == "6")
                            {
                                Console.WriteLine("\tDelim 6");
                                client.del(6);
                            }
                            else if (input2 == "7")
                            {
                                Console.WriteLine("\tDelim 7");
                                client.del(7);
                            }
                            else if (input2 == "8")
                            {
                                Console.WriteLine("\tDelim 8");
                                client.del(8);
                            }
                            else if (input2 == "9")
                            {
                                Console.WriteLine("\tDelim 9");
                                client.del(9);
                            }
                        }
                    }
                    else if (input1 == "F")
                    {
                        if (client.IsConnected == false)
                        {
                            Console.WriteLine("\tNOT_CONNECTED");
                        }
                        else
                        {
                            if (input2 == "1")
                            {
                                Console.WriteLine("\tDelim 10a");
                                client.del(101);
                            }
                            else if (input2 == "2")
                            {
                                Console.WriteLine("\tDelim 10b");
                                client.del(102);
                            }
                            else if (input2 == "3")
                            {
                                Console.WriteLine("\tDelim 10c");
                                client.del(103);
                            }
                            else if (input2 == "4")
                            {
                                Console.WriteLine("\tDelim 11");
                                client.del(11);
                            }
                            else if (input2 == "5")
                            {
                                Console.WriteLine("\tDelim 12");
                                client.del(12);
                            }
                            else if (input2 == "6")
                            {
                                Console.WriteLine("\tDelim 13");
                                client.del(13);
                            }
                            else if (input2 == "7")
                            {
                                Console.WriteLine("\tDelim 14");
                                client.del(14);
                            }
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
