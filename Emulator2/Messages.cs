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
        static byte[] GetDelimiter15Packet(short packetID, DateTime recordedTime, TrackData values)
        {
            var buffer = GetBuffer(delimiter: 15, needsAck: false, length: 64, packetID: packetID, recordedTime: recordedTime, ms: 0);


            int index = 8;

            Array.Copy(BitConverter.GetBytes(values.Latitude), 0, buffer, index, 8);
            index += 8;
            Array.Copy(BitConverter.GetBytes(values.Longitude), 0, buffer, index, 8);
            index += 8;
            Array.Copy(BitConverter.GetBytes(values.Altitude), 0, buffer, index, 4);
            index += 4;
            Array.Copy(BitConverter.GetBytes(values.Speed), 0, buffer, index, 4);
            index += 4;
            buffer[index++] = values.GPS_Satellites;
            buffer[index++] = values.GLONASS_Satellites;
            buffer[index++] = values.BeiDou_Satellites;
            buffer[index++] = values.NS_Indicator;
            buffer[index++] = values.EW_Indicator;
            Array.Copy(BitConverter.GetBytes(values.Course), 0, buffer, index, 4);
            index += 4;
            Array.Copy(BitConverter.GetBytes(values.PDPOP), 0, buffer, index, 4);
            index += 4;
            Array.Copy(BitConverter.GetBytes(values.HDPOP), 0, buffer, index, 4);
            index += 4;
            Array.Copy(BitConverter.GetBytes(values.VDPOP), 0, buffer, index, 4);
            index += 4;
            buffer[index++] = values.Fix_Mode;
            buffer[index++] = values.Galileo_Satellites;
            buffer[index++] = values.Valid_Satellites;
            buffer[index++] = values.Valid_Fix;
            Array.Copy(BitConverter.GetBytes(values.GNSS_Epoch), 0, buffer, index, 4);
            index += 4;
            buffer[index++] = values.GNSS_Ms;


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

        public static void SendDelim15(IoTClient client, short packetID, DateTime recordedTime)
        {
            var buffer = GetDelimiter15Packet(packetID, recordedTime, new TrackData(true));

            if (client.IsConnected)
            {
                var hexstring = BitConverter.ToString(buffer).Replace("-", "");
                Console.WriteLine("Sent: {0}", hexstring);
                client.Send(Encoding.ASCII.GetBytes(hexstring));
            }
        }


        public static void SendAck(IoTClient client)
        {
            var buffer = new byte[] { 0x06 };

            if (client.IsConnected)
            {
                var hexstring = BitConverter.ToString(buffer).Replace("-", "");
                Console.WriteLine("Sent Ack: {0}", hexstring);
                client.Send(Encoding.ASCII.GetBytes(hexstring));
            }
        }
        public static void SendNack(IoTClient client)
        {
            var buffer = new byte[] { 0x15 };

            if (client.IsConnected)
            {
                var hexstring = BitConverter.ToString(buffer).Replace("-", "");
                Console.WriteLine("Sent Nack: {0}", hexstring);
                client.Send(Encoding.ASCII.GetBytes(hexstring));
            }
        }
        public static void SendInitiation(IoTClient client)
        {
            var buffer = new byte[] { 0x43 };

            if (client.IsConnected)
            {
                var hexstring = BitConverter.ToString(buffer).Replace("-", "");
                Console.WriteLine("Sent Nack: {0}", hexstring);
                client.Send(Encoding.ASCII.GetBytes(hexstring));
            }
        }


    }
}
