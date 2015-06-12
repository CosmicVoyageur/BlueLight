namespace SoloFi.Entity.RFID
{
    public class ReaderState
    {
        public ReaderState(int power = 20)
        {
            TransmitPower = power;
        }
        public int TransmitPower { get; set; }
        public bool GeigerMode { get; set; }
    }
}
