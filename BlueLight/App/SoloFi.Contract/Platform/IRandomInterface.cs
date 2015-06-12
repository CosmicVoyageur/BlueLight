using System.Threading.Tasks;

namespace SoloFi.Contract.Platform
{
    public interface IRandomInterface
    {
        Task DoSOmething(int counter, bool ok);
    }
}
