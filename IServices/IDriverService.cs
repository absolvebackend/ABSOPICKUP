using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface IDriverService
    {
        bool AddDriverPersonalInfo(DriverDetails model);
        DriverDetails FindByPhoneNumberAsync(string phoneNumber);
        DriverDetails FindByEmailAddressAsync(string email);
        bool AddDriverDocuments(string profilepic, string dob, DriverDocuments model);
        bool AddDocuments(DriverDocuments model, DriverDocuments datamodel);
        Task<DriverDetails> FindDriverById(string id);
        bool UpdateAsync(DriverDetails model);
        bool UpdateDriver(string id, string devicetype, string devicetoken);
        DriverInfoViewModel GetDriverInfo(int driverId);
        Task<UserDetailsViewModel> GetDriverLoginResponse(string userId, string accessToken, string loginType);
        AvailableRequestViewModel GetAvailableRequestDetails(int noticeId);
        UnAssignedRequestsViewModel GetAllUnassignedParcels();
        Task<int> SaveNoticeForDriver(DriverPositionViewModel driverPosition);
        List<AvailableRequestViewModel> GetRequestNoticeForTopDrivers(int driverId);
        List<AvailableRequestViewModel> GetAllOpenRequestNotice(int driverId);
        Task<int> DriverResponseToNewRequest(AssignRequestViewModel reqDetails);
        double GetAverageDriverRating(int driverId);
        int SaveDriverRating(int OrderId, DriverRatings rating);
        DriverRatingViewModel GetDriverRating(int driverId);
        List<DriverAcceptedRequests> GetDriverAcceptedRequest(int driverId);
        Task<int> AddDriverWorkStatus(int driverId, bool Online);
        List<DriverWorkStatus> GetDriverWorkStatus(int driverId, DateTime start, DateTime finish);
        Task<Orders> CreateOrderForAcceptedRequest(int requestId, int driverId);
        List<string> GetSenderDeviceTokens(int orderId);
        List<string> GetDriverDeviceTokens(int orderId);
        List<string> GetDeviceTokenForDriver(int driverId);
        Task<ParcelDelivery> VerifyDeliveryPIN(VerifyPINViewModel model);
        bool IsEmailRegistered(string email);
        bool IsPhoneRegistered(string phoneNumber);
        RequestDetailsViewModel GetRequestDetails(int requestId);
        List<DriverAcceptedRequests> GetDriverWorkInProgress(int driverId);
        List<DriverAcceptedRequests> GetDriverCompletedRequests(int driverId);
        Task<int> SetDriverApplicationStatus(int driverId, int appStatus);
        Task<int> AddUpdateDriverBankDetails(DriverBankDetails model);
        DriverBankDetails GetDriverBankDetails(int driverId);
        List<GetDocTypeViewModel> GetPersonalIdDocType();
        int ChangePassword(int driverId, string OldPassword, string NewPassword);
        bool GetDriverCurrentWorkStatus(int driverId);
    }
}
