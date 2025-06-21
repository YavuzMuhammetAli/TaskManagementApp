using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using TaskManagement.Module.BaseClassess;

namespace TaskManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Görev")]
    public class Task : DbBase
    {
        public Task(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string taskName;
        public string TaskName
        {
            get => taskName;
            set => SetPropertyValue(nameof(TaskName), ref taskName, value);
        }

        private TaskStatus status;
        public TaskStatus Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        private Project project;
        public Project Project
        {
            get => project;
            set => SetPropertyValue(nameof(Project), ref project, value);
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

        [Association("Task-ApplicationUser")]
        public XPCollection<ApplicationUser> AssignedUsers
        {
            get => GetCollection<ApplicationUser>(nameof(AssignedUsers));
        }

        [Association("Task-Appointments")]
        public XPCollection<Appointment> Appointments
        {
            get => GetCollection<Appointment>(nameof(Appointments));
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Status != TaskStatus.Completed 
                && AssignedUsers != null 
                && AssignedUsers.FirstOrDefault(x => x.Oid.ToString() == SecuritySystem.CurrentUserId.ToString()) != null 
                && !Session.GetObjectsToSave().OfType<PendingTask>().Any())
            {
                var existing = Session.FindObject<PendingTask>(CriteriaOperator.Parse("RelatedTask = ?", this));

                if (existing == null)
                {
                    var pending = new PendingTask(Session)
                    {
                        Title = this.TaskName,
                        Description = "Görev atamasý gerçekleþtirilidi",
                        DueDate = this.StartDate,
                        Owner = Session.FindObject<ApplicationUser>(CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId)),
                        RelatedTask = this,
                        RelatedProject = this.Project
                    };
                }
            }
        }
    }
}