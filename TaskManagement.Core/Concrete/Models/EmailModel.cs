namespace TaskManagement.Core.Concrete.Models
{
    public class EmailModel
    {
        public string VerificationCode { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
