using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderUtilityWebConsole.Infrastructure.IService
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string htmlBody, string toEmail, int maxRetries = 3);
    }
}
