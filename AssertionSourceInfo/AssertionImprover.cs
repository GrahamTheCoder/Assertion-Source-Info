using System;
using System.Linq;

namespace Gtc.AssertionSourceInfo
{
    public class AssertionImprover
    {
        public static void AddAssertionSourceIfAvailable(Exception exception)
        {
            if (exception != null && exception.GetType().FullName.Contains("Assertion"))
            {
                var assertionStatementLines = StackFrames.GetAssertionStatementLines(exception).ToList();
                if (assertionStatementLines.Any())
                {
                    var message = MessageFormatter.GetMessage(exception, assertionStatementLines);
                    ExceptionMessage.ForceChange(exception, message);
                }
            }
        }
    }
}
