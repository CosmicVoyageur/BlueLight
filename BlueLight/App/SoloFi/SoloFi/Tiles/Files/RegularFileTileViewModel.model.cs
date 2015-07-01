using System.Windows.Input;
using BlueLight.Entity;
using XamlingCore.Portable.View.ViewModel.Base;

namespace BlueLight.Tiles.Files
{
    public class RegularFileTileViewModel : ItemViewModel<FileRepresentation>
    {
        public ICommand DeleteCommand { get; set; }
        public ICommand ShareCommand { get; set; }
    }
}
