using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniveralSerialPort
{
    class Program
    {

        private static SerialPort _serialPort;
        static void Main(string[] args)
        {
            var names = SerialPort.GetPortNames();
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine("Enter Port Name");
            var selectedPort = Console.ReadLine();
            if (names.Contains(selectedPort))
            {
                _serialPort = new SerialPort(selectedPort);
                Run();
            }
            else
            {
                Console.WriteLine("Bad port");
            }
        }

        private static void Run()
        {
            _serialPort.Open();
            ColourComposer composer = new ColourComposer(16);
            composer.CommandEvent += composer_CommandEvent;
            composer.StartGenerating(200);
            Thread.Sleep(Timeout.Infinite);
            Debug.WriteLine("end of run");
        }

        static void composer_CommandEvent(object sender, ColourCommandEventArgs e)
        {
            ColourCommander.Go(e.Command, _serialPort);
        }
    }
}
