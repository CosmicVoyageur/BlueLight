using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.Entity.CustomerSpecific.Iluka;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace SoloFi.Views.CustomerSpecific.Iluka.Parents
{
    public class ExportParentViewModel : XViewModel
    {
        private readonly IFileExportService _fileExportService;
        private readonly IParentCrateService _parentCrateService;
        private readonly INotifyService _notifyService;
        private string _fileName;

        public ExportParentViewModel(IFileExportService fileExportService, IParentCrateService parentCrateService, INotifyService notifyService)
        {
            _fileExportService = fileExportService;
            _parentCrateService = parentCrateService;
            _notifyService = notifyService;
        }

        public ICommand FileExportCommand { get; set; }

        public ParentCrate ReferenceParent { get; set; }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
            InitCommands();
            FileName = ReferenceParent.Name + ".csv";
        }

        private void InitCommands()
        {
            FileExportCommand = new Command(OnFileExport);
        }

        private async void OnFileExport()
        {
            var name = FileName;
            if (String.IsNullOrEmpty(FileName))
            {
                name = ReferenceParent.Name;
            }
            var data = await _parentCrateService.Serialise(ReferenceParent);
                await _fileExportService.SaveToFile(name, data);

        }
    }
}
