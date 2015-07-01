using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLight.Contract.Config
{
    public interface IClientConfigService
    {
        Task<List<string>> GetAllClientNames();
        Task<string> GetCurrentClient();
        string ComputeHash(string readerUniqueValue);
        string ComputeHash(string readerUniqueValue, string clientName);
    }
}