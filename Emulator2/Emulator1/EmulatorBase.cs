namespace Emulator2
{
    internal class EmulatorBase
    {

        public byte[] Hex2Byte(string hex)
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


    }
}
