using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace TaskManagement.Blazor.Server.Controllers
{
    public partial class ViewControllerResourceBase : ViewController
    {
        public ViewControllerResourceBase()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if(View is ListView listView)
            {
                if (listView.CollectionSource.Criteria.ContainsKey("ResourceOnlyCreatedBy"))
                {
                    listView.CollectionSource.Criteria["ResourceOnlyCreatedBy"] = CriteriaOperator.Parse("CreatedBy = CurrentUserId()");
                }
                else
                {
                    listView.CollectionSource.Criteria.Add("ResourceOnlyCreatedBy", CriteriaOperator.Parse("CreatedBy = CurrentUserId()"));
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
