using System;
using System.Collections.Generic;
using System.Diagnostics;
using SoloFi.Contract.EventArgs;
using SoloFi.Contract.Services;
using SoloFi.Entity.RFID;

namespace SoloFi.Model.Services
{
    public class GeigerService
    {
        private readonly IRfidReaderService _readerService;

        public GeigerService(IRfidReaderService readerService)
        {
            _readerService = readerService;
        }

        private readonly List<Tag> _searchTags = new List<Tag>(); 
        public void Start(IEnumerable<Tag> searchTags )
        {
            if (searchTags == null)
            {
                throw new NullReferenceException("Search Tags in Null in Geiger Service");
            }
            
            _readerService.RequestDesiredState(new ReaderState{GeigerMode = true});
            _searchTags.AddRange(searchTags);
            _readerService.TagEvent += ReaderServiceTagEvent;
        }

        public void Stop()
        {
            _readerService.TagEvent -= ReaderServiceTagEvent;
            _readerService.RequestDesiredState(new ReaderState{GeigerMode = false});
            _searchTags.Clear();
        }

        void ReaderServiceTagEvent(object sender, EventArgs e)
        {
            var typedArgs = e as TagEventArgs;
            if (typedArgs == null) return;
            int proximity = GetProximity(typedArgs.Rssi);
            if (_searchTags.Contains(typedArgs.Tag))
            {
                _readerService.AlertForGeiger(proximity);
                Debug.WriteLine(proximity);
                OnTagFound(typedArgs);
            }
        }

        private int GetProximity(int? rssi)
        {
            if (!rssi.HasValue)
            {
                return 3; // return lowest val
            }
            Debug.WriteLine(rssi);
            if (rssi > -45)
            {
                return 0;
            }
            else if (rssi > -55)
            {
                return 1;
            }
            else if (rssi > -65)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        private void OnTagFound(TagEventArgs tagEventArgs)
        {
            if (TagFound != null)
            {
                TagFound(this, tagEventArgs);
            }
        }

        public event EventHandler TagFound;
    }
}
