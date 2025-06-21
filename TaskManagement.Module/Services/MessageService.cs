using DevExpress.ExpressApp;

namespace TaskManagement.Module.Services
{
    public static class MessageService
    {
        public static void ShowSuccess(XafApplication application, string message, int duration = 3000)
        {
            Show(application, message, InformationType.Success, duration);
        }

        public static void ShowError(XafApplication application, string message, int duration = 3000)
        {
            Show(application, message, InformationType.Error, duration);
        }

        public static void ShowWarning(XafApplication application, string message, int duration = 3000)
        {
            Show(application, message, InformationType.Warning, duration);
        }

        private static void Show(XafApplication application, string message, InformationType type, int duration)
        {
            var options = new MessageOptions
            {
                Message = message,
                Type = type,
                Duration = duration,
                Web = { Position = InformationPosition.Top },
            };
            application.ShowViewStrategy.ShowMessage(options);
        }
    }
}
