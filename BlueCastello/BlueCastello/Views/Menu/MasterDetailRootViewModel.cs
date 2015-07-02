using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCastello.Views.Main;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace BlueCastello.Views.Menu
{
    public class MasterDetailRootViewModel : XMasterDetailViewModel
    {
        public MasterDetailRootViewModel(IViewResolver viewResolver) : base(viewResolver)
        {
            Title = "Master";
        }

        public override void OnInitialise()
        {
            NavigationTint = Color.Silver;

            AddPackage<MainViewModel>();
            AddPackage<RGBViewModel>();

            SetMaster(CreateContentModel<MasterDetailMenuViewModel>());
            Build();

            base.OnInitialise();
        }
    }
}
