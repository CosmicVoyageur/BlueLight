using System;
using SoloFi.Entity.RFID;

namespace SoloFi.Model.RFID.CustomEventArgs
{
    public class BatteryStateEventArgs : EventArgs
    {
        public BatteryState State { get; set; }
    }
}
