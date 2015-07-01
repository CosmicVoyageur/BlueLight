using System.Threading.Tasks;
using Android.App;
using BlueLight.Contract.Platform;
using Xamarin.Forms;

namespace BlueLight.Droid.Platform
{
    public class RandomThing : IRandomInterface
    {
        public async Task DoSOmething(int counter, bool ok)
        {
            var b = new AlertDialog.Builder(Forms.Context);
            b.SetTitle("An Alert");
            b.SetMessage("A Mesasge");
            b.SetPositiveButton("Yes", delegate { } );
            b.Show();
            
        }
    }
}
