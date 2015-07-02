using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BlueCastello.Contract.Platform;
using BlueCastello.Contract.Platform.Tcp;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace BlueCastello.Views.Main
{
    public class TcpConnectionViewModel : XViewModel
    {
        private readonly ITcpSocketService _tcpSocketService;
        private readonly INotifyService _notifyService;

        public TcpConnectionViewModel(ITcpSocketService tcpSocketService, INotifyService notifyService)
        {
            _tcpSocketService = tcpSocketService;
            _notifyService = notifyService;
            Title = "TCP";
        }

        public ICommand ConnectCommand { get; set; }

        public string IpText { get; set; }
        public int Port { get; set; }

        public override void OnInitialise()
        {
            InitCommands();
            base.OnInitialise();
        }

        private void InitCommands()
        {
            ConnectCommand = new Command(OnConnect);
        }

        private async void OnConnect()
        {
            if (await _tcpSocketService.Initialise(IpText, Port))
            {
                _notifyService.ToastNotificationWithPicture("Connected", IpText + ":" + Port);
            }
            else
            {
                _notifyService.ToastNotificationWithPicture("Error", IpText + ":" + Port);
            }
            
        }
    }
}
