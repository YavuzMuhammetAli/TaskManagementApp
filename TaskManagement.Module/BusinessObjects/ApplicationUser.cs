﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System.ComponentModel;

namespace TaskManagement.Module.BusinessObjects;

[MapInheritance(MapInheritanceType.ParentTable)]
[DefaultProperty(nameof(UserName))]
[CurrentUserDisplayImage(nameof(Photo))]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo, ISecurityUserLockout
{
    private int accessFailedCount;
    private DateTime lockoutEnd;
    private string email;

    public ApplicationUser(Session session) : base(session) { }

    [Browsable(false)]
    public int AccessFailedCount
    {
        get { return accessFailedCount; }
        set { SetPropertyValue(nameof(AccessFailedCount), ref accessFailedCount, value); }
    }

    [Browsable(false)]
    public DateTime LockoutEnd
    {
        get { return lockoutEnd; }
        set { SetPropertyValue(nameof(LockoutEnd), ref lockoutEnd, value); }
    }

    [Browsable(false)]
    [DevExpress.Xpo.Aggregated, Association("User-LoginInfo")]
    public XPCollection<ApplicationUserLoginInfo> LoginInfo
    {
        get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
    }

    public string Email
    {
        get => email;
        set => SetPropertyValue(nameof(Email), ref email, value);
    }

    private string verificationCode;
    [Browsable(false)]
    public string VerificationCode
    {
        get => verificationCode;
        set => SetPropertyValue(nameof(VerificationCode), ref verificationCode, value);
    }

    private bool emailVerification;
    [Browsable(false)]
    public bool EmailVerification
    {
        get => emailVerification;
        set => SetPropertyValue(nameof(EmailVerification), ref emailVerification, value);
    }

    private MediaDataObject photo;
    [XafDisplayName("User Photo")]
    public MediaDataObject Photo
    {
        get => photo;
        set => SetPropertyValue(nameof(Photo), ref photo, value);
    }

    private Task task;
    [Association("Task-ApplicationUser")]
    public Task Task
    {
        get => task;
        set => SetPropertyValue(nameof(Task), ref task, value);
    }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey)
    {
        ApplicationUserLoginInfo result = new ApplicationUserLoginInfo(Session);
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }
}
