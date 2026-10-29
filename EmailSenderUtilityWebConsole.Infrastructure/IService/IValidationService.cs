using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderUtilityWebConsole.Infrastructure.IService
{
    public interface IValidationService
    {
        List<Domain.Lead> Initiate(List<Domain.Lead> mLeads);
    }
}
