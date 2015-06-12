using System;
using System.Threading.Tasks;

namespace SoloFi.Contract.Platform
{
    public interface INotifyService
    {
        void ToastNotificationWithPicture(string title, string message, string imageUrl);
        Task AskYesOrCancel(string question, Action callback);
    }
}