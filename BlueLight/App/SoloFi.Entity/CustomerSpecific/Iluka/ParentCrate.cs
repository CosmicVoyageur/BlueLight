using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity.CustomerSpecific.Iluka
{
    public class ParentCrate : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IlukaUhfTag Tag { get; set; }
        public List<ChildSample> Children { get; set; }

        public static ParentCrate NewParent(string name)
        {
            return new ParentCrate
            {
                Id = Guid.NewGuid(),
                Children = new List<ChildSample>(),
                Name = name
            };
        }

    }
}
