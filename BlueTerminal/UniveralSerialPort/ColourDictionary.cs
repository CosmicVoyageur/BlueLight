using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniveralSerialPort
{
    public static class ColourDictionary
    {
        public static RGBColour Green
        {
            get
            {
                return new RGBColour(0,255,0);
            }
        }

        public static RGBColour Blue
        {
            get
            {
                return new RGBColour(0, 0, 255);
            }
        }

        public static RGBColour Red
        {
            get
            {
                return new RGBColour(255, 0, 0);
            }
        }
    }
}
