using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloFi.Model.RFID.CustomEventArgs
{
    public class SerialNumberEventArgs : EventArgs
    {
        public string SerialNumber { get; set; }
    }
}
