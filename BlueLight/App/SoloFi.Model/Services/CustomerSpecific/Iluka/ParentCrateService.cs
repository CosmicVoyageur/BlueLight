using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloFi.Contract.Services.CustomerSpecific.Iluka;
using SoloFi.Entity.CustomerSpecific.Iluka;
using SoloFi.Entity.RFID;
using XamlingCore.Portable.Contract.Entities;

namespace SoloFi.Model.Services.CustomerSpecific.Iluka
{
    public class ParentCrateService : LocalEntityService<ParentCrate>, IParentCrateService
    {
        private readonly IEntityManager<ParentCrate> _manager;

        public ParentCrateService(IEntityCache entityCache, IEntityManager<ParentCrate> manager) : base(entityCache, manager)
        {
            _manager = manager;
        }

        public async Task<IList<ParentCrate>> GetAllLocalParents()
        {
            return await base.GetAllLocalEntities();
        }

        public async Task SaveParent(ParentCrate crate)
        {
            await _manager.Set(crate);
        }

        public async Task<ParentCrate> GetCrateByTag(Tag tag)
        {
            var parents = await GetAllLocalParents();
            var parent = parents.FirstOrDefault((p) => tag.Equals(p.Tag));
            return parent;
        }

        public async Task<string> Serialise(ParentCrate parent)
        {
            return BuildCsvString(parent, ',', '\n');
        }

        private static string BuildCsvString(ParentCrate parent, char delimiter, char endOfLine)
        {
            StringBuilder result = new StringBuilder();
            foreach (var child in parent.Children)
            {
                result.Append(BuildCsvLine(parent.Name, child.SampleIdentifier, delimiter, endOfLine));
            }
            return result.ToString();
        }

        private static string BuildCsvLine(string crateId, string sampleId, char delimiter, char endOfLine)
        {
            StringBuilder result = new StringBuilder();
            result.Append(crateId).Append(delimiter).Append(sampleId).Append(endOfLine);
            return result.ToString();
        }
    }
}
