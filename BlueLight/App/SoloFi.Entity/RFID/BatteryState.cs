namespace SoloFi.Entity.RFID
{
    public class BatteryState
    {
        public BatteryState(int stateOfCharge, bool charging)
        {
            StateOfCharge = stateOfCharge;
            Charging = charging;
        }
        public int StateOfCharge { get; set; }
        public bool Charging { get; set; }
    }
}
