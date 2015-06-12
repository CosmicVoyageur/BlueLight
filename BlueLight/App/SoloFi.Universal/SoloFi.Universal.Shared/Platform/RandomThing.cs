using System;
using System.Threading.Tasks;
using SoloFi.Contract.Platform;

namespace SoloFi.Universal.Platform
{
    public class RandomThing : IRandomInterface
    {
        public async Task DoSOmething(int counter, bool ok)
        {
            await
                new Windows.UI.Popups.MessageDialog("Man why did they have to make this so complicated in Win 8")
                    .ShowAsync();
        }
    }
}
