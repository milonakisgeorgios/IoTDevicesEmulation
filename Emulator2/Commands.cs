using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator2
{
    public enum Commands : byte
    {
        None = 0,
        RequestVentilationStatus = 30,
        ReadConfigurationProfile = 60,
        WriteConfigurationProfile = 61,
        TestDevice = 62,
        SetDeviceOutput = 63,
        Restart = 64,
        InitRtcNvme = 65,
        ActionCommand = 66,
        HandleDeviceNetwork = 67,
        TestNoiseFrequency = 68,
        ReadFreqCalibrationTable = 69,
        WriteFreqCalibrationTable = 70,
        HandlePeripheral = 71,
        PowerDown = 72,
        SetupReplyErrorCmd = 89,
        AnalogCmd = 90,
        DigitalCmd = 91,
        RequestVentilationStatusReply = 92,
        FirmwareUpgrade = 120
    }
}
