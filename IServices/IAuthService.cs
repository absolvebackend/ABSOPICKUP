using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface IAuthService
    {
        bool BusinessUserDetails(string id, BusinessDetails model);
        bool BusinessUserDocuments(string id, BusinessDocuments model);
        UserDetails FindByPhoneNumberAsync(string phoneNumber);
        Task<UserDetailsViewModel> GetUserLoginResponse(string userId, string accessToken);
        Task<AdminDetailsViewModel> GetAdminLoginResponse(string userId, string accessToken);
        Task<UserDetailsViewModel> GetUserRegisterResponse(string userId, string accessToken);
        Task<BusinessUserDetailsViewModel> GetBusinessUserRegisterResponse(string userId, string accessToken);
        Task<UserDetails> CheckUserExistsOrNot(ClaimsPrincipal User);
        bool UploadUserDocuments(IndividualUserDocuments userdoc);
        bool UpdateUserDocuments(IndividualUserDocuments userdoc);
        List<GetDocTypeViewModel> GetDocumentType();
        Task<List<BusinessDetails>> GetBusinessUsers();
        bool IsEmailRegistered(string email);
        bool IsPhoneRegistered(string phoneNumber);
        Task<int> SetUserApplicationStatus(string userId, int appStatus);
    }
}
