using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.CustomViews;
using SoloFi.Entity.CustomerSpecific.Iluka;
using SoloFi.Tiles.CustomerSpecific.Iluka;
using SoloFi.Views.CustomerSpecific.Iluka.Parents;
using SoloFi.Views.HomeHub.Extras;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views.CustomerSpecific.Iluka
{
    public class QuickPickViewModel : DisplayListViewModel<ItemViewModel<ChildSample>, ChildSample>
    {
        private readonly IIlukaReaderService _readerService;
        private readonly INotifyService _notifyService;
        private readonly IParentCrateService _parentService;

        public QuickPickViewModel(IIlukaReaderService readerService, INotifyService notifyService, IParentCrateService parentService)
        {
            _readerService = readerService;
            _notifyService = notifyService;
            _parentService = parentService;
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public override void OnInitialise()
        {
            base.OnInitialise();
            InitCommands();
            Items = new TrulyObservableCollection<ItemViewModel<ChildSample>>();
            _readerService.Initialise();
        }

        public override void OnActivated()
        {
            base.OnActivated();
            _readerService.TransponderReadEvent += _readerService_TransponderReadEvent;
            _readerService.BarcodeReadEvent += _readerService_BarcodeReadEvent;
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            _readerService.TransponderReadEvent -= _readerService_TransponderReadEvent;
            _readerService.BarcodeReadEvent -= _readerService_BarcodeReadEvent;
        }

        protected override async void OnItemSelected(ChildSample selectedItem)
        {
            base.OnItemSelected(selectedItem);
            var parent = await _parentService.GetCrateByTag(selectedItem.Transponder);
            if (parent != null)
            {
                _tempParent = parent;
                await _notifyService.AskYesOrCancel("Do you want to manage " + parent.Name, OnManageParent);
            }
            else
            {
                _tempSample = selectedItem;
                await
                    _notifyService.AskYesOrCancel("Create New Parent from " + selectedItem.SampleIdentifier + " ?", OnNewParent);
            }
           
        }

        private ParentCrate _tempParent;
        private void OnManageParent()
        {
            NavigateTo<ManageParentViewModel>(_=>_.ReferenceParent = _tempParent);
            _tempParent = null;
        }

        private ChildSample _tempSample;
        private async void OnNewParent()
        {
            var parent = ParentCrate.NewParent(_tempSample.SampleIdentifier);
            parent.Tag = _tempSample.Transponder;
            parent.Children.Add(_tempSample);
            NavigateTo<ManageParentViewModel>(_ => _.ReferenceParent = parent);
            _tempSample = null;
        }
        private void InitCommands()
        {
            EditCommand = new Command(OnEditSingle);
            DeleteCommand = new Command(OnDeleteSingle);
        }

        private void OnDeleteSingle(object obj)
        {
            var vm = obj as IlukaSampleTileViewModel;
            if (vm == null) return;
            Dispatcher.Invoke(() => Items.Remove(vm));
        }

        private void OnEditSingle(object obj)
        {
            var vm = obj as IlukaSampleTileViewModel;
            if (vm == null) return;
            NavigateTo<EditTransponderViewModel>(_ => _.ReferenceTag = vm.Item.Transponder);
        }

        void _readerService_BarcodeReadEvent(object sender, Contract.EventArgs.BarcodeEventArgs e)
        {
            UpdateListUi(ChildSample.CreateChild(e.Data));

        }

        void _readerService_TransponderReadEvent(object sender, Tsl.AsciiProtocol.Pcl.TransponderDataEventArgs e)
        {
            var tag = IlukaUhfTag.ConvertTag(e.Transponder);
            var sample = ChildSample.CreateChild(tag);
            UpdateListUi(sample);
        }

        private void UpdateListUi(ChildSample sampleIdentifier)
        {
            if (sampleIdentifier == null) return;
            var vm = CreateContentModel<IlukaSampleTileViewModel>(_ =>
            {
                _.Item = sampleIdentifier;
                _.DeleteCommand = this.DeleteCommand;
                _.EditCommand = this.EditCommand;
            });

            Dispatcher.Invoke(() =>
            {
                if (vm.Item == null) return;
                if (!Items.Contains(vm)) Items.Add(vm);
                else
                {
                    var firstVm = Items.FirstOrDefault(
                        a =>
                            a.Item !=null &&
                            a.Item.Transponder != null &&
                            vm.Item.Transponder != null &&
                            a.Item.Transponder.TagNumber == vm.Item.Transponder.TagNumber);
                    if (firstVm != null && vm.Item.Transponder.UserMemoryHex != null && !String.Equals(firstVm.Item.Transponder.UserMemoryHex, vm.Item.Transponder.UserMemoryHex))
                    {
                        var index = Items.IndexOf(firstVm);
                        Items.Remove(firstVm);
                        Items.Insert(index, vm);
                    }
                }
            });
        }
    }
}
