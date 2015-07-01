using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace SoloFi.Views
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
