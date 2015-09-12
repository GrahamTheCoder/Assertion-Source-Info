using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gtc.AssertionSourceInfo
{
    /// <remarks>
    /// TODO: Consider using Roslyn to actually parse the statement if:
    ///  It can be references from .NET 3.5
    ///  It can be nugetted in way not requring VS 2015
    /// </remarks>
    internal class StatementReader
    {
        private const int linesToReadEitherSide = 10;
        private string filename;
        private int lineIndex;
        private int col;

        public StatementReader(string filename, int lineNumber, int col)
        {
            this.filename = filename;
            this.lineIndex = lineNumber - 1;
            this.col = col;
        }

        public bool HasStatement => !string.IsNullOrEmpty(filename) && File.Exists(filename);

        public IEnumerable<string> GetStatementLines()
        {
            if (!HasStatement) return new string[0];
            var firstLineToCheck = Math.Max(lineIndex - linesToReadEitherSide, 0);

            var surroundingLines = File.ReadAllLines(filename).Skip(firstLineToCheck).Take(linesToReadEitherSide * 2).ToList();
            var startLines = GetStartLines(surroundingLines);
            var endLines = GetEndLines(surroundingLines);
            var statementLines = startLines.Concat(endLines).ToArray();
            return statementLines;
        }

        private static IEnumerable<string> GetEndLines(System.Collections.Generic.List<string> surroundingLines)
        {
            var potentialEndLines = surroundingLines.Skip(linesToReadEitherSide);
            var endLinesToTake = GetEndOfStatement(potentialEndLines);
            return endLinesToTake;
        }

        private List<string> GetStartLines(System.Collections.Generic.List<string> surroundingLines)
        {
            return surroundingLines.Take(linesToReadEitherSide).Reverse().TakeWhile(ProbablyNotStartOfMultilineStatement).Reverse().ToList();
        }

        private static IEnumerable<string> GetEndOfStatement(System.Collections.Generic.IEnumerable<string> potentialEndLines)
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