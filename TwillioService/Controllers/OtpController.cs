using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwillioService.Interfaces;

namespace TwillioService.Controllers
{
    [Route("api/otp")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly ILogger<OtpController> _logger;

        public OtpController(IOtpService otpService, ILogger<OtpController> logger)
        {
            _otpService = otpService;
            _logger = logger;
        }

        /// <summary>
        /// Sends an OTP (One-Time Password) to the specified phone number.
        /// </summary>
        /// <param name="phoneNumber">The recipient phone number.</param>
        /// <returns>
        /// An IActionResult indicating the result of the operation: 
        /// - Ok("OTP sent successfully") if the OTP was sent successfully.
        /// - BadRequest("Failed to send OTP") if the OTP could not be sent.
        /// - StatusCode(500, "An error occurred while sending OTP") if an unexpected error occurs.
        /// </returns>
        /// <remarks>
        /// This method calls the SendOtpAsync method of the OTP service to send an OTP to the specified phone number.
        /// Logs errors and returns appropriate HTTP status codes based on the outcome.
        /// </remarks>
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(string phoneNumber)
        {
            try
            {
                bool result = await _otpService.SendOtpAsync(phoneNumber);
                if (result)
                    return Ok("OTP sent successfully");
                else
                    return BadRequest("Failed to send OTP");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending OTP to {PhoneNumber}", phoneNumber);
                return StatusCode(500, "An error occurred while sending OTP");
            }
        }

        /// <summary>
        /// Verifies the provided OTP against the stored OTP for the specified phone number.
        /// </summary>
        /// <param name="phoneNumber">The recipient phone number.</param>
        /// <param name="otp">The OTP to be verified.</param>
        /// <returns>
        /// An IActionResult indicating the result of the operation: 
        /// - Ok("OTP verified successfully") if the OTP was verified successfully.
        /// - BadRequest("Invalid OTP") if the provided OTP does not match the stored OTP.
        /// - StatusCode(500, "An error occurred while verifying OTP") if an unexpected error occurs.
        /// </returns>
        /// <remarks>
        /// This method calls the VerifyOtpAsync method of the OTP service to verify the provided OTP.
        /// Logs errors and returns appropriate HTTP status codes based on the outcome.
        /// </remarks>
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp(string phoneNumber, string otp)
        {
            try
            {
                bool result = _otpService.VerifyOtpAsync(phoneNumber, otp);
                if (result)
                    return Ok("OTP verified successfully");
                else
                    return BadRequest("Invalid OTP");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while verifying OTP for {PhoneNumber}", phoneNumber);
                return StatusCode(500, "An error occurred while verifying OTP");
            }
        }
    }
}
