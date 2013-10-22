using System;
using System.Text;

namespace Sixeyed.Caching.Extensions
{
    /// <summary>
    /// Extensions to <see cref="Exception"/>
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns the nested Message values from the full exception stack
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string FullMessage(this Exception exception)
        {
            return exception.FullMessage(Environment.NewLine);
        }

        /// <summary>
        /// Returns the nested Message values from the full exception stack
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FullMessage(this Exception exception, string separator)
        {
            var builder = new StringBuilder();
            while (exception != null)
            {
                builder.AppendFormat("{0}{1}", exception.Message, separator);
                exception = exception.InnerException;
            }
            return builder.ToString();
        }
    }
}
