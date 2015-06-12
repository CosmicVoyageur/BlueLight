using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Services;
using SoloFi.Entity;
using SoloFi.Model.RFID.CustomEventArgs;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.View.ViewModel;

namespace SoloFi.Views.HomeHub.Extras
{
    public class SettingsViewModel : XViewModel
    {
        //commands
        public ICommand DeleteLocalDataCommand { get; set; }
        // bindable properties
        public string ReaderPowerText { get; set; }
        public string ReaderBatteryPercentage { get; set; }

        public string ServerBaseUrl
        {
            get { return _serverBaseUrl; }
            set
            {
                _serverBaseUrl = value;
                OnPropertyChanged();
            }
        }

        public string ReaderStatusText
        {
            get { return _readerStatusText; }
            set
            {
                _readerStatusText = value;
                OnPropertyChanged();
            }
        }

        public Color ReaderStatusColour
        {
            get { return _readerStatusColour; }
            set
            {
                _readerStatusColour = value;
                OnPropertyChanged();
            }
        }

        private readonly IRfidReaderService _readerService;
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IEntityCache _entityCache;
        private string _readerStatusText;
        private Color _readerStatusColour;

        public SettingsViewModel(IRfidReaderService readerService, IApplicationSettingsService applicationSettingsService, IEntityCache entityCache )
        {
            _readerService = readerService;
            _applicationSettingsService = applicationSettingsService;
            _entityCache = entityCache;
        }

        // reference properties
        private ApplicationSettingsEntity _applicationSettings;
        private string _serverBaseUrl;

        public override void OnInitialise()
        {
            base.OnInitialise();
            _initCommands();
            _initEventsFromReaderService();
            _fireRequestEvents();
            _tryGetSavedSettings();
        }

        private void _initCommands()
        {
            DeleteLocalDataCommand = new Command(_deleteAll);
        }

        private async void _deleteAll()
        {
            await Loader(_deleteLocalData(), "Removing Local Data");
        }

        private async Task _deleteLocalData()
        {
            try
            {
                await _entityCache.DeleteAll<Item>();
            }
            catch
            {
                
            }
            try
            {
                await _entityCache.DeleteAll<LookupEntity>();
            }
            catch
            {
                
            }
            try
            {
                await _entityCache.DeleteAll<Place>();
            }
            catch
            {
                
            }
            try
            {
                await _entityCache.DeleteAll<SmartTrackInventory>();
            }
            catch
            {
                
            }
            try
            {
                await _entityCache.DeleteAll<User>();
            }
            catch
            {
                
            }
        }

        private async void _tryGetSavedSettings()
        {
            _applicationSettings = await _applicationSettingsService.GetApplicationSettings();
            ServerBaseUrl = _applicationSettings.ServerBaseUrl;
        }

        private void _initEventsFromReaderService()
        {
            _readerService.BatteryEvent += _readerService_BatteryEvent;
        }

        void _readerService_BatteryEvent(object sender, EventArgs e)
        {
            var typedArgs = e as BatteryStateEventArgs;
            if (typedArgs == null) return;
            ReaderBatteryPercentage = typedArgs.State.StateOfCharge + "%";
        }

        private async void _fireRequestEvents()
        {
            if (await _readerService.IsReaderConnected())
            {
                OnReaderIsConnected();
            }
            else
            {
                OnReaderIsDisconnected();
            }
        }

        private void OnReaderIsDisconnected()
        {
            ReaderStatusColour = Color.Red;
            ReaderStatusText = "Disconnected";
        }

        private void OnReaderIsConnected()
        {
            _readerService.RequestBatteryStatus();
            ReaderStatusColour = Color.Green;
            ReaderStatusText = "Connected";
            
        }

    }
}
