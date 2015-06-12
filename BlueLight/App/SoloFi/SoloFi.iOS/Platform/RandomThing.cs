using System.Threading.Tasks;
using iQMobile2.Contract.Platform;
using UIKit;
using Xamarin.Forms;

namespace iQMobile2.iOS.Platform
{
    public class RandomThing : IRandomInterface
    {
        public async Task DoSOmething(int counter, bool ok)
        {
            var _error = new UIAlertView("My Title Text", "This is my IOS Alert", null, "Ok", null);

            _error.Show();
            //var b = new AlertDialog.Builder(Forms.Context);
            //b.SetTitle("An Alert");
            //b.SetMessage("A Mesasge");
            //b.SetPositiveButton("Yes", delegate { } );
            //b.Show();
        }
    }
}
