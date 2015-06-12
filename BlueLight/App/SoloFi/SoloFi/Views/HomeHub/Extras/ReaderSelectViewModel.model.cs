using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services;
using SoloFi.Entity.RFID;
using SoloFi.Model.RFID.CustomEventArgs;
using SoloFi.Tiles.Readers;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views.HomeHub.Extras
{
    public class ReaderSelectViewModel : DisplayListViewModel<ItemViewModel<Reader>, Reader>
    {
        private readonly IRfidReaderService _readerService;
        private readonly INotifyService _notifyService;
        private const string ReaderImgUrl = @"http://www.xda-developers.com/wp-content/uploads/2012/09/bluetooth.png";

        public ICommand DoneCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public ReaderSelectViewModel(IRfidReaderService readerService, INotifyService notifyService)
        {
            _readerService = readerService;
            _notifyService = notifyService;
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
            _init();
            _initNavigationCommands();
            _initReaderConnectedNotification();
        }

        private void _initNavigationCommands()
        {
            DoneCommand = new Command(NavigateBack);
            BackCommand = new Command(NavigateBack);
        }

        private void _initReaderConnectedNotification()
        {
            _readerService.StateEvent += _readerService_StateEvent;
        }

        void _readerService_StateEvent(object sender, EventArgs e)
        {
            var typedArgs = e as ReaderStateEventArgs;
            if (typedArgs == null) return;
            Dispatcher.Invoke(()=> _notifyService.ToastNotificationWithPicture("Reader Connected", "", ReaderImgUrl)); //TODO: change to localised string
        }

        protected override async void OnItemSelected(Reader selectedItem)
        {
            base.OnItemSelected(selectedItem);
            try
            {
                await Loader(_readerService.Connect(selectedItem.Name), "Connecting...");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _notifyService.ToastNotificationWithPicture("Failure","The Bluetooth Reader did not respond as expected","");
            }
        }
        
        private async void _init()
        {
            Items = new ObservableCollection<ItemViewModel<Reader>>();
            foreach (var r in from name in await _readerService.GetPairedDevices() select new Reader {Id = new Guid(), Name = name})
            {
                var connected = await _readerService.IsReaderConnected(r);
                var r1 = r;
                var vm = CreateContentModel<RegularReaderTileViewModel>(_ =>
                {
                    _.Item = r1; // closure
                    if (connected)
                    {
                        _.BackgroundColor = Color.Green;
                    }
                });
                Dispatcher.Invoke(()=> Items.Add(vm));
            }
        }
    }
}
