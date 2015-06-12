using System.Diagnostics;
using SoloFi.Entity.RFID;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Tiles.Tags
{
    public class RegularTagTileViewModel : ItemViewModel<Tag>
    {
        public override bool Equals(object obj)
        {
            // pass reference equality down to item
            var tagVm = obj as RegularTagTileViewModel;
            if (tagVm == null) return false;
            if (Item.Equals(tagVm.Item))
            {
                Debug.WriteLine("Equal!");
                return true;
            }
            return false;
        }
    }
}
