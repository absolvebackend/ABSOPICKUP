using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface IAdminService
    {
        Task<List<ParcelCategory>> GetAllParcelCategory();
        Task<List<ParcelSubCategoryViewModel>> GetAllParcelSubCategory(int id);
        public bool ApproveDriverDetails(ApproveDriverViewModel model);
        public bool ApproveAppUserDetails(ApproveAppUserViewModel model);
        List<DriverListViewModel> GetAllDriverDetails(int pgSize, int pgNumber);
        List<UserListViewModel> GetAllUsersList(int pgSize, int pgNumber);
        List<UserListViewModel> GetAllApprovedUsersList(int pgSize, int pgNumber);
        List<UserListViewModel> GetAllRejectedUsersList(int pgSize, int pgNumber);
        //List<BusinessUserDetailViewModel> GetAllBusinessUserDetails(int pgSize, int pgNumber);
        Task<int> UploadDriverDocuments(DriverDocuments model);
        Task<int> UploadIndividualUserDocuments(IndividualUserDocuments model);
        Task<int> UploadBusinessUserDocuments(BusinessUserDocumentsViewModel model);
        string DoesDriverExists(int driverId);
        string DoesUserExists(int userTypeId, string userId);
        List<DeliveryDetailReportViewModel> GetParcelDetailsByDriver(int driverId, int pgSize, int pgNumber);
        List<DeliveryDetailReportViewModel> GetParcelDetailsByUser(string userId, int pgSize, int pgNumber);
        BusinessUserDetailViewModel GetBusinessUserDetailsById(string userId);
        IndividualUserDetailsViewModel GetIndiviualUserDetailsById(string userId);
        Task<int> UpdateDriverInfo(DriverInfoUpdateViewModel model);
        Task<int> AdminDriverSendVetNotifications(int driverId);
        Task<int> AdminUserSendVetNotifications(string userId);
    }
}
