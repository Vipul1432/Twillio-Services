using SendGrid;
using SendGrid.Helpers.Mail;
using TwillioService.Interfaces;

namespace TwillioService.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// Sends an email using SendGrid API.
        /// </summary>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="plainTextContent">The plain text content of the email.</param>
        /// <param name="htmlContent">The HTML content of the email.</param>
        /// <returns>True if the email was sent successfully; otherwise, false.</returns>
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string plainTextContent, string htmlContent)
        {
            try
            {
                var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
                var from = new EmailAddress(_configuration["SendGrid:From"], _configuration["SendGrid:Name"]);
                var to = new EmailAddress(toEmail);

                var msg = MailHelper.CreateSingleEmail(
                    from,
                    to,
                    subject,
                    plainTextContent,
                    htmlContent);

                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to send email to {ToEmail}. Status code: {StatusCode}", toEmail, response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email to {ToEmail}", toEmail);
                return false;
            }
        }
    }
}
