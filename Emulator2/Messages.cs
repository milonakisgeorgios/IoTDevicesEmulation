using System.Text;

namespace Emulator2
{
    internal class Messages
    {
        static byte[] GetBuffer(byte delimiter, bool needsAck, int length, short packetID, DateTime recordedTime, byte ms)
        {
            var buffer = new byte[length];

            byte[] packetIDBytes = BitConverter.GetBytes(packetID);


            Int32 epochSeconds = (Int32)(recordedTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            byte[] epochBytes = BitConverter.GetBytes(epochSeconds);

            Array.Copy(packetIDBytes, 0, buffer, 0, 2); // 2 bytes
            buffer[2] = (byte)(delimiter & 0b01111111); // 1 byte

            if (needsAck)
            {
                buffer[2] = (byte)(buffer[2] | 0b10000000);
            }

            Array.Copy(epochBytes, 0, buffer, 3, 4); // 4 bytes
            buffer[7] = ms; // 1 byte

            return buffer;
        }


        static byte[] GetDelim1Packet(short packetID, DateTime recordedTime, byte sensorType, byte sensorNumber, UInt16 value)
        {
            var buffer = GetBuffer(delimiter: 1, needsAck: false, length: 14, packetID: packetID, recordedTime: recordedTime, ms: 0);

            int index = 8;

            buffer[index++] = sensorType;                                               // 1 byte
            buffer[index++] = sensorNumber;                                             // 1 byte
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, index, 2);              // 2 bytes
            index += 2;


            ushort crc = CRC16.ComputeCrc16(buffer, buffer.Length - 2);
            buffer[buffer.Length - 2] = (byte)crc;                                      // Low byte of CRC
            buffer[buffer.Length - 1] = (byte)(crc >> 8);                               // High byte of CRC

            return buffer;
        }

        static byte[] GetDelim2Packet(short packetID, DateTime recordedTime, byte sensorType, byte sensorNumber, byte value)
        {
            var buffer = GetBuffer(delimiter: 2, needsAck: false, length: 13, packetID: packetID, recordedTime: recordedTime, ms: 0);


            int index = 8;

            buffer[index++] = sensorType;                                               // 1 byte
            buffer[index++] = sensorNumber;                                             // 1 byte
            buffer[index++] = value;                                                    // 1 byte
            index += 4;


            ushort crc = CRC16.ComputeCrc16(buffer, buffer.Length - 2);
            buffer[buffer.Length - 2] = (byte)crc;                                      // Low byte of CRC
            buffer[buffer.Length - 1] = (byte)(crc >> 8);                               // High byte of CRC

            return buffer;
        }

        static byte[] GetDelim3Packet(short packetID, DateTime recordedTime, byte sensorType, byte sensorNumber, byte value)
        {
            var buffer = GetBuffer(delimiter: 3, needsAck: false, length: 13, packetID: packetID, recordedTime: recordedTime, ms: 0);


            int index = 8;

            buffer[index++] = sensorType;                                               // 1 byte
            buffer[index++] = sensorNumber;                                             // 1 byte
            buffer[index++] = value;                                                    // 1 byte
            index += 4;


            ushort crc = CRC16.ComputeCrc16(buffer, buffer.Length - 2);
            buffer[buffer.Length - 2] = (byte)crc;                                      // Low byte of CRC
            buffer[buffer.Length - 1] = (byte)(crc >> 8);                               // High byte of CRC
            return buffer;
        }


        public static void SendDelim1(IoTClient client, short packetID, DateTime recordedTime, byte sensorType = 0, byte sensorNumber = 0, UInt16 value = 100)
        {
            var buffer = GetDelim1Packet(packetID, recordedTime, sensorType, sensorNumber, value);

            if(client.IsConnected)
            {
                var hexstring = BitConverter.ToString(buffer).Replace("-", "");
                Console.WriteLine("Sent: {0}", hexstring);
                client.Send(Encoding.ASCII.GetBytes(hexstring));
            }
        }


        public static void SendDelim2(IoTClient client, short packetID, DateTime recordedTime, byte sensorType = 0, byte sensorNumber = 0, byte value = 0)
        {
            var buffer = GetDelim2Packet(packetID, recordedTime, sensorType, sensorNumber, value);

            if (client.IsConnected)
            {
                var hexstring = BitConverter.ToString(buffer).Replace("-", "");
                Console.WriteLine("Sent: {0}", hexstring);
                client.Send(Encoding.ASCII.GetBytes(hexstring));
            }
        }
        public static void SendDelim3(IoTClient client, short packetID, DateTime recordedTime, byte sensorType = 0, byte sensorNumber = 0, byte value = 0)
        {
            var buffer = GetDelim3Packet(packetID, recordedTime, sensorType, sensorNumber, value);

            if (client.IsConnected)
            {
                var hexstring = BitConverter.ToString(buffer).Replace("-", "");
                Console.WriteLine("Sent: {0}", hexstring);
                client.Send(Encoding.ASCII.GetBytes(hexstring));
            }
        }



    }
}
