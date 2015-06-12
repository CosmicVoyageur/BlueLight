using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SoloFi.Contract.Services;
using SoloFi.Entity;
using XamlingCore.Portable.Contract.Entities;

namespace SoloFi.Model.Services
{
    public class ApplicationSettingsService : LocalEntityService<ApplicationSettingsEntity>, IApplicationSettingsService
    {
        private const string SettingsEntityKey = "SettingsEntityKey";
        private readonly IEntityCache _entityCache;
        private readonly IEntityManager<ApplicationSettingsEntity> _manager;

        public ApplicationSettingsService(IEntityCache entityCache, IEntityManager<ApplicationSettingsEntity> manager  )
            :base(entityCache, manager)
        {
            _entityCache = entityCache;
            _manager = manager;
        }

        public async Task<ApplicationSettingsEntity> GetApplicationSettings()
        {
            ApplicationSettingsEntity appSettings = null;
            try
            {
                var listOfsettings = await base.GetAllLocalEntities();
                if (listOfsettings.Count > 1) Debug.WriteLine("Warning - multiple stored app settings detected");
                appSettings = listOfsettings.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return appSettings ?? new ApplicationSettingsEntity();
        }

        public async Task<bool> SaveApplicationSettings(ApplicationSettingsEntity applicationSettings)
        {
            return await _entityCache.SetEntity(SettingsEntityKey, applicationSettings);
        }

        public async Task UpdateStoredToken(Tokens tokens)
        {
            var settings = await GetApplicationSettings();
            settings.StoredToken = tokens;
            await SaveApplicationSettings(settings);
            _tokens = tokens;
        }

        private Tokens _tokens;
        public async Task<Tokens> GetAuthTokens()
        {
            if (_tokens==null)
            {
                var settings = await GetApplicationSettings();
                return settings.StoredToken;
            }
            else
            {
                return _tokens;
            }
        }

        public async Task SetUserName(string userName)
        {
            var settings = await GetApplicationSettings();
            settings.CurrentUserName = userName;
            await SaveApplicationSettings(settings);
        }
    }
}
