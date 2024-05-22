using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwillioService.Dtos;
using TwillioService.Interfaces;

namespace TwillioService.Controllers
{
    [Route("api/twilio")]
    [ApiController]
    public class TwillioController : ControllerBase
    {
        private readonly ISmsService _smsService;
        private readonly ILogger<TwillioController> _logger;

        public TwillioController(ISmsService smsService, ILogger<TwillioController> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to send an SMS message asynchronously.
        /// </summary>
        /// <remarks>
        /// This method receives an SMS request via HTTP POST, sends the SMS using the SMS service, and returns the appropriate HTTP response.
        /// If the message is sent successfully, it returns an Ok result. If the message fails to send, it returns a BadRequest response.
        /// If an exception occurs during the process, it returns an Internal Server Error response.
        /// </remarks>
        /// <param name="request">The SMS request DTO containing the recipient's phone number and the message body.</param>
        /// <returns>An asynchronous action result representing the outcome of the SMS send operation.</returns>
        [HttpPost("send-sms")]
        public async Task<IActionResult> SendSmsAsync([FromBody] SmsRequestDto request)
        {
            try
            {
                bool result = await _smsService.SendSmsAsync(request.To, request.Body);
                if (result)
                {
                    _logger.LogInformation("Message sent successfully");
                    return Ok();
                }
                else
                {
                    _logger.LogError("Failed to send SMS.");
                    return BadRequest("Failed to send SMS.");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while sending SMS.");
                return StatusCode(500, "An error occurred while sending SMS.");
            }
        }

        /// <summary>
        /// Endpoint to send a WhatsApp message asynchronously.
        /// </summary>
        /// <remarks>
        /// This method receives a WhatsApp message request via HTTP POST, sends the message using the SMS service's WhatsApp functionality, 
        /// and returns the appropriate HTTP response. If the message is sent successfully, it returns an Ok result. 
        /// If the message fails to send, it returns a BadRequest response. If an exception occurs during the process, 
        /// it returns an Internal Server Error response.
        /// </remarks>
        /// <param name="request">The SMS request DTO containing the recipient's phone number (prefixed with "whatsapp:") and the message body.</param>
        /// <returns>An asynchronous action result representing the outcome of the WhatsApp message send operation.</returns>
        [HttpPost("send-whatsapp-sms")]
        public async Task<IActionResult> SendWhatsAppSmsAsync([FromBody] SmsRequestDto request)
        {
            try
            {
                bool result = await _smsService.SendWhatsAppSmsAsync(request.To, request.Body);
                if (result)
                {
                    _logger.LogInformation("Message sent successfully");
                    return Ok();
                }
                else
                {
                    _logger.LogError("Failed to send SMS.");
                    return BadRequest("Failed to send SMS.");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while sending SMS.");
                return StatusCode(500, "An error occurred while sending SMS.");
            }
        }
    }
}
