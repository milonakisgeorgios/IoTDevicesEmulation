using System.Reflection;
using Emulator2.Configuration.Sections;
using Microsoft.Extensions.Configuration;

namespace Emulator2.Configuration
{
    internal class EmulatorConfiguration
    {
        public bool IsDevelopment { get; } = false;

        public IotServerSection Server { get; }



        public EmulatorConfiguration(string settingsFileName = "appsettings.json", string sectionName = "EmulatorSettings")
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                 .AddJsonFile(settingsFileName)
                 .Build();


            var root = config.GetSection(sectionName);
            if (root.Exists() == false)
            {
                throw new InvalidOperationException($"There is no {sectionName} section in {settingsFileName}");
            }


            var value = root["IsDevelopment"];
            this.IsDevelopment = value != null && Convert.ToBoolean(value);



            try
            {
                this.Server = new IotServerSection(root, required: true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while creating a IotServerSection", ex);
            }

        }
    }
}
