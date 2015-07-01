using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace BlueLight.Views.Menu
{
    public class MasterDetailRootViewModel : XMasterDetailViewModel
    {

        public MasterDetailRootViewModel(IViewResolver viewResolver)
            : base(viewResolver)
        {
        }

        public override async void OnInitialise()
        {
            NavigationTint = Color.Silver;

            AddPackage<MainViewModel>();

            SetMaster(CreateContentModel<MasterDetailMenuViewModel>());
            Build();

            base.OnInitialise();

        }
    }
}
