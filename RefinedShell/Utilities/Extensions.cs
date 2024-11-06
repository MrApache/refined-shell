namespace RefinedShell.Utilities
{
    public static class Extensions
    {
        public static string Substring(this string source, ProblemSegment segment)
        {
            return source.Substring(segment.Start, segment.Length);
        }
    }
}