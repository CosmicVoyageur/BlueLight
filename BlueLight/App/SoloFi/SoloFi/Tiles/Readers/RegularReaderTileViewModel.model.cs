using SoloFi.Entity.RFID;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Tiles.Readers
{
    class RegularReaderTileViewModel : ItemViewModel<Reader>
    {
        public RegularReaderTileViewModel()
        {
            BackgroundColor = Color.Transparent;
        }

        public Color BackgroundColor { get; set; }
    }
}
