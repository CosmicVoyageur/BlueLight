using Android.App;
using Android.Content.PM;
using Android.OS;
using SoloFi.Droid.Glue;
using SoloFi.Views.HomeHub;
using XamlingCore.XamarinThings.Content.Navigation;

namespace SoloFi.Droid
{
    [Activity(Label = "SoloFi", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var xApp = new AppEntryXApplication();
            xApp.Init<XNavigationPageTypedViewModel<MainMenuViewModel>, ProjectGlue>();

            LoadApplication(xApp);
        }
    }
}

