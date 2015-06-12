using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloFi.Contract.Platform
{
    public interface IFileExportService
    {
        Task<bool> FileExists(string fileName);
        Task<bool> SaveToFile(string fileName, string data);
        Task<string> ReadFile(string fileName);
        Task<IEnumerable<string>> GetAllFileNames();
        Task<bool> DeleteFile(string fileName);
        Task<string> GetPathForSharing(string fileName);
    }
}