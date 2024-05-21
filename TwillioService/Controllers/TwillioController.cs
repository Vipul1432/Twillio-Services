using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwillioService.Dtos;
using TwillioService.Interfaces;

namespace TwillioService.Controllers
{
    [Route("api/[controller]")]
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
    }
}
