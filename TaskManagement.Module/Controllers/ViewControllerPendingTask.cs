using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using TaskManagement.Module.Helpers;

namespace TaskManagement.Module.Controllers
{
    public partial class ViewControllerPendingTask : ViewController
    {
        public ViewControllerPendingTask()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if (SecurityHelper.IsAdmin())
                return;

            if (View is ListView listView)
            {
                // Sadece IsCompleted == false olanlar gösterilecek
                if (listView.CollectionSource.Criteria.ContainsKey("PendingOnly"))
                {
                    listView.CollectionSource.Criteria["PendingOnly"] = CriteriaOperator.Parse("IsCompleted = false");
                }
                else
                {
                    listView.CollectionSource.Criteria.Add("PendingOnly", CriteriaOperator.Parse("IsCompleted = false"));
                }
            }
            
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
