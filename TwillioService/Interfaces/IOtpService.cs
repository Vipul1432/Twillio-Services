namespace TwillioService.Interfaces
{
    public interface IOtpService
    {
        /// <summary>
        /// Sends an OTP (One-Time Password) to the specified phone number.
        /// </summary>
        /// <param name="to">The recipient phone number.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a result of true if the OTP was sent successfully, otherwise false.
        /// </returns>
        /// <remarks>
        /// This method generates an OTP, sends it to the specified phone number using Twilio, 
        /// and stores the OTP in the session for future verification. Logs messages indicating the success or failure of the operation.
        /// </remarks>
        Task<bool> SendOtpAsync(string to);

        /// Verifies the provided OTP against the stored OTP for the specified phone number.
        /// </summary>
        /// <param name="to">The recipient phone number.</param>
        /// <param name="otp">The OTP to be verified.</param>
        /// <returns>
        /// True if the provided OTP matches the stored OTP; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method retrieves the stored OTP from the session and compares it with the provided OTP.
        /// Logs messages indicating the success or failure of the verification.
        /// </remarks>
        bool VerifyOtpAsync(string to, string otp);
    }
}
