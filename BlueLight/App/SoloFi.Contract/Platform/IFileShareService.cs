using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloFi.Contract.Platform
{
    public interface IFileShareService
    {
        Task ShareFilesPreferEmail(List<string> paths, string subject);
    }
}