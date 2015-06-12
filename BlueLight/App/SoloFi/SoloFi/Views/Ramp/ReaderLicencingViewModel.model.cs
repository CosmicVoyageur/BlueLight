using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Config;
using SoloFi.Contract.Services.Utilities;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace SoloFi.Views.Ramp
{
    public class ReaderLicencingViewModel : XViewModel
    {
        private readonly IAdminTslLicencingService _adminTslLicencingService;
        private readonly IClientConfigService _clientConfigService;
        private bool _isLicenceValid;
        private string _message;

        public ReaderLicencingViewModel(IAdminTslLicencingService adminTslLicencingService, IClientConfigService clientConfigService)
        {
            _adminTslLicencingService = adminTslLicencingService;
            _clientConfigService = clientConfigService;
        }

        public List<string> ClientList { get; set; }
        public string SelectedClient { get; set; }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public bool IsLicenceValid
        {
            get { return _isLicenceValid; }
            set
            {
                _isLicenceValid = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetLicenceCommand { get; set; }
        public ICommand VerifyLicenceCommand { get; set; }

        public override void OnInitialise()
        {
            base.OnInitialise();
            InitCommands();
            InitClientList();
        }

        private async void InitClientList()
        {
            ClientList = await _clientConfigService.GetAllClientNames();
            Message = "Choose client to licence";
        }

        private void InitCommands()
        {
            SetLicenceCommand = new Command(OnSetLicence);
            VerifyLicenceCommand = new Command(OnVerifyLicence);
        }

        private async void OnVerifyLicence()
        {
            IsLicenceValid = await _adminTslLicencingService.IsLicenceValid(SelectedClient);
            if (IsLicenceValid) Message = "Valid Licence!";
            else Message = "Invalid Licence :(";
        }

        private void OnSetLicence()
        {
            Message = "Setting licence for " + SelectedClient;
            if (String.IsNullOrEmpty(SelectedClient)) return;
            _adminTslLicencingService.SetLicenceKey(SelectedClient);
        }

        
    }
}
