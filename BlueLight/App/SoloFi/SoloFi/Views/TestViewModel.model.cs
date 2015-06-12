using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services;
using SoloFi.Entity;
using SoloFi.Views.HomeHub;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views
{
    class TestViewModel : DisplayListViewModel<ItemViewModel<Item>, Item>
    {
        private readonly INotifyService _notifyService;
        private readonly IEntityCache _entityCache;
        public ICommand CommandOne { get; set; }
        public ICommand CommandTwo { get; set; }
        public ICommand GoToLoginCommand { get; set; }
        public ICommand GoToNewHomeCommand { get; set; }
        public ICommand DeleteLocalEntitiesCommand { get; set; }

        public TestViewModel(INotifyService notifyService, IEntityCache entityCache)
        {
            _notifyService = notifyService;
            _entityCache = entityCache;
            CommandOne = new Command(_notify);
            CommandTwo = new Command(_fiveSecondTask);
            
            GoToLoginCommand = new Command(_goToLogin);
            GoToNewHomeCommand = new Command(_goToNewHome);
            DeleteLocalEntitiesCommand = new Command(_deleteLocalEntities);
        }

        private async void _fiveSecondTask()
        {
            Dispatcher.Invoke(async ()=> await Loader(Wait(), "doing a thing"));
        }

        private async Task Wait()
        {
            await Task.Delay(5000);
            Debug.WriteLine("done the loading");
        }

        private void _waitThenLoadItems()
        {
            Task t = Task.Factory.StartNew(_waitABit);
            t.ContinueWith(_loadRandomItemsTask);
        }

        private void _loadRandomItemsTask(Task obj)
        {
            _loadRandomItems();
        }

        private void _waitABit()
        {
            Task.Delay(1000);
        }


        private void _deleteLocalEntities()
        {
            _entityCache.DeleteAll<Item>();
            _entityCache.DeleteAll<LookupEntity>();
            _entityCache.DeleteAll<Place>();
            _entityCache.DeleteAll<SmartTrackInventory>();
            _entityCache.DeleteAll<User>();
        }

        private void _goToNewHome()
        {
            NavigateTo<MainMenuViewModel>();
        }

        private void _goToLogin()
        {
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
            Items = new ObservableCollection<ItemViewModel<Item>>();
            _waitThenLoadItems();
        }

        private int _count = 0;
        private async void _loadRandomItems()
        {
            
        }

        private void _notify()
        {
            _notifyService.ToastNotificationWithPicture("notify title", "notify message", "http://exmoorpet.com/wp-content/uploads/2012/08/cat.png");
        }


    }
}