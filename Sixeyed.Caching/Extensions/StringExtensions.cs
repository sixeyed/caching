
namespace Sixeyed.Caching.Extensions
{
    ///<summary>
    /// Extension to <see cref="string"/>
    ///</summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Returns the result of calling <seealso cref="string.Format(string,object[])"/> with the supplied arguments
        /// </summary>
        /// <param name="formatString"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string formatString, params object[] args)
        {
            return args == null || args.Length == 0 ? formatString : string.Format(formatString, args);
        }

        ///<summary>
        /// Tests the string to see if it is null or empty
        ///</summary>
        ///<returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        ///<summary>
        /// Tests the string to see if it is null, empty or whitespace
        ///</summary>
        ///<returns></returns>
        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}

