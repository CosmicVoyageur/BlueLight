using System;
using System.Linq;
using SoloFi.Entity.RFID;

namespace SoloFi.Model.RFID
{
    public static class Ascii2Helper
    {
        public static ReaderState ParseInventoryParameters(string value)
        {
            var readerState = new ReaderState();
            var thing = value.Split(' ').ToList();
            var next = false; // this is a short hack for now.
            foreach (var s in thing)
            {
                if (s.Contains("-o")) next = true;
                if (next)
                {
                    readerState.TransmitPower = Convert.ToInt32(s);
                }
            }
            return readerState;
        }
    }
}
