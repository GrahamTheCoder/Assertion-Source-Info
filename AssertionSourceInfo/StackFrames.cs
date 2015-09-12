using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gtc.AssertionSourceInfo
{
    internal static class StackFrames
    {
        public static IEnumerable<string> GetAssertionStatementLines(Exception exception)
        {
            return AboveAssert(exception)
                .Select(x => new StatementReader(x.GetFileName(), x.GetFileLineNumber(), x.GetFileColumnNumber()).GetStatementLines())
                .FirstOrDefault() ?? new string[0];
        }

        private static IEnumerable<StackFrame> AboveAssert(Exception exception)
        {
            return new StackTrace(exception, true).GetFrames()
                    .SkipWhile(f => f.GetMethod().DeclaringType.FullName.EndsWith("Assert"));
        }

    }
}
