using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using TaskManagement.Module.BaseClassess;

namespace TaskManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class MailMessageTemplate : DbBase
    {
        public MailMessageTemplate(Session session) : base(session) { }

        private string templateName;
        public string TemplateName
        {
            get => templateName;
            set => SetPropertyValue(nameof(TemplateName), ref templateName, value);
        }

        private string subject;
        public string Subject
        {
            get => subject;
            set => SetPropertyValue(nameof(Subject), ref  subject, value);
        }

        private string body;
        [Size(SizeAttribute.Unlimited)]
        public string Body
        {
            get => body;
            set => SetPropertyValue(nameof(Body), ref  body, value);
        }
    }
}