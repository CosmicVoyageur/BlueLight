using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.Views.CustomerSpecific.Iluka.Parents;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace SoloFi.Views.CustomerSpecific.Iluka
{
    public class IlukaHomeViewModel : XViewModel
    {
        private readonly IIlukaReaderService _readerService;
        private bool _isReaderLicenced;

        public IlukaHomeViewModel(IIlukaReaderService readerService)
        {
            _readerService = readerService;
        }

        public ICommand ViewParentsCommand { get; set; }
        public ICommand QuickScanCommand { get; set; }
        public ICommand FilesCommand { get; set; }
        public ICommand HelpCommand { get; set; }

        public bool IsReaderLicenced
        {
            get { return _isReaderLicenced; }
            set
            {
                _isReaderLicenced = value;
                OnPropertyChanged();
            }
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
            _initCommands();
        }

        public override void OnActivated()
        {
            base.OnActivated();
            VerifyReaderLicence();
        }

        private async void VerifyReaderLicence()
        {
            IsReaderLicenced = await _readerService.VerifyReaderLicence();
        }

        private void _initCommands()
        {
            ViewParentsCommand = new Command(()=>NavigateTo<ListParentsViewModel>());
            QuickScanCommand = new Command(()=> NavigateTo<QuickPickViewModel>());
            FilesCommand = new Command(()=>NavigateTo<FilesViewModel>());
            HelpCommand = new Command(()=>NavigateTo<IlukaHelpViewModel>());
        }
    }
}
