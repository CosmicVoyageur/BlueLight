using System;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity
{
    public class Reason : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var reason = obj as Reason;
            if (reason == null) return false;
            return reason.Id == this.Id;
        }
    }
}
