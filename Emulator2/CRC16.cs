namespace Emulator2
{
    internal class CRC16
    {
        private const ushort CrcPolynomial = 0x1021;        // XMODEM CRC-16 polynomial
        private const ushort CrcInitialValue = 0x0000;      // Initial value for CRC calculation

        public static ushort ComputeCrc16(byte[] data, int length)
        {
            if (length < 0 || length > data.Length)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative and within data bounds.");

            ushort crc = CrcInitialValue;
            for (int i = 0; i < length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)(crc << 1 ^ CrcPolynomial);
                    else
                        crc <<= 1;
                }
            }

            return crc;

        }
    }
}
