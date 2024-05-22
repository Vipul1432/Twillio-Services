namespace TwillioService.Dtos
{
    public class EmailRequestDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }

    }
}
