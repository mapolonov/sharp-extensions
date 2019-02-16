using System;
using System.Text;

namespace Csharp.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToLogString(this Exception exception)
        {
            var stringBuilder = new StringBuilder(exception.ToString());

            if (exception.Data.Keys.Count <= 0) return stringBuilder.ToString();

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("DataCollection:");
            foreach (var key in exception.Data.Keys)
            {
                stringBuilder.AppendLine($"    {key}: {exception.Data[key]}");
            }

            return stringBuilder.ToString();
        }
    }
}
