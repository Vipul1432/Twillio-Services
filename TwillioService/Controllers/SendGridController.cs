using Microsoft.AspNetCore.Mvc;
using TwillioService.Dtos;
using TwillioService.Interfaces;

namespace TwillioService.Controllers
{
    [Route("api/sendgrid")]
    [ApiController]
    public class SendGridController : ControllerBase
    {
        private readonly ILogger<SendGridController> _logger;
        private readonly IEmailService _emailService;
        public SendGridController(ILogger<SendGridController> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        /// Sends an email asynchronously by accepting an EmailRequestDto containing email details.
        /// 
        /// Parameters:
        ///   request (EmailRequestDto): An object containing email details such as recipient email address, subject, plain text content, and HTML content.
        /// 
        /// Returns:
        ///   Task<IActionResult>: A task representing the asynchronous operation. The task result is an IActionResult representing the result of the email sending operation.
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequestDto request)
        {
            try
            {
                bool result = await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.PlainTextContent, request.HtmlContent);
                if (result)
                {
                    _logger.LogInformation("Message sent successfully");
                    return Ok();
                }
                else
                {
                    _logger.LogError("Failed to send email.");
                    return BadRequest("Failed to send email.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending email.");
                return StatusCode(500, "An error occurred while sending email.");
            }
        }
    }
}
