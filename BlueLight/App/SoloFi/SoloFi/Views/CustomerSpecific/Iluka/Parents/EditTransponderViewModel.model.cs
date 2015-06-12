using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.Entity.CustomerSpecific.Iluka;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace SoloFi.Views.CustomerSpecific.Iluka.Parents
{
    public class EditTransponderViewModel : XViewModel
    {
        private readonly IIlukaReaderService _readerService;

        public EditTransponderViewModel(IIlukaReaderService readerService)
        {
            _readerService = readerService;
        }

        public string NewSampleIdentifier { get; set; }
        
        public ICommand WriteCommand { get; set; }

        public IlukaUhfTag ReferenceTag { get; set; }

        public override void OnInitialise()
        {
            base.OnInitialise();
            InitCommands();
        }

        private void InitCommands()
        {
            WriteCommand = new Command(OnWrite);
        }

        private void OnWrite()
        {
            var update = new IlukaUhfTag
            {
                TagNumber = ReferenceTag.TagNumber,
                SampleIdentifier = NewSampleIdentifier
            };
            if (_readerService.WriteToTransponderUserMemory(ReferenceTag, update))
            {
                Debug.WriteLine("Successful write");
            }
        }
    }
}
