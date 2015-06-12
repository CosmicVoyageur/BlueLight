using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity
{
    public class SmartTrackInventory : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
        public string PlaceId { get; set; }
        [JsonIgnore]
        public Snapshot ReferenceSnapshot { get; set; }
        public string SnapshotId { get; set; }
        public List<string> FoundItemsExpected { get; set; }
        public List<string> FoundItemsUnexpected { get; set; }
        public List<string> MissingItems { get; set; }
        public List<STIReason> Reasons { get; set; }

        

        public static SmartTrackInventory NewInventory(Snapshot snapshot)
        {
            if (snapshot == null) return null;
            var inventory = new SmartTrackInventory
            {
                FoundItemsExpected = new List<string>(),
                FoundItemsUnexpected = new List<string>(),
                MissingItems = new List<string>(),
                Reasons = new List<STIReason>(),
                ReferenceSnapshot = snapshot,
                SnapshotId = snapshot.Id.ToString(),
                PlaceId = snapshot.PlaceId
            };
            return inventory;
        }

    }

    public class STIReason
    {
        public string ItemId { get; set; }  // the item that the reason relates to
        public string ReasonId { get; set; } // the reason for the item

        public override bool Equals(object obj)
        {
            var reason = obj as STIReason;
            if (reason == null) return false;
            return reason.ItemId == this.ItemId && reason.ReasonId == this.ReasonId;
        }
    }
}
