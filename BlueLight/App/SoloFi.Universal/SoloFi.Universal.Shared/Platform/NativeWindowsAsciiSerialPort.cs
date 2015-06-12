using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Tsl.AsciiProtocol.Pcl;

namespace SoloFi.Universal.Platform
{
    public class NativeWindowsAsciiSerialPort : IAsciiSerialPort
    {

        private RfcommDeviceService _rfService;
        private StreamSocket _socket;

        private bool _isInitialised;

        private NativeWindowsAsciiSerialPort(DeviceInformation device)
        {
            _isInitialised = false;
            
        }

        public static async Task<NativeWindowsAsciiSerialPort> GetInstance(DeviceInformation device)
        {
            var nativeService = new NativeWindowsAsciiSerialPort(device);
            await nativeService._init(device);
            if (nativeService._isInitialised)
            {
                return nativeService;
            }

            return null;
        }

        private async Task _init(DeviceInformation device)
        {
            try
            {
                _rfService = await RfcommDeviceService.FromIdAsync(device.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
            
            _socket = new StreamSocket();
            _socket.Control.KeepAlive = true;
            await _socket.ConnectAsync(_rfService.ConnectionHostName, _rfService.ConnectionServiceName).AsTask()
                .ContinueWith(_initWriter)
                .ContinueWith(_initReader)
                .ContinueWith(_completeConnection);
            
        }

        private void _initWriter(Task obj)
        {
            _writer = new DataWriter(_socket.OutputStream);
        }

        private void _initReader(Task obj)
        {
            _reader = new DataReader(_socket.InputStream) 
                {InputStreamOptions = InputStreamOptions.Partial};
            Task.Factory.StartNew(() => FillBufferForeverWithCancelation(new CancellationToken(false)));
        }

        private void _completeConnection(Task obj)
        {
            _isInitialised = true;
        }

        public void Dispose()
        {
            _writer.Dispose();
            _socket.Dispose();
        }

        private DataWriter _writer;
        public void WriteLine(string value)
        {

            while (!_isInitialised)
            {
            }
            _writer.WriteString(value + "\r\n");
            _writer.StoreAsync();

        }

        private readonly ConcurrentQueue<string> _readBuffer = new ConcurrentQueue<string>(); // internal buffer
        public string ReadLine()
        {
            string result;
            return _readBuffer.TryDequeue(out result) ? result : "";
        }

        public bool IsDataAvailable
        {
            get { return _readBuffer.Count > 0; }
        }

        public event EventHandler Received;

        private void FillBufferForeverWithCancelation(CancellationToken cancellationToken)
        {
            StringBuilder line = new StringBuilder();
            while (true)
            {
                int temp;
                char c;

                temp = _socket.InputStream.AsStreamForRead().ReadByte();

                
                c = Convert.ToChar(temp);
                line.Append(c);
                if (c == '\r' || c == '\n')
                {
                    AddToBuffer(line.ToString());
                    line.Clear();
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

        }
        private readonly StringBuilder _tempBuffer = new StringBuilder(); // for storing unfinished strings returned by reader
        private DataReader _reader;

        private void AddToBuffer(string result)
        {
            if (result == null) return;
            if (result.Length == 0) return; // check for empty lines
            result += _tempBuffer.ToString();
            _tempBuffer.Clear();
            var lines = result.Split('\n');
            foreach (string line in lines)
            {
                if (line.Length > 1 && !line.Contains("\0\0"))
                {
                    if (line.Contains("\r")) // is full line
                    {
                        _readBuffer.Enqueue(line);
                    }
                    else
                    {
                        _tempBuffer.Append(line);
                    }
                }
            }
            if (result.Contains("OK:") || result.Contains("ER:"))
            { // this means we reached the end of the response

                // this signals the last line in the returned ascii result
                OnLineReceived();
            }
        }

        private void OnLineReceived()
        {
            if (_socket != null)
            {
                if (_readBuffer.Count > 0)
                {
                    if (Received != null) Received(this, null);
                }
            }
        }
    }
}
