using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniveralSerialPort
{
    public class ColourCommander
    {
        public static void Go(ColourCommand command, SerialPort port)
        {
            try
            {
                var pos = Convert.ToByte(command.Position);
                var r = Convert.ToByte(command.Colour.Red);
                var g = Convert.ToByte(command.Colour.Green);
                var b = Convert.ToByte(command.Colour.Blue);
                byte[] bytes = new[] {pos, r, g, b};

                port.Write(bytes, 0, bytes.Length);
                Debug.WriteLine(command);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           

        }
    }
}
