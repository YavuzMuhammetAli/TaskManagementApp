using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using TaskManagement.Module.BaseClassess;
using TaskManagement.Module.BusinessObjects;
using TaskManagement.Module.Services;

namespace TaskManagement.Module.Controllers
{
    public partial class VerificationPopupController : ViewController
    {
        public VerificationPopupController()
        {
            InitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (View.IsRoot && View.Id != "VerifyModel_DetailView")
            {
                var currentUser = SecuritySystem.CurrentUser as ApplicationUser;
                if (currentUser != null && !currentUser.EmailVerification)
                {
                    ShowVerificationPopup();
                }
            }
        }

        private void ShowVerificationPopup()
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(VerifyModel));
            var model = objectSpace.CreateObject<VerifyModel>();
            model.Code = ""; // Boş başlat

            var detailView = Application.CreateDetailView(objectSpace, model);
            detailView.ViewEditMode = ViewEditMode.Edit;

            var dc = Application.CreateController<DialogController>();
            dc.Accepting += (s, e) =>
            {
                var verifyModel = detailView.CurrentObject as VerifyModel;

                var currentUser = SecuritySystem.CurrentUser as ApplicationUser;
                if (verifyModel == null || string.IsNullOrWhiteSpace(verifyModel.Code))
                {
                    throw new UserFriendlyException("Doğrulama kodu boş olamaz.");
                }

                if (verifyModel.Code != currentUser.VerificationCode)
                {
                    throw new UserFriendlyException("Doğrulama kodu yanlış.");
                }

                currentUser.EmailVerification = true;
                currentUser.VerificationCode = null;
                currentUser.Session.CommitTransaction();

                MessageService.ShowSuccess(Application, "Doğrulama işlemi başarılı");
            };
            dc.CancelAction.Active["HideCancel"] = true;

            dc.Cancelling += (s, e) =>
            {
                throw new UserFriendlyException("Doğrulama tamamlanmadan sistemi kullanamazsınız.");
            };

            detailView.ViewEditMode = ViewEditMode.Edit;

            var showViewParams = new ShowViewParameters(detailView)
            {
                TargetWindow = TargetWindow.NewModalWindow
            };

            showViewParams.Controllers.Add(dc);

            Application.ShowViewStrategy.ShowView(
                showViewParams,
                new ShowViewSource(Frame, null)
            );
        }
    }
}
