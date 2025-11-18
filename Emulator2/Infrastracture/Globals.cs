using Emulator2.Configuration;
using Emulator2.Configuration.Sections;

namespace Emulator2
{
    internal static class Globals
    {


        public static EmulatorConfiguration Configuration = new EmulatorConfiguration();
        public static IotServerSection ServerConfiguration = Configuration.Server;

    }
}
