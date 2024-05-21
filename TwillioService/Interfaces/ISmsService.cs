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

        /// <summary>
        /// Asynchronously sends a WhatsApp message to the specified recipient using the Twilio API.
        /// </summary>
        /// <param name="to">The phone number of the recipient, in E.164 format, prefixed with "whatsapp:".</param>
        /// <param name="body">The content of the WhatsApp message.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean indicating whether the WhatsApp message was successfully sent.
        /// </returns>
        Task<bool> SendWhatsAppSmsAsync(string to, string body);
    }
}
