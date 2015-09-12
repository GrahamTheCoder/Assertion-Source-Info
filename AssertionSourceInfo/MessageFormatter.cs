using System;
using System.Collections.Generic;
using System.Linq;

namespace Gtc.AssertionSourceInfo
{
    internal static class MessageFormatter
    {
        public static string GetMessage(Exception exception, List<string> statementLines)
        {
            var formattedLines = GetFormattedStatementLines(statementLines);
            if (formattedLines.Count() > 1)
            {
                formattedLines.Insert(0, "");
            }
            var statement = string.Join("\r\n", formattedLines.ToArray());
            return $"In:{statement}\r\n{exception.Message}";
        }

        private static List<string> GetFormattedStatementLines(List<string> statementLines)
        {
            var normalizedWhitespaceStatementLines = statementLines.Select(l => l.Replace("\t", "    "));
            var minSpaces = normalizedWhitespaceStatementLines.Where(l => !IsWhitespace(l)).Min(l => l.TakeWhile(c => c == ' ').Count());
            return normalizedWhitespaceStatementLines.Select(l => IsWhitespace(l) ? l : $" {l.Substring(minSpaces)}").ToList();
        }

        private static bool IsWhitespace(string l)
        {
            return string.IsNullOrEmpty(l.Trim());
        }
    }
}
