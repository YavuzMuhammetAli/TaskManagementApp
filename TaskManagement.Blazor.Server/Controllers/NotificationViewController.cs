using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Notifications;
using DevExpress.Persistent.Base;
using TaskManagement.Module.BusinessObjects;

namespace TaskManagement.Blazor.Server.Controllers
{
    public partial class NotificationViewController : ViewController
    {
        private SimpleAction showNotificationAction;
        public NotificationViewController()
        {
            showNotificationAction = new SimpleAction(this, "ShowNotification", PredefinedCategory.Notifications)
            {
                Caption = "0",
                ImageName = "BO_Notifications",
                ToolTip = "Kullanıcıya bildirim göster"
            };

            showNotificationAction.Execute += ShowNotificationAction_Execute;
            InitializeComponent();
        }

        private void ShowNotificationAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var listView = Application.CreateListView(typeof(PendingTask), true);

            e.ShowViewParameters.CreatedView = listView;
            e.ShowViewParameters.TargetWindow = TargetWindow.Current;
            e.ShowViewParameters.Context = TemplateContext.View;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            UpdateNotificationCount();
            var notificationController = Frame.GetController<NotificationsController>();
            if (notificationController != null)
            {
                notificationController.ShowNotificationsAction.Active["Active"] = false;
            }
        }

        private void UpdateNotificationCount()
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(PendingTask));

            var currentUserName = SecuritySystem.CurrentUserName;
            var count = objectSpace.GetObjects<PendingTask>().Count(x => x.Owner != null && x.Owner.UserName == currentUserName && !x.IsCompleted);

            showNotificationAction.Caption = count.ToString();
        }
    }
}
