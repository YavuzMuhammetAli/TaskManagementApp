using System.Net;
using System.Net.Mail;

namespace TaskManagement.Module.Services
{
    public class EmailService
    {
        public void SendMail(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("bilgilendirme@bestoftheyear.com.tr", "ucof opun lahx abqy"),
                EnableSsl = true,
            };

            var mail = new MailMessage("aliyavuz03795@gmail.com", to, subject, body);
            smtp.Send(mail);
        }
    }
}
