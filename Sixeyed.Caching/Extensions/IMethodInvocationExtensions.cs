using Microsoft.Practices.Unity.InterceptionExtension;
using Sixeyed.Caching.Configuration;
using Sixeyed.Caching.Serialization;
using System.Reflection;
using System.Text;

namespace Sixeyed.Caching.Extensions
{
    /// <summary>
    /// Extensions to <see cref="IMethodInvocation"/>
    /// </summary>
    public static class IMethodInvocationExtensions
    {
        /// <summary>
        /// Returns the name and parameters of the invocation as a formatted string
        /// </summary>
        /// <remarks>
        /// Use to retrieve a readable string for logging, with parameter names included
        /// and values separated by commas
        /// </remarks>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public static string ToTraceString(this IMethodInvocation invocation)
        {
            return ToTraceString(invocation, Serializer.GetCurrent(CacheConfiguration.Current.DefaultSerializationFormat), null, true, ": ", ", ");
        }

        /// <summary>
        /// Returns the name and parameters of the invocation as a formatted string
        /// </summary>
        /// <remarks>
        /// Use to retrieve a code usable for key values, with parameter names omitted
        /// and values separated by underscores
        /// </remarks>
        /// <param name="invocation"></param>
        /// <param name="argumentSerializer"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string ToTraceString(this IMethodInvocation invocation, ISerializer argumentSerializer, string prefix = null)
        {
            return ToTraceString(invocation, argumentSerializer, prefix, false, "", "_");
        }

        /// <summary>
        /// Returns the name and parameters of the invocation as a formatted string
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="argumentSerializer"></param>
        /// <param name="prefix"></param>
        /// <param name="includeParameterNames">Whether the output includes parameter names</param>
        /// <param name="separator">Separator between parameters</param>
        /// <param name="valueSeparator">Separator between parameter names and values</param>
        /// <returns></returns>
        public static string ToTraceString(this IMethodInvocation invocation, ISerializer argumentSerializer, string prefix, 
                                            bool includeParameterNames, string separator, string valueSeparator)
        {
            if (prefix.IsNullOrEmpty())
            {
                prefix = invocation.GetMethodCallPrefix();
            }
            var callBuilder = new StringBuilder();
            callBuilder.Append(prefix);
            BuildArgumentsString(callBuilder, invocation, argumentSerializer, includeParameterNames, separator, valueSeparator);
            return callBuilder.ToString().TrimEnd(valueSeparator.ToCharArray());
        }

        /// <summary>
        /// Builds a method call prefix in the format:
        ///  ClassName_MethodName
        /// </summary>
        /// <remarks>
        /// Expected to be used in conjunction with <see cref="VirtualMethodInterceptor"/>, so
        /// the name of the base class is used, not the supplied proxy class
        /// </remarks>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMethodCallPrefix(this IMethodInvocation input)
        {
            return string.Format("{0}.{1} ", input.Target.GetType().BaseType.Name, input.MethodBase.Name);
        }

        private static void BuildArgumentsString(StringBuilder callBuilder, IMethodInvocation input, 
                                                 ISerializer argumentSerializer, bool includeParameterNames, 
                                                 string separator, string valueSeparator)
        {
            var argumentIndex = 0;
            if (input.Inputs == null)
                return;

            foreach (var argument in input.Inputs)
            {
                string argumentString;
                if (argument == null)
                {
                    argumentString = "[null]";
                }
                else
                {
                    argumentString = argumentSerializer.Serialize(argument).ToString();
                }
                var parameter = includeParameterNames ? input.MethodBase.GetParameters()[argumentIndex] : null;
                AppendToMethodCall(callBuilder, parameter, argumentString, separator, valueSeparator);
                argumentIndex++;
            }
        }

        private static void AppendToMethodCall(StringBuilder callBuilder, ParameterInfo parameter, string argumentString, 
                                               string seperator, string valueSeperator)
        {
            if (parameter != null)
            {
                callBuilder.AppendFormat("{0}{1}{2}{3}", parameter.Name, seperator, argumentString, valueSeperator);
            }
            else
            {
                callBuilder.AppendFormat("{0}{1}{2}", seperator, argumentString, valueSeperator);
            }
        }
    }
}
