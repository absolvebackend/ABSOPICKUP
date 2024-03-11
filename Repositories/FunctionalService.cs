using _AbsoPickUp.Common;
using _AbsoPickUp.Data;
using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace _AbsoPickUp.Repositories
{
    public class FunctionalService : IFunctionalService
    {
        private readonly UserManager<UserDetails> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;

        public FunctionalService(UserManager<UserDetails> userManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
        }
        
        public async Task SendEmailByGmailAsync(string fromEmail, string fromFullName, string subject, string messageBody, string toEmail, string toFullName, string smtpUser, string smtpPassword, string smtpHost, int smtpPort, bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = smtpUser,
                    Password = smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.EnableSsl = smtpSSL;
                await smtp.SendMailAsync(message);
            }
        }

        public async Task SendEmailBySendGridAsync(string apiKey, string fromEmail, string fromFullName, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);
        }


        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                UserDetails superAdmin = new UserDetails();
                superAdmin.Email = _superAdminDefaultOptions.Email;
                superAdmin.UserName = _superAdminDefaultOptions.Email;
                superAdmin.NormalizedUserName = _superAdminDefaultOptions.Email;
                superAdmin.NormalizedEmail = _superAdminDefaultOptions.Email;
                superAdmin.EmailConfirmed = true;
                superAdmin.FirstName = "Admin";
                superAdmin.LastName = "TwoMaMina";
                superAdmin.CreatedDate = DateTime.UtcNow;
                superAdmin.DeviceType = "web";
                superAdmin.DeviceToken = "web@123";
                superAdmin.CreatedBy = "AbsoPickUp API Dev";
                superAdmin.CreatedDate = DateTime.Now;
                superAdmin.UserTypeId = (int)GlobalVariables.AppUserTypes.Admin;

                await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
