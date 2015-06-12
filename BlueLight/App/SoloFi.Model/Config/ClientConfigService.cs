using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;
using SoloFi.Contract.Config;

namespace SoloFi.Model.Config
{
    public class ClientConfigService : IClientConfigService
    {
        public async Task<List<string>> GetAllClientNames()
        {
            // need to manually add each client name here. Only gets used by Ramp client
            var result = new List<string> { ClientNames.Ramp, ClientNames.Iluka, ClientNames.Screenex };
            return result;
        }

        public async Task<string> GetCurrentClient()
        {
            return ClientNames.CurrentClient;
        }


        private const string ApplicationSecret = "solofi"; // this shouldn't change, or hash will change

        public string ComputeHash(string readerUniqueValue)
        {
            string value = readerUniqueValue + ClientNames.CurrentClient + ApplicationSecret;
            var keyMaterial = Encoding.UTF8.GetBytes(value);
            byte[] data;
            var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha1);
            CryptographicHash hasher = algorithm.CreateHash(keyMaterial);
            byte[] mac = hasher.GetValueAndReset();
            string macBase64 = Convert.ToBase64String(mac);

            return macBase64;
        }

        public string ComputeHash(string readerUniqueValue, string clientName)
        {
            string value = readerUniqueValue + clientName + ApplicationSecret;
            var keyMaterial = Encoding.UTF8.GetBytes(value);
            byte[] data;
            var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha1);
            CryptographicHash hasher = algorithm.CreateHash(keyMaterial);
            byte[] mac = hasher.GetValueAndReset();
            string macBase64 = Convert.ToBase64String(mac);

            return macBase64;
        }
    }
}
