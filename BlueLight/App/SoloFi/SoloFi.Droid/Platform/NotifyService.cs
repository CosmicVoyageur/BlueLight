using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using SoloFi.Contract.Platform;

namespace SoloFi.Droid.Platform
{
    public class NotifyService : INotifyService
    {
        public void ToastNotificationWithPicture(string title, string message, string imageUrl)
        {
            Toast.MakeText(Application.Context, title + "\n" + message, ToastLength.Short).Show();

        }

        public async Task AskYesOrCancel(string question, Action callback)
        {
            callbackAction = callback;
            AlertDialog.Builder builder = new AlertDialog.Builder(Xamarin.Forms.Forms.Context);
            builder.SetTitle("Input Required");
            
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