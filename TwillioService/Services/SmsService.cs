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

        /// <summary>
        /// Asynchronously sends an SMS message to the specified recipient using the Twilio API.
        /// </summary>
        /// <param name="to">The phone number of the recipient, in E.164 format.</param>
        /// <param name="body">The content of the SMS message.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean indicating whether the SMS was successfully sent.
        /// </returns>
        /// <remarks>
        /// This method initializes the Twilio client with the account SID and auth token, then attempts to send the SMS message. 
        /// If the message is sent successfully, it logs the success and returns true. If an error occurs, it logs the error and returns false.
        /// </remarks>
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
