using System.Windows.Input;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.CustomViews;
using SoloFi.Entity.CustomerSpecific.Iluka;
using SoloFi.Tiles.CustomerSpecific.Iluka;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views.CustomerSpecific.Iluka.Parents
{
    public class ListParentsViewModel : DisplayListViewModel<ItemViewModel<ParentCrate>,ParentCrate>
    {
        private readonly IParentCrateService _parentService;

        public ListParentsViewModel(IParentCrateService parentService)
        {
            _parentService = parentService;
        }

        public ICommand NewParentCommand { get; set; }

        public override void OnInitialise()
        {
            base.OnInitialise();
            InitCommands();
        }

        public override void OnActivated()
        {
            base.OnActivated();
            Items = new TrulyObservableCollection<ItemViewModel<ParentCrate>>();
            RenewList();
        }

        protected override void OnItemSelected(ParentCrate selectedItem)
        {
            NavigateTo<ManageParentViewModel>(_=>_.ReferenceParent = selectedItem);
        }

        #region Private Methods

        private async void RenewList()
        {
            var parents = await _parentService.GetAllLocalParents();
            foreach (var parent in parents)
            {
                var p = parent;
                var vm = CreateContentModel<IlukaParentTileViewModel>(_ => _.Item = p);
                Dispatcher.Invoke(()=>Items.Add(vm));
            }
        }

        

        private void InitCommands()
        {
            NewParentCommand = new Command(OnNewParent);
        }

        private void OnNewParent()
        {
            NavigateTo<ManageParentViewModel>(_ => _.ReferenceParent = ParentCrate.NewParent("New Parent"));
        }

        #endregion

    }
}
