using System.Text;

namespace Emulator2.Emulators
{
    internal class CommandHandlersFW13
    {
        const int internalWaitTime = 500;// ενα χρονικο διαστημα (ms που πιστευουμε οτι χρειαζεται ενα device για να επεξεργαστει το command πριν στειλει το Reply

        public static void handle60(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("ReadConfigurationProfile - Received: {0}", response);

            Thread.Sleep(internalWaitTime);

            var resp = "96003C0D000A0101CDCCCC3F6666E63F00000040CDCCCC3DCDCCCC3DCDCCCC3D0000003FB80B0AD7233CCDCCCC3D0000A040D0078FC2F53C0000A0400000803F1400019A99993E3C00CDCCCC3DCDCCCC3D0600201C0B005A3C000FCDCC0C40D0074006E80332005802B4000000C8009411000501180010270000100032000160090A00FA000101A00F409C64006400C8000301002411";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }


        public static void handle61(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("WriteConfigurationProfile - Received: {0}", response);

            Thread.Sleep(internalWaitTime);

            var resp = "96003D0D000A0101CDCCCC3F6666E63F00000040CDCCCC3DCDCCCC3DCDCCCC3D0000003FB80B0AD7233CCDCCCC3D0000A040D0078FC2F53C0000A0400000803F1400019A99993E3C00CDCCCC3DCDCCCC3D0600201C0B005A3C000FCDCC0C40D0074006E80332005802B4000000C8009411000501180010270000100032000160090A00FA000101A00F409C64006400C800030100E449";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }

        public static void handle62(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestDevice - Received: {0}", response);

            Thread.Sleep(internalWaitTime);

            var resp = "57003E0D000C015898000063ADE368170F3852404094D2410000000000000000DD81C5412A2BC741096B0A4000000000000000000000000014721E3DBE9A193D000001010000001C00D003000049090000A0820000249F";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        public static void handle68(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestNoiseFrequency - Received: {0}", response);

            Thread.Sleep(internalWaitTime);

            var resp = "690044AE2CB142D67BAA429BB3AB4299C1AF42A8B3B9426E53B142FEBABA42E033B4420276C6424406D742FD97BF42457BBD423403D442A00FBD42BAA7D4429A79C742EDE4D14282D0CD4274A0954208EDAF42EE13C94222ECC54271DACB42BCDFBE424839B542B133";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }

        public static void handle73(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            //ReadGPSConfig
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("ReadGPSConfig - Received: {0}", response);

            Thread.Sleep(internalWaitTime);

            var resp = "4000490146B636425CDAB80BCC747B841146AF36393079030000000000000000000000000000000000000000000000000000000000000000000000000000103A";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        public static void handle74(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            //WriteGPSConfig
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("WriteGPSConfig - Received: {0}", response);

            Thread.Sleep(internalWaitTime);

            var resp = "40004A010000803F050003000500000020413C002C011E0000000000000000000000000000000000000000000000000000000000000000000000000000006BA0";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        public static void handle75(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            //TestGPSDevice
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("TestGPSDevice - Received: {0}", response);

            Thread.Sleep(internalWaitTime);

            var resp = "781E4B1F43F76800D0D556EC2FE3424050FC1873D79A5EC000004A4233332341080604000100003443000020406666E63F9A99993F0305170134AB0869008D36";
            //var resp = "40004B87890869005D1EC6FF78FB424011983DBFDEBB37403333D3420E2D6A401007FF00000000C54233337340000040408FC2154003030D0180890869000667";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }
        public static void handle76(IoTClient client, byte[] rcvBuffer, int numOfBytes)
        {
            //FirmwareUpgradeFTP
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("FirmwareUpgradeFTP - Received: {0}", response);
            Console.WriteLine();


            Thread.Sleep(internalWaitTime);

            //var rnd = new System.Random();
            //bool failed = rnd.Next(1, 21) < 10;

            //if(failed)
            //{
            //    Console.WriteLine("FirmwareDownload ok");
            //    var data = GenerateCmd76Impl("ok");
            //    var resp = BitConverter.ToString(data).Replace("-", "");
            //    client.Send(Encoding.ASCII.GetBytes(resp));
            //}
            //else
            //{
            //    Console.WriteLine("FirmwareDownload failed");
            //    var data = GenerateCmd76Impl("ftp server crushed!");
            //    var resp = BitConverter.ToString(data).Replace("-", "");
            //    client.Send(Encoding.ASCII.GetBytes(resp));
            //}

            var resp = "00014C46545020446F776E6C6F616420436D64204661696C656420436D645F537461747573203020446C5F53746174652034206674705F737461747573202D310D0A00703A31302E3130312E35342E31310D0A6674705F7365727665725F706F72743A31373032310D0A66775F706174683A66775F6469726563746F72792F6570726573737572655F76332F66772F6550726573737572655F335F76322E782E73660D0A0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003C14";
            client.Send(Encoding.ASCII.GetBytes(resp));
        }



        public static void handle120(IoTClient client, byte[] rcvBuffer, int numOfBytes, string hexstring)
        {
            string response = Encoding.UTF8.GetString(rcvBuffer, 0, numOfBytes);
            Console.WriteLine("Firmware Upgrade - Received: {0}", hexstring);

            Thread.Sleep(internalWaitTime);


            if (client.IsConnected)
            {
                var _hexstring = BitConverter.ToString(rcvBuffer).Replace("-", "");
                Console.WriteLine("Sent: {0}", _hexstring);
                client.Send(Encoding.ASCII.GetBytes(_hexstring));
            }
        }
    }
}
