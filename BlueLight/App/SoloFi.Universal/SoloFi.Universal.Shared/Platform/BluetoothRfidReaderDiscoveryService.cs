using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using SoloFi.Contract.Platform;
using Tsl.AsciiProtocol.Pcl;

namespace SoloFi.Universal.Platform
{
    /// <summary>
    /// This class implements a the Reader Discovery Service.
    /// It must be able to enumerate the names of paired BT devices, 
    /// and then allow the user to create an IAsciiSerialPort
    /// </summary>
    public class BluetoothRfidReaderDiscoveryService : IBluetoothRfidReaderDiscoveryService
    {

        public async Task<IAsciiSerialPort> GetAsciiSerialPort(string deviceName)
        {
            var devices = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
            var device = devices.FirstOrDefault(s => s.Name == deviceName);
            if (device == null) return null;
            IAsciiSerialPort serialPort = await NativeWindowsAsciiSerialPort.GetInstance(device);
            return serialPort;
        }

        public async Task<IEnumerable<string>> GetPairedBtDevices()
        {
            var devices = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
            var device = devices.FirstOrDefault();

            var results = devices.Select(p => p.Name).ToList();
            return results;
        }



    }
}
