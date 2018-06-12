using System.Web.Mvc;
using Nop.Web.Framework.Security;
using Nop.Web.Models.Common;
using Nop.Services.Chonves;
using Nop.Core.Domain.Chonves;
using Nop.Web.Framework.UI.Captcha;
using Nop.Services.Messages;
using System;
using Nop.Core.Domain.Messages;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        private readonly IChonVeService _chonveService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly EmailAccountSettings _emailAccountSettings;
        public HomeController(IEmailAccountService emailAccountService, IQueuedEmailService queuedEmailService, IChonVeService chonveService, EmailAccountSettings emailAccountSettings)
        {
            this._chonveService = chonveService;
            this._emailAccountService = emailAccountService;
            this._queuedEmailService = queuedEmailService;
            this._emailAccountSettings = emailAccountSettings;
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Index(string req)
        {
           
           return RedirectToAction("Index", "NhaXes");
           

        }
        public ActionResult timkiem()
        {
            //return RedirectToAction("Index","NhaXes");
            return View();

        }
        public ActionResult PhanMem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKyPhanMem(DangKyPhanMemModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new DangKyPhanMem();
                item.Ten = model.Ten;
                item.Email = model.Email;
                item.SoDienThoai = model.SoDienThoai;
                item.DiaChi = model.DiaChi;
                item.GhiChu = model.GhiChu;
                _chonveService.InsertDangKyPhanMem(item);
                //gui email
                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
                if (emailAccount == null)
                    //No email account found with the specified id
                    return Json("OK");

                try
                {

                    string subject = string.Format("Nhà xe - {0} - đăng ký sử dụng phần mềm", item.Ten);
                    string body = "<p><strong>Thông tin nhà xe đăng ký sử dụng phần mềm:</strong></p>";
                    body = body + "<table style='width:100%;border-collapse:collapse;border:1px solid #808080;text-align:left;' border='1' cellpadding='5px' cellspacing='5px'>"
                               + "<tr>"
                                   + "<td style='width:30%;'><strong>Tên nhà xe:</strong></td>"
                                   + "<td>" + item.Ten + "</td>"
                               + "</tr>"
                               + "<tr>"
                                   + "<td><strong>Email:</strong></td>"
                                   + "<td>" + item.Email + "</td>"
                               + "</tr>"
                               + "<tr>"
                                   + "<td><strong>Số điện thoại:</strong></td>"
                                   + "<td>" + item.SoDienThoai + "</td>"
                               + "</tr>"
                               + "<tr>"
                                   + "<td><strong>Địa chỉ: </strong></td>"
                                   + "<td>" + item.DiaChi + "</td>"
                               + "</tr>"
                               + "<tr>"
                                   + "<td><strong>Tin nhắn: </strong></td>"
                                   + "<td>" + item.GhiChu + "</td>"
                               + "</tr>"
                               + "</table>";

                    var email = new QueuedEmail
                    {
                        Priority = 5,
                        EmailAccountId = emailAccount.Id,
                        FromName = emailAccount.DisplayName,
                        From = emailAccount.Email,
                        ToName = "Lương Tuấn",
                        To = "anhtuan51@gmail.com",
                        Subject = subject,
                        Body = body,
                        CC = "lenguyentat@gmail.com",
                        CreatedOnUtc = DateTime.UtcNow,
                    };
                    _queuedEmailService.InsertQueuedEmail(email);

                    //var ccemail=new System.Collections.Generic.List<string>();
                    //ccemail.Add("lenguyentat@gmail.com");
                    //_emailSender.SendEmail(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName,"anhtuan51@gmail.com", null, null, null, null, ccemail);
                }
                catch
                {

                }
            }
            return Json("OK");
        }
    }
}
