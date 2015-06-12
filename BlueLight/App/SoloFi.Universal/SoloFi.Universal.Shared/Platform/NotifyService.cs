using System;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using SoloFi.Contract.Platform;

namespace SoloFi.Universal.Platform
{
    public class NotifyService : INotifyService
    {

        public async void ToastNotificationWithPicture(string title, string message, string imageUrl)
        {


            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(title));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(message));

            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", _getUrl(imageUrl));
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "meow");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);


            
        }

        public async Task MessageBox(string title, string message)
        {
            await
                new Windows.UI.Popups.MessageDialog(message, title)
                    .ShowAsync();
        }

        private string _getUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return @"http://exmoorpet.com/wp-content/uploads/2012/08/cat.png";
            }
            else return imageUrl;
        }

        public async Task AskYesOrCancel(string question, Action callback)
        {
            Callback = callback;
            var box = 
                new Windows.UI.Popups.MessageDialog(question);

            box.Commands.Add(new UICommand("Yes",CommandInvokedHandler));
            box.Commands.Add(new UICommand("Cancel"));
            await box.ShowAsync();
        }

        private Action Callback;
        private void CommandInvokedHandler(IUICommand command)
        {
            Callback();
            Callback = null;
        }

    }
}
