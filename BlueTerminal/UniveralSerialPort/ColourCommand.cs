using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniveralSerialPort
{
    public class ColourCommand
    {
        public int Position { get; set; }
        public RGBColour Colour { get; set; }

        public override string ToString()
        {
            return "P:" + Position + " R:" + Colour.Red + " G:" + Colour.Green + " B:" + Colour.Blue;
        }
    }
}
