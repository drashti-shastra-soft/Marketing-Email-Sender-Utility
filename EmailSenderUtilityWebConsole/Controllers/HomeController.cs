using EmailSenderUtilityWebConsole.Infrastructure;
using EmailSenderUtilityWebConsole.Infrastructure.IService;
using EmailSenderUtilityWebConsole.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EmailSenderUtilityWebConsole.Infrastructure.Enums;

namespace EmailSenderUtilityWebConsole.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileReaderService _fileReaderService;
        private readonly IValidationService _validationService;
        private readonly ILeadService _leadService;

        public HomeController(IFileReaderService fileReaderService, IValidationService validationService, ILeadService leadService)
        {
            _fileReaderService = fileReaderService;
            _validationService = validationService;
            _leadService = leadService;
        }

        public ActionResult Index()
        {
            ViewBag.EmailTemplates = _leadService.GetEmailTemplates().GetAwaiter().GetResult();
            return View(new Domain.Lead());
        }

        [HttpPost]
        public JsonResult Import(HttpPostedFileBase file)
        {
            var mImportFiles = new List<Domain.Lead>();
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Path.Combine(Server.MapPath("~/Uploads"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(Server.MapPath("~/Uploads"), $"{Infrastructure.StaticMethod.GenerateGUID(15)}{Path.GetExtension(file.FileName)}");
                file.SaveAs(filePath);
                mImportFiles = _fileReaderService.ReadLeads(filePath);
                mImportFiles = _validationService.Initiate(mImportFiles);
            }
            return Json(mImportFiles, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SendEmail(Domain.Lead lead, string templateType)
        {
            //Thread.Sleep(3000);
            try
            {
                EmailTemplate template = (EmailTemplate)Enum.Parse(typeof(EmailTemplate), templateType);
                _leadService.SendEmailOnLeads(lead, template);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
    }
}