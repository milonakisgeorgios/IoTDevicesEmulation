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
            Console.WriteLine("\t\t*        Ε -> Send Delim15 Packet");
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
                if (input1 == "Q" || input1=="q")
                {
                    client.Stop();
                    break;
                }
                if(input1 == "A" || input1 == "a")
                {
                    Messages.SendDelim1(client, _packetId++, DateTime.Now);
                }
                if (input1 == "S" || input1 == "s")
                {
                    Messages.SendDelim2(client, _packetId++, DateTime.Now);
                }
                if (input1 == "D" || input1 == "d")
                {
                    Messages.SendDelim3(client, _packetId++, DateTime.Now);
                }
                if (input1 == "E" || input1 == "e")
                {
                    Messages.SendDelim15(client, _packetId++, DateTime.Now);
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

            if (cmd == (byte)Commands.ReadConfigurationProfile)
            {
                handle60(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.WriteConfigurationProfile)
            {
                handle61(rcvBuffer, numOfBytes);
            }
            else if (cmd == (byte)Commands.TestDevice)
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
            else if (cmd == (byte)Commands.FirmwareDownload)
            {
                handle76(rcvBuffer, numOfBytes);
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

            Thread.Sleep(600);

            var resp = "96003C0D000A0101CDCCCC3F6666E63F00000040CDCCCC3DCDCCCC3DCDCCCC3D0000003FB80B0AD7233CCDCCCC3D0000A040D0078FC2F53C0000A0400000803F1400019A99993E3C00CDCCCC3DCDCCCC3D0600201C0B005A3C000FCDCC0C40D0074006E80332005802B4000000C8009411000501180010270000100032000160090A00FA000101A00F409C64006400C8000301002411";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        void handle61(byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("WriteConfigurationProfile - Received: {0}", response);

            Thread.Sleep(600);

            var resp = "96003D0D000A0101CDCCCC3F6666E63F00000040CDCCCC3DCDCCCC3DCDCCCC3D0000003FB80B0AD7233CCDCCCC3D0000A040D0078FC2F53C0000A0400000803F1400019A99993E3C00CDCCCC3DCDCCCC3D0600201C0B005A3C000FCDCC0C40D0074006E80332005802B4000000C8009411000501180010270000100032000160090A00FA000101A00F409C64006400C800030100E449";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }

        void handle62(byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestDevice - Received: {0}", response);

            Thread.Sleep(600);

            var resp = "57003E0D000C015898000063ADE368170F3852404094D2410000000000000000DD81C5412A2BC741096B0A4000000000000000000000000014721E3DBE9A193D000001010000001C00D003000049090000A0820000249F";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        void handle68(byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestNoiseFrequency - Received: {0}", response);

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

            var resp = "781E4B1F43F76800D0D556EC2FE3424050FC1873D79A5EC000004A4233332341080604000100003443000020406666E63F9A99993F0305170134AB0869008D36";
            //var resp = "40004B87890869005D1EC6FF78FB424011983DBFDEBB37403333D3420E2D6A401007FF00000000C54233337340000040408FC2154003030D0180890869000667";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        void handle76(byte[] rcvBuffer, int numOfBytes) 
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
                var data = GenerateCmd76Impl("ok");
                var resp = BitConverter.ToString(data).Replace("-", "");
                client.Send(Encoding.ASCII.GetBytes(resp));
            }
            else
            {
                Console.WriteLine("FirmwareDownload failed");
                var data = GenerateCmd76Impl("ftp server crushed!");
                var resp = BitConverter.ToString(data).Replace("-", "");
                client.Send(Encoding.ASCII.GetBytes(resp));
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
