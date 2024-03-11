namespace _AbsoPickUp.Common
{
    public class ResponseMessages
    {
        public static readonly string msgParametersNotCorrect = "The parameters are incorrect";
        public static readonly string msgSamePasswords = "Old password and new password cannot be same";
        public static readonly string msgEmailNotCorrect = "This email already exists.";
        public static readonly string msgCreationSuccess = "created successfully";
        public static readonly string msgSomethingWentWrong = "Something went wrong. ";
        public static readonly string msgOTPSentSuccess = "OTP sent on your email";
        public static readonly string msgPhoneNoVerifiedSuccess = "Your phone number verified successfully";
        public static readonly string msgOTPSentOnMobileSuccess = "OTP sent on your phone no";
        public static readonly string msgVerifiedUser = "You are verified successfully";
        public static readonly string msgInvalidOTP = "OTP is invalid";
        public static readonly string msgUserLoginSuccess = "User login successfully";
        public static readonly string msgDriverLoginSuccess = "Driver login successful";
        public static readonly string msgAdminLoginSuccess = "Admin login successfull";
        public static readonly string msgInvalidCredentials = "Username or password is incorrect";
        public static readonly string msgTokenExpired = "Access token is expired";
        public static readonly string msgPhoneNumberNotConfirmed = "Phone Number not confirmed. Please confirm your phone number to access your account";
        public static readonly string msgEmailNotConfirmed = "Email not confirmed. Please confirm your email to access your account";
        public static readonly string msgCouldNotFoundAssociatedUser = "User not found";
        public static readonly string msgUserStatusPendingForApproval = "Your account is under review. Our team will contact you soon regarding approval.";
        public static readonly string msgUserBlockedByAdmin = "You are blocked or rejected by admin. Please contact site administrator";
        public static readonly string msgProfilePicUpdated = "Profile picture updated successfully";
        public static readonly string msgUpdationSuccess = " updated successfully";
        public static readonly string msgAddUpdationSuccess = " added / updated successfully";
        public static readonly string msgUpdateFailed = " could not be updated. Please try again.";
        public static readonly string msgAddUpdateFailed = " could not be added / updated. Please try again.";
        public static readonly string msgGotSuccess = "Data shown successfully";
        public static readonly string msgNotificationDeleted = "Notification deleted successfully";
        public static readonly string msgPasswordChangeSuccess = "Password changed successfully";
        public static readonly string msgPaymentSuccess = "Payment created successfully.";
        public static readonly string msgLogoutSuccess = "User logout successfully";
        public static readonly string msgUploadedSuccessfully = "Uploaded successfully";
        public static readonly string msgDatanotfound = "Data not found";
        public static readonly string msgRequestStatusNotValid = "Data available only for UnAssigned, Assigned or Completed Requests.";
        public static readonly string msgRequestNotFound = "No new requests found near your location.";
        public static readonly string msgAdditionSuccess = " added successfully";
        public static readonly string msgPINSentSuccess = " added successfully. PIN sent successfully to receiver.";
        public static readonly string msgPINVerificationSuccess = "PIN verification success.";
        public static readonly string msgPINVerificationFailure = "PIN verification failed.";
        public static readonly string msgDocUploadSuccess = "Your account will be activated after documents are approved in 2-3 business days.";
        public static readonly string msgDocsIncomplete = "Your documents not upload successfully. Please upload again.";
        public static readonly string msgDocsRejected = "Your documents are rejected by us. Reject Reason - ";
        public static readonly string msgPhoneAlreadyExists = "Phone Number already exist. Please try to login.";
        public static readonly string msgUploadSuccess = "image uploaded successfully";
        public static readonly string msgRegister = "Register successfully";
        public static readonly string msgDeletionSuccess = " deleted successfully";
        public static readonly string msgCancelled = " cancelled successfully";
        public static readonly string msgAlreadyUsedCredentials = "Credentials are already being used.";
        public static readonly string msgRequestAlreadyAccepted = "This request is already accepted.";
        public static readonly string msgRequestRejected = "This request is successfully rejected.";
        public static readonly string msgOrderCreated = "Request successfully accepted. Issued Order No. : ";
        public static readonly string msgRequestOrderCancelled = "Request has either been cancelled by user or assigned to another driver or previous order not cancelled.";
        public static readonly string msgAlreadyRatedForOrder = "Driver already rated by user for this Order.";
        public static readonly string msgRequestCannotBeCancelled = "This parcel delivery request cannot be cancelled.";
    }
}
