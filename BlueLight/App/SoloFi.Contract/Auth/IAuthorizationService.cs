using System.Threading.Tasks;
using SoloFi.Entity;

namespace SoloFi.Contract.Auth
{
    public interface IAuthorizationService
    {
        Task<Tokens> GetLocalAuthorizationToken();
        Task<bool> RemoteAuthorize(string userName, string password);
        Task<bool> RemoteReauthorize();
        Task DeauthorizeLocal();
        Task<Tokens> GetTokens();
        Task<bool> IsAuthorised();
    }
}