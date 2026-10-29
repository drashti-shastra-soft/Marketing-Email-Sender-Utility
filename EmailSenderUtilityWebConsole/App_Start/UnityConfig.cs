using EmailSenderUtilityWebConsole.Infrastructure;
using EmailSenderUtilityWebConsole.Infrastructure.IService;
using EmailSenderUtilityWebConsole.Service;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace EmailSenderUtilityWebConsole
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ICSVReaderService, CSVReaderService>();
            container.RegisterType<IEmailSenderService, EmailSenderService>();
            container.RegisterType<IFileReaderService, FileReaderService>();
            container.RegisterType<ILeadService, LeadService>();
            container.RegisterType<IValidationService, ValidationService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}