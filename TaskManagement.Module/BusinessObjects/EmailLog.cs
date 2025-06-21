using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using TaskManagement.Module.BaseClassess;

namespace TaskManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class EmailLog : DbBase
    { 
        public EmailLog(Session session) : base(session) { }

        private string mailSubject;
        private string mailBody;
        private string mailTo;
        private string mailBcc;
        private string mailCc;
        private string userName;

        public string MailTo
        {
            get => mailTo;
            set => SetPropertyValue(nameof(MailTo), ref  mailTo, value);
        }

        public string MailCc
        {
            get => mailCc;
            set => SetPropertyValue(nameof(MailCc), ref mailCc, value);
        }

        public string MailBcc
        {
            get => mailBcc;
            set => SetPropertyValue(nameof(MailBcc), ref mailBcc, value);
        }

        public string MailBody
        {
            get => mailBody;
            set => SetPropertyValue(nameof(MailBody), ref  mailBody, value);
        }

        public string MailSubject
        {
            get => mailSubject;
            set => SetPropertyValue(nameof(MailSubject), ref mailSubject, value);
        }

        public string UserName
        {
            get => userName;
            set => SetPropertyValue(nameof(UserName), ref userName, value);
        }
    }
}