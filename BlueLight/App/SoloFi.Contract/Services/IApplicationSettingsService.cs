using System.Threading.Tasks;
using SoloFi.Entity;

namespace SoloFi.Contract.Services
{
    public interface IApplicationSettingsService
    {
        Task<ApplicationSettingsEntity> GetApplicationSettings();
        Task<bool> SaveApplicationSettings(ApplicationSettingsEntity applicationSettings);
        Task SetUserName(string userName);
    }
}