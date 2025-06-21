using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler;
using DevExpress.Persistent.BaseImpl;
using TaskManagement.Module.Helpers;

namespace TaskManagement.Blazor.Server.Controllers
{
    public partial class ViewControllerEventBase : ViewController
    {
        public ViewControllerEventBase()
        {
            InitializeComponent();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (SecurityHelper.IsAdmin())
                return;

            var listEditor = ((ListView)View).Editor as SchedulerListEditorBase;
            if (listEditor != null)
            {
                listEditor.ResourceDataSourceCreating += SchedulerAdapter_ResourceDataSourceCreating;
            }
            View.Refresh();
        }

        private void SchedulerAdapter_ResourceDataSourceCreating(object sender, DevExpress.ExpressApp.Scheduler.ResourceDataSourceCreatingEventArgs e)
        {
            e.DataSource = ObjectSpace.GetObjects<Resource>(CriteriaOperator.Parse($"CreatedBy = '{SecuritySystem.CurrentUserId.ToString()}'"));         
            e.Handled = true;
        }
    }
}
