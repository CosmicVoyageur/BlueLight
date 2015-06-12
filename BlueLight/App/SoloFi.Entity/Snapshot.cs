using System;
using System.Collections.Generic;
using SoloFi.Entity.RFID;

namespace SoloFi.Entity
{
    public class Snapshot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PlaceId { get; set; }
        public string UserId { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime? EndTimeUtc { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Guid> Items { get; set; }

        public static Snapshot NewSnapshot(Guid placeId, Guid userId)
        {
            var now = DateTime.UtcNow;
            var snapshot = new Snapshot
            {
                Id = Guid.NewGuid(),
                Name = "",
                PlaceId = placeId.ToString(),
                UserId = userId.ToString(),
                StartTimeUtc = now,
                EndTimeUtc = now.AddSeconds(-1),
                Tags = new List<Tag>(),
                Items = new List<Guid>()
            };
            return snapshot;
        }
    }
}
