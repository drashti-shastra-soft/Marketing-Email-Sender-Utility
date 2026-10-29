using EmailSenderUtilityWebConsole.Infrastructure.IService;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSenderUtilityWebConsole.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly string _host = ConfigurationManager.AppSettings["SmtpHost"];
        private readonly int _port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        private readonly bool _enableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
        private readonly string _username = ConfigurationManager.AppSettings["SmtpUsername"];
        private readonly string _password = ConfigurationManager.AppSettings["SmtpPassword"];
        private readonly string _fromEmail = ConfigurationManager.AppSettings["SmtpFromEmail"];
        private readonly string _fromName = ConfigurationManager.AppSettings["SmtpFromName"];

        public async Task SendEmailAsync(string htmlBody, string toEmail, int maxRetries = 3)
        {
            int attempt = 0;
            Exception lastException = null;

            while (attempt < maxRetries)
            {
                try
                {
                    using (var smtpClient = new SmtpClient(_host, _port))
                    {
                        smtpClient.Credentials = new NetworkCredential(_username, _password);
                        smtpClient.EnableSsl = _enableSsl;

                        using (var mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(_fromEmail, _fromName);
                            mailMessage.To.Add(toEmail);
                            mailMessage.Subject = "Automated Email";
                            mailMessage.Body = htmlBody;
                            mailMessage.IsBodyHtml = true;

                            await smtpClient.SendMailAsync(mailMessage);
                        }
                    }

                    File.AppendAllText("Log.txt", $"{DateTime.Now}: Sent to {toEmail}{Environment.NewLine}");
                    return; // success
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    attempt++;
                    File.AppendAllText("ErrorLog.txt", $"{DateTime.Now}: Attempt {attempt} failed - {ex.Message}{Environment.NewLine}");
                    await Task.Delay(1000 * attempt); // exponential backoff
                }
            }

            throw new InvalidOperationException($"Failed to send email after {maxRetries} attempts.", lastException);
        }

    }
}
