using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderUtilityWebConsole.Domain
{
    public class Lead
    {
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Category { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string EmailTemplate { get; set; }
        public List<string> Validation { get; set; } = new List<string>();
    }
}
