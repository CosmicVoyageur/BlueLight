using System;
using System.Collections.Generic;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity
{
    public class LookupEntity : IEntity
    {
        public Guid Id { get; set; }

        public Dictionary<Guid, List<Guid>> PlaceToThingLookup { get; set; }

        public Dictionary<string, Guid> TagToThingLookup { get; set; }

        public int LocalPlaceCount { get; set; }
        public int LocalThingCount { get; set; }

    }
}
