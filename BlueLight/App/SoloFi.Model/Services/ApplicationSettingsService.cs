using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlueLight.Contract.Services;
using BlueLight.Entity;
using XamlingCore.Portable.Contract.Entities;

namespace BlueLight.Model.Services
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


        public async Task SetUserName(string userName)
        {
            var settings = await GetApplicationSettings();
            settings.CurrentUserName = userName;
            await SaveApplicationSettings(settings);
        }
    }
}
