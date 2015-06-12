using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Entity.CustomerSpecific.Iluka;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Tiles.CustomerSpecific.Iluka
{
    public class IlukaSampleTileViewModel : ItemViewModel<ChildSample>
    {
        private bool _isParentTag;
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SetParentCommand { get; set; }

        public bool IsParentTag
        {
            get { return _isParentTag; }
            set
            {
                _isParentTag = value;
                OnPropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            // pass equality through to Item
            var vm = obj as ItemViewModel<ChildSample>;
            return vm != null && vm.Item!=null && Item!=null && Item.Equals(vm.Item);
        }

    }

    
}
