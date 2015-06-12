using System;
using SoloFi.Entity.RFID;

namespace SoloFi.Model.RFID.CustomEventArgs
{
    public class ReaderStateEventArgs : EventArgs
    {
        public ReaderState State { get; set; }
    }
}
