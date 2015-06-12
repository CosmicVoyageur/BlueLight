using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoloFi.Contract.Config;
using SoloFi.Contract.EventArgs;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services;
using SoloFi.Entity.RFID;
using SoloFi.Model.RFID;
using SoloFi.Model.RFID.CustomEventArgs;
using Tsl.AsciiProtocol.Pcl;
using Tsl.AsciiProtocol.Pcl.Commands;

namespace SoloFi.Model.Services
{
    public class BaseRfidReaderService : IRfidReaderService
    {
        protected readonly IBluetoothRfidReaderDiscoveryService BluetoothRfidReaderDiscoveryService;
        private readonly IClientConfigService _clientConfigService;
        protected readonly AsciiCommander Commander = new AsciiCommander();
        protected readonly CustomResponder Responder = new CustomResponder();

        private Reader _currentReader;

        public BaseRfidReaderService(IBluetoothRfidReaderDiscoveryService bluetoothRfidReaderDiscoveryService, IClientConfigService clientConfigService)
        {
            BluetoothRfidReaderDiscoveryService = bluetoothRfidReaderDiscoveryService;
            _clientConfigService = clientConfigService;
        }

        public async Task<bool> VerifyLicence()
        {
            if (await IsReaderConnected(_currentReader))
            {
                var key = await GetLicencekey();
                var serialNumber = await GetSerialNumber();
                var computedKey = _clientConfigService.ComputeHash(serialNumber);
                return key==computedKey;
            }
            return false;
        }

        protected async Task<string> GetLicencekey()
        {
            var command = new LicenceKeyCommand();
            Responder.LicenceKeyEvent += OnLicenceKeyEvent;
            Commander.ExecuteCommand(command,null);
            while (!LicenceKeyRetrieved)
            {
                await Task.Delay(250);
            }
            Responder.LicenceKeyEvent -= OnLicenceKeyEvent;
            return LicenceKey;
        }

        private bool LicenceKeyRetrieved;
        private string LicenceKey;
        void OnLicenceKeyEvent(object sender, LicenceKeyEventArgs e)
        {
            LicenceKey = e.LicenceKey;
            LicenceKeyRetrieved = true;
        }

        protected async Task<string> GetSerialNumber()
        {
            var version = new VersionInformationCommand();
            Responder.SerialNumberEvent += OnSerialNumberEvent;
            Commander.ExecuteCommand(version, null);
            while (!SerialNumberRetrieved)
            {
                await Task.Delay(250);
            }
            Responder.SerialNumberEvent -= OnSerialNumberEvent;
            return SerialNumber;
        }
        private bool SerialNumberRetrieved;
        private string SerialNumber;
        void OnSerialNumberEvent(object sender, SerialNumberEventArgs e)
        {
            throw new NotImplementedException();
        }
        

        public async void AlertForGeiger(int proximity) // 0 is closest, 3+ is furthest
        {
            var command = new AlertCommand
            {
                AlertDuration = AlertDuration.Short,
                BuzzerEnabled = TriState.Yes,
                VibrateEnabled = TriState.No
            };
           
            switch (proximity)
            {
                case 0:
                    command.VibrateEnabled = TriState.Yes;
                    command.BuzzerTone = BuzzerTone.High;
                    break;
                case 1:
                    command.BuzzerTone = BuzzerTone.High;
                    break;
                case 2:
                    command.BuzzerTone = BuzzerTone.Medium;
                    break;
                case 3:
                    command.BuzzerTone = BuzzerTone.Low;
                    break;
                default:
                    command.BuzzerTone = BuzzerTone.Low;
                    break;
            }

            Commander.ExecuteCommand(command, null);
        }

        public async void TriggerStateEvent()
        {
            if (!await IsReaderConnected(_currentReader)) return;
            var command = new InventoryCommand
            {
                TakeNoAction = true,
                ReadParameters = true
            };
            Commander.ExecuteCommand(command,null);
        }

        public async void RequestDesiredState(ReaderState state)
        {
            if (! await IsReaderConnected(_currentReader)) return;
            _inventoryOnlyArg = state.GeigerMode ? " -iooff" : " -ioon";
            _alertArg = state.GeigerMode ? " -aloff" : " -alon";
            _setOutputPower(state.TransmitPower);
            _initTriggerAsInventory();
        }

        private void _setOutputPower(int transmitPower)
        {
            var command = new InventoryCommand
            {
                TakeNoAction = true,
                OutputPower = transmitPower
            };
            Commander.ExecuteCommand(command,null);
        }

        public async void RequestBatteryStatus()
        {
            if (!await IsReaderConnected(_currentReader)) return;
            var command = new BatteryStatusCommand();
            Commander.ExecuteCommand(command,null);
        }

        public async Task<IEnumerable<string>> GetPairedDevices()
        {
            return await BluetoothRfidReaderDiscoveryService.GetPairedBtDevices();
        }

        private string _deviceName;
        public async Task<bool> Connect(string deviceName)
        {
            _deviceName = deviceName;
            await BluetoothRfidReaderDiscoveryService.GetAsciiSerialPort(deviceName)
                .ContinueWith(_initCommander)
                .ContinueWith(_initReaderState)
                .ContinueWith(_initTriggerCommand);

            _currentReader = new Reader {Id = Guid.NewGuid(), Name = deviceName};
            if (await IsReaderConnected(_currentReader))OnReaderStateNotification(this, new ReaderStateEventArgs
                {
                    State = new ReaderState(20)
                });
            return await IsReaderConnected(_currentReader);
        }

        public async Task<bool> IsReaderConnected(Reader reader)
        {
            return (reader.Name == _deviceName && Commander.IsConnected);
        }
        public async Task<bool> IsReaderConnected()
        {
            return Commander.IsConnected;
        }

        private void _initReaderState(Task obj)
        {
            var command = new BatteryStatusCommand();
            Commander.ExecuteCommand(command,null);
        }

        private void _initTriggerCommand(Task obj)
        {
            _initTriggerAsInventory();
        }

        private void _initCommander(Task<IAsciiSerialPort> obj)
        {
            Commander.Connect(obj.Result);
            Commander.AddResponder(Responder);

            Responder.ReaderStateNotification += (s, e) => OnReaderStateNotification(this, e);
            Responder.BatteryStateNotification += (s, e) => OnBatteryStateNotification(this, e);
        }

        private void OnReaderStateNotification(object sender, ReaderStateEventArgs readerStateEventArgs)
        {
            if (StateEvent != null)
            {
                StateEvent(this, readerStateEventArgs);
            }
        }

        void OnTagReceived(object sender, TagEventArgs e)
        {
            if (TagEvent != null)
            {
                TagEvent(this, e);
            }
        }

        void OnBatteryStateNotification(object sender, BatteryStateEventArgs e)
        {
            if (BatteryEvent != null)
            {
                BatteryEvent(this, e);
            }
        }


        public event EventHandler<TagEventArgs> TagEvent; // TODO: hook up to barcode event
        public event EventHandler StateEvent; // TODO: Add state event from and Transmit Pwr Response
        public event EventHandler BatteryEvent;

        private string _alertArg;
        private string _inventoryOnlyArg;
        private void _initTriggerAsInventory()
        {
            if (Commander == null || !Commander.IsConnected) return; // check if we are connected before executing
            var command = new SwitchSinglePressUserActionCommand { SinglePressUserAction = ".iv -ron" + _alertArg + _inventoryOnlyArg };
            Commander.ExecuteCommand(command, null);

            var command2 = new SwitchActionCommand {SinglePressAction = SwitchAction.User};
            Commander.ExecuteCommand(command2, null);
        }
    }
}
