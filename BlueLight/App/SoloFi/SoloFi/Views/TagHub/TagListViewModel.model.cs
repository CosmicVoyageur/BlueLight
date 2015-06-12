using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloFi.Contract.EventArgs;
using SoloFi.Contract.Services;
using SoloFi.CustomViews;
using SoloFi.Entity.RFID;
using SoloFi.Tiles.Tags;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Views.TagHub
{
    public class TagListViewModel : DisplayListViewModel<ItemViewModel<Tag>,Tag>
    {
        private readonly IRfidReaderService _readerService;

        public TagListViewModel(IRfidReaderService readerService)
        {
            _readerService = readerService;
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
            Items = new TrulyObservableCollection<ItemViewModel<Tag>>();
        }

        public override void OnActivated()
        {
            base.OnActivated();
            _readerService.TagEvent += _readerService_TagEvent;
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            _readerService.TagEvent -= _readerService_TagEvent;
        }

        void _readerService_TagEvent(object sender, TagEventArgs e)
        {
            var tag = e.Tag;
            var vm = CreateContentModel<RegularTagTileViewModel>(_ => _.Item = tag);
            Dispatcher.Invoke(() =>
            {
                if (!Items.Contains(vm))
                {
                    Items.Add(vm);
                }
            });
        }

        public Action<Tag> OnTagSelected { get; set; }

        protected override void OnItemSelected(Tag selectedItem)
        {
            if (OnTagSelected != null)
            {
                OnTagSelected(selectedItem);
            }
            
            NavigateBack();
        }
    }

}
