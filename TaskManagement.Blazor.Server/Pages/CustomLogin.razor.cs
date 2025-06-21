using Microsoft.AspNetCore.Components;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Services;

namespace ManageMind.Blazor.Server.Pages
{
    public partial class CustomLogin : ComponentBase
    {
        [Inject]
        IXafApplicationProvider ApplicationProvider { get; set; }
        protected string UserName { get; set; }
        protected string Password { get; set; }

        public bool ProcessLogin()
        {
            bool result = true;
            var app = ApplicationProvider.GetApplication();
            var logonParams = ((AuthenticationStandardLogonParameters)app.Security.LogonParameters);
            logonParams.UserName = UserName;
            logonParams.Password = Password;
            try
            {
                app.Logon();
            }
            catch (Exception ex)
            {
                result = false;
                app.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error);
            }

            return result;
        }

        protected void Send()
        {

        }

    }
}
