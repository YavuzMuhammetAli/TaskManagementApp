using DevExpress.ExpressApp;
using TaskManagement.Module.BusinessObjects;

namespace TaskManagement.Module.Helpers
{
    public static class SecurityHelper
    {
        public static bool IsAdmin()
        {
            var user = SecuritySystem.CurrentUser as ApplicationUser;
            return user?.Roles.Any(r => r.Name == "Administrators") == true;
        }

        public static bool IsInRole(string roleName)
        {
            var user = SecuritySystem.CurrentUser as ApplicationUser;
            return user?.Roles.Any(r => r.Name == roleName) == true;
        }
    }
}
