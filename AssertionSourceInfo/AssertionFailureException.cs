using System;

namespace Gtc.AssertionSourceInfo
{
    internal class AssertionFailureException : Exception
    {
        public AssertionFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
