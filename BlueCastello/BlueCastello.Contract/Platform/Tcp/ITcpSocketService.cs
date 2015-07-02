using System;
using System.IO;
using System.Threading.Tasks;

namespace BlueCastello.Contract.Platform.Tcp
{
    public interface ITcpSocketService : IDisposable
    {
        Task<bool> Initialise(string ip, int port);
        Stream GetStream();
    }
}