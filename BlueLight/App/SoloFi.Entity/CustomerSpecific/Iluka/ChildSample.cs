using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity.CustomerSpecific.Iluka
{
    public class ChildSample
    {
        public IlukaBarcode Barcode { get; set; }
        public IlukaUhfTag Transponder { get; set; }

        public string SampleIdentifier
        {
            get
            {
                if (Barcode != null) return Barcode.SampleIdentifier;
                else if (Transponder != null) return Transponder.SampleIdentifier;
                else return "Internal Error";
            }
        }

        public string EpcOrOtherMessage {
            get
            {
                if (Barcode != null) return "Barcode";
                else if (Transponder != null) return Transponder.TagNumber;
                else return "Internal Error";
            }
        }

        public static ChildSample CreateChild(IlukaBarcode barcode)
        {
            return new ChildSample
            {
                Barcode = barcode
            };
        }

        public static ChildSample CreateChild(IlukaUhfTag tag)
        {
            return new ChildSample
            {
                Transponder = tag
            };
        }

        public override bool Equals(object obj)
        {
            var child = obj as ChildSample;
            if (child == null) return false;
            if (child.Barcode != null)
            {
                return child.Barcode.Equals(this.Barcode);
            }
            if (child.Transponder != null)
            {
                return child.Transponder.Equals(this.Transponder);
            }
            return false;
        }
    }
}
