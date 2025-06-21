using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using TaskManagement.Module.BusinessObjects;

namespace TaskManagement.Blazor.Server.CustomPage
{
    public class CustomAuthenticationStandard : AuthenticationStandard<CustomLogonParameters, ApplicationUser>
    {
        public override object Authenticate(IObjectSpace objectSpace)
        {
            var parameters = (CustomLogonParameters)LogonParameters;
            var user = objectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == parameters.UserName);
            if (user == null || !VerifyPassword(parameters.Password, user))
            {
                throw new AuthenticationException("Invalid username or password", user.UserName);
            }
            return user;
        }

        private bool VerifyPassword(string inputPassword, ApplicationUser user)
        {
            return user.ComparePassword(inputPassword);
        }
    }
    public class CustomLogonParameters : IAuthenticationStandardLogonParameters
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
