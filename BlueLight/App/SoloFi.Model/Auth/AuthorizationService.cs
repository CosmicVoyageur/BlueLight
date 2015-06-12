using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SoloFi.Contract.Auth;
using SoloFi.Contract.Repo;
using SoloFi.Contract.Services;
using SoloFi.Entity;
using XamlingCore.Portable.Contract.Entities;

namespace SoloFi.Model.Auth
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IEntityCache _entityCache;
        private readonly ITokenRepo _tokenRepo;
        private readonly IApplicationSettingsService _settingsService;

        public AuthorizationService(IEntityCache entityCache, ITokenRepo tokenRepo, IApplicationSettingsService settingsService)
        {
            _entityCache = entityCache;
            _tokenRepo = tokenRepo;
            _settingsService = settingsService;
        }

        private bool _isAuthorised;

        public async Task<bool> IsAuthorised()
        {
            return _isAuthorised;
        }

        public async Task<bool> RemoteAuthorize(string userName, string password)
        {
            var model = new AuthUser { username = userName, password = password };
            AuthorizationTokenDto result;
            try
            {
                result = await _tokenRepo.Post(model,"Login");
                await SetAuthorizationToken(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            await _settingsService.SetUserName(userName);
            _isAuthorised = result.success;
            return result.success;
        }

        public async Task<bool> RemoteReauthorize()
        {
            var token = await GetLocalAuthorizationToken();
            if (token == null) return false;
            var model = new RefreshModel {refreshToken = token.Refresh};
            AuthorizationTokenDto result;
            try
            {
                result = await _tokenRepo.Post(model,"RefreshLogin");
                await SetAuthorizationToken(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            _isAuthorised = result.success;
            return result.success;
        }

        public async Task<Tokens> GetLocalAuthorizationToken()
        {
            return await _settingsService.GetAuthTokens();
        }

        public async Task<Tokens> GetTokens()
        {
            return await _settingsService.GetAuthTokens();
        }

        public async Task DeauthorizeLocal()
        {
            _isAuthorised = false;
            await DeleteAllStoredTokens();
        }

        private async Task<AuthorizationTokenDto> SetAuthorizationToken(AuthorizationTokenDto token)
        {
            try
            {
                await DeleteAllStoredTokens();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            await _settingsService.UpdateStoredToken(token.tokens);
            return token;
        }

        private async Task DeleteAllStoredTokens()
        {
            await _settingsService.UpdateStoredToken(null);
        }


        #region Model

        private class AuthUser
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        private class RefreshModel
        {
            public string refreshToken { get; set; }
        }

        #endregion

    }
}
