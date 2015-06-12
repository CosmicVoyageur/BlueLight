using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace UniveralSerialPort
{
    public class ColourComposer
    {
        private readonly int _ledCount;
        private CancellationTokenSource tokenSource;
        public ColourComposer(int ledCount)
        {
            _ledCount = ledCount;
        }

        public void StartGenerating(int delay)
        {
            _delay = delay;
            tokenSource = new CancellationTokenSource();
            var run = Task.Factory.StartNew(Generator, tokenSource.Token);
        }

        public void StopGenerating()
        {
            tokenSource.Cancel();
        }

        private int _delay;
        private async void Generator()
        {
            while (true)
            {
                var com = GenerateCommand();
                OnCommand(com);
                await Task.Delay(_delay);
            }
        }

        private void OnCommand(ColourCommand com)
        {
            var handler = CommandEvent;
            if (handler == null) return;
            handler(this, new ColourCommandEventArgs(com));
        }


        private ColourCommand GenerateCommand()
        {
            bool x = DateTime.Now.Second%2==0;
            var colour = x ? ColourDictionary.Red : ColourDictionary.Blue;
            var command = new ColourCommand
            {
                Position = DateTime.Now.Second%_ledCount,
                Colour = colour
            };
            return command;
        }

        public event EventHandler<ColourCommandEventArgs> CommandEvent;
    }
}
