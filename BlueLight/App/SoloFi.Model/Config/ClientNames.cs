using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;

namespace SoloFi.Model.Config
{
    public class ClientNames
    {

        public const string Ramp = "Ramp";
        public const string Iluka = "Iluka";
        public const string Screenex = "Screenex";

        public static string CurrentClient { get; set; }

        static ClientNames()
        {
            CurrentClient = Ramp; // this is where you set the client for this build.
        }

        
    }
}
