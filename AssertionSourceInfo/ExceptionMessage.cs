using System;
using System.Reflection;

namespace Gtc.AssertionSourceInfo
{
    internal static class ExceptionMessage
    {
        /// <summary>
        /// Mutates the existing exception or throws a new one.
        /// </summary>
        /// <remarks>
        /// TODO: Investigate whether extensibility points for NUnit 3 will allow us to set this without hacks
        /// </remarks>
        public static void ForceChange(Exception exception, string message)
        {
            try
            {
                typeof(Exception)
                    .GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(exception, message);
            }
            catch
            {
                throw NewException(exception, message);
            }
        }

        private static Exception NewException(Exception exception, string message)
        {
            try
            {
                return (Exception)Activator.CreateInstance(exception.GetType(), message);
            }
            catch
            {
                return new AssertionFailureException(message, exception);
            }
        }
    }
}
