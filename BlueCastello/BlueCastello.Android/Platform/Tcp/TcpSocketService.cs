using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using System.Threading.Tasks;

using BlueCastello.Contract.Platform.Tcp;
using Microsoft.Win32.SafeHandles;

namespace BlueCastello.Android.Platform.Tcp
{
    public class TcpSocketService : ITcpSocketService
    {
        private IPAddress _ip;
        private int _port;
        private readonly TcpClient _client;
        public TcpSocketService()
        {
            _client = new TcpClient();
        }

        public async Task<bool> Initialise(string ip, int port)
        {
            if (ip == null) return false;
            _port = port;
            if (!IPAddress.TryParse(ip, out _ip)) return false;
            try
            {
                await _client.ConnectAsync(_ip, _port); // 
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return _client.Connected;
        }

        public System.IO.Stream GetStream()
        {
            return _client.GetStream();
            
        }



        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        bool disposed = false;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                _client.Close();
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }
        
    }
}