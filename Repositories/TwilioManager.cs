﻿using _AbsoPickUp.Common;
using _AbsoPickUp.IServices;
using _AbsoPickUp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;

namespace _AbsoPickUp.Repositories
{
    public class TwilioManager : ITwilioManager
    {
        public TwilioManager()
        {
            TwilioClient.Init(GlobalVariables.twilio_accountSid, GlobalVariables.twilio_authToken);
        }

        public Task SendMessage(string body, string to)
        {
            var message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber(GlobalVariables.twilio_phoneNumber),
                to: new Twilio.Types.PhoneNumber(to)
            );

            if (message.Status.ToString() == "Sent")
            {
                return Task.CompletedTask;
            }
            else
                return Task.CompletedTask;
        }

        public async Task<TwilioVerificationResult> StartVerificationAsync(string phoneNumber, string channel)
        {
            try
            {
                var verificationResource = await VerificationResource.CreateAsync(
                    to: phoneNumber,
                    channel: channel,
                    pathServiceSid: GlobalVariables.twilio_verificationSid
                );
                return new TwilioVerificationResult(verificationResource.Sid);
            }
            catch (TwilioException e)
            {
                return new TwilioVerificationResult(new List<string> { e.Message });
            }
        }

        public async Task<TwilioVerificationResult> CheckVerificationAsync(string phoneNumber, string code)
        {
            try
            {
                var verificationCheckResource = await VerificationCheckResource.CreateAsync(
                    to: phoneNumber,
                    code: code,
                    pathServiceSid: GlobalVariables.twilio_verificationSid
                );
                return verificationCheckResource.Status.Equals("approved") ?
                    new TwilioVerificationResult(verificationCheckResource.Sid) :
                    new TwilioVerificationResult(new List<string> { "Wrong code. Try again." });
            }
            catch (TwilioException e)
            {
                return new TwilioVerificationResult(new List<string> { e.Message });
            }
        }
    }
}
