using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gtc.AssertionSourceInfo
{
    internal static class StackFrames
    {
        public static IEnumerable<string> GetAssertionStatementLines(IEnumerable<StackFrame> stackTrace)
        {
            return AboveAssert(stackTrace)
                .Select(x => new StatementReader(x.GetFileName(), x.GetFileLineNumber(), x.GetFileColumnNumber()).GetStatementLines())
                .FirstOrDefault() ?? new string[0];
        }

        private static IEnumerable<StackFrame> AboveAssert(IEnumerable<StackFrame> stackTrace)
        {
            return stackTrace.SkipWhile(f => f.GetMethod().DeclaringType.FullName.EndsWith("Assert"));
        }

    }
}
