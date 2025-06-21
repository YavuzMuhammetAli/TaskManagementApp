using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;
using TaskManagement.Module.BaseClassess;

namespace TaskManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Appointment : DbBase
    {
        public Appointment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string subject;
        public string Subject
        {
            get => subject;
            set => SetPropertyValue(nameof(Subject), ref subject, value);
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set => SetPropertyValue(nameof(StartDate), ref startDate, value);
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set => SetPropertyValue(nameof(EndDate), ref endDate, value);
        }

        private string description;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        private Task relatedTask;
        [Association("Task-Appointments")]
        public Task RelatedTask
        {
            get => relatedTask;
            set => SetPropertyValue(nameof(RelatedTask), ref relatedTask, value);
        }

        private ApplicationUser user;
        [Browsable(false)]
        public ApplicationUser User
        {
            get => user;
            set => SetPropertyValue(nameof(User), ref user, value);
        }

        private Event schedulerEvent;
        public Event SchedulerEvent
        {
            get => schedulerEvent;
            set => SetPropertyValue(nameof(SchedulerEvent), ref schedulerEvent, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (!Session.IsNewObject(this) || Session.GetObjectsToSave().OfType<Event>().Any())
                return;

            var currentUserName = SecuritySystem.CurrentUserName;
            var resource = Session.FindObject<Resource>(CriteriaOperator.Parse("Caption = ?", currentUserName)) ?? CreateNewResource(currentUserName);

            UpdateAuditFields(resource);

            this.schedulerEvent = new Event(Session)
            {
                StartOn = this.StartDate,
                EndOn = this.EndDate,
                Status = 2,
                Subject = this.Subject,
                Label = 2,
                Description = this.Description,
                ResourceId = GenerateResourceIdXml(resource.Oid)
            };

            UpdateAuditFields(this.schedulerEvent);
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();

            if (this.SchedulerEvent != null)
            {
                var linkedEvent = Session.FindObject<Event>(CriteriaOperator.Parse("Oid = ?", this.SchedulerEvent.Oid));
                if (linkedEvent != null)
                    Session.Delete(linkedEvent);
            }
        }

        private Resource CreateNewResource(string userName)
        {
            return new Resource(Session)
            {
                Caption = userName,
                Color = System.Drawing.Color.Aqua
            };
        }

        private void UpdateAuditFields(BaseObject obj)
        {
            obj.SetMemberValue("CreatedOn", DateTime.Now);
            obj.SetMemberValue("CreatedBy", this.CreatedBy);
            obj.SetMemberValue("LastModifiedOn", DateTime.Now);
            obj.SetMemberValue("LastModifiedBy", this.LastModifiedBy);
        }

        private string GenerateResourceIdXml(Guid oid)
        {
            return $"<ResourceIds><ResourceId Type=\"System.Guid\" Value=\"{oid}\" /></ResourceIds>";
        }

    }
}