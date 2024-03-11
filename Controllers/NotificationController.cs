using _AbsoPickUp.Common;
using _AbsoPickUp.IServices;
using _AbsoPickUp.LoggerService;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace _AbsoPickUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILoggerManager _logger;
        public NotificationController(ILoggerManager logger, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        #region Get User Notifications
        [Authorize]
        [HttpGet("GetUserNotifications")]
        public IActionResult GetUserNotifications()
        {
            try
            {
                var userId = CommonFunctions.getUserId(User);
                var _notificationList = _notificationService.GetAllUserPushNotifications(userId);
                if (_notificationList.Count > 0)
                {
                    return Ok(new { status = true, data = _notificationList, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Notification/GetUserNotifications : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = Array.Empty<NotificationViewModel>(), message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Notification/GetUserNotifications : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get User Notifications
        [Authorize]
        [HttpGet("GetDriverNotifications")]
        public IActionResult GetDriverNotifications(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Notification/GetDriverNotifications : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _notificationList = _notificationService.GetAllDriverPushNotifications(driverId);
                if (_notificationList.Count > 0)
                {
                    return Ok(new { status = true, data = _notificationList, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Notification/GetDriverNotifications : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = Array.Empty<NotificationViewModel>(), message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("API: Notification/GetDriverNotifications : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Delete User Notifications
        [Authorize]
        [HttpDelete("DeleteUserNotification")]
        public async Task<IActionResult> DeleteNotification(string notifId)
        {
            try
            {
                var _notifId = string.IsNullOrEmpty(notifId) ? string.Empty : notifId;
                var userId = CommonFunctions.getUserId(User);
                var _del = await _notificationService.DeletePushNotifications(_notifId, userId);
                if (_del > 0)
                {
                    return Ok(new { status = true, data = new { }, message = ResponseMessages.msgNotificationDeleted, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Notification/DeleteUserNotification : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("API: Notification/DeleteUserNotification : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpDelete("DeleteDriverNotification")]
        public async Task<IActionResult> DeleteDriverNotification(int driverId, string notifId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Notification/DeleteDriverNotification : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _notifId = string.IsNullOrEmpty(notifId) ? string.Empty : notifId;
                var _del = await _notificationService.DeleteDriverNotifications(_notifId, driverId);
                if (_del > 0)
                {
                    return Ok(new { status = true, data = new { }, message = ResponseMessages.msgNotificationDeleted, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Notification/DeleteDriverNotification : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("API: Notification/DeleteDriverNotification : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion
    }
}
