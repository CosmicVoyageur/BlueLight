using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueCastello.Tiles.Menu;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;

namespace BlueCastello.Views.Menu
{
    public class MasterDetailMenuViewModel : DisplayListViewModel<MenuOptionViewModel, XViewModel>
    {
        public override void OnActivated()
        {
            base.OnActivated();
            _refreshList();
        }

        private void _refreshList()
        {
            if (DataList == null)
            {
                return;
            }

            Items = new ObservableCollection<MenuOptionViewModel>();

            foreach (var item in DataList)
            {
                var i = CreateContentModel<MenuOptionViewModel>();

                i.Item = item;
                i.Title = item.Title;

                Items.Add(i);
            }

            UpdateItemCount();
        }
    }
}
