using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.CustomViews;
using SoloFi.Entity.CustomerSpecific.Iluka;
using SoloFi.Tiles.CustomerSpecific.Iluka;
using SoloFi.Views.HomeHub.Extras;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views.CustomerSpecific.Iluka.Parents
{
    public class ManageParentViewModel : DisplayListViewModel<ItemViewModel<ChildSample>,ChildSample>
    {
        private readonly IParentCrateService _parentService;
        private readonly IIlukaReaderService _readerService;
        private readonly INotifyService _notifyService;
        private string _newParentName;

        public ManageParentViewModel(IParentCrateService parentService, IIlukaReaderService readerService, INotifyService notifyService)
        {
            _parentService = parentService;
            _readerService = readerService;
            _notifyService = notifyService;
        }

        public ICommand SettingsCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SetParentCommand { get; set; }

        public ParentCrate ReferenceParent { get; set; }

        public string NewParentName
        {
            get { return _newParentName; }
            set
            {
                _newParentName = value;
                OnPropertyChanged();
            }
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
            InitCommands();
            Items = new TrulyObservableCollection<ItemViewModel<ChildSample>>();
            AddReferenceChildrenToUi();
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
            var question = "Set " + selectedItem.SampleIdentifier + " as parent?";
            _tempSample = selectedItem;
            await _notifyService.AskYesOrCancel(question, OnSelectedParentCallback);
        }

        private ChildSample _tempSample;
        private void OnSelectedParentCallback()
        {
            if (_tempSample == null) return;
            var vm = Items.FirstOrDefault((a) => _tempSample.Equals(a.Item));
            var tvm = vm as IlukaSampleTileViewModel;
            if (tvm == null) return;
            SetParentVm(tvm);
        }

        private void InitCommands()
        {
            SettingsCommand= new Command(()=>NavigateTo<SettingsViewModel>());
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            ExportCommand = new Command(OnExport);
            EditCommand = new Command(OnEditSingle);
            DeleteCommand = new Command(OnDeleteSingle);
            SetParentCommand = new Command(OnSetParent);
        }

        private void OnSetParent(object obj)
        {
            var vm = obj as IlukaSampleTileViewModel;
            if (vm == null) return;
            SetParentVm(vm);
        }

        private void SetParentVm(IlukaSampleTileViewModel vm)
        {
            ReferenceParent.Tag = vm.Item.Transponder;
            NewParentName = vm.Item.Transponder.SampleIdentifier;
            RemoveOldParentIndicator();
            vm.IsParentTag = true;
        }

        private void RemoveOldParentIndicator()
        {
            foreach (var typedItem in Items.OfType<IlukaSampleTileViewModel>().Where(typedItem => typedItem.IsParentTag))
            {
                typedItem.IsParentTag = false;
            }
        }

        private void OnEditSingle(object obj)
        {
            var vm = obj as IlukaSampleTileViewModel;
            if (vm == null) return;
            NavigateTo<EditTransponderViewModel>(_=>_.ReferenceTag = vm.Item.Transponder);
        }

        private void OnDeleteSingle(object obj)
        {
            var vm = obj as IlukaSampleTileViewModel;
            if (vm == null) return;
            Dispatcher.Invoke(()=>Items.Remove(vm));
        }

        private void OnExport()
        {
            NavigateTo<ExportParentViewModel>(_=>_.ReferenceParent = this.ReferenceParent);
        }

        private void OnCancel()
        {
            _notifyService.AskYesOrCancel("Are you sure you want to exit?", NavigateBack);
        }

        private async void OnSave()
        {
            await _notifyService.AskYesOrCancel("Save?", SaveAndBack);
        }

        private async void SaveAndBack()
        {
            AddRemoveChildrenFromParent();
            if (!String.IsNullOrEmpty(NewParentName))
            {
                ReferenceParent.Name = NewParentName;
            }
            await _parentService.SaveParent(ReferenceParent);
            NavigateBack();
        }

        private void AddRemoveChildrenFromParent()
        {
            ReferenceParent.Children.Clear();
            foreach (var vm in Items)
            {
                ReferenceParent.Children.Add(vm.Item);
            }
        }

        private void AddReferenceChildrenToUi()
        {
            foreach (var child in ReferenceParent.Children)
            {
                UpdateListUi(child);
            }
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
                _.SetParentCommand = this.SetParentCommand;
                if (ReferenceParent.Tag != null && ReferenceParent.Tag.Equals(sampleIdentifier.Transponder))
                    _.IsParentTag = true;
            });
            
            Dispatcher.Invoke(() =>
            {
                if (vm.Item == null) return;
                if (!Items.Contains(vm)) Items.Add(vm);
                else
                {
                    var firstVm = Items.FirstOrDefault(
                        a =>
                            a.Item.Transponder != null &&
                            vm.Item.Transponder != null &&
                            a.Item.Transponder.TagNumber == vm.Item.Transponder.TagNumber);
                    if (firstVm != null && vm.Item.Transponder.UserMemoryHex!=null && !String.Equals(firstVm.Item.Transponder.UserMemoryHex,vm.Item.Transponder.UserMemoryHex))
                    {
                        var index = Items.IndexOf(firstVm);
                        Items.Remove(firstVm);
                        Items.Insert(index,vm);
                    }
                }
            });
        }

        
    }
}
