using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;
using Twilio.Types;
using TwillioService.Interfaces;

namespace TwillioService.Services
{
    public class SmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsService> _logger;

        public SmsService(IConfiguration configuration, ILogger<SmsService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _accountSid = _configuration["Twilio:AccountSid"];
            _authToken = _configuration["Twilio:AuthToken"];
            _fromNumber = _configuration["Twilio:fromNo"];
            if (string.IsNullOrEmpty(_accountSid) || string.IsNullOrEmpty(_authToken) || string.IsNullOrEmpty(_fromNumber))
            {
                _logger.LogCritical("Twilio secret key is not configured.");
                throw new InvalidOperationException("Twiliio secret key is not configured.");
            }
        }

        public async Task<bool> SendSmsAsync(string to, string body)
        {
            TwilioClient.Init(_accountSid, _authToken);

            try
            {
                var message = await MessageResource.CreateAsync(
                    body: body,
                    from: new Twilio.Types.PhoneNumber(_fromNumber),
                    to: to
                );
                _logger.LogInformation("Message sent successfully");
                return !string.IsNullOrEmpty(message.Sid);
            }
            catch (Twilio.Exceptions.ApiException ex)
            {
                _logger.LogError($"Twilio API Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending SMS: {ex.Message}");
                return false;
            }
        }
        
    }
}
