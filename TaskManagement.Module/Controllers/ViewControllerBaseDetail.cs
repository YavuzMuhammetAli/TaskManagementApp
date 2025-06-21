using DevExpress.ExpressApp;

namespace TaskManagement.Module.Controllers
{
    public partial class ViewControllerBaseDetail : ViewController
    {
        public ViewControllerBaseDetail()
        {
            InitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectSaved += ObjectSpaceSaved;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ObjectSpace.ObjectSaved -= ObjectSpaceSaved;
        }

        private void ObjectSpaceSaved(object sender, EventArgs e)
        {
            View.Refresh();
        }
    }
}
