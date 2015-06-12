using System;
using SoloFi.Contract.EventArgs;
using SoloFi.Entity.CustomerSpecific.Iluka;
using SoloFi.Entity.RFID;
using SoloFi.Model.RFID.CustomEventArgs;
using Tsl.AsciiProtocol.Pcl;
using BarcodeEventArgs = SoloFi.Contract.EventArgs.BarcodeEventArgs;

namespace SoloFi.Model.RFID
{
    public class CustomResponder : IAsciiCommandResponder
    {
        private readonly TransponderResponder _transponderResponder = new TransponderResponder();
        public CustomResponder()
        {
            _transponderResponder.TransponderReceived += _transponderResponder_TransponderReceived;
        }


        #region AsciiCommandResponder

        public bool ProcessReceivedLine(IAsciiResponseLine line, bool moreLinesAvailable)
        {
            _transponderResponder.ProcessReceivedLine(line.Header, line.Value);

            if (line.Header.Contains("BC")) // barcode
            {
                OnBarcode(line.Value);
            }
            if (line.Header.Contains("BP")) // battery percentage
            {
                OnBatteryPercentage(line.Value);
            }
            if (line.Header.Contains("CH")) // is charging
            {
                OnBatteryCharging(line.Value);
            }
            if (line.Header.Contains("PR")) //Parameter reader
            {
                OnParameterRead(line.Value);
            }
            if (line.Header.Contains("LK")) // licence key
            {
                OnLicenceKey(line.Value);
            }
            if (line.Header.Contains("US")) // serial number
            {
                OnSerialNumber(line.Value);
            }
            
            return true;
        }

        private void OnLicenceKey(string licenceKey)
        {
            var handler = LicenceKeyEvent;
            if (handler == null) return;
            handler(this,new LicenceKeyEventArgs{LicenceKey = licenceKey});
        }

        private void OnSerialNumber(string serialNumber)
        {
            var handler = SerialNumberEvent;
            if (handler == null) return;
            handler(this,new SerialNumberEventArgs{SerialNumber = serialNumber});
        }

        private void OnParameterRead(string value)
        {
            var state = Ascii2Helper.ParseInventoryParameters(value);
            if (ReaderStateNotification != null)
            {
                ReaderStateNotification(this,new ReaderStateEventArgs{State = state});
            }
        }

        private void OnBarcode(string value)
        {
            var handler = BarcodeReceived;
            if (handler == null) return;
            var args = new BarcodeEventArgs
            {
                Data = new IlukaBarcode
                {
                    SampleIdentifier = value
                }
            };
            handler(this, args);
        }

        private BatteryState _tempBatteryState;

        private void OnBatteryPercentage(string value)
        {
            if (_tempBatteryState == null)
            {
                int bat = Convert.ToInt32(value.Trim('%'));
                _tempBatteryState = new BatteryState(bat, false);
            }
        }
        private void OnBatteryCharging(string value)
        {
            _tempBatteryState.Charging = !value.Contains("Off");
            if (BatteryStateNotification != null)
            {
                BatteryStateNotification(this, new BatteryStateEventArgs{State =_tempBatteryState});
            }
            _tempBatteryState = null;
        }

        void _transponderResponder_TransponderReceived(object sender, TransponderDataEventArgs e)
        {
            OnTagFound(e);
        }

        #endregion

        public event EventHandler<TransponderDataEventArgs> TransponderReceived;
        public event EventHandler<BarcodeEventArgs> BarcodeReceived;
        public event EventHandler<ReaderStateEventArgs> ReaderStateNotification;
        public event EventHandler<BatteryStateEventArgs> BatteryStateNotification;
        public event EventHandler<SerialNumberEventArgs> SerialNumberEvent;
        public event EventHandler<LicenceKeyEventArgs> LicenceKeyEvent;
        

        protected virtual void OnTagFound(TransponderDataEventArgs e)
        {
            var handler = TransponderReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}