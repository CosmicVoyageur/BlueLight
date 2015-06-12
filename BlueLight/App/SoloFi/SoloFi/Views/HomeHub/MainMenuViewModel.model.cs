using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Services;
using SoloFi.Model.Config;
using SoloFi.Views.CustomerSpecific.Iluka;
using SoloFi.Views.HomeHub.Extras;
using SoloFi.Views.Ramp;
using SoloFi.Views.TagHub;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace SoloFi.Views.HomeHub
{
    public class MainMenuViewModel : XViewModel
    {
        public string CustomerName
        {
            get { return ClientNames.CurrentClient; } 
        }

        public MainMenuViewModel()
        {
            
        }

        #region Commands

        public ICommand ConnectToReaderCommand { get; set; }
        public ICommand TagListCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ICommand SpecificCustomerCommand { get; set; }

        #endregion


        public override void OnInitialise()
        {
            base.OnInitialise();
            _initCommands();
        }

        private void _initCommands()
        {
            ConnectToReaderCommand = new Command(()=> NavigateTo<ReaderSelectViewModel>());
            TagListCommand = new Command(()=>NavigateTo<TagListViewModel>());
            SettingsCommand = new Command(()=>NavigateTo<SettingsViewModel>());
            HelpCommand = new Command(()=> NavigateTo<HelpViewModel>());
            // customer specific
            SpecificCustomerCommand = new Command(NavigateToCustomerHome);
        }

        private void NavigateToCustomerHome()
        {
            if (ClientNames.CurrentClient == ClientNames.Iluka) NavigateTo<IlukaHomeViewModel>();
            if(ClientNames.CurrentClient == ClientNames.Ramp) NavigateTo<ReaderLicencingViewModel>();
            //if (CustomerName == CustomerNames.Screenex) NavigateTo<ScreenexHomeViewModel>();
        
        }
    }
}
