using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyPersonal.Shared;

namespace MyPersonal.Server.Services;

/// <summary>
/// SMTP settings bound from the "Email" configuration section.
/// The password should come from a secret (environment variable / user-secrets),
/// never be committed to source control.
/// </summary>
public class EmailOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 587;
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string FromName { get; set; } = "Portfolio Website";

    /// <summary>True only when the minimum required fields are present.</summary>
    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Host) &&
        !string.IsNullOrWhiteSpace(User) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(To);
}

public class EmailSender
{
    private readonly EmailOptions _options;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(EmailOptions options, ILogger<EmailSender> logger)
    {
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Sends a contact-form submission to the site owner. Returns false (without
    /// throwing) when email isn't configured or delivery fails, so the API can
    /// still succeed on the stored copy.
    /// </summary>
    public async Task<bool> SendContactAsync(ContactMessage msg, CancellationToken ct = default)
    {
        if (!_options.IsConfigured)
        {
            _logger.LogWarning("Email not configured — skipping send. Message from {Email} was stored only.", msg.Email);
            return false;
        }

        try
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_options.FromName, string.IsNullOrWhiteSpace(_options.From) ? _options.User : _options.From));
            mail.To.Add(MailboxAddress.Parse(_options.To));
            // Let the owner reply straight to the visitor.
            mail.ReplyTo.Add(new MailboxAddress(msg.Name, msg.Email));
            mail.Subject = $"Portfolio enquiry from {msg.Name}";
            mail.Body = new BodyBuilder
            {
                TextBody =
                    $"You have a new message from your portfolio contact form.\n\n" +
                    $"Name:  {msg.Name}\n" +
                    $"Email: {msg.Email}\n" +
                    $"Sent:  {msg.CreatedUtc:yyyy-MM-dd HH:mm} UTC\n\n" +
                    $"Message:\n{msg.Message}\n"
            }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls, ct);
            await client.AuthenticateAsync(_options.User, _options.Password, ct);
            await client.SendAsync(mail, ct);
            await client.DisconnectAsync(true, ct);

            _logger.LogInformation("Contact email sent for message from {Email}.", msg.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact email for {Email}.", msg.Email);
            return false;
        }
    }
}
