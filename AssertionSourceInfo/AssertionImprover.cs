using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gtc.AssertionSourceInfo
{
    public static class AssertionImprover
    {
        public static void AddAssertionSourceIfAvailable(Exception exception, IEnumerable<StackFrame> overrideStackTrace = null)
        {
            if (exception != null && exception.GetType().FullName.Contains("Assertion"))
            {
                var stackTrace = overrideStackTrace ?? new StackTrace(exception, true).GetFrames();
                var assertionStatementLines = StackFrames.GetAssertionStatementLines(stackTrace).ToList();
                if (assertionStatementLines.Any())
                {
                    var message = MessageFormatter.GetMessage(exception, assertionStatementLines);
                    ExceptionMessage.ForceChange(exception, message);
                }
            }
        }
    }
}
