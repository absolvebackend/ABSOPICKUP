using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface INotificationService
    {
        Task<bool> SendUserPushNotifications(string[] deviceTokens, string title, string body, object data);
        Task<bool> SendDriverPushNotifications(string[] deviceTokens, string title, string body, object data);
        Task<int> SavePushNotifications(Notifications model);
        List<NotificationViewModel> GetAllUserPushNotifications(string userId);
        List<NotificationViewModel> GetAllDriverPushNotifications(int driverId);
        Task<int> DeletePushNotifications(string notificationId, string userId);
        Task<int> DeleteDriverNotifications(string notificationId, int driverId);
    }
}
