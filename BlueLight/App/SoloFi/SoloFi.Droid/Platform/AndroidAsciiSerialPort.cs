using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.Util;
using Tsl.AsciiProtocol.Pcl;

//using TechnologySolutions.Rfid.AsciiProtocol;

namespace SoloFi.Droid.Platform
{
    /// <summary>
    /// A Wrapper for the Android Bluetooth Socket, allowing ASCII protocol to connect to reader via bluetooth
    /// </summary>
    public class AndroidAsciiSerialService : IAsciiSerialPort, IDisposable
    {
        private BluetoothSocket _socket;
        private Task StreamChecker;
        CancellationTokenSource CancelSource;
        public AndroidAsciiSerialService(BluetoothDevice device)
        {
            if (device == null)
                throw new Exception("Bluetooth Device not found.");

            _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")); // this is the UUID for the Bluetooth Serial port service
            _socket.Connect();

            CancelSource = new CancellationTokenSource();

            //StreamChecker = Task.Factory.StartNew(FillBufferForever); // start the long running task that checks the serial buffer and writes to the internal buffer
            var token = CancelSource.Token;
            StreamChecker = Task.Factory.StartNew(() => FillBufferForeverWithCancelation(token), token);
        }

        private object FillBufferForeverWithCancelation(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];
            int temp;
            char c;
            StringBuilder line = new StringBuilder();
            while (true)
            {
                temp = _socket.InputStream.ReadByte();
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
        /// <summary>
        /// Wrapper for Socket.IsConnected
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (_socket == null) return false;
                return _socket.IsConnected;
            }
        }
        /// <summary>
        /// Raises event for line recieved
        /// </summary>
        private void RaiseLineReceivedEvent()
        {
            if (_socket != null)
            {
                if (_socket.IsConnected)
                {
                    if (ReadBuffer.Count > 0)
                    {
                        if (Received != null) Received(this, null);
                    }
                }
            }
        }

        private StringBuilder _tempBuffer = new StringBuilder(); // for storing unfinished strings returned by reader
        /// <summary>
        /// Adds to internal buffer. Will store unfinished lines in _tempBuffer
        /// </summary>
        /// <param name="result"></param>
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
                        ReadBuffer.Enqueue(line);
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
                RaiseLineReceivedEvent();
            }
        }

        /// <summary>
        /// When raised, responders will query for data
        /// </summary>
        public event EventHandler Received;
        /// <summary>
        /// Determines whether there is data available to be read from the internal buffer
        /// </summary>
        public bool IsDataAvailable
        {
            get { return ReadBuffer.Count > 0; }
        }
        /// <summary>
        /// This method should be run in a separate thread/task
        /// It constantly listens on the serial port, and saves data to the internal buffer
        /// </summary>
        async private void FillBufferForever()
        {
            byte[] buffer = new byte[1024];
            int temp;
            char c;
            StringBuilder line = new StringBuilder();
            while (true)
            {
                temp = _socket.InputStream.ReadByte();
                c = Convert.ToChar(temp);
                line.Append(c);
                if (c == '\r' || c == '\n')
                {
                    AddToBuffer(line.ToString());
                    line.Clear();
                }
                await Task.Yield();
            }
        }

        private ConcurrentQueue<string> ReadBuffer = new ConcurrentQueue<string>(); // internal buffer
        /// <summary>
        /// Part of IAsciiSerialPort. Returns the next line
        /// </summary>
        /// <returns>A line of ASCII response data</returns>
        public string ReadLine()
        {
            string result;
            if (ReadBuffer.TryDequeue(out result))
            {
                return result;
            }
            return "";
        }

        /// <summary>
        /// For writing to the ASCII stream
        /// </summary>
        /// <param name="value">ASCII command. Is not validated here.</param>
        public void WriteLine(string value)
        {
            if (!value.EndsWith("\n"))
            {
                value += "\n";
            }
            _socket.OutputStream.Write(Encoding.ASCII.GetBytes(value), 0, value.Length);

        }

        public void Dispose()
        {
            CancelSource.Cancel();
            _socket.Close();
            _socket.Dispose();
        }
    }
}