using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoloFi.Contract.EventArgs;
using SoloFi.Entity.RFID;

namespace SoloFi.Contract.Services
{
    public interface IRfidReaderService
    {
        // for operating a reader
        event EventHandler<TagEventArgs> TagEvent;
        event EventHandler StateEvent;
        void RequestDesiredState(ReaderState state);
        // for connecting to a reader
        Task<IEnumerable<string>> GetPairedDevices(); // maps from IBluetoothRfidReaderDiscoveryService
        Task<bool> Connect(string deviceName);

        Task<bool> IsReaderConnected(Reader reader);

        void AlertForGeiger(int proximity); // 0 is closest, 3+ is furthest

        void RequestBatteryStatus();
        event EventHandler BatteryEvent;
        Task<bool> IsReaderConnected();
    }
}
