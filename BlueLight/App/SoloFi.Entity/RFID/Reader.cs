using System;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity.RFID
{
    public class Reader : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
