using System;

namespace RefinedShell
{
    public interface IShellInterface
    {
        public bool FuzzyFind { get; set; }
        public uint MaxEntriesCount { get; set; }
        public void OnEntriesFound(ReadOnlyMemory<char>[] possibleCommands);
        public void OnArgumentParsing(bool[] parsingOutput);
    }
}