using System.Net;
using Microsoft.Extensions.Configuration;

namespace Emulator2.Configuration.Sections
{
    internal class IotServerSection
    {
        private const string SectionName = "IotServer";

        public string RemoteIP { get; } = "127.0.0.1";
        /// <summary>
        /// 
        /// </summary>
        public int RemotePort { get; } = 38967;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="required"></param>
        internal IotServerSection(IConfigurationSection configuration, bool required = false)
        {
            ArgumentNullException.ThrowIfNull(configuration);


            var section = configuration.GetSection(SectionName);
            if (!section.Exists())
            {
                if (required)
                {
                    throw new InvalidOperationException($"Missing required configuration section '{SectionName}'.");
                }
                return;
            }


            var value = section["RemoteIP"];
            if (IPAddress.TryParse(value, out var _))
            {
                this.RemoteIP = value;
            }


            value = section["RemotePort"];
            if (Int32.TryParse(value, out int remoteport))
            {
                if (!IsValidPort(remoteport))
                {
                    throw new InvalidOperationException($"Invalid value for {SectionName}:RemotePort. Must be a valid port number (1024-65535).");
                }
                this.RemotePort = remoteport;
            }
        }

        private static bool IsValidPort(int port) => port is >= 1024 and <= 65535;
    }
}
