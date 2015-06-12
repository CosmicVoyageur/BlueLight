using System;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity
{
    public class ItemSearchParameters : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

    }
}
