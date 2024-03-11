using _AbsoPickUp.Data;
using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Repositories
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendUserPushNotifications(string[] deviceTokens, string title, string body, object data)
        {
            try
            {
                bool sent = false;
                if (deviceTokens.Count() > 0)
                {
                    // object Creation
                    var messageInformation = new FirebaseNotificationsMessageViewModel()
                    {

                        registration_ids = deviceTokens,
                        notification = new FirebaseNotificationsViewModel()
                        {
                            title = title,
                            body = body
                        },
                        data = data

                    };
                    string jsonMessage = JsonConvert.SerializeObject(messageInformation);
                    // Create request to FirebaseAPI
                    var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
                    request.Headers.TryAddWithoutValidation("Authorization", "Key=" + FirebaseUserAppServerKey);
                    request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                    using var Client = new HttpClient();
                    HttpResponseMessage result = await Client.SendAsync(request);
                    if (result.IsSuccessStatusCode)
                    {
                        sent = true;
                    }

                }

                return sent;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SendDriverPushNotifications(string[] deviceTokens, string title, string body, object data)
        {
            try
            {
                bool sent = false;
                if (deviceTokens.Count() > 0)
                {
                    // object Creation
                    var messageInformation = new FirebaseNotificationsMessageViewModel()
                    {

                        registration_ids = deviceTokens,
                        notification = new FirebaseNotificationsViewModel()
                        {
                            title = title,
                            body = body
                        },
                        data = data

                    };

                    string jsonMessage = JsonConvert.SerializeObject(messageInformation);
                    // Create request to FirebaseAPI
                    var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
                    request.Headers.TryAddWithoutValidation("Authorization", "Key=" + FirebaseDriverAppServerKey);
                    request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                    using var Client = new HttpClient();
                    HttpResponseMessage result = await Client.SendAsync(request);
                    if (result.IsSuccessStatusCode)
                    {
                        sent = true;
                    }

                }

                return sent;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> SavePushNotifications(Notifications model)
        {
            try
            {
                _context.Notifications.Add(model);
                return await _context.SaveChangesAsync();
            }
            catch { throw; }
        }

        public List<NotificationViewModel> GetAllUserPushNotifications(string userId)
        {
            try
            {
                var _allNotices = (from notif in _context.Notifications
                                   where notif.ToUserId == userId && !notif.IsDeleted
                                   select new NotificationViewModel()
                                   {
                                       Id = notif.Id,
                                       ToUserId = notif.ToUserId,
                                       ToDriverId = 0,
                                       TypeId = notif.Type,
                                       Type = ((NotificationTypes)notif.Type).ToString(),
                                       RequestId = notif.RequestId,
                                       Text = notif.Text,
                                       CreatedOn = notif.CreatedOn.ToString(DefaultDateTimeFormat + ":ss")
                                   }).ToList();
                return _allNotices;
            }
            catch { throw; }
        }

        public List<NotificationViewModel> GetAllDriverPushNotifications(int driverId)
        {
            try
            {
                var _allNotices = (from notif in _context.Notifications
                                   where notif.ToDriverId == driverId && !notif.IsDeleted
                                   select new NotificationViewModel()
                                   {
                                       Id = notif.Id,
                                       ToDriverId = notif.ToDriverId,
                                       ToUserId = string.Empty,
                                       TypeId = notif.Type,
                                       Type = ((NotificationTypes)notif.Type).ToString(),
                                       RequestId = notif.RequestId,
                                       Text = notif.Text,
                                       CreatedOn = notif.CreatedOn.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat + ":ss")
                                   }).ToList();
                return _allNotices;
            }
            catch { throw; }
        }

        public async Task<int> DeletePushNotifications(string notificationId, string userId)
        {
            try
            {
                if (notificationId == string.Empty && userId != string.Empty)
                {
                    var _notifications = _context.Notifications.Where(x => x.ToUserId == userId).ToList();
                    if (_notifications.Count() > 0)
                    {
                        foreach (var _notif in _notifications)
                        {
                            _notif.IsDeleted = true;
                            _context.Notifications.Update(_notif);
                        }
                    }
                }
                else if (notificationId != string.Empty && userId != string.Empty)
                {
                    var _notification = _context.Notifications.FirstOrDefault(x => x.Id == notificationId && x.ToUserId == userId);
                    if (_notification != null)
                    {
                        _notification.IsDeleted = true;
                        _context.Notifications.Update(_notification);
                    }
                }

                return await _context.SaveChangesAsync();
            }
            catch { throw; }
        }

        public async Task<int> DeleteDriverNotifications(string notificationId, int driverId)
        {
            try
            {
                if (notificationId == string.Empty && driverId > 0)
                {
                    var _notifications = _context.Notifications.Where(x => x.ToDriverId == driverId).ToList();
                    if (_notifications.Count() > 0)
                    {
                        foreach (var _notif in _notifications)
                        {
                            _notif.IsDeleted = true;
                            _context.Notifications.Update(_notif);
                        }
                    }
                }
                else if (notificationId != string.Empty && driverId > 0)
                {

                    var _notification = _context.Notifications.FirstOrDefault(x => x.Id == notificationId && x.ToDriverId == driverId);
                    if (_notification != null)
                    {
                        _notification.IsDeleted = true;
                        _context.Notifications.Update(_notification);
                    }
                }

                return await _context.SaveChangesAsync();
            }
            catch { throw; }
        }
    }
}
