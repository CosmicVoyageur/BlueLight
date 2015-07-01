using System.Threading.Tasks;

namespace BlueLight.Contract.Platform
{
    public interface IRandomInterface
    {
        Task DoSOmething(int counter, bool ok);
    }
}
