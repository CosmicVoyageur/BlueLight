using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;
using SoloFi.Contract.Config;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services;
using SoloFi.Contract.Services.Utilities;
using SoloFi.Entity;
using SoloFi.Model.RFID.CustomEventArgs;
using Tsl.AsciiProtocol.Pcl;
using Tsl.AsciiProtocol.Pcl.Commands;

namespace SoloFi.Model.Services.Utilities
{
    public class AdminTslLicencingService : BaseRfidReaderService, IAdminTslLicencingService
    {
        private readonly IClientConfigService _clientConfigService;

        public AdminTslLicencingService(IBluetoothRfidReaderDiscoveryService bluetoothRfidReaderDiscoveryService, IClientConfigService clientConfigService)
            : base(bluetoothRfidReaderDiscoveryService,clientConfigService)
        {
            _clientConfigService = clientConfigService;
        }

        private string _customerName;

        public async void SetLicenceKey(string customerName)
        {
            _customerName = customerName;
            Responder.SerialNumberEvent +=OnSerialNumberHandler_Set;


            var version = new VersionInformationCommand();
            Commander.ExecuteCommand(version, null);
        }

        private string _serialNumber;
        void OnSerialNumberHandler_Set(object sender, RFID.CustomEventArgs.SerialNumberEventArgs e)
        {
            Responder.SerialNumberEvent -= OnSerialNumberHandler_Set; // remove this method from handler

            ExecuteSetLicenceKeyCommand(e.SerialNumber);
        }

        private void ExecuteSetLicenceKeyCommand(string serialNumber)
        {
            _serialNumber = serialNumber;
            var command = new LicenceKeyCommand
            {
                DeleteKey = Deletion.Yes,
                LicenceKey = _clientConfigService.ComputeHash(_serialNumber, _customerName)
            };
            Commander.ExecuteCommand(command, null);
        }

        public async Task<bool> IsLicenceValid(string customerName)
        {
            _customerName = customerName;
            _licenceCheckComplete = false;
            _licenceIsValid = false;
            Responder.SerialNumberEvent += OnSerialNumberHandler_Verify;


            var version = new VersionInformationCommand();
            Commander.ExecuteCommand(version, null);

            int i = 0;
            while (!_licenceCheckComplete && i < 60) // wait max 15 seconds
            {
                await Task.Delay(250);
                i++;
            }
            return _licenceIsValid;

        }

        private bool _licenceCheckComplete;
        private bool _licenceIsValid;

        void OnSerialNumberHandler_Verify(object sender, SerialNumberEventArgs e)
        {
            Responder.SerialNumberEvent -= OnSerialNumberHandler_Verify; // remove this method from handler
            Responder.LicenceKeyEvent += VerifyLicenceHandler;
            _serialNumber = e.SerialNumber;
            var command = new LicenceKeyCommand();
            Commander.ExecuteCommand(command,null);

        }

        void VerifyLicenceHandler(object sender, LicenceKeyEventArgs e)
        {
            _licenceIsValid = String.Equals(e.LicenceKey,_clientConfigService.ComputeHash(_serialNumber,_customerName));
            _licenceCheckComplete = true;
        }


    }
}
