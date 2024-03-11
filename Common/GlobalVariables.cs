using System;

namespace _AbsoPickUp.Common
{
    public static class GlobalVariables
    {
        #region " Folder Path i.e Images Container Names"
        public static readonly string profilePictureContainer = "UserImages/";
        public static readonly string ReceiverPINSentSMSText = "Hi, This is AbsoPickUp Delivery! Our driver is on the way with your parcel. Your PIN for accepting parcel is ";
        public static readonly string DriverEnRouteSentSMSTextETA = "Hi, This is AbsoPickUp Delivery! Our driver is on the way with your parcel. Delivery will be arriving in ";
        public static readonly string DriverEnRouteSentSMSTextAddress = ". Parcel will be delivered to this address - ";
        public static readonly string ParcelWithDriverNotificationText = "Your parcel has been received by our executive.";
        public static readonly string ParcelEnRouteNotificationText = "Your parcel is out for delivery.";
        public static readonly string ParcelArrivedNotificationText = "Your parcel has arrived at the destination.";
        public static readonly string ParcelDeliveredNotificationText = "Your parcel has been delivered.";
        public static readonly string NewParcelRequestNotificationText = "New parcel request available in your area.";
        public static readonly string NewUserAcceptedNotificationText = "Your application for AbsoPickUp delivery app has been accepted.";
        public static readonly string NewDriverAcceptedNotificationText = "Your application for AbsoPickUp delivery app has been accepted.";
        public static readonly string NewUserRejectedNotificationText = "Your application for AbsoPickUp delivery app has been rejected. Reason for rejection - ";
        public static readonly string NewDriverRejectedNotificationText = "Your application for AbsoPickUp delivery app has been rejected. Reason for rejection - ";
        public static readonly string NotificationTitle = "AbsoPickUp Delivery App";


        public static readonly int addHoursToUTCDatetimeForSA = 2;
        public static readonly int notifyDriverRequestSentTimeInMins = 5;
        public static readonly int notifyDriverOpenLastMins = 10;
        public static readonly int notifyDriverWithinKms = 5;
        public static readonly int notifyNoOfTopDrivers = 3;

        public static readonly string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm";
        public static readonly string DefaultDateFormat = "yyyy-MM-dd";
        public static readonly string DefaultTimeFormat = "HH:mm";

        #endregion
        public enum LoginType
        {
            Email,
            Facebook,
            Google
        }

        #region "Twillio Validate Credentials"
        public const string twilio_accountSid = "ACb8eb0ba9b105c746968838cc692be1ea";
        public const string twilio_authToken = "ae8416b04b8b3a118c46bfcff19252d9";
        public const string twilio_verificationSid = "VA1e59b90d55b07883e09ba82b26d3b5bd";
        public const string twilio_phoneNumber = "+14255376731";
        #endregion

        #region "Paypal Account Credentials"
        public const string paypal_client_id = "AXWyVsYc1hQnAhTa-gZc8mCaNoWbP7nhi6Yxvma4K9wOKh84a_xq4KdKrA1TpKnti6t7sy3i3bH-QJL-";
        public const string paypal_client_secret = "EKyOTmn0uv47g9g6dVNB8ckrFKJBccKLPVwUfV99lo81guWlzdkcv3WygBv4i9yrW42qW9SIGDceYmpa";
        public const string paypal_mode = "sandbox";
        #endregion

        #region " Folder Path i.e Document Files Container Names"
        public static readonly string LicenceFileContainer = "LicenceFiles/";
        public static readonly string VATFileContainer = "VATfiles/";
        public static readonly string ChamberCommerceFileContainer = "CCFiles/";
        public static readonly string AgreementFileContainer = "AgreementFiles/";
        public static readonly string ProfilePicContainer = "ProfilePic/";
        public static readonly string DriverDocumentContainer = "DriverDocuments/";
        public static readonly string IDProofDocContainer = "IDProof/";
        public static readonly string SelfiePicDocContainer = "SelfiePic/";
        public static readonly string ParcelContainer = "ParcelPhotos/";
        public static readonly string VehicleImagesContainer = "VehicleImages/";
        public static readonly string AdminFilesContainer = "AdminFiles/";
        #endregion

        #region " Google Firebase region"
        public static readonly string FirebaseUserAppServerKey = "AAAAvKcFOIk:APA91bF9i36u71CE8Y4f3hsAIq5v3FR_rXJjoEZuYSTZ-hXYar9TTiraL4c3feGOTzKI4bvrLwZXbfR-rRZD4QNZeNq4qc4MfURpPNBzTL81eiMpPWCM7AJbVsng26xDqOFAPV_DEUIs";
        public static readonly string FirebaseDriverAppServerKey = "AAAAgBeOs7U:APA91bEFr7qlGV5VmXMETBFXiliQeYjZinADxGDR7q2tlnM9pqZG7JyENDViJR8Fh1gHfUKD6ivheYnc-6AA0QgHS9vfBxXGQ02FDFCaB7yVSC9Iamdb-QtNnM7o7D_im9CK5Dqs7Tt4";
        public static readonly Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");

        public static string API_KEY = "AIzaSyBgLMQ8wvy5yda0qP1_8y1e_aJJ_HrTdZw";
        #endregion

        public enum AppUserTypes
        {
            Individual = 1,
            Business,
            Admin
        }
        public enum TwilioChannelTypes
        {
            Sms = 1,
            Call,
            Email
        }
        public enum ApplicationStatus
        {
            Incomplete = 1,
            AwaitingVerification,
            Verified,
            Rejected
        }

        public enum ParcelStatus
        {
            Unassigned = 1,
            Assigned,
            WithDriver,
            DeliveryOnRoute,
            Arrived,
            Delivered,
            Cancelled
        }

        public enum DocTypes
        {
            License = 1,
            ProofOfResidence,
            NumberPlatePic,
            VehicleRegisteration,
            IDBook,
            IDCard,
            Selfie,
            ElectionCard,
            BirthCertificate,
            Passport,
            CarDiscPic,
            DriverProfilePic,
            AnyIDProof,
            ImageBeforePacking,
            ImageAfterPacking,
            VehicleFrontsideImage,
            VehicleBacksideImage,
            IDCardBack,
            VATFile,
            ChamberOfCommerceFile,
            BusinessLicense,
            AgreementFile
        }

        public enum TransactionType
        {
            Credit = 1,
            Debit,
            Refund
        }

        public enum PaymentStatus
        {
            Pending = 0, //your payment has not yet been sent to the bank.
            Success, //your checking, savings or other bank account payment has been processed and accepted
            Complete, //your checking, savings or other bank account payment has been processed and accepted
            Canceled, //you stopped the payment before it was processed
            Rejected //your payment was not accepted when it was processed by the bank or credit card company.
        }

        public enum ParcelType
        {
            Normal = 1,
            Express,
            Bakkie
        }

        public enum VehicleType
        {
            Scooter = 1,
            Car,
            Bakkie
        }

        public enum Screens
        {
            DriverPersonalInfo = 1,
            DriverDocuments,
            VehicleDetails,
            VehicleDocs
        }

        public enum OrderStatus
        {
            Pending = 1,
            Processing,
            Complete,
            Cancelled
        }

        public enum PaymentMode
        {
            CreditCard = 1,
            DebitCard,
            Paypal,
            Other
        }

        public enum NotificationTypes
        {
            NEW_PARCEL_REQUEST = 1,
            CANCELLED_REQUEST,
            CONFIRMED_REQUEST,
            REJECTED_REQUEST,
            ORDER_CONFIRMED,
            ORDER_CANCELLED,
            ORDER_COMPLETED,
            MESSAGE_RECIEVED,
            ADMIN_MESSAGES,
            ADMIN_APPROVED,
            ADMIN_REJECTED,
            SENDER_WAITING,
            CALL_REMIND_ALERT,
            CALL_REMAINING_TIME_ALERT,
            DRIVER_ASSIGNED,
            PARCEL_WITH_DRIVER,
            PARCEL_ENROUTE,
            PARCEL_ARRIVED,
            PARCEL_DELIVERED
        }
    }
}
