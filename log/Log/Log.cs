using System;
using System.IO;
using System.Linq;
using Extension;

namespace Log
{
    /// <summary>
    /// Класс дял формирования лога
    /// </summary>
    public static class Log
    {
        private static readonly string Path = Environment.CurrentDirectory + @"\Log";
        private static readonly object Locker = new object();

        public static void SetLog(string tag, string message)
        {
            lock (Locker)
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                using (StreamWriter streamWriter = new StreamWriter(Path + $@"\{DateTime.Now:ddMM_HH}.log", true))
                {
                    string messageLog = $"---{DateTime.Now:g}---{Environment.NewLine}"
                                        + tag
                                        + Environment.NewLine
                                        + message
                                        + Environment.NewLine
                                        + new string('-', 30);
                    streamWriter.WriteLine(messageLog);
                }
            }
        }

        public static void ClearLog()
        {
            DirectoryInfo folder = new DirectoryInfo(Path);
            folder.GetFiles().ForEach(file => file.Delete());
        }
    }
}