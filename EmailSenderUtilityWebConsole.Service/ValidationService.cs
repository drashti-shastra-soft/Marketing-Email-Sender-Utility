using EmailSenderUtilityWebConsole.Infrastructure;
using EmailSenderUtilityWebConsole.Infrastructure.IService;
using System.Collections.Generic;

namespace EmailSenderUtilityWebConsole.Service
{
    public class ValidationService : IValidationService
    {
        public List<Domain.Lead> Initiate(List<Domain.Lead> mLeads)
        {
            if (mLeads != null && mLeads.Count > 0)
            {

                foreach (var mLead in mLeads)
                {
                    if (mLead.CompanyName.IsNullOrEmpty())
                        mLead.Validation.Add("Company is required!");

                    if (mLead.ContactPerson.IsNullOrEmpty())
                        mLead.Validation.Add("Contact Person is required!");

                    if (mLead.PhoneNumber.IsNullOrEmpty())
                        mLead.Validation.Add("Phone Number is required!");
                }
            }
            return mLeads;
        }
    }
}
