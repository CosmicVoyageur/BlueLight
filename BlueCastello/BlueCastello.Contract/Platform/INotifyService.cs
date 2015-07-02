namespace BlueCastello.Contract.Platform
{
    public interface INotifyService
    {
        void ToastNotificationWithPicture(string title, string message, string imageUrl=null);
    }
}