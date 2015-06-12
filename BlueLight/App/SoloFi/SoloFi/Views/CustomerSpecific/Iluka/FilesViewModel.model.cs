using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Platform;
using SoloFi.CustomViews;
using SoloFi.Entity;
using SoloFi.Tiles.Files;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views.CustomerSpecific.Iluka
{
    public class FilesViewModel : DisplayListViewModel<ItemViewModel<FileRepresentation>,FileRepresentation>
    {
        private readonly IFileExportService _fileExportService;
        private readonly IFileShareService _fileShareService;
        private readonly INotifyService _notifyService;

        public FilesViewModel(IFileExportService fileExportService, IFileShareService fileShareService, INotifyService notifyService)
        {
            _fileExportService = fileExportService;
            _fileShareService = fileShareService;
            _notifyService = notifyService;
        }

        public ICommand ShareCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public override void OnInitialise()
        {
            base.OnInitialise();
            Items = new TrulyObservableCollection<ItemViewModel<FileRepresentation>>();
            InitCommands();
        }

        public override void OnActivated()
        {
            base.OnActivated();
            RenewList();
        }

        private async void RenewList()
        {
            var names = await _fileExportService.GetAllFileNames();
            Dispatcher.Invoke(Items.Clear);
            foreach (var vm in names.Select(name => new FileRepresentation {Name = name}).Select(fileRep => CreateContentModel<RegularFileTileViewModel>(
                _ =>
                {
                    _.Item = fileRep;
                    _.ShareCommand = this.ShareCommand;
                    _.DeleteCommand = this.DeleteCommand;
                })))
            {
                var vm1 = vm; // closure
                Dispatcher.Invoke(()=>Items.Add(vm1));
            }
        }

        private void InitCommands()
        {
            ShareCommand = new Command(OnShareCommand);
            DeleteCommand = new Command(OnDeleteCommand);
        }

        private void OnDeleteCommand(object obj)
        {
            var vm = obj as RegularFileTileViewModel;
            if (vm == null) return;
            Delete(vm.Item);
            Dispatcher.Invoke(()=>Items.Remove(vm));
        }

        private void OnShareCommand(object obj)
        {
            var vm = obj as RegularFileTileViewModel;
            if (vm == null) return;
            Share(vm.Item);
        }


        protected override async void OnItemSelected(FileRepresentation selectedItem)
        {
            base.OnItemSelected(selectedItem);
            tempFile = selectedItem;
            await _notifyService.AskYesOrCancel("Share File?", OnShareCallback);
        }
        private FileRepresentation tempFile;
        private void OnShareCallback()
        {
            Share(tempFile);
        }

        private async void Delete(FileRepresentation file)
        {
            await _fileExportService.DeleteFile(file.Name);
        }

        private async void Share(FileRepresentation file)
        {
            var path = await _fileExportService.GetPathForSharing(file.Name);
            await _fileShareService.ShareFilesPreferEmail(new List<string>(){path}, "Data Export");
        }

        
        
    }
}
