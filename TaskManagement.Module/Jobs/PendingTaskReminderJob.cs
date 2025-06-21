using DevExpress.ExpressApp;
using System.Text;
using TaskManagement.Module.BusinessObjects;
using TaskManagement.Module.Services;

namespace TaskManagement.Module.Jobs
{
    public class PendingTaskReminderJob
    {
        private readonly IObjectSpaceFactory _objectSpaceFactory;
        private readonly EmailService _emailService;

        public PendingTaskReminderJob(IObjectSpaceFactory objectSpaceFactory, EmailService emailService)
        {
            _objectSpaceFactory = objectSpaceFactory;
            _emailService = emailService;
        }

        public void SendUncompletedPendingTasksReport()
        {
            using var objectSpace = _objectSpaceFactory.CreateObjectSpace<PendingTask>();
            var pendingTasks = objectSpace.GetObjects<PendingTask>()
                .Where(x => !x.IsCompleted)
                .ToList();

            if (pendingTasks.Any())
            {
                var body = CreateEmailBody(pendingTasks);
                _emailService.SendMail("hedef@example.com", "Günlük Görev Raporu", body);
            }
        }

        private string CreateEmailBody(List<PendingTask> pendingTasks)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Tamamlanmamış Görevler:");
            foreach (var task in pendingTasks)
            {
                sb.AppendLine($"- {task.Title} (Son Tarih: {task.DueDate:d})");
            }
            return sb.ToString();
        }
    }
}
