using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator2
{
    internal class FirmwareUpgradeEmulator
    {
        string _serverIP;
        int _serverPort;
        short _packetId = 1;
        IoTClient client = null;
        System.Random _random = new System.Random();

        public FirmwareUpgradeEmulator(string serverIP, int serverPort)
        {
            _serverIP = serverIP;
            _serverPort = serverPort;
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
            client = new IoTClient(_serverIP, _serverPort, this.ProcessReceivedData);
            client.Connect();

            while (true)
            {
                PrintMenu();
                var input1 = Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant();

                Console.Clear();
                if (input1 == "Q")
                {
                    client.Stop();
                    break;
                }
                if (input1 == "N" || input1 == "Ν")
                {
                    Messages.SendNack(client);
                }
                if (input1 == "A" || input1 == "Α")
                {
                    Messages.SendAck(client);
                }
            }
        }


        public static byte[] Hex2Byte(string hex)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < hex.Length; i += 2)
            {
                string byteString = hex.Substring(i, 2);
                byte byteValue = Convert.ToByte(byteString, 16);
                bytes.Add(byteValue);
            }
            return bytes.ToArray();
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
                handle120(hexstring, rcv_buffer, rcv_buffer.Length);

                Thread.Sleep(2000);
                Console.WriteLine("Send Initial NACK");
                Messages.SendInitiation(client);          //Initialization NACK
            }
            else if (rcv_buffer.Length == 133 && firstByte == 0x01)
            {
                if(firstPacketReceived == false)
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
                    client.Stop();
                    Console.WriteLine("Firmware Upgrade Finished!");
                    firstPacketReceived = false;
                }
            }
            else if(rcv_buffer.Length == 1 && firstByte == 4)
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


        void handle120(string hexstring, byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("Firmware Upgrade - Received: {0}", hexstring);

            Thread.Sleep(1200);


            if (client.IsConnected)
            {
                var _hexstring = BitConverter.ToString(rcvBuffer).Replace("-", "");
                Console.WriteLine("Sent: {0}", _hexstring);
                client.Send(Encoding.ASCII.GetBytes(_hexstring));
            }
        }
    }
}
