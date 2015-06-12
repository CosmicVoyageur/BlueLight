using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Entity;
using XamlingCore.Portable.View.ViewModel.Base;

namespace SoloFi.Tiles.Files
{
    public class RegularFileTileViewModel : ItemViewModel<FileRepresentation>
    {
        public ICommand DeleteCommand { get; set; }
        public ICommand ShareCommand { get; set; }
    }
}
