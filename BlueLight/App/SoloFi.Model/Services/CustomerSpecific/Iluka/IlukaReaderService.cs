using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloFi.Contract.Config;
using SoloFi.Contract.EventArgs;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.Entity.CustomerSpecific.Iluka;
using Tsl.AsciiProtocol.Pcl;
using Tsl.AsciiProtocol.Pcl.Commands;
using BarcodeEventArgs = SoloFi.Contract.EventArgs.BarcodeEventArgs;

namespace SoloFi.Model.Services.CustomerSpecific.Iluka
{
    public class IlukaReaderService : BaseRfidReaderService, IIlukaReaderService
    {
        private const int UserMemoryLength = 32;

        public IlukaReaderService(IBluetoothRfidReaderDiscoveryService bluetoothRfidReaderDiscoveryService, IClientConfigService clientConfigService) 
            : base(bluetoothRfidReaderDiscoveryService, clientConfigService)
        {
            
        }

        public async void Initialise()
        {
            SetSingleTriggerPressToReadUserMemory();
            SetDoubleTriggerPressToReadBarcode();
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            Responder.TransponderReceived += Responder_TransponderReceived;
            Responder.BarcodeReceived += Responder_BarcodeReceived;
        }

        void Responder_BarcodeReceived(object sender, BarcodeEventArgs e)
        {
            var handler = BarcodeReadEvent;
            if (handler == null) return;
            handler(this, e);
        }

        void Responder_TransponderReceived(object sender, TransponderDataEventArgs e)
        {
            var handler = TransponderReadEvent;
            if (handler == null) return;
            handler(this, e);
        }

        public async Task<bool> VerifyReaderLicence()
        {
            return await base.VerifyLicence();
        }

        public bool WriteToTransponderUserMemory(IlukaUhfTag select, IlukaUhfTag update)
        {
            if (Commander == null || !Commander.IsConnected) return false; // checks if connected
            ZeroPadToLength(update);
            var write = new WriteSingleTransponderCommand
            {
                Bank = Databank.User,
                Data = update.UserMemoryHex,
                Length = update.UserMemoryHex.Length / 4,
                SelectBank = Databank.ElectronicProductCode,
                SelectData = @select.TagNumber,
                SelectLength = 96,
                SelectOffset = 32
            };

            // epc is always 96 length
            // offset is always 32 when selecting epc

            Commander.ExecuteCommand(write, null);
            return true;
        }

        public event EventHandler<TransponderDataEventArgs> TransponderReadEvent;
        public event EventHandler<BarcodeEventArgs> BarcodeReadEvent;

        private bool SetSingleTriggerPressToReadUserMemory()
        {
            if (Commander == null || !Commander.IsConnected) return false; // check if we are connected before executing
            var command = new SwitchSinglePressUserActionCommand { SinglePressUserAction = ".rd -alon -sbusr -dbusr -dl18" };
            Commander.ExecuteCommand(command, null);

            var command2 = new SwitchActionCommand { SinglePressAction = SwitchAction.User };
            Commander.ExecuteCommand(command2, null);
            return true;
        }

        private bool SetDoubleTriggerPressToReadBarcode()
        {
            if (Commander == null || !Commander.IsConnected) return false; // check if we are connected before executing
            var command = new SwitchDoublePressUserActionCommand { DoublePressUserAction = ".bc -t1" }; // 1 second read delay
            Commander.ExecuteCommand(command, null);

            var command2 = new SwitchActionCommand { DoublePressAction = SwitchAction.User };
            Commander.ExecuteCommand(command2, null);
            return true;
        }

        

        private static void ZeroPadToLength(IlukaUhfTag tag)
        {
            tag.UserMemoryHex = tag.UserMemoryHex.PadRight(UserMemoryLength, '0'); // null character
        }
    }
}
