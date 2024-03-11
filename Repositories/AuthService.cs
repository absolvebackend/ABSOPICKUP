using _AbsoPickUp.Common;
using _AbsoPickUp.Data;
using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserDetails> _userManager;
        public AuthService(ApplicationDbContext context, UserManager<UserDetails> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserDetails> CheckUserExistsOrNot(ClaimsPrincipal User)
        {
            try
            {
                string CurrentUserId = CommonFunctions.getUserId(User);
                if (!string.IsNullOrEmpty(CurrentUserId))
                {
                    var user = await _userManager.FindByIdAsync(CurrentUserId);
                    if (user != null)
                        return user;
                }
                return null;
            }
            catch
            {
                throw;
            }

        }
        //add update business user
        public bool BusinessUserDetails(string id, BusinessDetails model)
        {
            try
            {
                var dataModel = _context.BusinessDetails.FirstOrDefault(x => x.UserId == model.UserId);
                if (dataModel == null)
                {
                    _context.BusinessDetails.Update(model);
                }
                else
                {
                    var details = new BusinessDetails
                    {
                        UserId = id,
                        Website = model.Website,
                        ContactPerson = model.ContactPerson,
                        LicenceNumber = model.LicenceNumber,
                        VAT = model.VAT,
                        ExternalContractNumber = model.ExternalContractNumber
                    };
                    _context.BusinessDetails.Add(details);
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
        //add business user documents
        public bool BusinessUserDocuments(string id, BusinessDocuments model)
        {
            try
            {
                var dataModel = _context.BusinessDocuments.FirstOrDefault(x => x.UserId == model.UserId);
                if (dataModel == null)
                {
                    _context.BusinessDocuments.Update(model);
                }
                else
                {
                    var documents = new BusinessDocuments
                    {
                        UserId = id,
                        LicenceFilePath = model.LicenceFilePath,
                        VATFilePath = model.VATFilePath,
                        ChamberCommerceFilePath = model.ChamberCommerceFilePath,
                        AgreementFilePath = model.AgreementFilePath
                    };
                    _context.BusinessDocuments.Add(documents);
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
        //register new user response
        public async Task<UserDetailsViewModel> GetUserRegisterResponse(string userId, string accessToken)
        {
            try
            {
                var _registerResponse = await (from u in _context.UserDetails
                                               join d in _context.IndividualUserDocuments
                                               on u.Id equals d.IndividualUserId
                                               where u.Id == userId && u.IsDeleted == false
                                               select new UserDetailsViewModel()
                                               {
                                                   FirstName = u.FirstName,
                                                   LastName = u.LastName,
                                                   Email = u.Email,
                                                   IsEmailConfirmed = u.EmailConfirmed,
                                                   DialCode = u.DialCode,
                                                   PhoneNumber = u.PhoneNumber,
                                                   IsPhoneNoVerified = u.PhoneNumberConfirmed,
                                                   UserTypeId = u.UserTypeId,
                                                   IDProof = d.DocImgPath,
                                                   Selfie = d.DocImgPath,
                                                   accessToken = accessToken,
                                                   CountryCode = CountryCodes.GetCountryCode("+" + u.DialCode)
                                               }).FirstOrDefaultAsync();
                return _registerResponse;
            }
            catch
            {
                throw;
            }
        }
        //register new business user response
        public async Task<BusinessUserDetailsViewModel> GetBusinessUserRegisterResponse(string userId, string accessToken)
        {
            try
            {
                var _registerResponse = await (from u in _context.UserDetails
                                               join s in _context.BusinessDetails
                                               on u.Id equals s.UserId
                                               join m in _context.BusinessDocuments
                                               on u.Id equals m.UserId
                                               where u.Id == userId && u.IsDeleted == false
                                               select new BusinessUserDetailsViewModel()
                                               {
                                                   FirstName = u.FirstName,
                                                   LastName = u.LastName,
                                                   Email = u.Email,
                                                   DialCode = u.DialCode,
                                                   PhoneNumber = u.PhoneNumber,
                                                   Website = s.Website,
                                                   ContactPerson = s.ContactPerson,
                                                   LicenceNumber = s.LicenceNumber,
                                                   VAT = s.VAT,
                                                   ExternalNumber = s.ExternalContractNumber,
                                                   LicenceFilePath = m.LicenceFilePath,
                                                   VATFilePath = m.VATFilePath,
                                                   ChamberCommerceFilePath = m.ChamberCommerceFilePath,
                                                   AgreementFilePath = m.AgreementFilePath,
                                                   UserTypeId = u.UserTypeId,
                                                   AccessToken = accessToken,
                                                   CountryCode = CountryCodes.GetCountryCode("+" + u.DialCode),
                                                   ApplicationStatusId = u.ApplicationStatus,
                                                   ApplicationStatus = ((ApplicationStatus)u.ApplicationStatus).ToString()
                                               }
                                               ).FirstOrDefaultAsync();
                return _registerResponse;
            }
            catch
            {
                throw;
            }
        }
        //find user by phone number
        public UserDetails FindByPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                return _context.UserDetails.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //login user response
        public async Task<UserDetailsViewModel> GetUserLoginResponse(string userId, string accessToken)
        {
            try
            {
                var _loginResponse = await _context.UserDetails.Where(x => x.Id == userId && x.IsDeleted == false)
                                      .Select(x => new UserDetailsViewModel
                                      {
                                          FirstName = x.FirstName,
                                          LastName = x.LastName,
                                          Email = x.Email,
                                          IsEmailConfirmed = x.EmailConfirmed,
                                          DialCode = x.DialCode,
                                          PhoneNumber = x.PhoneNumber,
                                          IsPhoneNoVerified = x.PhoneNumberConfirmed,
                                          UserTypeId = x.UserTypeId,
                                          accessToken = accessToken,
                                          ApplicationStatusId = x.ApplicationStatus,
                                          ApplicationStatus = ((ApplicationStatus)x.ApplicationStatus).ToString(),
                                          ProfilePic = string.IsNullOrEmpty(x.ProfilePic) ? string.Empty : x.ProfilePic,
                                          CountryCode = CountryCodes.GetCountryCode("+" + x.DialCode),
                                          AppStatusMessage = CommonFunctions.GetAppStatusMessage(x.ApplicationStatus, x.RejectReason)
                                      }).FirstOrDefaultAsync();
                return _loginResponse;
            }
            catch
            {
                throw;
            }
        }

        //login admin response
        public async Task<AdminDetailsViewModel> GetAdminLoginResponse(string userId, string accessToken)
        {
            try
            {
                var _loginResponse = await _context.UserDetails.Where(x => x.Id == userId && x.IsDeleted == false)
                                      .Select(x => new AdminDetailsViewModel
                                      {
                                          FirstName = x.FirstName,
                                          LastName = x.LastName,
                                          Email = x.Email,
                                          IsEmailConfirmed = x.EmailConfirmed,
                                          UserTypeId = x.UserTypeId,
                                          accessToken = accessToken,
                                      }).FirstOrDefaultAsync();
                return _loginResponse;
            }
            catch
            {
                throw;
            }
        }
        //upload user documents
        public bool UploadUserDocuments(IndividualUserDocuments userdoc)
        {
            try
            {
                _context.IndividualUserDocuments.Add(userdoc);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateUserDocuments(IndividualUserDocuments userdoc)
        {
            try
            {
                //if previous doctype matches new doctype for ID Proof
                var _uDoc = _context.IndividualUserDocuments.FirstOrDefault(x => x.IndividualUserId == userdoc.IndividualUserId && x.DocTypeId == userdoc.DocTypeId);
                if (_uDoc != null)
                {
                    _uDoc.DocImgPath = userdoc.DocImgPath;
                    _context.IndividualUserDocuments.Update(_uDoc);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    //remove previous ID Proof records
                    var _doctypes = new List<int>
                    {
                        (int)DocTypes.License,
                        (int)DocTypes.IDBook,
                        (int)DocTypes.Passport,
                        (int)DocTypes.IDCard,
                        (int)DocTypes.IDCardBack
                    };

                    //remove user submitted doctypes as not updating
                    _doctypes.Remove(userdoc.DocTypeId);
                    if (userdoc.DocTypeId == (int)DocTypes.IDCardBack)
                    {
                        //remove ID Card when submitting ID Card Back doc
                        _doctypes.Remove((int)DocTypes.IDCard);
                    }

                    //get userdocs matching above doc types
                    var _indUserDoc = _context.IndividualUserDocuments.Where(x => x.IndividualUserId == userdoc.IndividualUserId && _doctypes.Contains(x.DocTypeId)).ToList();
                    if (_indUserDoc.Count > 0)
                    {
                        //remove already existing docs
                        _context.IndividualUserDocuments.RemoveRange(_indUserDoc);
                        _context.SaveChanges();
                    }

                    //add new ID Proof
                    _context.IndividualUserDocuments.Add(userdoc);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }
        //get document types for dropdown
        public List<GetDocTypeViewModel> GetDocumentType()
        {
            try
            {
                var _docType = (from d in _context.DocumentTypes
                                where new[] {
                                    (int)DocTypes.IDBook,
                                    (int)DocTypes.IDCard,
                                    (int)DocTypes.Passport,
                                    (int)DocTypes.License }.Contains(d.DocTypeId)
                                select new GetDocTypeViewModel
                                {
                                    DocTypeId = d.DocTypeId,
                                    DocTypeName = d.DocTypeDescription
                                }).ToList();
                return _docType;
            }
            catch
            {
                throw;
            }
        }
        //get all business users
        public async Task<List<BusinessDetails>> GetBusinessUsers()
        {
            try
            {
                var _businessUser = await (from x in _context.BusinessDetails
                                           join p in _context.BusinessDocuments
                                           on x.UserId equals p.UserId
                                           join m in _context.UserDetails
                                           on p.UserId equals m.Id
                                           where m.IsDeleted == false
                                           select x).ToListAsync();
                return _businessUser;
            }
            catch
            {
                throw;
            }
        }
        //check if email registered
        public bool IsEmailRegistered(string email)
        {
            bool _IsEmailRegistered = false;
            var NoOfUsers = _context.UserDetails.Where(x => x.Email == email).ToList().Count;
            if (NoOfUsers > 0)
            {
                _IsEmailRegistered = true;
            }
            return _IsEmailRegistered;
        }
        //check if phone number registered
        public bool IsPhoneRegistered(string phoneNumber)
        {
            bool _IsPhoneRegistered = false;
            var NoOfUsers = _context.UserDetails.Where(x => x.PhoneNumber == phoneNumber).ToList().Count;
            if (NoOfUsers > 0)
            {
                _IsPhoneRegistered = true;
            }
            return _IsPhoneRegistered;
        }
        //update user application status
        public async Task<int> SetUserApplicationStatus(string userId, int appStatus)
        {
            var _user = _context.UserDetails.FirstOrDefault(x => x.Id == userId);
            _user.ApplicationStatus = appStatus;
            _context.UserDetails.Update(_user);
            return await _context.SaveChangesAsync();
        }
    }
}
