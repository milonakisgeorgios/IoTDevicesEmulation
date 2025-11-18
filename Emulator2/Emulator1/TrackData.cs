namespace Emulator2
{
    internal class TrackData
    {
        /// <summary>
        /// float64_t		La
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// float64_t		Lo
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// float32_t		ALT
        /// </summary>
        public float Altitude { get; set; }
        /// <summary>
        /// float32_t		Speed
        /// </summary>
        public float Speed { get; set; }
        /// <summary>
        /// uint8_t		gps sats
        /// </summary>
        public byte GPS_Satellites { get; set; }
        /// <summary>
        /// uint8_t		glonass sats
        /// </summary>
        public byte GLONASS_Satellites { get; set; }
        /// <summary>
        /// uint8_t		beidou sats
        /// </summary>
        public byte BeiDou_Satellites { get; set; }
        /// <summary>
        /// uint8_t	
        ///     North/South Indicator
        ///     0:North    
        ///     1:South
        /// </summary>
        public byte NS_Indicator { get; set; }
        /// <summary>
        /// uint8_t	
        ///     East/West Indicator
        ///     0:East    
        ///     1:West
        /// </summary>
        public byte EW_Indicator { get; set; }
        /// <summary>
        /// float32_t		course
        /// Bearing or track angle (heading in degrees, 0-360°).
        /// </summary>
        public float Course { get; set; }
        /// <summary>
        /// Position Dilution of Precision (overall 3D accuracy metric; lower = better, e.g., <2.0 ideal).
        /// </summary>
        /// 
        public float PDPOP { get; set; }
        /// <summary>
        /// Horizontal Dilution of Precision (2D horizontal accuracy).
        /// </summary>
        public float HDPOP { get; set; }
        /// <summary>
        /// Vertical Dilution of Precision (altitude accuracy).
        /// </summary>
        public float VDPOP { get; set; }

        /// <summary>
        /// uint8_t		Fix Mode
        /// </summary>
        public byte Fix_Mode { get; set; }
        /// <summary>
        /// uint8_t		Galileo sats
        /// </summary>
        public byte Galileo_Satellites { get; set; }
        /// <summary>
        /// uint8_t		Valid sats
        /// </summary>
        public byte Valid_Satellites { get; set; }

        /// <summary>
        /// uint8_t		valid fix
        /// </summary>
        public byte Valid_Fix { get; set; }

        /// <summary>
        /// uint32_t		gnss epoch
        /// </summary>
        public UInt32 GNSS_Epoch { get; set; }
        /// <summary>
        /// uint8_t		gnss ms

        /// </summary>
        public byte GNSS_Ms { get; set; }


        public TrackData(bool random = false)
        {
            if (random)
            {
                Random rnd = new Random();

                GNSS_Epoch = (uint)rnd.Next(1577836800, 1798761600);
                GNSS_Ms = (byte)rnd.Next(0, 255);
                Latitude = rnd.NextDouble() * 180.0 - 90.0; // -90 to +90
                Longitude = rnd.NextDouble() * 360.0 - 180.0; // -180 to +180
                Altitude = (float)(rnd.NextDouble() * 100500.0 - 500.0); // -500 to 100,000
                Speed = (float)(rnd.NextDouble() * 100.0);
                GPS_Satellites = (byte)rnd.Next(0, 13);
                GLONASS_Satellites = (byte)rnd.Next(0, 9);
                BeiDou_Satellites = (byte)rnd.Next(0, 9);
                NS_Indicator = (byte)(Latitude >= 0 ? 0 : 1);
                EW_Indicator = (byte)(Longitude >= 0 ? 0 : 1);
                Course = (float)(rnd.NextDouble() * 360.0);

                PDPOP = (float)(rnd.NextDouble() * 19.0 + 1.0);
                HDPOP = (float)(rnd.NextDouble() * 9.5 + 0.5);
                VDPOP = (float)(rnd.NextDouble() * 9.5 + 0.5);


                // FixMode: Random between 0 (no fix), 1 (2D), 2 (3D), 3 (DGPS)
                Fix_Mode = (byte)rnd.Next(0, 4);

                // GalileoSatellites: Random between 0 and 8
                Galileo_Satellites = (byte)rnd.Next(0, 9);

                // ValidSatellites: Sum of satellites or random between 0 and 30
                Valid_Satellites = (byte)(GPS_Satellites + GLONASS_Satellites + BeiDou_Satellites + Galileo_Satellites);
                if (Valid_Satellites > 30) Valid_Satellites = (byte)rnd.Next(0, 31);

                // ValidFix: Randomly 0 (invalid) or 1 (valid)
                Valid_Fix = (byte)rnd.Next(0, 2);
            }
        }
    }
}
