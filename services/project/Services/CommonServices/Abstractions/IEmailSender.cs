using System;

namespace Services.CommonServices.Abstractions
{
    public interface IEmailSender
    {
        (bool success, Exception exception) SendOne(string receiver, string content, string subject, bool isHtml);
    }
}