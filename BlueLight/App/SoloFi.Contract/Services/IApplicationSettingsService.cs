using System.Threading.Tasks;
using SoloFi.Entity;

namespace SoloFi.Contract.Services
{
    public interface IApplicationSettingsService
    {
        Task<ApplicationSettingsEntity> GetApplicationSettings();
        Task<bool> SaveApplicationSettings(ApplicationSettingsEntity applicationSettings);
        Task UpdateStoredToken(Tokens tokens);
        Task<Tokens> GetAuthTokens();
        Task SetUserName(string userName);
    }
}