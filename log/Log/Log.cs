using System;
using System.IO;
using Extension;

namespace Log
{
    /// <summary>
    ///     Класс дял формирования лога
    /// </summary>
    public static class Log
    {
        private static readonly string Path = Environment.CurrentDirectory + @"\Log";
        private static readonly object Locker = new object();

        public static void SetLog(string tag, string message)
        {
            lock (Locker)
            {
                if (!Directory.Exists(path: Path))
                    Directory.CreateDirectory(path: Path);

                using (StreamWriter streamWriter = new StreamWriter
                    (path: Path + $@"\{DateTime.Now:ddMM_HH}.log", append: true))
                {
                    string messageLog = $"---{DateTime.Now:g}---{Environment.NewLine}"
                                        + tag
                                        + Environment.NewLine
                                        + message
                                        + Environment.NewLine
                                        + new string(c: '-', count: 30);
                    streamWriter.WriteLine(value: messageLog);
                }
            }
        }

        public static void ClearLog()
        {
            DirectoryInfo folder = new DirectoryInfo(path: Path);
            folder.GetFiles().ForEach(action: file => file.Delete());
        }
    }
}