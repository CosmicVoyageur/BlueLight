using System.Collections.Generic;
using System.Threading.Tasks;
using SoloFi.Entity.CustomerSpecific.Iluka;
using SoloFi.Entity.RFID;

namespace SoloFi.Contract.Services.CustomerSpecific.Iluka
{
    public interface IParentCrateService
    {
        Task<IList<ParentCrate>> GetAllLocalParents();
        Task SaveParent(ParentCrate crate);
        Task<ParentCrate> GetCrateByTag(Tag tag);
        Task<string> Serialise(ParentCrate parent);
    }
}