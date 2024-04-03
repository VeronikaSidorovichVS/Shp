namespace Shops.Login;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}
public class SendGridEmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly IConfiguration _configuration;


    public SendGridEmailService(ISendGridClient sendGridClient, IConfiguration configuration)
    {
        _sendGridClient = sendGridClient;
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var fromEmail = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);
        var toEmail = new EmailAdress(email);

        var msg = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, message, message);

        await _sendGridClient.SendEmailAsync(msg);
    }
}
public class AccountController
{
    private readonly IEmailService _emailService;

    public AccountController(IEmailService emailService)
    {
        _emailService = emailService;
    }
}
