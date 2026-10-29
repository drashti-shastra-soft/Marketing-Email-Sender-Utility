using EmailSenderUtilityWebConsole.Infrastructure;
using EmailSenderUtilityWebConsole.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using static EmailSenderUtilityWebConsole.Infrastructure.Enums;

namespace EmailSenderUtilityWebConsole.Service
{
    public class LeadService : ILeadService
    {
        private readonly IFileReaderService _fileReaderService;
        private readonly IEmailSenderService _emailSenderService;

        public LeadService(IFileReaderService fileReaderService, IEmailSenderService emailSenderService)
        {
            _fileReaderService = fileReaderService;
            _emailSenderService = emailSenderService;
        }

        public void SendEmailOnLeads(Domain.Lead mLead, EmailTemplate templateType)
        {
            string templatePath = EnumsHelper.GetTemplatePath(templateType);

            string content = _fileReaderService.ReadFileContent(templatePath);

            string finalContent = PopulateEmailTemplate(content, mLead);

            Task.Run(async () =>
            {
                try
                {
                    await _emailSenderService.SendEmailAsync(finalContent, mLead.EmailAddress);
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText("ErrorLog.txt", $"{DateTime.Now}: Background send failed - {ex.Message}{Environment.NewLine}");
                }
            });
        }

        private string PopulateEmailTemplate(string template, Domain.Lead mLead)
        {
            var replacements = new Dictionary<string, string>
            {
                { "{{ContactPerson}}", mLead.ContactPerson},
                { "{{EmailAddress}}", mLead.EmailAddress },
                { "{{PhoneNumber}}", mLead.PhoneNumber },
            };

            foreach (var pair in replacements)
            {
                template = template.Replace(pair.Key, pair.Value);
            }

            return template;
        }

        public Task<object> GetEmailTemplates()
        {
            var mSelectListItems = Enum.GetValues(typeof(EmailTemplate))
                                       .Cast<EmailTemplate>()
                                       .Select(mTemplate => new SelectListItem
                                       {
                                           Text = mTemplate.ToString(),
                                           Value = ((int)mTemplate).ToString()
                                       })
                                       .ToList();

            return Task.FromResult((object)mSelectListItems);
        }
    }
}
