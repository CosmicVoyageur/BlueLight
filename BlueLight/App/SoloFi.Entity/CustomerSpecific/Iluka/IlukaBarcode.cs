using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloFi.Entity.CustomerSpecific.Iluka
{
    public class IlukaBarcode //: ISampleIdentifier
    {
        public string SampleIdentifier { get; set; }

        public string EpcOrOtherMessage
        {
            get { return "Barcode"; }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            var barcode = obj as IlukaBarcode;
            if (barcode == null) return false;
            return barcode.SampleIdentifier == this.SampleIdentifier;

        }


    }
}
