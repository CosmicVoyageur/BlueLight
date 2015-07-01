using System.Collections.Generic;
using System.Threading.Tasks;
using BlueLight.Contract.Services;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Net.DownloadConfig;
using XamlingCore.Portable.Net.Service;

namespace BlueLight.Model.Config
{
    public class TransferConfigService: HttpTransferConfigServiceBase
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        public TransferConfigService(IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
        }

        public override async Task<IHttpTransferConfig> GetConfig(string url, string verb)
        {
            var settings = await _applicationSettingsService.GetApplicationSettings();
            var baseUrl = settings.ServerBaseUrl;
            var finalUrl = baseUrl + url;
            var config = new StandardHttpConfig
            {
                Accept = "application/json",
                IsValid = true,
                Url = finalUrl,
                BaseUrl = baseUrl,
                Verb = verb,
                Headers = new Dictionary<string, string>()
            };


            
            return config;
        }

    }
}
