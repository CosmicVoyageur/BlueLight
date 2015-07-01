using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloFi.CustomViews;
using SoloFi.Tiles.Menu;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views
{
    public class MasterDetailMenuViewModel : DisplayListViewModel<MenuOptionTileViewModel, XViewModel>
    {
        public override void OnActivated()
        {
            _refreshList();
            base.OnActivated();
        }

        private void _refreshList()
        {
            if (DataList == null)
            {
                return;
            }

            Items = new TrulyObservableCollection<MenuOptionTileViewModel>();

            foreach (var item in DataList)
            {
                var i = CreateContentModel<MenuOptionTileViewModel>();

                i.Item = item;
                i.Title = item.Title;

                Items.Add(i);
            }

            UpdateItemCount();
        }
    }
}
