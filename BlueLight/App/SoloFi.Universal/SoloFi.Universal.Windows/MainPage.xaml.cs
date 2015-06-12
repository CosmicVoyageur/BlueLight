using SoloFi.Universal.Glue;
using SoloFi.Views.HomeHub;
using Xamarin.Forms.Platform.WinRT;
using XamlingCore.XamarinThings.Content.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SoloFi.Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            var xApp = new AppEntryXApplication();
            
            xApp.Init<XNavigationPageTypedViewModel<MainMenuViewModel>, ProjectGlue>();
            LoadApplication(xApp);
        }
    }
}
