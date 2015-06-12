using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using SoloFi.Contract.Platform;
using Tsl.AsciiProtocol.Pcl;

namespace SoloFi.Droid.Platform
{
    class BluetoothRfidReaderDiscoveryService :IBluetoothRfidReaderDiscoveryService
    {
        public async Task<IEnumerable<string>> GetPairedBtDevices()
        {
            var devices = BluetoothAdapter.DefaultAdapter.BondedDevices;
            return devices.Select(device => device.Name).ToList();
        }

        public async Task<IAsciiSerialPort> GetAsciiSerialPort(string deviceName)
        {
            var devices = BluetoothAdapter.DefaultAdapter.BondedDevices;
            var device = devices.FirstOrDefault((a) => a.Name == deviceName);
            var serialPort = new AndroidAsciiSerialService(device);
            return serialPort;
        }


        /// <summary>
        /// Checks if BT is enabled. If not, calls dialog
        /// </summary>
        private void TryEnableBluetooth()
        {
            BluetoothAdapter BtAdapter = BluetoothAdapter.DefaultAdapter;
            if (!BtAdapter.IsEnabled)
            {
                EnableBluetoothDialog();
            }
        }
        /// <summary>
        /// Asks user to enable Bluetooth
        /// </summary>
        private void EnableBluetoothDialog()
        {
            Android.App.AlertDialog.Builder builder = new AlertDialog.Builder(Application.Context);
            AlertDialog alert = builder.Create();
            alert.SetTitle("Write to Transponder");
            // build warning message

            alert.SetMessage("Enable Bluetooth?");
            alert.SetButton("No", CancelClicked);
            alert.SetButton2("Yes", EnableBluetooth);

            alert.Show();
        }

        private static void EnableBluetooth(object sender, DialogClickEventArgs e)
        {
            BluetoothAdapter.DefaultAdapter.Enable();
        }
        private static void CancelClicked(object sender, DialogClickEventArgs e)
        {
            // do nothing
        }
    }
}