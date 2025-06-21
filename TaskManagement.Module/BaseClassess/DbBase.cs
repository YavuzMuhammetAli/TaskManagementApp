using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using TaskManagement.Module.BusinessObjects;

namespace TaskManagement.Module.BaseClassess
{
    [NonPersistent]
    public class DbBase : XPBaseObject
    {
        public DbBase(Session session) : base(session) { }

        private int oid = -1;
        [NonCloneable]
        [Persistent("OID")]
        [Key(AutoGenerate = true)]
        [XafDisplayName("Object ID")]
        [SearchMemberOptions(SearchMemberMode.Include)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int Oid
        {
            get  => oid;            
            set  => SetPropertyValue("Oid", ref oid, value);
            
        }

        private ApplicationUser createdBy;
        [Browsable(false)]
        public ApplicationUser CreatedBy
        {
            get => createdBy;
            set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value);
        }
        private ApplicationUser lastModifiedBy;
        [Browsable(false)]
        public ApplicationUser LastModifiedBy
        {
            get => lastModifiedBy;
            set => SetPropertyValue(nameof(LastModifiedBy), ref lastModifiedBy, value);
        }
        private DateTime createdOn;
        [Browsable(false)]
        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }
        private DateTime lastModifiedOn;
        [Browsable(false)]
        public DateTime LastModifiedOn
        {
            get => lastModifiedOn;
            set => SetPropertyValue(nameof(LastModifiedOn), ref  lastModifiedOn, value);
        }



        protected override void OnSaving()
        {
            if(SecuritySystem.CurrentUserId != null)
            {
                ApplicationUser currentUser = this.Session.FindObject<ApplicationUser>(CriteriaOperator.Parse($"Oid = '{SecuritySystem.CurrentUserId}'"));
                if (this.CreatedBy == null)
                {
                    this.CreatedBy = currentUser;
                    this.CreatedOn = DateTime.Now;
                }
                this.lastModifiedBy = currentUser;
                this.LastModifiedOn = DateTime.Now;
            }
            base.OnSaving();
        }
    }
}
