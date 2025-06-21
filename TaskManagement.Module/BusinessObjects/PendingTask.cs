using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using TaskManagement.Module.BaseClassess;

namespace TaskManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Notifications")]
    [NavigationItem("Tasks")]
    public class PendingTask : DbBase
    {
        public PendingTask(Session session) : base(session) { }

        private string title;
        [XafDisplayName("Title")]
        public string Title
        {
            get => title;
            set => SetPropertyValue(nameof(Title), ref title, value);
        }

        private string description;
        [XafDisplayName("Description")]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        private DateTime dueDate;
        [XafDisplayName("Due Date")]
        public DateTime DueDate
        {
            get => dueDate;
            set => SetPropertyValue(nameof(DueDate), ref dueDate, value);
        }

        private DateTime? shownOn;
        [XafDisplayName("Shown To User On")]
        public DateTime? ShownOn
        {
            get => shownOn;
            set => SetPropertyValue(nameof(ShownOn), ref shownOn, value);
        }

        private DateTime? completedOn;
        [XafDisplayName("Completed On")]
        public DateTime? CompletedOn
        {
            get => completedOn;
            set => SetPropertyValue(nameof(CompletedOn), ref completedOn, value);
        }

        private bool isCompleted;
        [XafDisplayName("Is Completed")]
        public bool IsCompleted
        {
            get => isCompleted;
            set => SetPropertyValue(nameof(IsCompleted), ref isCompleted, value);
        }

        private int documentOid;
        [XafDisplayName("Document Oid")]
        public int DocumentOid
        {
            get => documentOid;
            set => SetPropertyValue(nameof(DocumentOid), ref documentOid, value);
        }

        private string objectTypeName;
        [XafDisplayName("Object Type Name")]
        public string ObjectTypeName
        {
            get => objectTypeName;
            set => SetPropertyValue(nameof(ObjectTypeName), ref objectTypeName, value);
        }

        private ApplicationUser owner;
        [XafDisplayName("Owner User")]
        public ApplicationUser Owner
        {
            get => owner;
            set => SetPropertyValue(nameof(Owner), ref owner, value);
        }

        private Task relatedTask;
        [XafDisplayName("Related Task")]
        public Task RelatedTask
        {
            get => relatedTask;
            set => SetPropertyValue(nameof(RelatedTask), ref relatedTask, value);
        }

        private Appointment relatedAppointment;
        [XafDisplayName("Related Appointment")]
        public Appointment RelatedAppointment
        {
            get => relatedAppointment;
            set => SetPropertyValue(nameof(RelatedAppointment), ref relatedAppointment, value);
        }

        private Project relatedProject;
        [XafDisplayName("Related Project")]
        public Project RelatedProject
        {
            get => relatedProject;
            set => SetPropertyValue(nameof(RelatedProject), ref relatedProject, value);
        }
    }
}