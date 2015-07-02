using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BlueCastello.Contract.Platform;

namespace BlueCastello.Android.Platform
{
    public class NotifyService : INotifyService
    {
        public void ToastNotificationWithPicture(string title, string message, string imageUrl=null)
        {
            Toast.MakeText(Application.Context, title + "\n" + message, ToastLength.Short).Show();

        }

        public async Task AskYesOrCancel(string question, Action callback)
        {
            callbackAction = callback;
            AlertDialog.Builder builder = new AlertDialog.Builder(Xamarin.Forms.Forms.Context);

            builder.SetTitle("Input Required");
            builder.SetMessage(question);
            builder.SetPositiveButton("Yes", handler);
            builder.SetNegativeButton("No", emptyHandler);
            var dialog = builder.Create();
            Xamarin.Forms.Device.BeginInvokeOnMainThread(dialog.Show);
        }

        private void emptyHandler(object sender, DialogClickEventArgs e)
        {

        }

        private Action callbackAction;
        private void handler(object sender, DialogClickEventArgs e)
        {
            callbackAction();
        }
    }
}