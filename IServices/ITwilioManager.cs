using _AbsoPickUp.ViewModels;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface ITwilioManager
    {
        Task SendMessage(string body, string to);
        Task<TwilioVerificationResult> StartVerificationAsync(string phoneNumber, string channel);
        Task<TwilioVerificationResult> CheckVerificationAsync(string phoneNumber, string code);
    }
}
