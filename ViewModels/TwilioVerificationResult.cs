using System.Collections.Generic;

namespace _AbsoPickUp.ViewModels
{
    public class TwilioVerificationResult
    {
        public TwilioVerificationResult(string sid)
        {
            Sid = sid;
            IsValid = true;
        }

        public TwilioVerificationResult(List<string> errors)
        {
            Errors = errors;
            IsValid = false;
        }

        public bool IsValid { get; set; }

        public string Sid { get; set; }

        public List<string> Errors { get; set; }
    }
}
