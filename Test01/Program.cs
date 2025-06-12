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
            Console.WriteLine("\t\t*        S -> Sewnd");
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
                    else if (input == "S")
                    {
                        Console.WriteLine("\tSend");
                        client.Send();
                    }
                }

            });


            quitEvent.WaitOne(-1);
        }
    }
}
