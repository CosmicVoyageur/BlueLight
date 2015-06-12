using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Model.Services
{
    public abstract class LocalEntityService<T> where T:class, IEntity, new()
    {
        private readonly IEntityCache _entityCache;
        private readonly IEntityManager<T> _manager;

        protected LocalEntityService(IEntityCache entityCache, IEntityManager<T> manager)
        {
            _entityCache = entityCache;
            _manager = manager;
        }

        public async Task<IList<T>> GetAllLocalEntities()
        {
            List<T> result;
            try
            {
                result =  await _entityCache.GetAll<T>();
            }
            catch
            {
                return new List<T>();
            }
            return result ?? new List<T>();
        }

        public async Task<T> GetLocalEntityById(Guid id)
        {
            var result = await _manager.Get(id);
            return result;
        }

        public async Task<T> GetLocalEntityById(string id)
        {
            Guid guid;
            if (Guid.TryParse(id, out guid))
            {
                return await GetLocalEntityById(guid);
            }
            return null;
        }

        
    }
}
