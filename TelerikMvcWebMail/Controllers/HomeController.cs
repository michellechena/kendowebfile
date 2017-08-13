using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TelerikMvcWebMail.Filter;
using TelerikMvcWebMail.Models;

namespace TelerikMvcWebMail.Controllers
{
    [SessionExpire]
    public class HomeController : Controller
    {
        private MailsService mailsService;

        public HomeController()
        {

            mailsService = new MailsService(new WebMailEntities());

        }

        public ActionResult Index()
        {
            var FirstFolderId = mailsService.GetFirstFolderId(Session["UserId"].ToString());
            ViewBag.FirstFolderId = FirstFolderId;
            return View();
        }

        public ActionResult NewMail(string id, string mailTo, string subject)
        {
            var idString = "";

            if (!String.IsNullOrEmpty(id))
            {
                idString = id + ": ";
            }
            ViewBag.MailTo = mailTo;
            ViewBag.Subject = idString + HttpUtility.UrlDecode(subject);

            return PartialView("NewMail");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return PartialView();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request, string AjaxRequest,string MailBoxId)
        {
            var data = mailsService.Read(Session["UserId"].ToString(), AjaxRequest, MailBoxId);
            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MailViewModel mail)
        {
            if (mail.Category == "Disable")
            {
                bool Result = mailsService.UpdateEmailSubject(mail, "Disable");
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (mail != null && ModelState.IsValid)
                {

                    mailsService.Update(mail);
                }
            }

            return Json(new[] { mail }.ToDataSourceResult(request, ModelState));
        }

        [ValidateInput(false)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, MailViewModel mail)
        {
            if (mail != null && ModelState.IsValid)
            {
                mailsService.Create(mail);
            }

            return Json(new[] { mail }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetSelectedMailBoxData(long MailBoxId, string Defoult)
        {
            long DefoultFolderId = 0;
            TelerikMvcWebMail.DataLayer.CommonFunctions Obj = new DataLayer.CommonFunctions();
            List<MailBoxFolderModel> Model = Obj.MailBoxFolderList(MailBoxId, Session["UserId"].ToString());
            List<Folders> _FolderList = Model.Select(x => new Folders
            {
                MailBox = MailBoxId.ToString(),
                text = x.MailBoxFolderName,
                value = x.MailBoxFolderId.ToString(),
                Owner= x.Owner
            }).ToList();
            if (!string.IsNullOrEmpty(Defoult))
            {
                DefoultFolderId = Model.Select(x => x.MailBoxFolderId).Take(1).FirstOrDefault();
                return Json(DefoultFolderId, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(_FolderList, JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        public ActionResult ReadMailDetails(string MailId)
        {
            MailViewModel Model = new MailViewModel();
            Model = mailsService.ReadMailDetails(MailId);
            return PartialView("EmailDetailes", Model);

        }

        [HttpPost]
        public ActionResult UpdateEmailSubject(MailViewModel Model)
        {
            bool Result = mailsService.UpdateEmailSubject(Model);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}
