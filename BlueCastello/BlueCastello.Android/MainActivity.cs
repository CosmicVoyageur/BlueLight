using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BlueCastello.Android.Glue;
using BlueCastello.Views.Menu;
using Xamarin.Forms.Platform.Android;
using XamlingCore.XamarinThings.Content.Navigation;

namespace BlueCastello.Android
{
    [Activity(Label = "BlueCastello.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FormsApplicationActivity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var xApp = new App();
            xApp.Init<MasterDetailRootViewModel, ProjectGlue>();

            LoadApplication(xApp);

        }
    }
}

