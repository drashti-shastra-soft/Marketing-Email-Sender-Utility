using System.ComponentModel.DataAnnotations;

namespace EmailSenderUtilityWebConsole.Infrastructure
{
    public static class Enums
    {
        public enum FileType
        {
            [Display(Name = "CompanyName")]
            Company_Name = 1,
            [Display(Name = "ContactPerson")]
            Contact_Person = 2,
            [Display(Name = "Category")]
            Category = 3,
            [Display(Name = "PhoneNumber")]
            Phone_Number = 4,
            [Display(Name = "EmailAddress")]
            Email_Address = 5,
            [Display(Name = "Website")]
            Website = 6
        }
        
        public enum EmailTemplate
        {
            ShastraSoftSendEmail = 1,
            DirghDrashtiSendEmail = 2
        }
    }
}
