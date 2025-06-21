using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Module.BusinessObjects;

namespace TaskManagement.Module.DatabaseUpdate;

public class Updater : ModuleUpdater
{
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion)
    {
    }
    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();
//#if !RELEASE
        var defaultRole = CreateDefaultRole();
        var adminRole = CreateAdminRole();

        ObjectSpace.CommitChanges();

        UserManager userManager = ObjectSpace.ServiceProvider.GetRequiredService<UserManager>();

        // If a user named 'User' doesn't exist in the database, create this user
        if (userManager.FindUserByName<ApplicationUser>(ObjectSpace, "User") == null)
        {
            // Set a password if the standard authentication type is used
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "User", EmptyPassword, (user) =>
            {
                // Add the Users role to the user
                user.Roles.Add(defaultRole);
                user.EmailVerification = true;
            });
        }

        // If a user named 'Admin' doesn't exist in the database, create this user
        if (userManager.FindUserByName<ApplicationUser>(ObjectSpace, "Admin") == null)
        {
            // Set a password if the standard authentication type is used
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "Admin", EmptyPassword, (user) =>
            {
                // Add the Administrators role to the user
                user.Roles.Add(adminRole);
                user.EmailVerification = true;
            });
        }
        if(ObjectSpace.FirstOrDefault<MailMessageTemplate>(x => x.TemplateName == "NewAccount") == null)
        {
            var mailMessageTemplate = ObjectSpace.CreateObject<MailMessageTemplate>();
            mailMessageTemplate.TemplateName = "NewAccount";
            mailMessageTemplate.Subject = "Best Of The Year Doğrulama Maili";
            mailMessageTemplate.Body = "<!DOCTYPE html>" +
                "\r\n<html lang=\"tr\">\r\n<head>" +
                "\r\n    <meta charset=\"UTF-8\">" +
                "\r\n    <title>Doğrulama Kodu</title>" +
                "\r\n</head>" +
                "\r\n<body style=\"margin:0;padding:0;background-color:#f2f2f2;font-family:'Segoe UI', sans-serif;\">" +
                "\r\n    <table role=\"presentation\" style=\"width:100%;border-collapse:collapse;background-color:#f2f2f2;\">" +
                "\r\n        <tr>\r\n            <td align=\"center\" style=\"padding:40px 0;\">" +
                "\r\n                <table role=\"presentation\" style=\"width:100%;max-width:600px;background:white;border-radius:10px;box-shadow:0 4px 10px rgba(0,0,0,0.1);padding:40px;\">" +
                "\r\n                    <tr>" +
                "\r\n                        <td style=\"text-align:center;\">" +
                "\r\n                            <h2 style=\"margin-bottom:10px;color:#333;\">Doğrulama Kodu</h2>" +
                "\r\n                            <p style=\"margin:0;font-size:16px;color:#555;\">" +
                "\r\n                                Aşağıdaki doğrulama kodunu kullanarak işleminizi tamamlayabilirsiniz:" +
                "\r\n                            </p>\r\n                            <div style=\"margin:30px 0;\">" +
                "\r\n                                <span style=\"display:inline-block;font-size:32px;letter-spacing:6px;color:#1a73e8;font-weight:bold;background:#f1f1f1;padding:10px 20px;border-radius:8px;\">" +
                "\r\n                                    $VerificationCode" +
                "\r\n                                </span>" +
                "\r\n                            </div>" +
                "\r\n                            <p style=\"font-size:14px;color:#777;\">" +
                "\r\n                                Bu kod 10 dakika boyunca geçerlidir. Kod sizin isteğiniz dışında geldiyse, bu e-postayı görmezden gelebilirsiniz." +
                "\r\n                            </p>" +
                "\r\n                        </td>" +
                "\r\n                    </tr>" +
                "\r\n                    <tr>" +
                "\r\n                        <td style=\"text-align:center;margin-top:30px;padding-top:30px;border-top:1px solid #eee;font-size:12px;color:#aaa;\">" +
                "\r\n                            &copy; 2025 TaskManagement • Tüm hakları saklıdır." +
                "\r\n                        </td>\r\n                    </tr>" +
                "\r\n                </table>\r\n            </td>" +
                "\r\n        </tr>\r\n    </table>" +
                "\r\n</body>" +
                "\r\n</html>";
        }

        ObjectSpace.CommitChanges(); //This line persists created object(s).
//#endif
    }

    public override void UpdateDatabaseBeforeUpdateSchema()
    {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
    private PermissionPolicyRole CreateAdminRole()
    {
        PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if (adminRole == null)
        {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
            adminRole.IsAdministrative = true;
        }
        return adminRole;
    }
    private PermissionPolicyRole CreateDefaultRole()
    {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if (defaultRole == null)
        {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermission<Appointment>(SecurityOperations.Read, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<Appointment>(SecurityOperations.Write, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<Appointment>(SecurityOperations.Delete, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Appointment>(SecurityOperations.Create, SecurityPermissionState.Allow);

            defaultRole.AddObjectPermission<TaskManagement.Module.BusinessObjects.Task>(SecurityOperations.Read, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<TaskManagement.Module.BusinessObjects.Task>(SecurityOperations.Write, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<TaskManagement.Module.BusinessObjects.Task>(SecurityOperations.Delete, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<TaskManagement.Module.BusinessObjects.Task>(SecurityOperations.Create, SecurityPermissionState.Allow);

            defaultRole.AddObjectPermission<Project>(SecurityOperations.Read, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<Project>(SecurityOperations.Write, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<Project>(SecurityOperations.Delete, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Project>(SecurityOperations.Create, SecurityPermissionState.Allow);

            defaultRole.AddTypePermissionsRecursively<EmailLog>(SecurityOperations.Read,  SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<EmailLog>(SecurityOperations.Write, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<EmailLog>(SecurityOperations.Delete, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<EmailLog>(SecurityOperations.Create, SecurityPermissionState.Deny);

            defaultRole.AddObjectPermission<Event>(SecurityOperations.Read, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<Event>(SecurityOperations.Write, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<Event>(SecurityOperations.Delete, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Event>(SecurityOperations.Create, SecurityPermissionState.Allow);

            defaultRole.AddObjectPermission<PendingTask>(SecurityOperations.Read, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<PendingTask>(SecurityOperations.Write, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<PendingTask>(SecurityOperations.Delete, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PendingTask>(SecurityOperations.Create, SecurityPermissionState.Allow);

            defaultRole.AddObjectPermission<Resource>(SecurityOperations.Read, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<Resource>(SecurityOperations.Write, "[CreatedBy.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Resource>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Resource>(SecurityOperations.Delete, SecurityPermissionState.Deny);


            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Appointment", SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Task", SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Project", SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Scheduler Event", SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/PendingTask", SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Email Log", SecurityPermissionState.Deny);


            defaultRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "Photo", "Oid = CurrentUserId()", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Read, "Photo", "Oid = CurrentUserId()", SecurityPermissionState.Allow);

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Users", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.ReadWriteAccess, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddObjectPermission<ModelDifference>(SecurityOperations.ReadWriteAccess, "UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, "Owner.UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermission<AuditDataItemPersistent>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddObjectPermissionFromLambda<AuditDataItemPersistent>(SecurityOperations.Read, a => a.UserId == CurrentUserIdOperator.CurrentUserId().ToString(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermission<AuditedObjectWeakReference>(SecurityOperations.Read, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
