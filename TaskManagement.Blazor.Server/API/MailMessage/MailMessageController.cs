using DevExpress.ExpressApp;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TaskManagement.Core.Concrete.Models;
using TaskManagement.Module.BusinessObjects;
using TaskManagement.Core.Abstract;

namespace TaskManagement.Blazor.Server.API.MailMessage
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailMessageController : ControllerBase
    {
        private readonly IDataLayer _dataLayer;
        private IApplicationLogService _logService;
        public MailMessageController(IDataLayer dataLayer, IApplicationLogService logService)
        {
            _dataLayer = dataLayer;
            _logService = logService;
        }     

        [HttpPost("sendMail")]
        public IActionResult SendMail([FromBody] EmailModel model)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUser = "Examplr@gmail.com";
            string smtpPassword = "";
            string fromEmail = smtpUser;
            using var uow = new UnitOfWork(_dataLayer);

            try
            {
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(model.Email);
                    mail.Subject = model.Subject;
                    mail.Body = model.Body.Replace("$VerificationCode", $"{model.VerificationCode}");
                    mail.IsBodyHtml = true;
                    try
                    {
                        using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                        {
                            smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                            smtpClient.EnableSsl = true;
                            smtpClient.Send(mail);
                        }
                    }
                    catch (Exception ex)
                    {
                        var emailLog = new EmailLog(uow);
                        emailLog.MailTo = string.Join(", ", mail.To.Select(x => x.Address));
                        emailLog.MailCc = string.Join(", ", mail.CC.Select(x => x.Address));
                        emailLog.MailBcc = string.Join(", ", mail.Bcc.Select(x => x.Address));
                        emailLog.MailBody = mail.Body;
                        emailLog.UserName = model.UserName;
                        uow.CommitChanges();
                        throw new UserFriendlyException(ex.Message);
                    }
                }

                return Ok("E-posta başarıyla gönderildi!");
            }
            catch (Exception ex)
            {
                _logService.LogError("E-posta gönderilemedi:", ex);

                return BadRequest($"E-posta gönderilemedi: {ex.Message}");
            }
        }

    }
}
