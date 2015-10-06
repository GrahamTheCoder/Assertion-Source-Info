using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gtc.AssertionSourceInfo
{
    /// <remarks>
    /// Note that column is currently ignored, so a line with multiple statements will be broken (and presumably many other statement styles)
    /// TODO: Consider using Roslyn to actually parse the statements
    /// </remarks>
    internal class StatementReader
    {
        private const int LinesToReadEitherSide = 10;
        private readonly string m_FilePath;
        private readonly int m_LineIndex;
        private int m_Col;

        public StatementReader(string filePath, int lineNumber, int col)
        {
            m_FilePath = filePath;
            m_LineIndex = lineNumber - 1;
            m_Col = col;
        }

        private bool HasStatement => !string.IsNullOrEmpty(m_FilePath) && File.Exists(m_FilePath);

        public IEnumerable<string> GetStatementLines()
        {
            if (!HasStatement) return new string[0];
            var firstLineToCheck = Math.Max(m_LineIndex - LinesToReadEitherSide, 0);

            var surroundingLines = File.ReadAllLines(m_FilePath).Skip(firstLineToCheck).Take(LinesToReadEitherSide * 2).ToList();
            var startLines = GetStartLines(surroundingLines);
            var endLines = GetEndLines(surroundingLines);
            var statementLines = startLines.Concat(endLines).ToArray();
            return statementLines;
        }

        private static IEnumerable<string> GetEndLines(List<string> surroundingLines)
        {
            var potentialEndLines = surroundingLines.Skip(LinesToReadEitherSide);
            var endLinesToTake = GetEndOfStatement(potentialEndLines);
            return endLinesToTake;
        }

        private List<string> GetStartLines(List<string> surroundingLines)
        {
            return surroundingLines.Take(LinesToReadEitherSide).Reverse().TakeWhile(ProbablyNotStartOfMultilineStatement).Reverse().ToList();
        }

        private static IEnumerable<string> GetEndOfStatement(IEnumerable<string> potentialEndLines)
        {
            foreach(var line in potentialEndLines)
            {
                yield return line;
                if (line.Trim().EndsWith(";"))
                {
                    yield break;
                }
            }
        }

        private bool ProbablyNotStartOfMultilineStatement(string line)
        {
            var trimmed = line.Trim();
            return !trimmed.EndsWith(";") && !trimmed.EndsWith("{");
        }
    }
}