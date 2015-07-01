using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using BlueLight.Contract.Platform;

namespace BlueLight.Droid.Platform
{
    public class FileShareService : IFileShareService
    {

        public async Task ShareFilesPreferEmail(List<string> paths, string subject)
        {
            Intent intent =
                    new Intent(Intent.ActionSendMultiple);
            //emailIntent.SetType("message/rfc822");
            intent.SetType("text/csv");
            intent.PutExtra(Intent.ExtraSubject, subject);
            intent.SetFlags(ActivityFlags.NewTask);
            var uris = new List<IParcelable>();
            paths.ForEach(file =>
            {
                var fileIn = new Java.IO.File(file);
                var uri = Android.Net.Uri.FromFile(fileIn);
                uris.Add(uri);
            });

            intent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
            Application.Context.StartActivity(intent);
            
        }
}
}