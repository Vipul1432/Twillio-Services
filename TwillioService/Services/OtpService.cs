using Twilio;
using Twilio.Rest.Api.V2010.Account;
using TwillioService.Interfaces;

namespace TwillioService.Services
{
    public class OtpService : IOtpService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OtpService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OtpService(IConfiguration configuration, ILogger<OtpService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _accountSid = _configuration["Twilio:AccountSid"] ?? String.Empty;
            _authToken = _configuration["Twilio:AuthToken"] ?? String.Empty;
            _fromNumber = _configuration["Twilio:fromNo"]?? String.Empty;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            if (string.IsNullOrEmpty(_accountSid) || string.IsNullOrEmpty(_authToken) || string.IsNullOrEmpty(_fromNumber))
            {
                _logger.LogCritical("Twilio secret key is not configured.");
                throw new InvalidOperationException("Twiliio secret key is not configured.");
            }
        }

        /// <summary>
        /// Sends an OTP (One-Time Password) to the specified phone number asynchronously using Twilio.
        /// </summary>
        /// <param name="to">The recipient phone number to which the OTP will be sent.</param>
        /// <returns>
        /// True if the OTP message is sent successfully; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method generates a random OTP, sends it via Twilio's SMS service to the specified phone number,
        /// and stores the OTP for verification purposes.
        /// </remarks>
        public async Task<bool> SendOtpAsync(string to)
        {
            TwilioClient.Init(_accountSid, _authToken);

            try
            {
                string otp = GenerateOtp();

                var message = await MessageResource.CreateAsync(
                    body: $"Your OTP is: {otp}",
                    from: new Twilio.Types.PhoneNumber(_fromNumber),
                    to: new Twilio.Types.PhoneNumber(to)
                );

                StoreOtp(to, otp);

                _logger.LogInformation("Message sent successfully");
                return !string.IsNullOrEmpty(message.Sid);
            }
            catch (Twilio.Exceptions.ApiException ex)
            {
                _logger.LogError(ex, "Twilio API Error while sending OTP to {To}", to);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending OTP to {To}", to);
                return false;
            }
        }

        /// <summary>
        /// Verifies the provided OTP (One-Time Password) against the stored OTP for the specified phone number.
        /// </summary>
        /// <param name="to">The recipient phone number whose OTP needs to be verified.</param>
        /// <param name="otp">The OTP provided by the user for verification.</param>
        /// <returns>
        /// True if the provided OTP matches the stored OTP; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method retrieves the stored OTP for the specified phone number and compares it with the provided OTP.
        /// If they match, it returns true indicating successful verification; otherwise, it returns false.
        /// </remarks>
        public bool VerifyOtpAsync(string to, string otp)
        {
            try
            {
                string storedOtp = GetStoredOtp(to);

                if (storedOtp == otp)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while verifying OTP for {To}", to);
                return false;
            }
        }

        /// <summary>
        /// Generates a 6-digit OTP (One-Time Password) consisting of numeric characters.
        /// </summary>
        /// <returns>A string representing the generated 6-digit OTP.</returns>
        /// <remarks>
        /// This method uses a random number generator to create a 6-character string composed of digits from 0 to 9.
        /// </remarks>
        private string GenerateOtp()
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Stores the generated OTP in the session associated with the specified phone number.
        /// </summary>
        /// <param name="to">The recipient phone number.</param>
        /// <param name="otp">The generated OTP to be stored.</param>
        /// <remarks>
        /// This method stores the OTP in the session under a key based on the recipient phone number.
        /// If the HttpContext is null, it logs a warning message indicating that the OTP could not be stored.
        /// </remarks>
        private void StoreOtp(string to, string otp)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Session.SetString($"OTP_{to}", otp);
            }
            else
            {
                _logger.LogWarning("HttpContext is null. Unable to store OTP in the session.");
            }
        }

        /// <summary>
        /// Retrieves the stored OTP for the specified phone number from the session.
        /// </summary>
        /// <param name="to">The recipient phone number whose OTP needs to be retrieved.</param>
        /// <returns>
        /// The stored OTP if found; otherwise, an empty string.
        /// </returns>
        /// <remarks>
        /// This method retrieves the OTP from the session using the key based on the recipient phone number.
        /// If the HttpContext or session is null, it logs a warning message and returns an empty string.
        /// </remarks>
        private string GetStoredOtp(string to)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var session = httpContext.Session;
                if (session != null)
                {
                    return session.GetString($"OTP_{to}") ?? String.Empty;
                }
                else
                {
                    _logger.LogWarning("Session object is null. Unable to retrieve OTP from the session.");
                }
            }
            else
            {
                _logger.LogWarning("HttpContext is null. Unable to retrieve OTP from the session.");
            }

            return String.Empty;
        }
    }
}
