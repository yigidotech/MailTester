using System;
using System.IO;

namespace Logger
{
    public static class LogManager
    {
        private static string _fileName = "Log";
        private static string _startupPath = string.Empty;
        private static string _filePath = string.Empty;
        private static StreamWriter sw = null;

        public static int _deleteBeforDays = 10;

        public static void SetLogSettings( string startupPath)
        {
            _startupPath = startupPath; 
            string date = DateTime.Now.ToString("yyyy_MM_dd");
            _fileName = $"{_fileName}_{date}.txt";
            _startupPath = $"{_startupPath}/Logs";
            _filePath = $"{_startupPath}/{_fileName}";
            CreateFileOrDirectory();
        }

        private static void DeleteLogFiles()
        {
            string beforeDeleteDateTime = DateTime.Now.AddDays(-10).ToString("yyyy_MM_dd");
            string[] files = Directory.GetFiles(_startupPath);
            if (true)
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(_startupPath);
            }
            if (!File.Exists(_filePath))
            {
                FileStream fs = File.Create(_filePath);
                fs.Close();
            }
        }

        private static void CreateFileOrDirectory()
        {
            if (!Directory.Exists(_startupPath))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(_startupPath);
            }
            if (!File.Exists(_filePath))
            {
                FileStream fs = File.Create(_filePath);
                fs.Close();
            }
        }

        private static string GetLogTime()
        {
            return DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");
        }

        public static void Error(Exception ex = null, string message = "")
        {
            if (!File.Exists(_filePath))
            {
                CreateFileOrDirectory();
            }
            using (StreamWriter sw = File.AppendText(_filePath))
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        message += $" InnerException: {ex.InnerException}";
                    }
                    if (ex.StackTrace != null)
                    {
                        message += $" StackTrace: {ex.StackTrace}";
                    }
                }
                sw.WriteLine($"[ERROR] [{GetLogTime()}] Message: {message} ");
            }
        }

        public static void Info(string message)
        {
            if (!File.Exists(_filePath))
            {
                CreateFileOrDirectory();
            }
            using (StreamWriter sw = File.AppendText(_filePath))
            {
                sw.WriteLine($"[Info] [{GetLogTime()}] Message: {message}");
            }
        }

        public static void Warning(string message)
        {
            if (!File.Exists(_filePath))
            {
                CreateFileOrDirectory();
            }
            using (StreamWriter sw = File.AppendText(_filePath))
            {
                sw.WriteLine($"[Warning] [{GetLogTime()}] Message: {message}");
            }
        }

    }
}
