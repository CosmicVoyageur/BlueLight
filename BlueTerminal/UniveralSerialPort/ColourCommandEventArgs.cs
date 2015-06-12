using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniveralSerialPort
{
    public class ColourCommandEventArgs : EventArgs
    {
        public ColourCommandEventArgs(ColourCommand command)
        {
            Command = command;
        }
        public ColourCommand Command { get; set; }
    }
}
