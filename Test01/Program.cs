namespace Test01
{
    internal class Program
    {

        static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("\t\t*******************************************");
            Console.WriteLine("\t\t*        Q -> Quit");
            Console.WriteLine("\t\t*        C -> Connect");
            Console.WriteLine("\t\t*        1 -> Send1");
            Console.WriteLine("\t\t*        2 -> Send2");
            Console.WriteLine("\t\t*        3 -> Send3a");
            Console.WriteLine("\t\t*        e -> Send3b");
            Console.WriteLine("\t\t*        4 -> Send4");
            Console.WriteLine("\t\t*        D -> Drop");
            Console.WriteLine("\t\t*******************************************");
        }
        static void Main(string[] args)
        {
            var quitEvent = new ManualResetEvent(false);
            var client = new IoTClient("192.168.1.6", 5122, quitEvent);

            Task.Factory.StartNew(() => {

                while (true)
                {
                    PrintMenu();
                    var input = Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant();

                    if (input == "Q")
                    {
                        Console.WriteLine("\nQuit");
                        client.Quit();
                        quitEvent.Set();
                        Thread.Sleep(500);
                    }
                    else if (input == "C")
                    {
                        Console.WriteLine("\tConnect");
                        client.Connect();
                    }
                    else if (input == "D")
                    {
                        Console.WriteLine("\tDrop");
                        client.Drop();
                    }
                    else if (input == "1")
                    {
                        Console.WriteLine("\tSend1");
                        client.Send1();
                    }
                    else if (input == "2")
                    {
                        Console.WriteLine("\tSend2");
                        client.Send2();
                    }
                    else if (input == "3")
                    {
                        Console.WriteLine("\tSend3a");
                        client.Send3a();
                    }
                    else if (input == "e")
                    {
                        Console.WriteLine("\tSend3b");
                        client.Send3b();
                    }
                    else if (input == "4")
                    {
                        Console.WriteLine("\tSend4");
                        client.Send4();
                    }
                }

            });


            quitEvent.WaitOne(-1);
        }
    }
}
