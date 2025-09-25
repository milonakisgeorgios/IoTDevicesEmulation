using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Emulator2
{
    internal class Emulator1
    {
        string _serverIP;
        int _serverPort;
        short _packetId = 1;
        IoTClient client = null;

        public Emulator1(string serverIP, int serverPort)
        {
            _serverIP = serverIP;
            _serverPort = serverPort;
        }

        void PrintMenu()
        {
            Console.WriteLine("\t\t*****************Emulator1*****************");
            Console.WriteLine("\t\t*        A -> Send Delim1 Packet");
            Console.WriteLine("\t\t*        S -> Send Delim2 Packet");
            Console.WriteLine("\t\t*        D -> Send Delim3 Packet");
            Console.WriteLine("\t\t*");
            Console.WriteLine("\t\t*        Q -> Disconect & Return");
            Console.WriteLine("\t\t********************************************");
        }


        public void Start()
        {
            client = new IoTClient(_serverIP, _serverPort, this.ProcessReceivedData);
            client.Connect();

            while(true)
            {
                PrintMenu();
                var input1 = Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant();

                Console.Clear();
                if (input1 == "Q")
                {
                    client.Stop();
                    break;
                }
                if(input1 == "A")
                {
                    Messages.SendDelim1(client, _packetId++, DateTime.Now);
                }
                if (input1 == "S")
                {
                    Messages.SendDelim2(client, _packetId++, DateTime.Now);
                }
                if (input1 == "D")
                {
                    Messages.SendDelim3(client, _packetId++, DateTime.Now);
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


        public void ProcessReceivedData(byte[] rcvBuffer, int numOfBytes)
        {
            string hexstring = System.Text.Encoding.ASCII.GetString(rcvBuffer.AsSpan(0, numOfBytes));
            //Console.WriteLine($"Received: {hexstring}");

            var reply_buffer = Hex2Byte(hexstring);
            byte cmd = reply_buffer[2];

            if(cmd == (byte) Commands.ReadConfigurationProfile)
            {
                handle60(rcvBuffer, numOfBytes);
            }
            else if(cmd == (byte)Commands.TestDevice)
            {
                handle62(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.TestNoiseFrequency)
            {
                handle68(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.ReadGPSConfig)
            {
                handle73(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.WriteGPSConfig)
            {
                handle74(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.TestGPSDevice)
            {
                handle75(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.TestGPSDevice)
            {
                handle75(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.FirmwareDownload)
            {
                handle121(rcvBuffer, numOfBytes);
            }
            else
            {
                Console.WriteLine($"Received: {hexstring}");
                Console.WriteLine($"UNKNOWN COOMMAND {cmd}");
            }
        }

        void handle60(byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("ReadConfigurationProfile - Received: {0}", response);

            Thread.Sleep(2000);

            var resp = "96003C0D000A0101CDCCCC3F6666E63F00000040CDCCCC3DCDCCCC3DCDCCCC3D0000003FB80B0AD7233CCDCCCC3D0000A040D0078FC2F53C0000A0400000803F1400019A99993E3C00CDCCCC3DCDCCCC3D0600201C0B005A3C000FCDCC0C40D0074006E80332005802B4000000C8009411000501180010270000100032000160090A00FA000101A00F409C64006400C8000301002411";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }

        void handle62(byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestDevice - Received: {0}", response);

            Thread.Sleep(600);

            var resp = "3C003E04000301C03A0000FE46D2680075A73441245E554080446E4086C3CC4126E9D64119014C7B2F41D4F1B642D9B0B742EA6AB842AD69B042990B";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        void handle68(byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestDevice - Received: {0}", response);

            Thread.Sleep(600);

            var resp = "690044AE2CB142D67BAA429BB3AB4299C1AF42A8B3B9426E53B142FEBABA42E033B4420276C6424406D742FD97BF42457BBD423403D442A00FBD42BAA7D4429A79C742EDE4D14282D0CD4274A0954208EDAF42EE13C94222ECC54271DACB42BCDFBE424839B542B133";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }

        void handle73(byte[] rcvBuffer, int numOfBytes)
        {
            //ReadGPSConfig
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("ReadGPSConfig - Received: {0}", response);

            Thread.Sleep(600);

            var resp = "4000490146B636425CDAB80BCC747B841146AF36393079030000000000000000000000000000000000000000000000000000000000000000000000000000103A";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        void handle74(byte[] rcvBuffer, int numOfBytes)
        {
            //WriteGPSConfig
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("WriteGPSConfig - Received: {0}", response);

            Thread.Sleep(600);

            var resp = "40004A010000803F050003000500000020413C002C011E0000000000000000000000000000000000000000000000000000000000000000000000000000006BA0";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        void handle75(byte[] rcvBuffer, int numOfBytes)
        {
            //TestGPSDevice
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestGPSDevice - Received: {0}", response);

            Thread.Sleep(600);

            //var resp = "40004A010000803F050003000500000020413C002C011E0000000000000000000000000000000000000000000000000000000000000000000000000000006BA0";
            //client.Send(Encoding.ASCII.GetBytes(resp));
        }
        void handle121(byte[] rcvBuffer, int numOfBytes) 
        {
            //FirmwareUpgradeFTP
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("FirmwareUpgradeFTP - Received: {0}", response);
            Console.WriteLine();


            Thread.Sleep(600);

            var rnd = new System.Random();
            bool failed = rnd.Next(1, 21) < 10;

            if(failed)
            {
                Console.WriteLine("FirmwareDownload ok");
                var data = GenerateCmd121Impl("ok");
                var resp = BitConverter.ToString(data).Replace("-", "");
                client.Send(Encoding.ASCII.GetBytes(resp));
            }
            else
            {
                Console.WriteLine("FirmwareDownload failed");
                var data = GenerateCmd121Impl("ftp server crushed!");
                var resp = BitConverter.ToString(data).Replace("-", "");
                client.Send(Encoding.ASCII.GetBytes(resp));
            }
        }


        static byte[] GenerateCmd121Impl(string content)
        {
            const int packetLength = 256;
            const byte cmdId = 121;
            const int maxContentBytes = packetLength - 5; // 253

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("Command 121 must have valid content!");
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
