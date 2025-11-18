using Emulator2.Emulators;

namespace Emulator2
{
    internal class FirmwareUpgradeEmulator : EmulatorBase
    {
        short _packetId = 1;
        IoTClient client = null!;
        private volatile bool _shouldStop = false;
        private readonly object _lock = new object();
        System.Random _random = new System.Random();


        string ServerIP => Globals.ServerConfiguration.RemoteIP;
        int ServerPort => Globals.ServerConfiguration.RemotePort;


        public FirmwareUpgradeEmulator()
        {

        }

        void PrintMenu()
        {
            Console.WriteLine("\t\t*****************Emulator1*****************");
            Console.WriteLine("\t\t*        N -> Send Nack");
            Console.WriteLine("\t\t*        A -> Send Ack");
            Console.WriteLine("\t\t*");
            Console.WriteLine("\t\t*        Q -> Disconect & Return");
            Console.WriteLine("\t\t********************************************");
        }


        public void Start()
        {
            // Συνδεομαστε στον Server
            client = new IoTClient(ServerIP, ServerPort, this.ProcessReceivedData, this.OnDisconnected);
            if (!client.Connect())
            {
                Console.WriteLine("Connection failed!");
                Console.WriteLine("Press a key to continue...");
                Console.ReadKey(true);  // true = intercept (key is not displayed)
                return;
            }

            PrintMenu();
            while (!_shouldStop)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).KeyChar;
                    char upper = char.ToUpperInvariant(key);

                    if (upper == 'Q')
                    {
                        StopClientAndExit();
                        break;
                    }
                    if (upper == 'N') Messages.SendNack(client);
                    if (upper == 'A') Messages.SendAck(client);

                    //Console.Clear();
                    PrintMenu();
                }

                Thread.Sleep(50);
            }

            Console.WriteLine("Press a key to continue...");
            Console.ReadKey(true);  // true = intercept (key is not displayed)
        }

        private void OnDisconnected()
        {
            Console.WriteLine("\n[Server disconnected!]");
            StopClientAndExit();
        }
        private void StopClientAndExit()
        {
            lock (_lock)
            {
                if (_shouldStop) return;
                _shouldStop = true;
            }

            client?.Stop();
        }



        bool firstPacketReceived = false;
        public void ProcessReceivedData(byte[] rcvBuffer, int numOfBytes)
        {
            string hexstring = System.Text.Encoding.ASCII.GetString(rcvBuffer.AsSpan(0, numOfBytes));


            var rcv_buffer = Hex2Byte(hexstring);
            byte firstByte = rcv_buffer.Length > 0 ? rcv_buffer[0] : (byte)0;
            byte secondByte = rcv_buffer.Length > 1 ? rcv_buffer[1] : (byte)0;
            byte thirdByte = rcv_buffer.Length > 2 ? rcv_buffer[2] : (byte)0;

            Console.WriteLine($"Received Length: {rcv_buffer.Length}");
            Console.WriteLine($"Received: {hexstring}");

            if (thirdByte == (byte)Commands.FirmwareUpgrade)
            {
                CommandHandlersFW13.handle120(client, rcv_buffer, rcv_buffer.Length, hexstring);

                Thread.Sleep(2000);
                Console.WriteLine("Send Initial NACK");
                Messages.SendInitiation(client);          //Initialization NACK
            }
            else if (rcv_buffer.Length == 133 && firstByte == 0x01)
            {
                if (firstPacketReceived == false)
                {
                    //START
                    Console.WriteLine("RECEIVED FILE HEADER");
                    firstPacketReceived = true;
                    Messages.SendAck(client);

                    Thread.Sleep(2000);
                    Console.WriteLine("Send START FILE NACK");
                    Messages.SendInitiation(client);
                }
                else
                {
                    //END
                    Console.WriteLine("Firmware Upgrade Finished!");
                    firstPacketReceived = false;
                    client.Stop();

                    //Κλεινουμε και φευγουμε απο το emulation
                    Thread.Sleep(500);
                    client = null!;
                    StopClientAndExit();
                }
            }
            else if (rcv_buffer.Length == 1 && firstByte == 4)
            {
                Console.WriteLine("RECEIVED X_EOT");
                Messages.SendAck(client);


                Thread.Sleep(2000);
                Console.WriteLine("Send Initial NACK");
                Messages.SendInitiation(client);          //Initialization NACK
            }
            else
            {
                Console.WriteLine($"Packet Number = {secondByte}");
                Thread.Sleep(100);

                //var sendACK = _random.Next(1, 20) > 6;
                //if (sendACK)
                Messages.SendAck(client);
                //else
                //    Messages.SendNack(client);

            }

        }
    }
}
