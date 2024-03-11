using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _AbsoPickUp.Common
{
    public class EmailMessages
    {
        public static readonly string regardsMsgFromTeam = "AbsoPickUp";
        public static readonly string confirmationEmailSubject = "Confirmation Email";
        public static readonly string resetPasswordSubject = "Reset Password";
        public static string GetUserRegistrationResendEmailConfirmationMsg(string Name, int Otp)
        {
            string msg = $"Hi {Name}, <br/><br/> Your new confirmation code to access your account is {Otp}. <br/><br/>Thanks <br/>" + regardsMsgFromTeam;
            return msg;
        }
        public static string GetUserForgotPasswordMsg(string Name, int Otp)
        {
            string msg = $"Hi {Name}, <br/><br/> Your one time password is {Otp} for reseting password on AbsoPickUp. <br/><br/>Thanks <br/>" + regardsMsgFromTeam;
            return msg;
        }
    }
}
