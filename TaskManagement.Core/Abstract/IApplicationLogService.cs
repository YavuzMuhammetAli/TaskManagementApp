namespace TaskManagement.Core.Abstract
{
    public interface IApplicationLogService
    {
        public void Log(string message);
        public void LogError(string message, Exception ex);
    }
}
