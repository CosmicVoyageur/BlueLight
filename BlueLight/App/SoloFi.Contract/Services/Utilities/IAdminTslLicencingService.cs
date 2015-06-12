using System.Threading.Tasks;

namespace SoloFi.Contract.Services.Utilities
{
    public interface IAdminTslLicencingService
    {
        void SetLicenceKey(string customerName);
        Task<bool> IsLicenceValid(string customerName);
    }
}