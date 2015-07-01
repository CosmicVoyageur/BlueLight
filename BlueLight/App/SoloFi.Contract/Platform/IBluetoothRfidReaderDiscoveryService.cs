using System.Collections.Generic;
using System.Threading.Tasks;
using Tsl.AsciiProtocol.Pcl;

namespace BlueLight.Contract.Platform
{
    public interface IBluetoothRfidReaderDiscoveryService
    {
        Task<IEnumerable<string>> GetPairedBtDevices();
        Task<IAsciiSerialPort> GetAsciiSerialPort(string deviceName);
    }
}