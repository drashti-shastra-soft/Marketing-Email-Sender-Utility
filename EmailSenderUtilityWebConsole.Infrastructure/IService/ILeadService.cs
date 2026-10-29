using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmailSenderUtilityWebConsole.Infrastructure.Enums;

namespace EmailSenderUtilityWebConsole.Infrastructure.IService
{
    public interface ILeadService
    {
        void SendEmailOnLeads(Domain.Lead mLead, EmailTemplate templateType);
        
        Task<object> GetEmailTemplates();
    }
}
