using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Models.Configs;
using Services.CommonServices.Abstractions;

namespace Services.CommonServices.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderConfig _options;

        public EmailSender(IOptions<EmailSenderConfig> options)
        {
            _options = options.Value;
        }
        
        public (bool success, Exception exception) SendOne(string receiver, string content, string subject, bool isHtml)
        {
            var smtpClient = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
            {
                EnableSsl = _options.UseSSL,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_options.SmtpLogin, _options.SmtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            var mailAddressTo = new MailAddress(receiver);
            var mailAddressFrom = new MailAddress(_options.SenderEmail, _options.SenderVisibleName);
            var mailMessage = new MailMessage(mailAddressFrom, mailAddressTo)
            {
                Subject = subject,
                Body = content,
                IsBodyHtml = isHtml
            };

            try
            {
                smtpClient.Send(mailMessage);
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }
    }
}