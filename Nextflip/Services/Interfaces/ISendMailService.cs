using Nextflip.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailHTMLAsync(string email, string subject, string htmlMessage);

        Task SendMailHTML(MailContent mailContent);
    }
}