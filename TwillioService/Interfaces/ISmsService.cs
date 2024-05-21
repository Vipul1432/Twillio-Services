namespace TwillioService.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string to, string body);
        Task<bool> SendWhatsAppSmsAsync(string to, string body);
    }
}
