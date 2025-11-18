using System.Text;
using Emulator2.Emulators;

namespace Emulator2
{
    internal class Emulator1 : EmulatorBase
    {
        short _packetId = 1;
        IoTClient client = null!;
        private volatile bool _shouldStop = false;
        private readonly object _lock = new object();

        string ServerIP => Globals.ServerConfiguration.RemoteIP;
        int ServerPort => Globals.ServerConfiguration.RemotePort;


        public Emulator1()
        {

        }

        void PrintMenu()
        {
            Console.WriteLine("\t\t*****************Emulator1*****************");
            Console.WriteLine("\t\t*        A -> Send Delim1 Packet");
            Console.WriteLine("\t\t*        S -> Send Delim2 Packet");
            Console.WriteLine("\t\t*        D -> Send Delim3 Packet");
            Console.WriteLine("\t\t*        E -> Send Delim15 Packet");
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
                    if (upper == 'A') Messages.SendDelim1(client, _packetId++, DateTime.Now);
                    if (upper == 'S') Messages.SendDelim2(client, _packetId++, DateTime.Now);
                    if (upper == 'D') Messages.SendDelim3(client, _packetId++, DateTime.Now);
                    if (upper == 'E') Messages.SendDelim15(client, _packetId++, DateTime.Now);

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





        public void ProcessReceivedData(byte[] rcvBuffer, int numOfBytes)
        {
            string hexstring = System.Text.Encoding.ASCII.GetString(rcvBuffer.AsSpan(0, numOfBytes));
            //Console.WriteLine($"Received: {hexstring}");

            var reply_buffer = Hex2Byte(hexstring);
            byte cmd = reply_buffer[2];

            if (cmd == (byte)Commands.ReadConfigurationProfile)
            {
                CommandHandlersFW13.handle60(client, rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.WriteConfigurationProfile)
            {
                CommandHandlersFW13.handle61(client, rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.TestDevice)
            {
                CommandHandlersFW13.handle62(client, rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.TestNoiseFrequency)
            {
                CommandHandlersFW13.handle68(client, rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.ReadGPSConfig)
            {
                CommandHandlersFW13.handle73(client, rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.WriteGPSConfig)
            {
                CommandHandlersFW13.handle74(client, rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.TestGPSDevice)
            {
                CommandHandlersFW13.handle75(client, rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.FirmwareDownload)
            {
                CommandHandlersFW13.handle76(client, rcvBuffer, numOfBytes);
            }
            else
            {
                Console.WriteLine($"Received: {hexstring}");
                Console.WriteLine($"UNKNOWN COOMMAND {cmd}");
            }
        }




        static byte[] GenerateCmd76Impl(string content)
        {
            const int packetLength = 255;
            const byte cmdId = 76;
            const int maxContentBytes = packetLength - 5; // 250

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("Command 76 must have valid content!");
            }

            byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            if (contentBytes.Length > maxContentBytes)
            {
                throw new ArgumentException($"Content exceeds {maxContentBytes} bytes.", nameof(content));
            }


            byte[] buffer = new byte[packetLength];

            byte[] cmdBytes = BitConverter.GetBytes(packetLength);
            Array.Copy(cmdBytes, 0, buffer, 0, 2);      // 2 bytes
            buffer[2] = cmdId;

            Array.Copy(contentBytes, 0, buffer, 3, contentBytes.Length);

            ushort computedCrc = CRC16.ComputeCrc16(buffer, packetLength - 2);
            buffer[packetLength - 2] = (byte)computedCrc;
            buffer[packetLength - 1] = (byte)(computedCrc >> 8);

            return buffer;
        }
    }
}
