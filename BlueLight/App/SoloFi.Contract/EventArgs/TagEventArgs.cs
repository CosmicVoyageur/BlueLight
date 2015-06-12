using SoloFi.Entity.RFID;

namespace SoloFi.Contract.EventArgs
{
    public class TagEventArgs : System.EventArgs
    {
        public TagEventArgs(string tagNumber)
        {
            Tag = new Tag {TagNumber = tagNumber};
        }
        public Tag Tag { get; private set; }
        public int ?Rssi { get; set; }
    }
}
