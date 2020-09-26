using System;

namespace LoadData
{
    /// <summary>
    ///     Класс, описывающий ошибки , возникающие в процессе работы класса Loader
    /// </summary>
    public class ExceptionLoader : Exception
    {
        public ExceptionLoader(string tag, string message)
        {
            Message = message;
            Log.Log.SetLog(tag: tag, message: message);
        }

        public override string Message { get; }
    }
}