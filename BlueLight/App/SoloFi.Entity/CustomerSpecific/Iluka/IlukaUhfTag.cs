using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloFi.Entity.RFID;
using Tsl.AsciiProtocol.Pcl;

namespace SoloFi.Entity.CustomerSpecific.Iluka
{
    public class IlukaUhfTag : Tag//, ISampleIdentifier
    {
        public string UserMemoryHex { get; set; }

        public string SampleIdentifier
        {
            get { return HexString2Ascii(UserMemoryHex); }
            set { UserMemoryHex = Ascii2Hex(value); }
        }

        #region Ascii Conversion Methods

        private string HexString2Ascii(string hexString)
        {
            if (hexString == null) return "";
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i <= hexString.Length - 2; i += 2)
            {
                var numericValue = Int32.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber);
                if (numericValue == 0) break;
                sb.Append(Convert.ToString(Convert.ToChar(numericValue)));
            }
            return sb.ToString();
        }

        private string Ascii2Hex(string input)
        {
            StringBuilder hex = new StringBuilder();
            char[] charValues = input.ToCharArray();
            foreach (char c in charValues)
            {
                hex.Append(String.Format("{0:X}", Convert.ToInt32(c)));
            }
            return hex.ToString();
        }

        #endregion

        public static IlukaUhfTag ConvertTag(TransponderData data)
        {
            var tag = new IlukaUhfTag {TagNumber = data.Epc, UserMemoryHex = data.ReadData};
            return tag;
        }
    }

    
}
