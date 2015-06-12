using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloFi.Entity.CustomerSpecific.Iluka;

namespace SoloFi.Contract.EventArgs
{
    public class BarcodeEventArgs : System.EventArgs
    {
        public IlukaBarcode Data { get; set; }
    }
}
