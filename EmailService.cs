using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string templateId, Dictionary<string, string> templateData)
    {
        var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
        var from = new EmailAddress("your-email@example.com", "Your Name");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, templateData);
        var response = await client.SendEmailAsync(msg);
    }
}
