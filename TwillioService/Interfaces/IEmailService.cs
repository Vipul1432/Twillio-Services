namespace TwillioService.Interfaces
{
    public interface IEmailService
    {
        /// Sends an email asynchronously using SendGrid API.
        /// 
        /// Parameters:
        ///   toEmail (string): The email address of the recipient.
        ///   subject (string): The subject of the email.
        ///   plainTextContent (string): The plain text content of the email.
        ///   htmlContent (string): The HTML content of the email.
        /// 
        /// Returns:
        ///   Task<bool>: A task representing the asynchronous operation. The task result is true if the email was sent successfully; otherwise, false.
        Task<bool> SendEmailAsync(string toEmail, string subject, string plainTextContent, string htmlContent);
    }
}
