using SendGrid;
using SendGrid.Helpers.Mail;

namespace ShopApp.WebUI.EmailServices;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Get API key from configuration or environment variable
        var sendGridKey = _configuration["SendGrid:ApiKey"] 
            ?? Environment.GetEnvironmentVariable("SENDGRID_API_KEY") 
            ?? throw new InvalidOperationException("SendGrid API key is not configured");
        
        return Execute(sendGridKey, subject, htmlMessage, email);
    }

    private static async Task Execute(string sendGridKey, string subject, string message, string email)
    {
        var client = new SendGridClient(sendGridKey);

        var msg = new SendGridMessage()
        {
            From = new EmailAddress("info@shopapp.com", "Shop App"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };

        msg.AddTo(new EmailAddress(email));
        await client.SendEmailAsync(msg);
    }
}
