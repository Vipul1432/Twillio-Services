namespace TwillioService.Interfaces
{
    public interface ISmsService
    {
        /// <summary>
        /// Asynchronously sends an SMS message to the specified recipient.
        /// </summary>
        /// <param name="to">The phone number of the recipient, in E.164 format.</param>
        /// <param name="body">The content of the SMS message.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the SMS was successfully sent.</returns>
        Task<bool> SendSmsAsync(string to, string body);
    }
}
