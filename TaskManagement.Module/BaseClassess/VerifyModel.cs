using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace TaskManagement.Module.BaseClassess
{
    [NonPersistent]
    [Appearance("HideOid", TargetItems = "Oid", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    public class VerifyModel : XPObject
    {
        public VerifyModel(Session session) : base(session)
        {
        }

        private string code;
        [NonPersistent]
        [RuleRequiredField(CustomMessageTemplate = "Doğrulama kodu gereklidir.")]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }
    }
}
