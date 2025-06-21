using TaskManagement.Core.Abstract;

namespace TaskManagement.Core.Concrete
{
    public class ApplicationLogService : IApplicationLogService
    {
        private readonly string logFilePath;

        public ApplicationLogService(string fileName = "Log.txt")
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            logFilePath = Path.Combine(basePath, fileName);
        }

        public void Log(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logEntry);
            }
            catch
            {
                // Hata durumunu sessizce yut
            }
        }

        public void LogError(string message, Exception ex)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - ERROR - {message} - {ex.Message}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logEntry);
            }
            catch
            {
                // Hata durumunu sessizce yut
            }
        }
    }
}
