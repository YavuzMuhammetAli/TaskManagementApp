using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using TaskManagement.Module.BaseClassess;

namespace TaskManagement.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Proje")]
    public class Project : DbBase
    {
        public Project(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string projectName;
        public string ProjectName
        {
            get => projectName;
            set => SetPropertyValue(nameof(ProjectName), ref projectName, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
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
    }
}