using System.Threading.Tasks;
using BlueLight.Entity;

namespace BlueLight.Contract.Services
{
    public interface IApplicationSettingsService
    {
        Task<ApplicationSettingsEntity> GetApplicationSettings();
        Task<bool> SaveApplicationSettings(ApplicationSettingsEntity applicationSettings);
        Task SetUserName(string userName);
    }
}